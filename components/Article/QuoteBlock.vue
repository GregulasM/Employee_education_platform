<template>
  <blockquote class="border-l-4 border-red-400 bg-orange-100 p-4 rounded-xl my-6">
    <p class="italic text-gray-700">“{{ block.text }}”</p>
    <footer v-if="block.author" class="text-xs text-right text-gray-500 mt-2">— {{ block.author }}</footer>
  </blockquote>
</template>
<script setup lang="ts">
const props = defineProps<{ block: any }>()
</script>
