using Business.Config;
using Business.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Business.SignalR
{
   
    public class NotificationHub:Hub
    {

        public Task SendMessageToUser(string connectionId, string message)
        {
            return Clients.Client(connectionId).SendAsync("NotificationMsg", message);
        }
       
        public override async Task OnConnectedAsync()
        {
            var user = Context.GetHttpContext().Items["User"];
            if (user != null)
            {
                var id = user.GetType().GetProperty("id")?.GetValue(user, null).ToString();
                await this.Groups.AddToGroupAsync(Context.ConnectionId, id);
                await this.Clients.Group(id).SendAsync("UserConnected", id);
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
