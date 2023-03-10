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

    async updateItem(item: TodoItem)
    {
        return this.sendRequest(`http://localhost:5067/api/Todo`, 'PUT', JSON.stringify({ id: item.id, title: item.title }))
    }

    async deleteItem(itemId: number)
    {
        return this.sendRequest(`http://localhost:5067/api/Todo/${itemId}`, 'DELETE', '')
    }

    async completeItem(itemId: number) {
        return this.sendRequest(`http://localhost:5067/api/Todo/${itemId}/complete`,'POST', '')
    }

    sendRequest(url: string, method: string, body: string) {
        return fetch(url, {
            method: method,
            body: body,
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }
        })
        .catch((error) => {
            console.log(error);
            return null
        });

    }
}
export default new TodoApi()
