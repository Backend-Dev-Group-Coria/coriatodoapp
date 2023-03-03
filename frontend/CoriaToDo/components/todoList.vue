<template>
  <div>
    <h1>List Name</h1>
    <ul v-if="todoItems && todoItems.length">
      <li v-for="item in todoItems" :key="item.id" :class="{ underline: item.completed }">{{ item.title }}</li>
    </ul>
    <input v-model="newTodoItemTitle" placeholder="Add new..." @keyup.enter="addNew" />
  </div>
</template>

<script setup lang="ts">
import { Ref, ref, onMounted } from 'vue';
import { TodoItem } from '../models/todo-item';
import todoApi from '../api/todo.api'

const todoItems: Ref<TodoItem[]> = ref([])
const newTodoItemTitle: Ref<string> = ref("")


onMounted(() => {
  getToDoItems()
})

async function addNew()
{
  const item = await todoApi.addItem(newTodoItemTitle.value)
  if (!item) return 
  item.title = newTodoItemTitle.value
  todoItems.value.push(item)
  newTodoItemTitle.value = ''
}

async function getToDoItems() {
  todoItems.value = await todoApi.getToDoItems()
}

</script>

<style>
.underline {
  text-decoration: underline;
}
</style>
