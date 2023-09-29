import { deleteTodo, getTodo, postTodo, putTodo } from '../composables/todoFetch';
import { TodoItem } from '../models/todo-item'

class TodoApi {
    async getToDoItems() {
        const { data: items } = await getTodo("Todo")
        return items.value ?? []
    }

    async addItem(title: string) {
        return postTodo("Todo", { title })
            .then((res) => res!.json())
            .then((data: TodoItem) => {
                return data
            })
            .catch((error) => {
                console.log(error);
                return null
            });
    }

    async updateItem(item: TodoItem) {
        return putTodo(`Todo`, { id: item.id, title: item.title })
    }

    async deleteItem(itemId: number) {
        return deleteTodo(`Todo/${itemId}`)
    }

    async completeItem(itemId: number) {
        return postTodo(`Todo/${itemId}/complete`)
    }
}
export default new TodoApi()
