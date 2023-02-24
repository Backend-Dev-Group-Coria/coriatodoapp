<template>
  <div>
    <h1>CoriaToDo App</h1>
    <ul v-if="todoItems">
      <li v-for="item in todoItems" :key="item.id">{{ item.title }}</li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { Ref, ref, onMounted } from 'vue';

const todoItems: Ref<any> = ref([])

onMounted(() => {
  login(1)
  getToDoItems()
})

async function login(id: number) {
  await useFetch<any>(`http://localhost:5067/api/auth/login/${id}`, {method: 'POST'});
}

async function getToDoItems() {
  const { data: items } = useFetch<any>("http://localhost:5067/api/Todo",);
  todoItems.value = items.value
}
</script>
