using DATA.DTO;

namespace CORE.ReportService
{
    public interface IReportService
    {
        Task<ResponseDto> UpdateReport(string reportId, ReportRequestDto reportRequest);
        Task<ResponseDto> DeleteReport(string reportId);
        Task<ResponseDto> CreateReport(ReportRequestDto reportRequest);
        Task<ResponseDto> GetReportByIdAsync(string reportId);
        Task<ResponseDto> GetAllReports();
        Task<ResponseDto> SearchReportsAsync(string keyword);
    }
}
