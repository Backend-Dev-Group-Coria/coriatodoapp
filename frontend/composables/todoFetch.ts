export const getTodo = (request: any, opts?: any) => {
    const config = useRuntimeConfig()
    return useFetch(request, { baseURL: config.public.baseURL, ...opts })
}

export const postTodo = (request: any, body: any = '') => {
    return requestTodo(request, 'POST', body)
}

export const putTodo = (request: any, body: any = '') => {
    return requestTodo(request, 'PUT', body)
}

export const deleteTodo = (request: any, body: any = '') => {
    return requestTodo(request, 'DELETE', body)
}

const requestTodo = (request: any, method: string, body: any) => {
    const config = useRuntimeConfig()
    return fetch(config.public.baseURL + request, {
        method: method,
        body: JSON.stringify(body),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    })
    .catch((error) => {
        console.log(error);
        return null
    });
}