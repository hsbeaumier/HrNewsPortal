using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HrNewsPortal.Models;
using Newtonsoft.Json;
using NLog.Fluent;

namespace HrNewsPortal.Services
{
    public class HrNewsWebApiService : IHrNewsWebApiService
    {
        #region fields

        private HttpClient client;

        #endregion

        #region constructor

        public HrNewsWebApiService(HrNewsClientSettings settings)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(settings.HrNewsApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        #region methods

        public async Task<int> GetMaxItemId()
        {
            try
            {
                var pathAndQuery = "maxitem.json?print=pretty";

                var response = await client.GetAsync(pathAndQuery);

                if (response.IsSuccessStatusCode)
                {
                    var maxItemIdString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(maxItemIdString))
                    {
                        return Convert.ToInt32(maxItemIdString);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error().Exception(ex).Message("Failed to get max item id.").Write();
            }

            return 1;
        }

        public async Task<List<Item>> GetItems(int startItemId, int takeItem)
        {
            var items = new List<Item>();
            
            var currentItemId = startItemId;

            try
            {
                for (var i = startItemId; i < (startItemId + takeItem); i++)
                {
                    var item = await GetItem(i);

                    items.Add(item);

                    currentItemId = i;
                }
            }
            catch (Exception ex)
            {
                Log.Error().Exception(ex).Message($"Failed to get items.  Failed at item id {currentItemId}.").Write();
            }

            return items;
        }
        
        public async Task<Item> GetItem(int itemId)
        {
            Item item = null;

            try
            {
                var pathAndQuery = GetPathAnyQuery(itemId);
                
                var response = await client.GetAsync(pathAndQuery);

                if (response.IsSuccessStatusCode)
                {
                    var itemJsonString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(itemJsonString))
                    {
                        item = JsonConvert.DeserializeObject<Item>(itemJsonString);

                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error().Exception(ex).Message($"Failed to get item id {itemId}.").Write();
            }

            return item;
        }

        private string GetPathAnyQuery(int itemId)
        {
            return $"item/{itemId}.json?print=pretty";
        }

        #endregion
    }
}
