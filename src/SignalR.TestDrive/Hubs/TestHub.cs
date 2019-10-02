using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalR.TestDrive.Hubs
{
    public class TestHub : Hub
    {
        private static int _counter = 1;

        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage", "Test " + _counter);
            _counter++;
        }
    }
}