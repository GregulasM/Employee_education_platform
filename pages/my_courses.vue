<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Мой курс</h2>
<!--      <NuxtLink-->
<!--          v-if="!activeCourse"-->
<!--          to="/courses"-->
<!--          class="btn btn-ghost  text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500"-->
<!--      >Выбрать курс</NuxtLink>-->
    </div>

    <div v-if="loading" class="p-8 text-center text-lg text-red-400">Загрузка...</div>
    <div v-else-if="error" class="p-8 text-center text-lg text-red-600">{{ error }}</div>

    <div v-else>
      <template v-if="activeCourse">
        <div class="flex flex-col md:flex-row items-center gap-6 p-6">
          <img :src="activeCourse.icon" class="w-24 h-24 rounded-xl object-cover shadow border border-red-300/60 bg-white" />
          <div class="flex-1">
            <h3 class="text-2xl font-bold text-black mb-2">{{ activeCourse.title }}</h3>
            <div class="flex flex-wrap gap-3 mt-2">
              <NuxtLink
                  :to="`/courses/${activeCourse.slug}`"
                  class="btn btn-ghost  text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500"
              >Подробнее</NuxtLink>
              <button
                  class="btn btn-ghost  text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500"
                  @click="delete_active_course"
              >Отказаться от курса</button>
              <NuxtLink
                  to="/courses"
                  class="btn btn-ghost  text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500"
              >Все курсы</NuxtLink>
            </div>
          </div>
        </div>
        <!-- Модули и статьи курса -->
        <div class="p-4">
          <div v-if="!activeCourse.modules.length" class="text-gray-400">У курса пока нет модулей</div>
          <div v-for="m in activeCourse.modules" :key="m.id"
               class="collapse collapse-arrow bg-orange-50/60 border border-red-300/50 rounded-lg mb-3">
            <input type="checkbox" />
            <div class="collapse-title flex items-center gap-3">
              <img :src="m.icon" class="w-10 h-10 rounded-box object-cover shadow" />
              {{ m.num }}. {{ m.title }}
            </div>
            <div class="collapse-content">
              <ul class="space-y-2 ml-4">
                <li v-for="a in m.articles" :key="a.slug"
                    class="flex items-center gap-2 inset-shadow-sm inset-shadow-red-500/50 p-2 rounded-lg">
                  <span class="text-xs opacity-60 w-12">{{ a.num }}</span>
                  <button class="text-left flex-1 hover:underline"
                          @click="openArticle(activeCourse.slug, m.id, a.slug)">
                    {{ a.title }}
                  </button>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </template>
      <template v-else>
        <div class="p-8 text-center text-gray-400 text-lg">
          У вас пока не выбран активный курс.<br>
          <NuxtLink to="/courses" class="btn btn-ghost  text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mt-2">Выбрать курс</NuxtLink>
        </div>
      </template>
    </div>
    <div class="mt-8"></div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { storeToRefs } from 'pinia'
import { useCoursesStore } from '@/stores/courses_store'
import { useUsersStore } from '~/stores/users_store'

const router = useRouter()
const coursesStore = useCoursesStore()
const userStore = useUsersStore()

const { loading, error } = storeToRefs(coursesStore)


onMounted(async () => {
  if (!coursesStore.allCourses.length) {
    await coursesStore.fetchAll()
  }
})


const activeCourse = computed(() => {
  const activeId = userStore.currentUser?.activeCourseId
  if (!activeId) return null
  return coursesStore.allCourses.find(c => c.id === activeId) || null
})


async function delete_active_course() {
  if (!userStore.currentUser?.id) return
  try {
    await userStore.updateUser(userStore.currentUser.id, { activeCourseId: null, isActiveCourseIdSet: true })
    const updated = await userStore.getUserById(userStore.currentUser.id)
    if (updated && userStore.currentUser) {
      userStore.currentUser.activeCourseId = null
      Object.assign(userStore.currentUser, updated)
      userStore.saveSession(userStore.currentUser, userStore.token)
    }
  } catch (e) {
    alert('Ошибка удаления курса')
  }
}


function openArticle(courseSlug: string, moduleId: number, articleSlug: string) {
  const article = coursesStore.findArticle(courseSlug, moduleId, articleSlug)
  if (!article) return
  router.push(`/courses/${courseSlug}/${moduleId}/${encodeURIComponent(article.title)}`)
}
</script>
