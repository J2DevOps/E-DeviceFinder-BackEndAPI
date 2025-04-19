using DATA.DTO;

namespace CORE.ItemServices
{
    public interface IItemService
    {
        Task<ResponseDto> SearchItemsAsync(string keyword);
    }
}
