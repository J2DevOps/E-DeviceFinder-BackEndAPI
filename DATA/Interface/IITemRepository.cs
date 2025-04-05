using DATA.Models;

namespace DATA.Interface
{
    public interface IITemRepository
    {
        Task DeleteItemAsync(string id);
        Task UpdateItemAsync(Item item);
        Task<bool> AddItemAsync(Item item);
        Task<Item> GetItemByIdAsync(string id);
        Task<IEnumerable<Item>> GetAllItems();

    }
}
