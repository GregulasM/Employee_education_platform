<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-auto max-w-3xl rounded-lg bg-orange-50 opacity-95">
    <div class="flex justify-between items-center text-xl font-bold text-white bg-red-500/50 p-4 rounded-t-lg">
      <h2>{{ moduleTitle }} / {{ articleTitle }}</h2>
      <span class="text-xs">Маршрут: /courses/{{ course }}/{{ module }}</span>
    </div>
    <div class="p-6 space-y-10">
      <TableOfContents :blocks="blocks" />
      <component
          v-for="(block, i) in blocks"
          :key="i"
          :is="resolveComponent(block.type)"
          :block="block"
          :idx="i"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import HeadingBlock from '@/components/Article/HeadingBlock.vue'
import ParagraphBlock from '@/components/Article/ParagraphBlock.vue'
import ImageBlock from '@/components/Article/ImageBlock.vue'
import SliderBlock from '@/components/Article/SliderBlock.vue'
import VideoBlock from '@/components/Article/VideoBlock.vue'
import AudioBlock from '@/components/Article/AudioBlock.vue'
import QuoteBlock from '@/components/Article/QuoteBlock.vue'
import TableOfContents from '@/components/Article/TableOfContents.vue'

const { course, module, article } = useRoute().params

// Заглушка для названия
const moduleTitle = 'Введение'
const articleTitle = 'Установка и настройка'

// Пока без API: просто импортируй json с блоками
const blocks = ref([])

onMounted(async () => {
  // Можешь заменить fetch на API-запрос из SurrealDB, когда будет готово
  const data = await $fetch('/data/article-example.json')
  blocks.value = data
})

function resolveComponent(type: string) {
  switch (type) {
    case 'heading': return HeadingBlock
    case 'paragraph': return ParagraphBlock
    case 'image': return ImageBlock
    case 'slider': return SliderBlock
    case 'video': return VideoBlock
    case 'audio': return AudioBlock
    case 'quote': return QuoteBlock
    default: return 'div'
  }
}
</script>
