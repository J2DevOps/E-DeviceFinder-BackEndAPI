using DATA.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATA.Models
{
    public class Report : BaseEntity
    {

        [Required]
        public string Title { get; set; }  // Brief summary of the report

        [Required]
        public string Description { get; set; } // Details of the lost/found item

        [Required]
        public ReportType Type { get; set; } // Enum: Lost or Found


        [Required]
        public string UserId { get; set; } // Foreign Key to User

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } // Navigation Property

        public string ItemId { get; set; } // Optional reference to a registered item

        [ForeignKey("ItemId")]
        public Item Item { get; set; } // Navigation Property
    }


}
