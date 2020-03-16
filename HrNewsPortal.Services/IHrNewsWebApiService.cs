using System.Collections.Generic;
using System.Threading.Tasks;
using HrNewsPortal.Models;

namespace HrNewsPortal.Services
{
    public interface IHrNewsWebApiService
    {
        Task<int> GetMaxItemId();

        Task<List<Item>> GetItems(int startItemId, int takeItem);

        Task<Item> GetItem(int itemId);
    }
}
