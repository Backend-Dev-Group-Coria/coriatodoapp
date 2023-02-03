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
        private ToDoItem _testItem;

        ToDoDbContext _dbContext;
        HttpClient _httpClient;

        public ToDoTest(TestFixture fixture)
        {
            _testFixture = fixture;
            _dbContext = fixture.DbContext;
            _httpClient = fixture.HttpClient;

            InitializeData(5);
        }

        private void InitializeData(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var testItem = new ToDoItem
                {
                    Title = $"Test {i}",
                    CreatedDate = DateTime.UtcNow,
                    UserId = _userId,
                    Order = i + 1
                };

                _testFixture.DbContext.ToDoItems.Add(testItem);
                _testItem = testItem;
            }

            _testFixture.DbContext.SaveChanges();
            _testItemId = _testItem.Id;

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
            _testFixture.DbContext.ToDoItems.FirstOrDefault(i => i.Id == result.Id).Order.Should().Be(_testItem.Order + 1);
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
                Title = "Test Updated",
            };
            // When PUT is called
            var response = await _testFixture.HttpClient.PutAsJsonAsync("api/Todo", updateTodo);

            // Then item should change

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _testFixture.DbContext.ToDoItems.AsNoTracking().FirstOrDefault(i => i.Id == _testItemId).Title.Should().Be("Test Updated");

        }

        [Fact]
        public async Task CompleteToDoSetTaskCompleted()
        {
            // When POST is called
            var response = await _testFixture.HttpClient.PostAsync($"api/Todo/{_testItemId}/complete", null);

            // Then item should be set to completed

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _testFixture.DbContext.ToDoItems.AsNoTracking().FirstOrDefault(i => i.Id == _testItemId).Completed.Should().BeTrue();

        }

        [Fact]
        public async Task TaskReorderToDo()
        {
            // Given TODO Items
            var allTodoItems = _testFixture.DbContext.ToDoItems.ToList();

            // When reorder items is called 
            var reoderToDo = new ReorderToDoItemsRequest
            {
                InsertBeforeId = allTodoItems[1].Id,
                TodoIds = new List<int> { allTodoItems[3].Id, allTodoItems[4].Id }
            };

            var reorderResponse = await _testFixture.HttpClient.PostAsJsonAsync("api/Todo/reorder", reoderToDo);

            // Then list should be reordered
            var listResponse = await _testFixture.HttpClient.GetAsync("api/Todo");
            var todoList = await listResponse.Content.ReadFromJsonAsync<List<ToDoItem>>();
            var expectedIdslist = new List<int> { allTodoItems[0].Id, allTodoItems[3].Id, allTodoItems[4].Id, allTodoItems[1].Id, allTodoItems[2].Id };
            var currentIdsList = todoList.Select(t => t.Id).ToList();

            currentIdsList.Should().BeEquivalentTo(expectedIdslist, o => o.WithStrictOrdering());
        }
    }
}