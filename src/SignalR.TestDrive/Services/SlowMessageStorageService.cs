using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.TestDrive.HostedServices
{
	public class SlowMessageStorageService
	{
		private readonly IList<Tuple<DateTime, string, string>> _messages;

		public SlowMessageStorageService()
		{
			_messages = new List<Tuple<DateTime, string, string>>();
		}

		public async Task Store(DateTime time, string username, string message)
		{
			await Task.Delay(1500);
			_messages.Add(new Tuple<DateTime, string, string>(time, username, message));
			await Task.Delay(1500);
		}
	}
}