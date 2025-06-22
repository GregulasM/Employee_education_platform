<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Тесты</h2>
    </div>

    <div class="p-4 space-y-4 font-semibold text-shadow-lg text-black">
      <div v-if="store.loading" class="text-center text-lg text-red-400">Загрузка...</div>
      <div v-if="store.error" class="text-center text-lg text-red-600">{{ store.error }}</div>

      <div v-if="accordionItems.length === 0" class="text-center text-sm opacity-60 mt-8">Нет тестов</div>

      <div
          v-for="item in accordionItems"
          :key="item.value"
          class="collapse collapse-arrow border-b border-red-500/50 cursor-pointer bg-white rounded-lg"
      >
        <input type="checkbox" :name="'accordion-'+item.value" />
        <div class="collapse-title font-semibold flex items-center gap-2">
          <span>Тесты из курса <b>{{ item.courseTitle }}</b></span>
        </div>
        <div class="collapse-content text-sm space-y-3">
          <div v-for="test in item.tests" :key="test.id" class="flex flex-col gap-1 mb-2">
            <NuxtLink
                :to="`/tests/${test.id}`"
                class="text-black hover:underline font-semibold flex items-center gap-2"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24">
                <path
                    fill="none"
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2.5"
                    d="M11.1 3.002c-3.648.007-5.56.096-6.78 1.317C3.002 5.637 3.002 7.758 3.002 12s0 6.363 1.318 7.681s3.438 1.318 7.68 1.318s6.363 0 7.681-1.318c1.221-1.22 1.31-3.132 1.317-6.78m-.518-9.384l-5.548 5.534m5.549-5.534c-.494-.494-3.822-.448-4.525-.438m4.525.438c.494.495.448 3.826.438 4.53"
                    color="currentColor"
                />
              </svg>
              {{ test.title }}
            </NuxtLink>
            <span v-if="test.description" class="text-xs opacity-80 ml-6">{{ test.description }}</span>
            <span v-if="test.module" class="ml-6 text-xs text-orange-500">Модуль: {{ test.module.title }}</span>
          </div>
        </div>
      </div>
    </div>

    <div class="mt-8"></div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useTestsStore } from '@/stores/tests_store'
import { useCoursesStore } from '@/stores/courses_store'

const store = useTestsStore()
const coursesStore = useCoursesStore()


const courseIdToTitle = computed(() => {
  const map: Record<number, string> = {}
  for (const course of coursesStore.allCourses) {
    map[course.id] = course.title
  }
  return map
})


const accordionItems = computed(() => {

  const courseIds = Array.from(new Set(store.tests.map(t => t.courseId)))
  return courseIds.map(courseId => ({
    value: `course-${courseId}`,
    courseTitle: courseIdToTitle.value[courseId] || 'Неизвестный курс',
    tests: store.tests.filter(t => t.courseId === courseId)
  }))
})


onMounted(async () => {
  if (!coursesStore.allCourses.length) {
    await coursesStore.fetchAll()
  }
  await store.fetchCourses()
  await store.fetchTests()
})
</script>
