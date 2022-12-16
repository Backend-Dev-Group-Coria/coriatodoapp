
using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CoriaToDo.API.Feature.Todo;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ToDoDbContext toDoDbContext;
    private readonly IMapper mapper;
    public TodoController(ToDoDbContext toDoDbContext, IMapper mapper)
    {
        this.mapper = mapper;
        this.toDoDbContext = toDoDbContext;
    }

    [HttpGet]
    public async Task<List<ToDoItem>> List()
    {
        var items = await toDoDbContext.ToDoItems.ToListAsync();
        return items;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddToDoItemRequest requestItem)
    {
        if (requestItem == null)
            return BadRequest();

        var newToDo = mapper.Map<ToDoItem>(requestItem);
        toDoDbContext.ToDoItems.Add(newToDo);

        await toDoDbContext.SaveChangesAsync();

        return Ok(new AddToDoItemResponse { Id = newToDo.Id });
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateToDoItemRequest request)
    {
        if (request == null || request.Id <= 0 || string.IsNullOrWhiteSpace(request.Title))
            return BadRequest();

        //todo: check userid for security

        var item = await toDoDbContext.ToDoItems.FirstOrDefaultAsync(i => i.Id == request.Id);
        if (item == null)
        {
            return BadRequest();
        }

        item.Title = request.Title;
        await toDoDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        //todo: check userid for security
        var item = await toDoDbContext.ToDoItems.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null)
        {
            return BadRequest();
        }
        toDoDbContext.ToDoItems.Remove(item);
        await toDoDbContext.SaveChangesAsync();
        return Ok();
    }
}