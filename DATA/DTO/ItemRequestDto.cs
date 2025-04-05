using DATA.Enum;
using Microsoft.AspNetCore.Http;

namespace DATA.DTO
{
    public class ItemRequestDto
    {
        public string Name { get; set; }  // Item name (e.g., "Laptop", "Phone")
        public ItemCategory Category { get; set; }  // e.g., Electronics, Accessories, Documents
        public string Description { get; set; }  // Details about the item
        public string SerialNumber { get; set; }  // Unique identifier (if applicable)
        public IFormFile Image { get; set; }
        // Foreign keys
        public string UserId { get; set; }  // Owner of the item
    }
}
