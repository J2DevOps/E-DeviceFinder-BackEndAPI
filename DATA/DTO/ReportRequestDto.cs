using DATA.Enum;
using System.ComponentModel.DataAnnotations;

namespace DATA.DTO
{
    public class ReportRequestDto
    {
        [Required]
        public string Title { get; set; }  // Brief summary of the report

        [Required]
        public string Description { get; set; }  // Brief summary of the report

        [Required]
        public ItemCategory Category { get; set; } // Details of the lost/found item

        [Required]

        public ReportType Type { get; set; } // Enum: Lost or Found
        [Required]
        public ItemRequestDto Item { get; set; }


        [Required]
        public string UserId { get; set; } // Foreign Key to User
    }
}
