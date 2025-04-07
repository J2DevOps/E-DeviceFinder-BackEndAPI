using DATA.Enum;

namespace DATA.DTO
{
    public class ReportResponseDto
    {

        public string Title { get; set; }  // Brief summary of the report

        public string Description { get; set; } // Details of the lost/found item

        public ItemCategory ItemCategory { get; set; } // Enum: Lost or Found

    }
}
