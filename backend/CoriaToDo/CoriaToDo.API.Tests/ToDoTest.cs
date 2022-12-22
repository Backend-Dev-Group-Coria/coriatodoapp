namespace CoriaToDo.API.Tests
{
    public class ToDoTest : IClassFixture<TestFixture>
    {
        TestFixture _testFixture;

        public ToDoTest(TestFixture fixture) {
            _testFixture = fixture;
        }

        [Fact]
        public void AddToDoAddsRowToDB()
        {            
            // Given JSON { title }
            // When POST /ToDo is called 
            // Then new DB row exists and returns OK()
        }

        [Fact]
        public void ToDoShouldReturnList()
        {
            var result = _testFixture.HttpClient.GetStringAsync("api/Todo").Result;
        }
    }
}