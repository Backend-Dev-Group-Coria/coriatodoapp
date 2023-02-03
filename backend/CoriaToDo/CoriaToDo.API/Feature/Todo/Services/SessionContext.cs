namespace CoriaToDo.API.Feature.Todo.Services;

public class SessionContext
{
    private IHttpContextAccessor _httpContextAccessor;
    private int _userId = 1;

    public SessionContext(IHttpContextAccessor httpContextAccessor) 
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get { return _userId; }
        set { _userId = value; }
    }
}
