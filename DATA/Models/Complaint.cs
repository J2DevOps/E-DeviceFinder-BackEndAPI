namespace DATA.Models
{
    public class Complaint : BaseEntity
    {
        public string UserId { get; set; }  // User who is making the complaint
        public string ReportId { get; set; }  // Related lost report
        public string Description { get; set; }  // Details of the complaint
        public string Status { get; set; }  // e.g., Pending, Resolved, Rejected

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Report Report { get; set; }
    }
}
