namespace TodoList.Application
{
    public class GetTodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsIncomplete { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
