namespace TaskService.Core;
using global::TaskService.Core.Repository;

public class TaskService
{
    private readonly ITaskRepository repository;

    public TaskService(ITaskRepository repository)
    {
        this.repository = repository;
    }

    public TaskItem AddTask(string title)
    {
        var task = new TaskItem(title);
        repository.Add(task);
        return task;
    }

    public List<TaskItem> GetTasks()
    {
        return repository.GetAll();
    }

    public void CompleteTask(Guid id)
    {
        var task = repository.GetById(id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task was not found");
        }

        task.IsCompleted = true;
        repository.Update(task);
    }

    public void DeleteTask(Guid id)
    {
        var task = repository.GetById(id);

        if (task == null)
        {
            throw new KeyNotFoundException("Task was not found");
        }

        repository.Delete(task);
    }
}
