using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATA.DTO;
using DATA.Interface;
using DATA.Models;
using System;
using System.Threading.Tasks;

namespace CORE.ReportService
{
    public class ReportService : IReportService
    {
        private readonly IReport _reportRepository;
        private readonly IITemRepository _iTem;
        private readonly Cloudinary _cloudinary;



        /// <summary>
        /// Service class for report-related operations.
        /// Uses IReportRepository for database interactions and returns standardized ResponseDto.
        /// </summary>

        public ReportService(IReport reportRepository, IITemRepository iTem, Cloudinary cloudinary)
        {
            _reportRepository = reportRepository;
            _iTem = iTem;
            _cloudinary = cloudinary;
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
            var reports = await _reportRepository.GetAllReportsAsync();
            return reports != null
                ? new ResponseDto { StatusCode = 200, Message = "Users found", Result = reports }
                : new ResponseDto { StatusCode = 404, Message = "Users not found" };
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
    }


}
