
using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ToDoDbContext toDoDbContext;
    public TodoController(ToDoDbContext toDoDbContext)
    {
        this.toDoDbContext = toDoDbContext;
    }

    [HttpGet]
    public async Task<List<ToDoItem>> List()
    {
        var items = await toDoDbContext.ToDoItems.ToListAsync();
        return items;
    }

    public async Task<IActionResult> Add(AddToDoItemRequest requestItem)
    {
        if (requestItem == null)
            return BadRequest();

        toDoDbContext.ToDoItems.Add(
            new ToDoItem() { Title = requestItem.Title });

        await toDoDbContext.SaveChangesAsync();

        return Ok();
    }
}