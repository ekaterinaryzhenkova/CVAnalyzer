using CVAnalyzer.Business.background_services.Interfaces;
using System.Collections.Concurrent;

namespace CVAnalyzer.Business.background_services
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<Task>> _tasks = new ConcurrentQueue<Func<Task>>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void EnqueueTask(Func<Task> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _tasks.Enqueue(task);
            _signal.Release();
        }

        public async Task<Func<Task>> DequeueTaskAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _tasks.TryDequeue(out var task);
            return task;
        }
    }
}