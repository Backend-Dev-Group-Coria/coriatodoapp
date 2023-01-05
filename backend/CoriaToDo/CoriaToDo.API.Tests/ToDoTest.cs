using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Model;
using FluentAssertions;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace CoriaToDo.API.Tests
{
    public class ToDoTest : IClassFixture<TestFixture>
    {
        TestFixture _testFixture;
        private int _userId = 1;
        private int _testItemId;

        ToDoDbContext _dbContext;
        HttpClient _httpClient;

        public ToDoTest(TestFixture fixture)
        {
            _testFixture = fixture;
            _dbContext = fixture.DbContext;
            _httpClient = fixture.HttpClient;

            InitilizeData();
        }

        private void InitilizeData()
        {
            var testItem = new ToDoItem
            {
                Title = "Test 1",
                CreatedDate = DateTime.UtcNow,
                UserId = _userId
            };
            _testFixture.DbContext.ToDoItems.Add(testItem);
            _testFixture.DbContext.SaveChanges();
            _testItemId = testItem.Id;
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

        [Fact]
        public async Task UpdateToDoUpdatesTask()
        {
            // Given JSON { title }
            var updateTodo = new UpdateToDoItemRequest
            {
                Id = _testItemId,
                Title = "Test 2",
            };
            // when PUT is called
            var response = await _testFixture.HttpClient.PutAsJsonAsync("api/Todo", updateTodo);

            // Then item should change

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _testFixture.DbContext.ToDoItems.AsNoTracking().FirstOrDefault(i => i.Id == _testItemId).Title.Should().Be("Test 2");

        }

        [Fact]
        public async Task CompleteToDoSetTaskCompleted()
        {
            // when POST is called
            var response = await _testFixture.HttpClient.PostAsync($"api/Todo/{_testItemId}/complete", null);

            // Then item should be set to completed

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _testFixture.DbContext.ToDoItems.AsNoTracking().FirstOrDefault(i => i.Id == _testItemId).Completed.Should().BeTrue();

        }
    }
}