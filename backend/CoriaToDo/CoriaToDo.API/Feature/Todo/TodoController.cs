
using CoriaToDo.API.Data;
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
    public async Task<List<ToDoItem>> TestAsync()
    {
        var items = await toDoDbContext.ToDoItems.ToListAsync();
        return items;
    }
}