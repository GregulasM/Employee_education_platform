<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-auto rounded-lg bg-orange-50 opacity-95">
    <div class="flex justify-between items-center text-xl font-bold text-white bg-red-500/50 p-4 rounded-t-lg">
      <h2>{{ moduleTitle }} : {{ articleTitle }}</h2>
      <span class="text-xs opacity-80">
        {{ publishedDate }}
      </span>
    </div>
    <div class="p-6 space-y-10">
      <TiptapViewer v-if="tiptapContent" :content="tiptapContent" />
      <div v-else-if="loading" class="text-center text-lg text-red-400">Загрузка...</div>
      <div v-else-if="error" class="text-center text-lg text-red-600">{{ error }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useArticlesStore } from '@/stores/articles_store'
import { storeToRefs } from 'pinia'

const route = useRoute()
const { module, article } = route.params

const store = useArticlesStore()
const { article: currentArticle, loading, error } = storeToRefs(store)

onMounted(async () => {
  await store.fetchArticle(module as string, article as string)
})

const moduleTitle  = computed(() => currentArticle.value?.moduleTitle || 'Без названия')
const articleTitle = computed(() => currentArticle.value?.title || 'Статья')
const tiptapContent = computed(() => currentArticle.value?.content ?? null)
const createdAt  = computed(() => currentArticle.value?.createdAt ?? null)
const updatedAt  = computed(() => currentArticle.value?.updatedAt ?? null)

const publishedDate = computed(() => {
  const src = updatedAt.value || createdAt.value
  if (!src) return ''
  const d = new Date(src)
  return d.toLocaleDateString('ru-RU', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
})
</script>
