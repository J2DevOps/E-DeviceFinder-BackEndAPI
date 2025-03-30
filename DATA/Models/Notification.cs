namespace DATA.Models
{
    public class Notification : BaseEntity
    {

        public string UserId { get; set; }  // Recipient of the notification
        public string Message { get; set; }  // Notification message
        public bool IsRead { get; set; } = false;  // Track if the notification has been read

        // Navigation property
        public ApplicationUser User { get; set; }
    }
}
