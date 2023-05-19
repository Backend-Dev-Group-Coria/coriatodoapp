using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Model;

namespace CoriaToDo.API.Feature.Todo;

public class ToDoMapper : AutoMapper.Profile
{
    public ToDoMapper()
    {
        CreateMap<AddToDoItemRequest, ToDoItem>();
        CreateMap<ToDoItem, AddToDoItemResponse>();
    }
}