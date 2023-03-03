import { TodoItem } from '../models/todo-item'

class TodoApi
{
    async getToDoItems() 
    {
        const { data: items } = await useFetch<TodoItem[]>("http://localhost:5067/api/Todo",)
        return items.value ?? []
    }

    async addItem(title: string)
    {
        return fetch("http://localhost:5067/api/Todo",{
            method: 'POST',
            body: JSON.stringify({
                title
            }),
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }
        })
        .then((res) => res.json())
        .then((data: TodoItem) => {
            return data
        })
        .catch((error)=> {
            console.log(error);
            return null
        });
    }
}
export default new TodoApi()
