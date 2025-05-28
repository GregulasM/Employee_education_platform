<template>
  <h2 v-if="block.level === 2" :id="`heading-${idx}`" class="font-bold mt-8 mb-3 scroll-mt-28 text-black text-shadow">
    {{ block.text }}
  </h2>
  <h3 v-else-if="block.level === 3" :id="`heading-${idx}`" class="font-bold mt-8 mb-3 scroll-mt-28 text-black text-shadow">
    {{ block.text }}
  </h3>
  <h4 v-else :id="`heading-${idx}`" class="font-bold mt-8 mb-3 scroll-mt-28 text-black text-shadow">
    {{ block.text }}
  </h4>
</template>

<script setup lang="ts">
const props = defineProps<{ block: any, idx: number }>()
</script>
