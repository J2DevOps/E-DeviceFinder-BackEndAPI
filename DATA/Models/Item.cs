using DATA.Enum;

namespace DATA.Models
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }  // Item name (e.g., "Laptop", "Phone")
        public ItemCategory Category { get; set; }  // e.g., Electronics, Accessories, Documents
        public string Description { get; set; }  // Details about the item
        public string SerialNumber { get; set; }  // Unique identifier (if applicable)
        public DateTime DateLost { get; set; }
        public DateTime? DateFound { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }  // e.g., Lost, Found, Claimed

        // Foreign keys
        public string UserId { get; set; }  // Owner of the item
        public string LocationId { get; set; }  // Where it was lost/found

        // Navigation properties
        public ApplicationUser User { get; set; }
        //public Location Location { get; set; }






    }
}
