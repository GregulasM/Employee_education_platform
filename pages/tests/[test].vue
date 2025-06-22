<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-auto rounded-lg bg-orange-50 opacity-95 max-w-3xl">
    <div class="flex justify-between items-center text-xl font-bold text-white bg-red-500/50 p-4 rounded-t-lg">
      <h2>
        {{ testTitle }}
        <span v-if="courseTitle" class="text-sm font-normal opacity-80 ml-2">
          (Курс: {{ courseTitle }})
        </span>
      </h2>
    </div>
    <div class="p-6 space-y-10">
      <div v-if="test">
        <div v-if="test.description" class="mb-6 text-base text-gray-800">
          {{ test.description }}
        </div>
        <!-- Здесь будет отображение вопросов (пока как текст) -->
        <div v-if="questions && questions.length" class="space-y-8">
          <div
              v-for="(q, idx) in questions"
              :key="idx"
              class="bg-orange-100/70 p-4 rounded-lg shadow"
          >
            <div class="font-semibold mb-2">
              Вопрос {{ idx + 1 }}: {{ q.question }}
            </div>
            <ul class="list-disc ml-6 space-y-1">
              <li v-for="(option, oidx) in q.options" :key="oidx">
                {{ option }}
              </li>
            </ul>
          </div>
        </div>
        <div v-else class="opacity-60 text-sm italic">Вопросы отсутствуют.</div>
      </div>
      <div v-else-if="store.loading" class="text-center text-lg text-red-400">Загрузка...</div>
      <div v-else-if="store.error" class="text-center text-lg text-red-600">{{ store.error }}</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useTestsStore } from '@/stores/tests_store'
import { useCoursesStore } from '@/stores/courses_store'

// Получаем id теста из параметров маршрута
const route = useRoute()
const testId = computed(() => Number(route.params.test))

const store = useTestsStore()
const coursesStore = useCoursesStore()

const test = computed(() => store.tests.find(t => t.id === testId.value) || null)


onMounted(async () => {
  if (!coursesStore.allCourses.length) {
    await coursesStore.fetchAll()
  }
  if (!store.tests.length) {
    await store.fetchTests()
  }
})


const testTitle = computed(() => test.value?.title || 'Тест')
const courseTitle = computed(() => {
  if (!test.value?.courseId) return ''
  const course = coursesStore.allCourses.find(c => c.id === test.value.courseId)
  return course ? course.title : ''
})


const questions = computed(() => {
  try {
    if (test.value?.questions) {

      return JSON.parse(test.value.questions)
    }
  } catch (e) {
    return []
  }
  return []
})

</script>
