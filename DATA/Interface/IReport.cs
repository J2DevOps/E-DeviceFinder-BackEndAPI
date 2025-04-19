using DATA.Models;

namespace DATA.Interface
{
    public interface IReport
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(string id);
        Task AddReportAsync(Report report);
        Task UpdateReportAsync(Report report);
        Task DeleteReportAsync(string id);
        Task<IEnumerable<Report>> SearchReportsAsync(string keyword);
        //Task<IEnumerable<Report>> GetAllReports();
    }
}
