<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg">
      <h2>{{ course?.title }}</h2>
    </div>

    <div class="p-4">
      <div v-if="loading" class="text-lg text-red-400">Загрузка...</div>
      <div v-if="error" class="text-lg text-red-600">{{ error }}</div>

      <div v-if="course" v-for="m in course.modules" :key="m.id"
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
import { computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useCoursesStore } from '@/stores/courses_store'

const route = useRoute()
const router = useRouter()
const store = useCoursesStore()
const { loading, error, allCourses } = storeToRefs(store)

const courseSlug = computed(() => route.params.course as string)


const course = computed(() =>
    store.findCourseBySlug(courseSlug.value)
)

function openArticle(moduleId: number, articleSlug: string) {
  const module = course.value?.modules.find(m => m.id == moduleId)
  if (!module) return
  const art = module.articles.find(a => a.slug == articleSlug)
  if (!art) return
  router.push(`/courses/${courseSlug.value}/${moduleId}/${encodeURIComponent(art.title)}`)
}


onMounted(async () => {
  if (!allCourses.value.length) {
    await store.fetchAll()
  }
})
</script>