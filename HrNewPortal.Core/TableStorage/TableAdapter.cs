using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HrNewsPortal.Core.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NLog.Fluent;

namespace HrNewsPortal.Core.TableStorage
{
    public class TableAdapter
    {
        #region fields

        private readonly CloudStorageAccount _account;
        private CloudTableClient _client;

        #endregion

        #region constructor

        public TableAdapter(string connectionString)
        {
            _account = CloudStorageAccountExtensions.Parse(connectionString);
        }

        #endregion

        #region methods
        
        private TableOperation BuildOperation<TModel>(TModel source, string partitionKey, string rowKey, Func<DynamicTableEntity, TableOperation> action)
        {
            var context = new OperationContext();
            var flattenedProperties = EntityPropertyConverter.Flatten(source, context);
            var model = new DynamicTableEntity(partitionKey, rowKey) { Properties = flattenedProperties };

            var operation = action(model);

            return operation;
        }

        private CloudTableClient GetClient()
        {
            if (_client == null)
                _client = _account.CreateCloudTableClient();

            return _client;
        }

        private CloudTable GetTable(string tableName)
        {
            var client = GetClient();
            var table = client.GetTableReference(tableName);

            return table;
        }

        public async void Insert<TModel>(TModel source, string tableName, string partitionKeyName, string rowKeyName)
        {
            try
            {
                var table = GetTable(tableName);

                var existsTask = table.ExistsAsync();
                existsTask.Wait();
                if (!existsTask.Result)
                {
                    var createTableTask = table.CreateIfNotExistsAsync();
                    createTableTask.Wait();
                }

                var operation = BuildOperation(source, partitionKeyName, rowKeyName, TableOperation.Insert);
                await table.ExecuteAsync(operation);
            }
            catch (Exception e)
            {
                Log.Error().Exception(e).Message($"Failed to log record for partition key \"{partitionKeyName}\" and row key \"{rowKeyName}\".").Write();
            }
        }

        public async void Save<TModel>(TModel source, string tableName, string partitionKeyName, string rowKeyName)
        {
            var table = GetTable(tableName);

            var existsTask = table.ExistsAsync();
            existsTask.Wait();
            if (!existsTask.Result)
            {
                var createTableTask = table.CreateIfNotExistsAsync();
                createTableTask.Wait();
            }

            var operation = BuildOperation(source, partitionKeyName, rowKeyName, TableOperation.InsertOrReplace);
            await table.ExecuteAsync(operation);
        }

        private async Task<DynamicTableEntity> GetSinglePointResult(string tableName, string partitionKey, string rowKey)
        {
            var table = GetTable(tableName);

            //Load
            var operation = TableOperation.Retrieve(partitionKey, rowKey);
            var result = await table.ExecuteAsync(operation);
            var entity = (DynamicTableEntity)result.Result;

            return entity;
        }

        public async Task<TModel> Get<TModel>(string tableName, string partitionKey, string rowKey)
        {
            var context = new OperationContext();
            var entity = await GetSinglePointResult(tableName, partitionKey, rowKey);
            var model = EntityPropertyConverter.ConvertBack<TModel>(entity.Properties, context);

            return model;
        }

        public async Task<List<TModel>> GetAll<TModel>(string tableName, TableQuery query, EntityResolver<TModel> resolver) where TModel : class, IBaseFieldsForAzureTableRecord
        {
            var results = new List<TModel>();
            
            var table = GetTable(tableName);
            
            var token = new TableContinuationToken();
            var entities = await table.ExecuteQuerySegmentedAsync(query, resolver, token);
            
            foreach (var entity in entities)
            {
                entity.PartitionKey = entity.PartitionKey;
                entity.RowKey = entity.RowKey;
                entity.Timestamp = entity.Timestamp;
                results.Add(entity);
            }

            return results;
        }
        
        #endregion
    }
}
