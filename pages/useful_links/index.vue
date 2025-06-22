<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Полезные ссылки</h2>
    </div>

    <div class="p-4 space-y-4 font-semibold text-shadow-lg text-black">
      <div v-if="loading" class="text-center text-lg text-red-400">Загрузка...</div>
      <div v-if="error" class="text-center text-lg text-red-600">{{ error }}</div>

      <div v-if="accordionItems.length === 0" class="text-center text-sm opacity-60 mt-8">Нет полезных ссылок</div>
      <div v-for="item in accordionItems" :key="item.value" class="collapse collapse-arrow border-b border-red-500/50 cursor-pointer bg-white rounded-lg">
        <input type="checkbox" :name="'accordion-'+item.value" />
        <div class="collapse-title font-semibold flex justify-between items-center">
          <span>{{ item.title }}</span>
          <span v-if="item.courseTitle" class="text-xs text-red-500/50 font-normal ml-2">Из курса <b>{{ item.courseTitle }}</b></span>
        </div>
        <div class="collapse-content text-sm space-y-3">
          <div v-for="link in item.links" :key="link.id" class="flex flex-col gap-1 mb-2">
            <a :href="link.url" target="_blank" class="text-black hover:underline font-semibold font-medium flex items-center gap-2">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><!-- Icon from Huge Icons by Hugeicons - undefined --><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M11.1 3.002c-3.648.007-5.56.096-6.78 1.317C3.002 5.637 3.002 7.758 3.002 12s0 6.363 1.318 7.681s3.438 1.318 7.68 1.318s6.363 0 7.681-1.318c1.221-1.22 1.31-3.132 1.317-6.78m-.518-9.384l-5.548 5.534m5.549-5.534c-.494-.494-3.822-.448-4.525-.438m4.525.438c.494.495.448 3.826.438 4.53" color="currentColor"/></svg>
              {{ link.title }}
            </a>
            <span v-if="link.description" class="text-xs opacity-80 ml-6">{{ link.description }}</span>
            <div v-if="link.tags && link.tags.length" class="ml-6 flex flex-wrap gap-1 mt-1">
              <span v-for="tag in link.tags" :key="tag" class="px-2 py-0.5 rounded bg-orange-200 text-500/70 text-xs">{{ tag }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="mt-8"></div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useUsefulLinksStore } from '@/stores/useful_links_store'
import { useCoursesStore } from '@/stores/courses_store'

const store = useUsefulLinksStore()

const coursesStore = useCoursesStore()

const courseIdToTitle = computed(() => {
  const map = {}
  for (const course of coursesStore.allCourses) {
    map[course.id] = course.title
  }
  return map
})

const accordionItems = computed(() => {
  const parents = store.links.filter(l => l.parentId === null)
  return parents.map(parent => {
    const childLinks = store.links.filter(l => l.parentId === parent.id)
    return {
      value: `link-parent-${parent.id}`,
      title: parent.title,
      courseTitle: courseIdToTitle.value[parent.courseId!] || '',
      links: childLinks.length ? childLinks : []
    }
  }).filter(item => item.links.length > 0)
})

onMounted(async () => {
  if (!coursesStore.allCourses.length) {
    await coursesStore.fetchAll()
  }
  await store.fetchLinks()
})
</script>
