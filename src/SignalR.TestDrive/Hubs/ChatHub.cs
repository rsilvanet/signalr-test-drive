using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalR.TestDrive.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}