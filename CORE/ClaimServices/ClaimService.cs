using DATA.DTO;
using DATA.Interface;
using DATA.Models;

namespace CORE.ClaimServices
{
    public class ClaimService : IClaimService
    {
        private readonly IClaim _claim;

        public ClaimService(IClaim claim)
        {
            _claim = claim;
        }
        public async Task<ResponseDto> CreateClaim(ClaimrequestDto reportRequest)
        {
            if(reportRequest == null)
            {
                return new ResponseDto { StatusCode = 400, Message = "Claim details cannot be null." };
            }

            if(string.IsNullOrWhiteSpace(reportRequest.ClaimReason))
            {
                return new ResponseDto { StatusCode = 400, Message = "Title, Description, and UserId are required fields." };
            }

            try
            {




                var newItem = new Claim
                {
                    ClaimReason = reportRequest.ClaimReason,
                    ClaimDate = DateTime.Today,
                    ItemId = reportRequest.ItemId,
                    UserId = reportRequest.UserId,
                };
                var newclaim = await _claim.AddClaimAsync(newItem);
                if(newclaim)
                {
                    return new ResponseDto { StatusCode = 201, Message = "Claim created successfully", Result = null };

                }
                return new ResponseDto { StatusCode = 301, Message = "Claim failed", Result = null };



            }
            catch(Exception ex)
            {
                // Log the exception
                return new ResponseDto { StatusCode = 500, Message = ex.Message };
            }
        }
        public async Task<ResponseDto> GetAllClaim()
        {

            try
            {

                var newclaim = await _claim.GetAllItems();
                if(newclaim != null)
                {
                    return new ResponseDto { StatusCode = 201, Message = "Claims Fetched successfully", Result = newclaim };

                }
                return new ResponseDto { StatusCode = 301, Message = "Error trying to fetch Claims ", Result = null };



            }
            catch(Exception ex)
            {
                // Log the exception
                return new ResponseDto { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}
