<template>
  <figure class="my-6 flex flex-col items-center">
    <img :src="block.src" :alt="block.alt" class="rounded-xl shadow max-w-full" />
    <figcaption v-if="block.caption" class="text-xs text-gray-500 mt-2">{{ block.caption }}</figcaption>
  </figure>
</template>
<script setup lang="ts">
const props = defineProps<{ block: any }>()
</script>
