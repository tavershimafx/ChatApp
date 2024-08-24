using Microsoft.AspNetCore.Identity;

namespace ChatApp.Models
{
    public class UserConnection
    {
        public UserConnection()
        {
            this.ConnectionTime = DateTime.Now;
        }

        public long Id { get; set; }
        public string ConnectionId { get; set; }
        public DateTimeOffset ConnectionTime { get; set; }
        public DateTimeOffset? ConnectionCloseTime { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}