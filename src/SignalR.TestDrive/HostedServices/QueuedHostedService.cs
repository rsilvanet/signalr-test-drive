using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SignalR.TestDrive.Background;

namespace SignalR.TestDrive.HostedServices
{
	public class QueuedHostedService : BackgroundService
	{
		private BackgroundTaskQueue _queue;
		private CancellationTokenSource _shutdown;
		private Task _backgroundTask;

		public QueuedHostedService(BackgroundTaskQueue queue)
		{
			_queue = queue;
			_shutdown = new CancellationTokenSource();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_backgroundTask = Task.Run(async () =>
			{
				while (!stoppingToken.IsCancellationRequested && !_shutdown.IsCancellationRequested)
				{
					var task = await _queue.DequeueTaskAsync(_shutdown.Token);
					await task(_shutdown.Token);
				}
			}, stoppingToken);

			await _backgroundTask;
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			_shutdown.Cancel();
			await Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, stoppingToken));
		}
	}
}