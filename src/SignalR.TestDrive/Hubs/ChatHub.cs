using Microsoft.AspNetCore.SignalR;
using SignalR.TestDrive.Background;
using SignalR.TestDrive.HostedServices;
using System;
using System.Threading.Tasks;

namespace SignalR.TestDrive.Hubs
{
    public class ChatHub : Hub
    {
        private readonly BackgroundTaskQueue _taskQueue;
        private readonly SlowMessageStorageService _messageStorage;

        public ChatHub(BackgroundTaskQueue taskQueue, SlowMessageStorageService messageStorage)
        {
            _taskQueue = taskQueue;
            _messageStorage = messageStorage;
        }

        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
            var storeTask = _messageStorage.StoreAsync(DateTime.Now, username, message);
            _taskQueue.EnqueueTask(async x => await storeTask);
        }
    }
}