<template>
  <article class="prose lg:prose-lg mx-auto mt-12 p-4">
    <h1>Статья {{ article }}</h1>
    <p>Маршрут: /courses/{{ course }}/{{ module }}/{{ article }}</p>
    <p>Тут будет контент.</p>
  </article>
</template>

<script setup lang="ts">
const { course, module, article } = useRoute().params
</script>