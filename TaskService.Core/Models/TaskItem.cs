public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public TaskItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
        IsCompleted = false;
    }
}