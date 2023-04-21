<template>
  <div>
    <h1>CoriaToDo App</h1>
    <todoList></todoList>
  </div>
</template>

<script setup lang="ts">
import { Ref, ref, onMounted } from 'vue';
import todoList from './components/todoList.vue';

onMounted(() => {
  login(1)
})

async function login(id: number) {
  await useFetch(`http://localhost:5067/api/auth/login/${id}`, { method: "POST"});
}

</script>

