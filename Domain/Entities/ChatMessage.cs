using System;

namespace Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? SessionId { get; set; }
        public bool IsGuest { get; set; }
        public string Sender { get; set; } = null!;
        public string MessageText { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

}
