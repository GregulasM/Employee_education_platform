<template>
  <div class="flex justify-center my-4">
    <audio controls :src="block.src" class="w-full max-w-lg"></audio>
  </div>
</template>
<script setup lang="ts">
const props = defineProps<{ block: any }>()
</script>