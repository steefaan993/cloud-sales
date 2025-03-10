using System.Collections.Concurrent;

namespace Purchase.API.CCP;

public class BackgroundTaskQueue
{
    private readonly ConcurrentQueue<Func<IServiceProvider, Task>> queue = new();

    public void PutTaskInQueue(Func<IServiceProvider, Task> task)
    {
        queue.Enqueue(task);
    }

    public Func<IServiceProvider, Task>? GetTaskFromQueue()
    {
        if (queue.TryDequeue(out var task))
        {
            return task;
        }

        return null;
    }
}
