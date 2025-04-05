using CORE.ReportService;
using DATA.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EF_API.Controllers.ReportController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {


        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Retrieves a report by its ID.
        /// </summary>
        /// <param name="id">The ID of the report.</param>
        /// <returns>Report details or an error message.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(string id)
        {
            var response = await _reportService.GetReportByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("reports")]
        public async Task<IActionResult> GetAllRepors()
        {
            var response = await _reportService.GetAllReports();

            return StatusCode(response.StatusCode, response);
        }


        /// <summary>
        /// Creates a new report.
        /// </summary>
        /// <param name="reportRequest">Details of the report to create.</param>
        /// <returns>A success or error message.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromForm] ReportRequestDto reportRequest)
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

            var response = await _reportService.CreateReport(reportRequest);

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Updates an existing report.
        /// </summary>
        /// <param name="id">The ID of the report to update.</param>
        /// <param name="reportRequest">Updated details of the report.</param>
        /// <returns>A success or error message.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(string id, [FromBody] ReportRequestDto reportRequest)
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

            var response = await _reportService.UpdateReport(id, reportRequest);

            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Deletes a report by its ID.
        /// </summary>
        /// <param name="id">The ID of the report to delete.</param>
        /// <returns>A success or error message.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(string id)
        {
            var response = await _reportService.DeleteReport(id);

            return StatusCode(response.StatusCode, response);
        }
    }


}
