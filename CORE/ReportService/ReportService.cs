using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATA.DTO;
using DATA.Interface;
using DATA.Models;

namespace CORE.ReportService
{
    public class ReportService : IReportService
    {
        private readonly IReport _reportRepository;
        private readonly IITemRepository _iTem;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;



        /// <summary>
        /// Service class for report-related operations.
        /// Uses IReportRepository for database interactions and returns standardized ResponseDto.
        /// </summary>

        public ReportService(IReport reportRepository, IITemRepository iTem,
            Cloudinary cloudinary, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _iTem = iTem;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a report by its ID.
        /// </summary>
        /// <param name="reportId">The ID of the report.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> GetReportByIdAsync(string reportId)
        {
            if(string.IsNullOrWhiteSpace(reportId))
            {
                return new ResponseDto { StatusCode = 400, Message = "Report ID cannot be null or empty." };
            }

            try
            {
                var report = await _reportRepository.GetReportByIdAsync(reportId);
                return report != null
                    ? new ResponseDto { StatusCode = 200, Message = "Report found", Result = report }
                    : new ResponseDto { StatusCode = 404, Message = "Report not found" };
            }
            catch(Exception)
            {
                // Log the exception (e.g., using ILogger)
                return new ResponseDto { StatusCode = 500, Message = "An error occurred while fetching the report." };
            }
        }
        public async Task<ResponseDto> GetAllReports()
        {
            var response = new ResponseDto();
            List<ReportResponseDto> listreports = new List<ReportResponseDto>();


            var appreports = await _reportRepository.GetAllReportsAsync();
            if(appreports.Any())
            {

                foreach(var report in appreports)
                {
                    var mappedreport = _mapper.Map<ReportResponseDto>(report);
                    listreports.Add(mappedreport);

                }
                response.Result = listreports;
                response.StatusCode = 200;
                return response;


            }
            response.Result = null;
            response.StatusCode = 404;
            return response;
        }

        /// <summary>
        /// Creates a new report.
        /// </summary>
        /// <param name="reportRequest">Report details.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> CreateReport(ReportRequestDto reportRequest)
        {
            if(reportRequest == null)
            {
                return new ResponseDto { StatusCode = 400, Message = "Report details cannot be null." };
            }

            if(string.IsNullOrWhiteSpace(reportRequest.Title) || string.IsNullOrWhiteSpace(reportRequest.Title) || string.IsNullOrWhiteSpace(reportRequest.UserId))
            {
                return new ResponseDto { StatusCode = 400, Message = "Title, Description, and UserId are required fields." };
            }

            try
            {





                // Upload the thumbnail to Cloudinary
                // Step 2: Open the thumbnail file stream (separate file from the video)
                if(reportRequest.Item.Image != null && reportRequest.Item.Image.Length > 0)
                {
                    await using var thumbnailStream = reportRequest.Item.Image.OpenReadStream();

                    // Upload the thumbnail to Cloudinary
                    var uploadImageParams = new ImageUploadParams
                    {
                        File = new FileDescription(reportRequest.Item.Name, thumbnailStream),
                        // Add any other Cloudinary options if needed
                    };

                    var uploadImageResult = await _cloudinary.UploadAsync(uploadImageParams);
                    // If thumbnail upload fails
                    if(uploadImageResult.Url == null)
                    {
                        return new ResponseDto
                        {
                            StatusCode = 400,
                            Message = "Title, Description, and UserId are required fields."
                        };
                    }
                    var newItem = new Item
                    {
                        Name = reportRequest.Item.Name,
                        Description = reportRequest.Item.Description,
                        SerialNumber = reportRequest.Item.SerialNumber,
                        Category = reportRequest.Item.Category,
                        CreatedAt = DateTime.Now,
                        ImageUrl = uploadImageResult.Url.ToString(),

                    };

                    await _iTem.AddItemAsync(newItem);
                    var newReport = new Report
                    {
                        Title = reportRequest.Title,
                        Description = reportRequest.Description,
                        Type = reportRequest.Type,
                        UserId = reportRequest.UserId,
                        ItemId = newItem.Id,
                    };
                    await _reportRepository.AddReportAsync(newReport);

                }


                return new ResponseDto { StatusCode = 201, Message = "Report created successfully", Result = null };
            }
            catch(Exception ex)
            {
                // Log the exception
                return new ResponseDto { StatusCode = 500, Message = ex.Message };
            }
        }

        /// <summary>
        /// Updates an existing report.
        /// </summary>
        /// <param name="reportId">The ID of the report to update.</param>
        /// <param name="reportRequest">Updated report details.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> UpdateReport(string reportId, ReportRequestDto reportRequest)
        {
            if(string.IsNullOrWhiteSpace(reportId))
            {
                return new ResponseDto { StatusCode = 400, Message = "Report ID cannot be null or empty." };
            }

            if(reportRequest == null)
            {
                return new ResponseDto { StatusCode = 400, Message = "Report details cannot be null." };
            }

            try
            {
                var existingReport = await _reportRepository.GetReportByIdAsync(reportId);
                if(existingReport == null)
                {
                    return new ResponseDto { StatusCode = 404, Message = "Report not found." };
                }

                // Update the report properties
                existingReport.Title = reportRequest.Title;
                existingReport.Description = reportRequest.Description;
                existingReport.Type = reportRequest.Type;
                existingReport.UserId = reportRequest.UserId;

                await _reportRepository.UpdateReportAsync(existingReport);

                return new ResponseDto { StatusCode = 200, Message = "Report updated successfully", Result = existingReport };
            }
            catch(Exception ex)
            {
                // Log the exception
                return new ResponseDto { StatusCode = 500, Message = ex.Message };
            }
        }

        /// <summary>
        /// Deletes a report by its ID.
        /// </summary>
        /// <param name="reportId">The ID of the report to delete.</param>
        /// <returns>ResponseDto indicating success or failure.</returns>
        public async Task<ResponseDto> DeleteReport(string reportId)
        {
            if(string.IsNullOrWhiteSpace(reportId))
            {
                return new ResponseDto { StatusCode = 400, Message = "Report ID cannot be null or empty." };
            }

            try
            {
                await _reportRepository.DeleteReportAsync(reportId);
                return new ResponseDto { StatusCode = 200, Message = "Report deleted successfully" };
            }

            catch(Exception ex)
            {
                // Log the exception
                return new ResponseDto { StatusCode = 500, Message = ex.Message };
            }
        }

        /// <summary>
        /// Searches reports by keyword (Item name, category, or report type).
        /// </summary>
        /// <param name="keyword">Search keyword.</param>
        /// <returns>ResponseDto with matching reports.</returns>
        public async Task<ResponseDto> SearchReportsAsync(string keyword)
        {
            var response = new ResponseDto();

            if(string.IsNullOrWhiteSpace(keyword))
            {
                response.StatusCode = 400;
                response.Message = "Keyword cannot be empty.";
                return response;
            }

            try
            {
                var matchedReports = await _reportRepository.SearchReportsAsync(keyword);

                if(!matchedReports.Any())
                {
                    response.StatusCode = 404;
                    response.Message = "No reports found matching the search criteria.";
                    return response;
                }

                var mappedReports = matchedReports
                    .Select(report => _mapper.Map<ReportResponseDto>(report))
                    .ToList();

                response.StatusCode = 200;
                response.Message = "Search successful.";
                response.Result = mappedReports;
                return response;
            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = $"An error occurred during the search: {ex.Message}";
                return response;
            }
        }
    }


}
