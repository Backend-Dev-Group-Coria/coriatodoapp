<template>
  <div>
    <h1>List Name</h1>
    <ul v-if="todoItems && todoItems.length">
      <li v-for="item in todoItems" :key="item.id" :class="{ underline: item.completed }">{{ item.title }}</li>
    </ul>
    <input v-model="newTodoItemTitle" placeholder="Add new..." @input="onInput" @keyup.enter="onEnter" />
  </div>
</template>

<script setup lang="ts">
import { Ref, ref, onMounted } from 'vue';
import { TodoItem } from '../models/todo-item';
import todoApi from '../api/todo.api'

const todoItems: Ref<TodoItem[]> = ref([])
const newTodoItemTitle: Ref<string> = ref("")


onMounted(async () => {
  todoItems.value = await todoApi.getToDoItems()
})

function onInput()
{
  console.log(newTodoItemTitle.value)
}

function onEnter()
{
  console.log('Miro je pritisnul enter.')
}

</script>

<style>
.underline {
  text-decoration: underline;
}
</style>
