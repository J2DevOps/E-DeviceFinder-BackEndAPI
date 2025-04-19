using DATA.Enum;

namespace DATA.DTO
{
    public class ItemResponseDto
    {
        public string Name { get; set; }  // Item name (e.g., "Laptop", "Phone")
        public string ImageUrl { get; set; }  // Item name (e.g., "Laptop", "Phone")

        public ItemCategory Category { get; set; }  // e.g., Electronics, Accessories, Documents
        public string Description { get; set; }  // Details about the item
    }
}
