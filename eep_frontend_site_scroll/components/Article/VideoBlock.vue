<template>
  <div class="flex justify-center my-8">
    <video controls :src="block.src" class="max-w-xl rounded-lg shadow"></video>
  </div>
</template>
<script setup lang="ts">
const props = defineProps<{ block: any }>()
</script>
