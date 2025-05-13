using DATA.Context;
using DATA.Interface;
using DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace DATA.Repository
{
    public class ClaimRepository : IClaim
    {
        private readonly EFDbContext _context;

        public ClaimRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddClaimAsync(Claim claim)
        {
            await _context.Claims.AddAsync(claim);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
        public async Task<IEnumerable<Claim>> GetAllItems()
        {
            return await _context.Claims
               .ToListAsync(); // Include the User navigation property


        }
        public async Task<bool> DeleteClaim(string Id)
        {
            var claim = await _context.Claims.FindAsync(Id);
            if(claim == null)
                return false;

            _context.Claims.Remove(claim);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}
