using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Model;
using FluentAssertions;
using System.Net.Http.Json;

namespace CoriaToDo.API.Tests
{
    public class ToDoTest : IClassFixture<TestFixture>
    {
        TestFixture _testFixture;

        public ToDoTest(TestFixture fixture)
        {
            _testFixture = fixture;
        }

        [Fact]
        public async Task AddToDoAddsRowToDB()
        {
            // Given JSON { title }
            var newTodo = new AddToDoItemRequest
            {
                Title = "Test"
            };
            // When POST /ToDo is called 
            var response = await _testFixture.HttpClient.PostAsJsonAsync("api/Todo", newTodo);

            // Then new DB row exists and returns OK()
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<AddToDoItemResponse>();
            result.Id.Should().BeGreaterThan(0);

        }

        [Fact]
        public async Task ToDoShouldReturnOk()
        {
            var response = await _testFixture.HttpClient.GetAsync("api/Todo");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}