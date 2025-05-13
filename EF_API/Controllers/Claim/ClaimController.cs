using CORE.ClaimServices;
using DATA.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EF_API.Controllers.Claim
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claim;

        public ClaimController(IClaimService claim)
        {
            _claim = claim;
        }
        [HttpPost]
        public async Task<IActionResult> CreateClaim([FromForm] ClaimrequestDto claimRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ResponseDto
                {
                    StatusCode = 400,
                    Message = "Invalid input data.",
                    Result = ModelState.ToString()
                });
            }

            var response = await _claim.CreateClaim(claimRequest);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet]

        public async Task<IActionResult> GetAllClaim()
        {


            var response = await _claim.GetAllClaim();

            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete]

        public async Task<IActionResult> DeletClaim(string Id)
        {


            var response = await _claim.DeliteClaim(Id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
