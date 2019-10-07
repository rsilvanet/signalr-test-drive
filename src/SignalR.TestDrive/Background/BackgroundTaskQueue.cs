using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.TestDrive.Background
{
	public class BackgroundTaskQueue
	{
		private readonly SemaphoreSlim _signal;
		private readonly ConcurrentQueue<Func<CancellationToken, Task>> _tasks;

		public BackgroundTaskQueue()
		{
			_signal = new SemaphoreSlim(2);
			_tasks = new ConcurrentQueue<Func<CancellationToken, Task>>();
		}

		public void EnqueueTask(Func<CancellationToken, Task> task)
		{
			if (task == null)
			{
				throw new ArgumentNullException(nameof(task));
			}

			_tasks.Enqueue(task);
			_signal.Release();
		}

		public async Task<Func<CancellationToken, Task>> DequeueTaskAsync(CancellationToken cancellationToken)
		{
			await _signal.WaitAsync(cancellationToken);
			_tasks.TryDequeue(out var task);
			return task;
		}
	}
}