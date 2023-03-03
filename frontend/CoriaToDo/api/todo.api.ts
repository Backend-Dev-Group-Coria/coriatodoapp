import { TodoItem } from '../models/todo-item'

class TodoApi
{
    async getToDoItems() 
    {
        const { data: items } = await useFetch<TodoItem[]>("http://localhost:5067/api/Todo",)
        return items.value ?? []
    }
}
export default new TodoApi()
