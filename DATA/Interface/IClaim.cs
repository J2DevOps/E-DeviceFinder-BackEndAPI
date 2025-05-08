using DATA.Models;

namespace DATA.Interface
{
    public interface IClaim
    {
        Task<bool> AddClaimAsync(Claim claim);
        Task<IEnumerable<Claim>> GetAllItems();
    }
}
