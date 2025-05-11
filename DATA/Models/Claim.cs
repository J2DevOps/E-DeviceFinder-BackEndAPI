namespace DATA.Models
{
    public class Claim : BaseEntity
    {

        public string UserId { get; set; }  // User who is claiming the item
        public string ItemId { get; set; }  // Item being claimed
        public string ItemName { get; set; }
        public string ClaimReason { get; set; }  // User's reason for claiming the item
        public DateTime ClaimDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }  // e.g., Pending, Approved, Rejected

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Item Item { get; set; }
    }
}
