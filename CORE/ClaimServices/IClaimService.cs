using DATA.DTO;

namespace CORE.ClaimServices
{
    public interface IClaimService
    {
        Task<ResponseDto> CreateClaim(ClaimrequestDto reportRequest);
        Task<ResponseDto> GetAllClaim();
        Task<ResponseDto> DeliteClaim(string Id);
    }
}
