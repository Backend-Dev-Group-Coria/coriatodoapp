using System.Net.Http;
using CoriaToDo.API.Data;

namespace CoriaToDo.API.Tests
{
    public class TestFixture
    {
        HttpClient _httpClient;
        ToDoDbContext _dbContext;

        [Fact]
        public void AddToDoAddsRowToDB()
        {
            // Given JSON { title }
            // When POST /ToDo is called 
            // Then new DB row exists and returns OK()
        }
    }
}