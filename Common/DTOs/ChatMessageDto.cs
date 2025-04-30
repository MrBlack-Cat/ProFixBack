namespace Common.DTOs
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }      
        public string? SessionId { get; set; } 
        public bool IsGuest { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string MessageText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
