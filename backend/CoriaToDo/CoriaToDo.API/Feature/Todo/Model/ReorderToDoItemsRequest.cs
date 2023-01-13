namespace CoriaToDo.API.Feature.Todo.Model
{
    public class ReorderToDoItemsRequest
    {
        /// <summary>
        /// ID of the Task before which we want to Insert
        /// </summary>
        public int InsertBeforeId { get; set; }

        /// <summary>
        /// ToDo Items list to reorder
        /// </summary>
        public List<int> TodoIds { get; set; }
    }
}
