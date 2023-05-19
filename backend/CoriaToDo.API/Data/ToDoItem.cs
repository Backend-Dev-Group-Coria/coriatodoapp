using System.ComponentModel.DataAnnotations.Schema;

namespace CoriaToDo.API.Data
{
    public class ToDoItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Completed { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }

        public double Order { get; set; }
    }
}
