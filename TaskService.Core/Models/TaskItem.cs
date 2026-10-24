public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public Guid UserId { get; set; }

    public TaskItem(string title, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        UserId = userId;
        IsCompleted = false;
    }
}