using Business.Config;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.SignalR
{

    public class NotificationHub:Hub
    {
        public static ConnectionCache<string> _connections =
            new ConnectionCache<string>();
        public async Task SendMessageToUser(string userId, string message)
        {
            var connectionId = _connections.GetConnections(userId).LastOrDefault();
            await Clients.Client(connectionId).SendAsync("UserConnected", message);
        }

        public static string GetConnectionIdFromUserId(string userId)
        {
            return _connections.GetConnections(userId).LastOrDefault();
        }
       
        public override async Task OnConnectedAsync()
        {
            var user = Context.GetHttpContext().Items["User"];
            if (user != null)
            {
                
                var id = user.GetType().GetProperty("id")?.GetValue(user, null).ToString();
                _connections.Add(id, Context.ConnectionId);
                await SendMessageToUser(id, Context.ConnectionId);
                await base.OnConnectedAsync();
            }
            await this.OnDisconnectedAsync(new Exception("Not Authenticated"));

        }

        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            

            await base.OnDisconnectedAsync(exception);
        }
        
        private string GetMessageToSend(string originalMessage)
        {
            return $"User connection id: {Context.ConnectionId}. Message: {originalMessage}";
        }
    }
}
