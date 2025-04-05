using DATA.Context;
using DATA.Interface;
using DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace DATA.Repository
{
    public class ItemRepository : IITemRepository
    {


        private readonly EFDbContext _context;

        public ItemRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItems()
        {
            return await _context.Items
                .Include(r => r.UserId).ToListAsync(); // Include the User navigation property

        }

        public async Task<Item> GetItemByIdAsync(string id)
        {
            return await _context.Items
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id); // Assuming BaseEntity has an Id property
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async Task UpdateItemAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(string id)
        {
            var item = await _context.Items.FindAsync(id);
            if(item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
        //public async Task<IEnumerable<Item>> GetAllItems()
        //{
        //    var items = await _context.Items.ToListAsync();
        //    return items.Count > 0 ? items : null;

        //}

    }
}
