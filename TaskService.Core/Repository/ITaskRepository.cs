namespace TaskService.Core.Repository;

public interface ITaskRepository
{
    void Add(TaskItem task);
    List<TaskItem> GetAll();
    TaskItem? GetById(Guid id);
    void Delete(TaskItem task);
    void Update(TaskItem task);
}