using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            SentTime = DateTime.Now;
        }

        public long Id { get; set; }
        public string Message { get; set; }
        public ChatMessageType MessageType { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset SentTime { get; set; }
        public DateTimeOffset ReadTime { get; set; }

        public string MediaId { get; set; }
        public ChatMedia ChatMedia { get; set; }

        public string ReceipientId { get; set; }
        [ForeignKey(nameof(ReceipientId))]
        public IdentityUser Receipient { get; set; }

        public string SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public IdentityUser Sender { get; set; }
    }
}