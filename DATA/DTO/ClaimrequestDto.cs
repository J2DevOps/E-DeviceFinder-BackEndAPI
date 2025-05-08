namespace DATA.DTO
{
    public class ClaimrequestDto
    {
        public string UserId { get; set; }  // User who is claiming the item
        public string ItemId { get; set; }  // Item being claimed
        public string ClaimReason { get; set; }  // User's reason for claiming the item
    }
}
