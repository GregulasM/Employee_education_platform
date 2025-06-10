<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg">
      <h2>{{ course.title }}</h2>
    </div>

    <div class="p-4">
      <div v-if="loading" class="text-lg text-red-400">Загрузка...</div>
      <div v-if="error" class="text-lg text-red-600">{{ error }}</div>

      <div v-for="m in course.modules" :key="m.id"
           class="collapse collapse-arrow bg-white border border-red-500/50 rounded-lg mb-4">
        <input type="checkbox">
        <div class="collapse-title font-semibold flex items-center gap-4">
          <img :src="m.icon" class="w-8 h-8 rounded-box object-cover">
          {{ m.num }}. {{ m.title }}
        </div>

        <div class="collapse-content">
          <ul class="space-y-2">
            <li v-for="a in m.articles" :key="a.slug"
                class="flex items-center gap-2 inset-shadow-sm inset-shadow-red-500/50 p-2 rounded-lg">
              <span class="text-xs opacity-60 w-12">{{ a.num }}</span>
              <button class="link-hover flex-1 text-left" @click="openArticle(m.id, a.slug)">
                {{ a.title }}
              </button>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const courseSlug = route.params.course as string

const course = reactive({
  title: '',
  modules: [] as any[]
})
const loading = ref(true)
const error = ref<string | null>(null)

function openArticle(moduleId, articleSlug) {

  const mod = course.modules.find(m => m.id == moduleId)
  if (!mod) return
  const art = mod.articles.find(a => a.slug == articleSlug)
  if (!art) return
  router.push(`/courses/${route.params.course}/${moduleId}/${encodeURIComponent(art.title)}`)
}
onMounted(async () => {
  loading.value = true
  error.value = null
  try {
    const courseRes = await $fetch(`http://localhost:5148/api/courses`)
    const courseData = (courseRes as any[]).find(
        c => c.publicId === courseSlug || c.title?.toLowerCase().replace(/\s+/g, '-') === courseSlug
    )
    if (!courseData) throw new Error('Курс не найден')

    course.title = courseData.title

    const modulesRes = await $fetch('http://localhost:5148/api/modules')
    const modulesOfCourse = (modulesRes as any[])
        .filter(m => m.courseId === courseData.id)
        .sort((a, b) => (a.order ?? 0) - (b.order ?? 0))

    const modulesWithArticles = await Promise.all(
        modulesOfCourse.map(async (mod, idx) => {
          let articles: any[] = []
          try {
            articles = await $fetch(`http://localhost:5148/api/modules/${mod.id}/articles`)
          } catch { }
          return {
            id: mod.id,
            num: idx + 1,
            title: mod.title,
            icon: mod.image || '/mascot/mascot.png',
            articles: (articles || []).map((art, idx2) => ({
              slug: art.id || art.title?.toLowerCase().replace(/\s+/g, '-'),
              num: `${idx + 1}.${idx2 + 1}`,
              title: art.title
            }))
          }
        })
    )
    course.modules = modulesWithArticles
  } catch (e: any) {
    error.value = e?.message || 'Ошибка загрузки'
  }
  loading.value = false
})
</script>
