<template>
  <p class="my-4 text-base leading-relaxed text-black">{{ block.text }}</p>
</template>
<script setup lang="ts">
const props = defineProps<{ block: any }>()
</script>
