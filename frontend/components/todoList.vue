<template>
  <div>
    <h1>List Name</h1>
    <ul v-if="todoItems && todoItems.length">
      <li
        v-for="item in todoItems"
        :key="item.id"
        :class="{ underline: item.completed }"
      >
        <input
          ref="editField"
          v-if="item.isEditing"
          :value="item.title"
          @blur="onEditingItem(item, false)"
          @change="test(item,$event)"
        />
        <span v-else @dblclick="onEditingItem(item, true)">{{
          item.title
        }}</span>

        <input
          v-model="item.completed"
          type="checkbox"
          :disabled="item.completed"
          @input="completeItem(item.id)"
        />
        <button @click="deleteItem(item.id)">Delete</button>
      </li>
    </ul>
    <input
      v-model="newTodoItemTitle"
      placeholder="Add new..."
      @keyup.enter="addNew"
    />
  </div>
</template>

<script type="module" setup lang="ts">
import { Ref, ref, onMounted, nextTick } from "vue";
import { TodoItem } from "../models/todo-item";
import todoApi from "../api/todo.api";

const todoItems: Ref<TodoItem[]> = ref([]);
const newTodoItemTitle: Ref<string> = ref("");

onMounted(() => {
  getToDoItems();
});

async function addNew() {
  const item = await todoApi.addItem(newTodoItemTitle.value);
  if (!item) return;
  item.title = newTodoItemTitle.value;
  todoItems.value.push(item);
  newTodoItemTitle.value = "";
}

async function getToDoItems() {
  todoItems.value = await todoApi.getToDoItems();
}

function test(item, event) {
  if (item.title !== event.target.value) {
    console.log('Saving update')
    item.title = event.target.value
    updateItem(item)
  }
}

async function deleteItem(itemId: number) {
  await todoApi.deleteItem(itemId);
  let i = todoItems.value.findIndex((e) => e.id === itemId);
  if (i >= 0) {
    todoItems.value.splice(i, 1);
  }
}

async function completeItem(itemId: number) {
  await todoApi.completeItem(itemId);
}

const editField: any = ref(null);

function onEditingItem(item: TodoItem, edit: boolean) {
  item.isEditing = edit;
  if (!edit) {
    return;
  }
  nextTick(() => {
    editField.value[0].focus();
  });
}

async function updateItem(item: TodoItem) {
  await todoApi.updateItem(item);
  item.isEditing = false;
}
</script>

<style>
.underline {
  text-decoration: underline;
}
</style>
