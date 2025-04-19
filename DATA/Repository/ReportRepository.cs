using DATA.Context;
using DATA.Interface;
using DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace DATA.Repository
{
    public class ReportRepository : IReport
    {


        private readonly EFDbContext _context;

        public ReportRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports
                .Include(r => r.User) // Include the User navigation property
                .Include(r => r.Item) // Include the Item navigation property
                .ToListAsync();
        }

        public async Task<Report> GetReportByIdAsync(string id)
        {
            return await _context.Reports
                .Include(r => r.User)
                .Include(r => r.Item)
                .FirstOrDefaultAsync(r => r.Id == id); // Assuming BaseEntity has an Id property
        }

        public async Task AddReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReportAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReportAsync(string id)
        {
            var report = await _context.Reports.FindAsync(id);
            if(report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
        // 🔍 Search by item name, category, or report type
        public async Task<IEnumerable<Report>> SearchReportsAsync(string keyword)
        {
            return await _context.Reports.Where(r => r.Item.Name == keyword || r.Item.Description.Contains(keyword)).ToListAsync();

        }
    }
}

