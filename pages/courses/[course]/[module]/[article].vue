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
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const { module, article } = route.params

/* данные статьи */
const moduleTitle  = ref('')
const articleTitle = ref('')

/* контент tiptap */
const tiptapContent = ref(null)

/* дата публикации / обновления */
const createdAt  = ref<string | null>(null)
const updatedAt  = ref<string | null>(null)

/* красиво форматируем дату */
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

onMounted(async () => {
  try {
    const encoded = encodeURIComponent(article as string)
    const res: any = await $fetch(
        `http://localhost:5148/api/admin_panel/modules/${module}/articles/${encoded}`
    )

    moduleTitle.value  = res.moduleTitle || 'Без названия'
    articleTitle.value = res.title       || 'Статья'

    createdAt.value = res.createdAt   || null
    updatedAt.value = res.updatedAt   || null

    /* контент */
    if (typeof res.content === 'string') {
      tiptapContent.value = JSON.parse(res.content)
    } else {
      tiptapContent.value = res.content
    }
  } catch (e) {
    articleTitle.value = 'Статья не найдена'
    tiptapContent.value = null
  }
})
</script>
