<template>
  <div class="carousel w-full my-6">
    <div v-for="(img, i) in block.images" :key="i" class="carousel-item relative w-full">
      <img :src="img.src" :alt="img.alt" class="w-full h-60 object-cover rounded-xl shadow" />
    </div>
  </div>
</template>
<script setup lang="ts">
const props = defineProps<{ block: any }>()
</script>
