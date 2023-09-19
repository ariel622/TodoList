namespace TodoList.Domain.TodoItem
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsIncomplete { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

