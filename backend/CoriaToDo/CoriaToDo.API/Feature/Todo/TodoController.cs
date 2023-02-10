
using AutoMapper;
using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Model;
using CoriaToDo.API.Feature.Todo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoriaToDo.API.Feature.Todo;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ToDoDbContext toDoDbContext;
    private readonly IMapper mapper;
    private readonly SessionContext _sessionContext;

    public TodoController(ToDoDbContext toDoDbContext, IMapper mapper, SessionContext sessionContext)
    {
        this.mapper = mapper;
        _sessionContext = sessionContext;
        this.toDoDbContext = toDoDbContext;
    }

    [HttpGet]
    public async Task<List<ToDoItem>> List()
    {
        var items = await toDoDbContext.ToDoItems.Where(i => i.UserId == _sessionContext.UserId).OrderBy(i => i.Order).ToListAsync();
        return items;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddToDoItemRequest requestItem)
    {
        if (requestItem == null)
            return BadRequest();

        double nextOrder;
        try
        {
            nextOrder = await toDoDbContext.ToDoItems.MaxAsync(i => i.Order) + 1;
        }
        catch (System.InvalidOperationException)
        {
            //If there is no data it will throw no sequence exception and we set nextOrder to 1
            nextOrder = 1;
        }

        var newToDo = mapper.Map<ToDoItem>(requestItem);
        newToDo.Order = nextOrder;
        newToDo.UserId = _sessionContext.UserId;
        toDoDbContext.ToDoItems.Add(newToDo);

        await toDoDbContext.SaveChangesAsync();

        return Ok(new AddToDoItemResponse { Id = newToDo.Id });
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateToDoItemRequest request)
    {
        if (request == null || request.Id <= 0 || string.IsNullOrWhiteSpace(request.Title))
            return BadRequest();

        var item = await toDoDbContext.ToDoItems.FirstOrDefaultAsync(i => i.Id == request.Id);
        if (item == null)
        {
            return BadRequest();
        }
        else if (item.UserId != _sessionContext.UserId)
        {
            //todo: return forbid when we set proper authentication
            return StatusCode(403);
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

    [HttpPost]
    [Route("{id}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        //todo: check userid for security
        var item = await toDoDbContext.ToDoItems.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null)
        {
            return BadRequest();
        }
        item.Completed = true;

        await toDoDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    [Route("reorder")]
    public async Task<IActionResult> Reorder(ReorderToDoItemsRequest request)
    {
        //todo: check userid for security
        var insertBeforeItem = await toDoDbContext.ToDoItems.FirstOrDefaultAsync(i => i.Id == request.InsertBeforeId);
        var insertAfterItem = await toDoDbContext.ToDoItems.Where(i => i.Order < insertBeforeItem.Order)
                                                           .OrderByDescending(i => i.Order)
                                                           .FirstOrDefaultAsync();
        var lowerOrder = insertAfterItem != null ? insertAfterItem.Order : 0;
        double higherOrder;
        if (insertBeforeItem != null) higherOrder = insertBeforeItem.Order;
        else
        {
            higherOrder = await toDoDbContext.ToDoItems.MaxAsync(i => i.Order) + 1;
        }

        var delta = (higherOrder - lowerOrder) / (request.TodoIds.Count + 1);
        var multiplicator = 1;
        foreach (var itemId in request.TodoIds)
        {
            var item = await toDoDbContext.ToDoItems.FirstOrDefaultAsync(i => i.Id == itemId);
            item.Order = lowerOrder + (delta * multiplicator);
            multiplicator++;
        }
        await toDoDbContext.SaveChangesAsync();

        return Ok();
    }
}