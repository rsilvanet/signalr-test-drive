using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SignalR.TestDrive.Hubs;

namespace SignalR.TestDrive.Services
{
	public class ClockHostedService : IHostedService, IDisposable
	{
		private Timer _timer;
		private IHubContext<ClockHub> _hubContext;
		
		public ClockHostedService(IHubContext<ClockHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(async (_) => 
			{
				await _hubContext.Clients.All.SendAsync("UpdateClock", DateTime.Now);
			}, 
			null, 
			TimeSpan.Zero, 
			TimeSpan.FromSeconds(1));

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}