namespace CoriaToDo.API.Feature.Todo.Services;

public class SessionContext
{
    private IHttpContextAccessor _httpContextAccessor;

    public SessionContext(IHttpContextAccessor httpContextAccessor) 
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get { return 1; }
    }
}
