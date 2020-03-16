using System.Collections.Generic;
using System.Threading.Tasks;
using HrNewsPortal.Models;

namespace HrNewsPortal.Data.Repositories
{
    public interface IHrNewsRepository
    {
        int GetMaxItemId();

        void InsertItemRecords(List<Item> items);
        
        Task<List<ItemRecord>> GetRangeItemRecords(string type, int takeItems, int startItemId, bool descending);

        Task<List<ItemRecord>> GetRangeItemRecords(List<int> itemIds);

        List<ItemRecord> SearchItemRecords(string type, Dictionary<string, object> keyValues);

        ItemRecord GetItem(string type, int itemId);
    }
}
