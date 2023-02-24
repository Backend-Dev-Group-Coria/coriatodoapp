<template>
  <div>
    <h1>CoriaToDo App</h1>
    <ul>
      <li v-for="item in todoItems" :key="item.id">{{ item.title }}</li>
    </ul>
  </div>
</template>

<script setup lang="ts">
const { data: login } = await useFetch<any>("http://localhost:5067/api/auth/login/0",{method: 'POST'});
const { data: todoItems } = useFetch<any>("http://localhost:5067/api/Todo",);
console.log(todoItems);
</script>
