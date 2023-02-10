<template>
  <div>
    <h1>CoriaToDo App</h1>
    <ul>
      <li v-for="item in todoItems" :key="item.id">{{ item.title }}</li>
    </ul>
  </div>
</template>

<script setup>
const { data: todoItems } = useFetch("http://localhost:5067/api/Todo",);
</script>



