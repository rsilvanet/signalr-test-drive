using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.TestDrive.Background
{
	public class BackgroundTaskQueue
	{
		private readonly SemaphoreSlim _signal;
		private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems;

		public BackgroundTaskQueue()
		{
			_signal = new SemaphoreSlim(0);
			_workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
		}

		public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
		{
			if (workItem == null)
			{
				throw new ArgumentNullException(nameof(workItem));
			}

			_workItems.Enqueue(workItem);
			_signal.Release();
		}

		public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
		{
			await _signal.WaitAsync(cancellationToken);
			_workItems.TryDequeue(out var workItem);
			return workItem;
		}
	}
}