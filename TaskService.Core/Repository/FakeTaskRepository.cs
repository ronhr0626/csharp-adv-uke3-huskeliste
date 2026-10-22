namespace TaskService.Core.Repository;

public class FakeTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> tasks = new();

    public void Add(TaskItem task)
    {
        tasks.Add(task);
    }

    public List<TaskItem> GetAll()
    {
        return tasks.ToList();
    }

    public TaskItem? GetById(Guid id)
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }
    public void Delete(TaskItem task)
    {
        tasks.Remove(task);
    }
    public void Update(TaskItem task)
    {
        var existingTask = tasks.First(t => t.Id == task.Id);
        tasks[tasks.IndexOf(existingTask)] = task;
    }
}