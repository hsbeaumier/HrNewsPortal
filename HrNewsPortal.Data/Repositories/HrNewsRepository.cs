using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrNewsPortal.Models;
using HrNewsPortal.Core.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;
using NLog.Fluent;

namespace HrNewsPortal.Data.Repositories
{
    public class HrNewsRepository : IHrNewsRepository
    {
        #region fields

        private readonly TableAdapter _adapter;
        private readonly string _tableName;
        private readonly string _minMaxRecordTableName;

        #endregion

        #region constructor

        public HrNewsRepository(GeneralSettings settings)
        {
            _tableName = "HrNewsRecords";
            _minMaxRecordTableName = "HrNewsRecordsStats";
            _adapter = new TableAdapter(settings.AzureStorageConnectionString);
        }
        
        #endregion

        #region methods

        public int GetMaxItemId()
        {
            var resolver = GetMinMaxItemRecordResolver();

            var query = new TableQuery();

            var task = _adapter.GetAll(_minMaxRecordTableName, query, resolver);
            
            task.Wait();

            if (task.Result != null)
            {
                var records = task.Result;

                if (records.Any())
                {
                    return records.Max(r => r.MaxItemId);
                }
            }

            
            return 0;
        }

        public int GetMaxItemId(string type)
        {
            var resolver = GetMinMaxItemRecordResolver();

            
            var query = new TableQuery();
            query.FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, type);

            var task = _adapter.GetAll(_minMaxRecordTableName, query, resolver);

            task.Wait();

            if (task.Result != null)
            {
                var records = task.Result;

                if (records.Any())
                {
                    return records.Max(r => r.MaxItemId);
                }
            }


            return 0;
        }

        public ItemRecord GetItem(string type, int itemId)
        {
            try
            {
                var task = _adapter.Get<ItemRecord>(_tableName, type, itemId.ToString().PadLeft(10,'0'));
            
                task.Wait();

                return task.Result;
            }
            catch (Exception e)
            {
                Log.Error().Exception(e).Message($"Failed to get item for type {type} and item id {itemId}").Write();
            }

            return null;
        }

        public void InsertItemRecords(List<Item> items)
        {
            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    if (item != null)
                    {
                        var record = new ItemRecord()
                        {
                            PartitionKey = item.Type,
                            RowKey = item.Id.ToString().PadLeft(10,'0'),
                            Id = item.Id,
                            Deleted = item.Deleted,
                            Type = item.Type,
                            By = item.By,
                            Time = item.UtcTime,
                            Text = item.Text,
                            Dead = item.Dead,
                            Parent = item.Parent,
                            Poll = item.Poll,
                            Kids = item.Kids == null ? string.Empty : string.Join(",",item.Kids),
                            Url = item.Url,
                            Score = item.Score,
                            Title = item.Title,
                            Parts = item.Parts == null ? string.Empty : string.Join(",", item.Parts),
                            Descendants = item.Descendants
                        };

                        _adapter.Insert(record, _tableName, record.PartitionKey, record.RowKey);
                    }
                }

                // Save off max item identifier by Type in separate table.
                // Create the resolver first.
                var resolver = GetItemRecordResolver();

                // Declare types that we will create a record for.
                var types = new [] {"job", "story", "comment", "poll", "pollopt"};

                foreach (var type in types)
                {
                    var typeFilter = TableQuery.GenerateFilterCondition("Type", QueryComparisons.Equal, type);
                    var notDeletedFilter = TableQuery.GenerateFilterConditionForBool("Deleted", QueryComparisons.Equal, false);
                    var notDeadFilter = TableQuery.GenerateFilterConditionForBool("Dead", QueryComparisons.NotEqual, true);
                    var exclusionsFilter = TableQuery.CombineFilters(notDeletedFilter, TableOperators.And, notDeadFilter);
                    var finalFilter = TableQuery.CombineFilters(typeFilter, TableOperators.And, exclusionsFilter);

                    var query = new TableQuery();
                    query.FilterString = finalFilter;
                    
                    var getAllTask = _adapter.GetAll(_tableName, query, resolver);
                    getAllTask.Wait();

                    var itemRecordsForType = getAllTask.Result;

                    var minMaxRecord = new MinMaxItemRecord()
                    {
                        PartitionKey = type,
                        RowKey = "0",
                        MinItemId = Convert.ToInt32(itemRecordsForType.Min(i => i.RowKey)),
                        MaxItemId = Convert.ToInt32(itemRecordsForType.Max(i => i.RowKey))
                    };

                    _adapter.Save(minMaxRecord, _minMaxRecordTableName, minMaxRecord.PartitionKey, minMaxRecord.RowKey);
                }
            }
        }

        public async Task<List<ItemRecord>> GetRangeItemRecords(string type, int takeItems, int startItemId, bool descending)
        {
            var resolver = GetItemRecordResolver();
            
            var typeCondition = TableQuery.GenerateFilterCondition("Type", QueryComparisons.Equal, type);

            var itemIdRangeCondition = descending ? TableQuery.GenerateFilterConditionForInt("Id", QueryComparisons.LessThanOrEqual, startItemId) :
                TableQuery.GenerateFilterConditionForInt("Id", QueryComparisons.GreaterThanOrEqual, startItemId);

            var conditions = TableQuery.CombineFilters(typeCondition, TableOperators.And, itemIdRangeCondition);
            
            var query = new TableQuery().Where(conditions).Take(takeItems);

            return await _adapter.GetAll(_tableName, query, resolver);
        }

        public async Task<List<ItemRecord>> GetRangeItemRecords(List<int> itemIds)
        {
            var resolver = GetItemRecordResolver();
            
            var query = new TableQuery();
            var filter = string.Empty;
            foreach (var itemId in itemIds)
            {
                var itemFilter = TableQuery.GenerateFilterConditionForInt("Id", QueryComparisons.Equal, itemId);

                if (string.IsNullOrWhiteSpace(filter))
                {
                    filter = itemFilter;
                }
                else
                {
                    filter = TableQuery.CombineFilters(filter, TableOperators.Or, itemFilter);
                }
            }

            query.FilterString = filter;

            return await _adapter.GetAll(_tableName, query, resolver);
        }
        
        public List<ItemRecord> SearchItemRecords(string type, Dictionary<string, object> keyValues)
        {
            var finalItemList = new List<ItemRecord>();

            var resolver = GetItemRecordResolver();

            var query = new TableQuery();
            query.FilterString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, type);
            
            var task = _adapter.GetAll(_tableName, query, resolver);
            task.Wait();
            var itemList =  task.Result;

            if (keyValues.ContainsKey("Text"))
            {
                var keyValue = keyValues.FirstOrDefault(kv => kv.Key.Equals("Text"));

                var filteredItemsList = itemList.Where(il => il.Text != null && il.Text.ToLower().Contains(Convert.ToString(keyValue.Value).ToLower()));

                foreach (var filteredItem in filteredItemsList)
                {
                    // Check to see if it does not already
                    // exist in the final list.
                    if (!finalItemList.Any() || finalItemList.All(fi => fi.Id != filteredItem.Id))
                    {
                        // Since it is not in the list.  Add it.
                        finalItemList.Add(filteredItem);
                    }
                }
            }

            if (keyValues.ContainsKey("Title"))
            {
                var keyValue = keyValues.FirstOrDefault(kv => kv.Key.Equals("Title"));

                var filteredItemsList = itemList.Where(il => il.Title != null && il.Title.ToLower().Contains(Convert.ToString(keyValue.Value).ToLower()));

                foreach (var filteredItem in filteredItemsList)
                {
                    // Check to see if it does not already
                    // exist in the final list.
                    if (!finalItemList.Any() || finalItemList.Any(fi => fi.Id != filteredItem.Id))
                    {
                        // Since it is not in the list.  Add it.
                        finalItemList.Add(filteredItem);
                    }
                }
            }

            if (keyValues.ContainsKey("Url"))
            {
                var keyValue = keyValues.FirstOrDefault(kv => kv.Key.Equals("Url"));

                var filteredItemsList = itemList.Where(il => il.Url != null && il.Url.ToLower().Contains(Convert.ToString(keyValue.Value).ToLower()));

                foreach (var filteredItem in filteredItemsList)
                {
                    // Check to see if it does not already
                    // exist in the final list.
                    if (!finalItemList.Any() || finalItemList.Any(fi => fi.Id != filteredItem.Id))
                    {
                        // Since it is not in the list.  Add it.
                        finalItemList.Add(filteredItem);
                    }
                }
            }

            if (keyValues.ContainsKey("By"))
            {
                var keyValue = keyValues.FirstOrDefault(kv => kv.Key.Equals("By"));

                var filteredItemsList = itemList.Where(il => il.By != null && il.By.ToLower().Contains(Convert.ToString(keyValue.Value).ToLower()));

                foreach (var filteredItem in filteredItemsList)
                {
                    // Check to see if it does not already
                    // exist in the final list.
                    if (!finalItemList.Any() || finalItemList.Any(fi => fi.Id != filteredItem.Id))
                    {
                        // Since it is not in the list.  Add it.
                        finalItemList.Add(filteredItem);
                    }
                }
            }

            if (keyValues.ContainsKey("Time"))
            {
                var keyValue = keyValues.FirstOrDefault(kv => kv.Key.Equals("Time"));

                var filteredItemsList = itemList.Where(il => il.Time != null && il.Time == Convert.ToDateTime(keyValue.Value));

                foreach (var filteredItem in filteredItemsList)
                {
                    // Check to see if it does not already
                    // exist in the final list.
                    if (!finalItemList.Any() || finalItemList.Any(fi => fi.Id != filteredItem.Id))
                    {
                        // Since it is not in the list.  Add it.
                        finalItemList.Add(filteredItem);
                    }
                }
            }

            if (keyValues.ContainsKey("Score"))
            {
                var keyValue = keyValues.FirstOrDefault(kv => kv.Key.Equals("Score"));

                var filteredItemsList = itemList.Where(il => il.Score == Convert.ToInt32(keyValue.Value));

                foreach (var filteredItem in filteredItemsList)
                {
                    // Check to see if it does not already
                    // exist in the final list.
                    if (!finalItemList.Any() || finalItemList.Any(fi => fi.Id != filteredItem.Id))
                    {
                        // Since it is not in the list.  Add it.
                        finalItemList.Add(filteredItem);
                    }
                }
            }

            return finalItemList;
        }
        
        private EntityResolver<ItemRecord> GetItemRecordResolver()
        {
            return new EntityResolver<ItemRecord>(
                (partKey, rowKey, timestamp, propertySegment, etag) =>
                {
                    var rec = new ItemRecord();
                    try
                    {
                        rec.PartitionKey = partKey;
                        rec.RowKey = rowKey;
                        rec.Timestamp = timestamp.Date;
                        rec.ETag = etag;

                        if (propertySegment.ContainsKey("Id"))
                        {
                            rec.Id = propertySegment.FirstOrDefault(ps => ps.Key == "Id").Value.Int32Value.GetValueOrDefault(0);
                        }

                        if (propertySegment.ContainsKey("Deleted"))
                        {
                            rec.Deleted = propertySegment.FirstOrDefault(ps => ps.Key == "Deleted").Value.BooleanValue.GetValueOrDefault(false);
                        }
                        
                        if (propertySegment.ContainsKey("Type"))
                        {
                            rec.Type = propertySegment.FirstOrDefault(ps => ps.Key == "Type").Value.StringValue;
                        }
                        
                        if (propertySegment.ContainsKey("By"))
                        {
                            rec.By = propertySegment.FirstOrDefault(ps => ps.Key == "By").Value.StringValue;
                        }

                        if (propertySegment.ContainsKey("Time"))
                        {
                            rec.Time = propertySegment.FirstOrDefault(ps => ps.Key == "Time").Value.DateTime;
                        }

                        if (propertySegment.ContainsKey("Text"))
                        {
                            rec.Text = propertySegment.FirstOrDefault(ps => ps.Key == "Text").Value.StringValue;
                        }

                        if (propertySegment.ContainsKey("Dead"))
                        {
                            rec.Dead = propertySegment.FirstOrDefault(ps => ps.Key == "Dead").Value.BooleanValue.GetValueOrDefault(false);
                        }

                        if (propertySegment.ContainsKey("Parent"))
                        {
                            rec.Parent = propertySegment.FirstOrDefault(ps => ps.Key == "Parent").Value.Int32Value.GetValueOrDefault(0);
                        }

                        if (propertySegment.ContainsKey("Poll"))
                        {
                            rec.Poll = propertySegment.FirstOrDefault(ps => ps.Key == "Poll").Value.Int32Value.GetValueOrDefault(0);
                        }

                        if (propertySegment.ContainsKey("Kids"))
                        {
                            rec.Kids = propertySegment.FirstOrDefault(ps => ps.Key == "Kids").Value.StringValue;
                        }

                        if (propertySegment.ContainsKey("Url"))
                        {
                            rec.Url = propertySegment.FirstOrDefault(ps => ps.Key == "Url").Value.StringValue;
                        }

                        if (propertySegment.ContainsKey("Score"))
                        {
                            rec.Score = propertySegment.FirstOrDefault(ps => ps.Key == "Score").Value.Int32Value.GetValueOrDefault(0);
                        }
                        
                        if (propertySegment.ContainsKey("Title"))
                        {
                            rec.Title = propertySegment.FirstOrDefault(ps => ps.Key == "Title").Value.StringValue;
                        }

                        if (propertySegment.ContainsKey("Parts"))
                        {
                            rec.Parts = propertySegment.FirstOrDefault(ps => ps.Key == "Parts").Value.StringValue;
                        }

                        if (propertySegment.ContainsKey("Descendants"))
                        {
                            rec.Descendants = propertySegment.FirstOrDefault(ps => ps.Key == "Descendants").Value.Int32Value.GetValueOrDefault(0);
                        }
                    }

                    catch (Exception ex)
                    {
                        Log.Error().Exception(ex).Message("Failed to create Item Record resolver.").Write();

                        throw;
                    }
                    return rec;
                });
        }

        private EntityResolver<MinMaxItemRecord> GetMinMaxItemRecordResolver()
        {
            return (partKey, rowKey, timestamp, propertySegment, etag) =>
            {
                var rec = new MinMaxItemRecord();
                try
                {
                    rec.PartitionKey = partKey;
                    rec.RowKey = rowKey;
                    rec.Timestamp = timestamp.Date;
                    rec.ETag = etag;
                    
                    if (propertySegment.ContainsKey("MinItemId"))
                    {
                        rec.MinItemId = propertySegment.FirstOrDefault(ps => ps.Key == "MinItemId").Value.Int32Value.GetValueOrDefault(0);
                    }

                    if (propertySegment.ContainsKey("MaxItemId"))
                    {
                        rec.MaxItemId = propertySegment.FirstOrDefault(ps => ps.Key == "MaxItemId").Value.Int32Value.GetValueOrDefault(0);
                    }
                }
                catch (Exception e)
                {
                    Log.Error().Exception(e).Message("Failed to create Min Max Item Record resolver.").Write();

                    throw;
                }
                return rec;
            };
        }

        #endregion
    }
}
