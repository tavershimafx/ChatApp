using ChatApp.Data;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatMessagingHub: Hub
    {
        private readonly ILogger<ChatMessagingHub> _logger;
        private readonly ApplicationDbContext _context;

        public ChatMessagingHub(ILogger<ChatMessagingHub> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task SendMessage(string receipient, string message)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == receipient);
            if (user != null)
            {
                var userConnections = _context.UserConnections
                    .Where(o =>  o.UserId == user.Id && o.ConnectionCloseTime == null)
                    .AsEnumerable()
                    .Select(k => k.ConnectionId);

                var sender = _context.Users.FirstOrDefault(u => u.Id == receipient);
                _context.ChatMessages.Add(new Models.ChatMessage()
                {
                    MessageType = Models.ChatMessageType.Text,
                    Message = message,
                    ReceipientId = user.Id,
                    SenderId = sender.Id
                });
                _context.SaveChanges();

                await Clients.Clients(userConnections).SendAsync("ChatMessage", Context.User.Identity.Name, message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == Context.User.Identity.Name);
            if (user != null)
            {
                var unread = _context.ChatMessages.Where(n => n.ReceipientId == user.Id && n.IsRead == false).ToList().AsEnumerable();
                await Clients.Clients(Context.ConnectionId).SendAsync("LoadMessages", Context.User.Identity.Name, unread);

                _context.UserConnections.Add(new Models.UserConnection() { ConnectionId = Context.ConnectionId, UserId = user.Id });
                _context.SaveChanges();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var con = _context.UserConnections.FirstOrDefault(k => k.ConnectionId == Context.ConnectionId);
            if (con != null)
            {
                con.ConnectionCloseTime = DateTime.Now;
                _context.UserConnections.Update(con);
                _context.SaveChanges();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
