<template>
  <div>
    <h1>List Name</h1>
    <ul v-if="todoItems && todoItems.length">
      <li v-for="item in todoItems" :key="item.id" :class="{ underline: item.completed }">{{ item.title }}</li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { Ref, ref, onMounted } from 'vue';
import { TodoItem } from '../models/todo-item';

const todoItems: Ref<TodoItem[]> = ref([])

onMounted(() => {
  getToDoItems()
})


async function getToDoItems() {
  const { data: items } = await useFetch<TodoItem[]>("http://localhost:5067/api/Todo",);
  todoItems.value = items.value ?? []
}
</script>

<style>
.underline {
  text-decoration: underline;
}
</style>
