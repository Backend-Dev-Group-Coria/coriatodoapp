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
import todoApi from '../api/todo.api'

const todoItems: Ref<TodoItem[]> = ref([])

onMounted(async () => {
  todoItems.value = await todoApi.getToDoItems()
})

</script>

<style>
.underline {
  text-decoration: underline;
}
</style>
