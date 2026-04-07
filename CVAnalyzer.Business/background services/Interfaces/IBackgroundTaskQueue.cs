namespace CVAnalyzer.Business.background_services.Interfaces
{
    public interface IBackgroundTaskQueue
    {
        void EnqueueTask(Func<Task> task);
        Task<Func<Task>> DequeueTaskAsync(CancellationToken cancellationToken);
    }
}