<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg">
      <h2>Профиль</h2>
    </div>

    <div class="flex flex-col lg:flex-row p-6 gap-6">
      <div class="flex flex-col items-center self-start inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4">
        <img :src="userStore.currentUser?.avatar || '/mascot/mascot.png'" class="w-24 h-24 object-cover rounded-xl shadow object-top" />
        <p class="mt-2 text-sm font-semibold">
          {{ userStore.currentUser?.login || 'Без логина' }}
        </p>
      </div>

      <div class="flex-1 inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4 space-y-1">
        <p>
          <span class="font-semibold">ФИО:</span>
          {{ [userStore.currentUser?.secondName, userStore.currentUser?.firstName, userStore.currentUser?.lastName].filter(Boolean).join(' ') || '—' }}
        </p>
        <p>
          <span class="font-semibold">Почта:</span>
          {{ userStore.currentUser?.email || '—' }}
        </p>
        <p>
          <span class="font-semibold">Тема:</span>
          {{ userStore.currentUser?.theme?.name || '—' }}
        </p>
        <p>
          <span class="font-semibold">Шрифт:</span>
          {{ userStore.currentUser?.font?.name || '—' }}
        </p>
      </div>

      <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4 text-center w-60 shrink-0">
        <p class="font-semibold mb-1">
          Рейтинг: {{ userStore.currentUser?.rating ?? '—' }}
        </p>
        <p class="text-yellow-500 text-lg select-none">
          ★<span v-if="userStore.currentUser?.rating>1">★</span><span v-if="userStore.currentUser?.rating>2">★</span><span v-if="userStore.currentUser?.rating>3">★</span><span v-if="userStore.currentUser?.rating>4">★</span>
        </p>
        <p class="text-sm mt-2">Прохождение: {{ coursePercent }}%</p>
        <button @click="logout"
                class="btn w-full mt-4 text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500
                 bg-red-500 hover:bg-red-500/70 border-none">
          Выход
        </button>
      </div>
    </div>
  </div>

  <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 mx-8 mt-12 p-2 rounded-t-lg">
    <h2>Текущий курс</h2>
  </div>
  <div
      class="mx-8 p-6 inset-shadow-sm rounded-b-lg shadow-md inset-shadow-red-300/60 bg-orange-50 opacity-90 space-y-4"
      v-if="activeCourse"
  >
    <h3 class="text-lg font-semibold text-shadow-lg">{{ activeCourse.title }}</h3>
    <p class="text-sm">{{ activeCourse.about }}</p>

    <div class="flex-col p-4 font-semibold text-shadow-lg text-black">
      <p class="mb-2">Пройдено модулей: {{ completedModules }} / {{ totalModules }}</p>
      <p>Процент прохождения: {{ coursePercent }}%</p>
      <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60">
        <Progress :model-value="coursePercent" class="progress progress-error w-full"></Progress>
      </div>
    </div>
    <div class="flex flex-col sm:flex-row gap-4">
      <NuxtLink to="/my_courses" class="flex-1">
        <button class="btn w-full text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500
                       bg-red-500/50 hover:bg-red-500/70 border-none">
          Перейти к курсу
        </button>
      </NuxtLink>

      <button @click="changeCourse"
              class="btn flex-1 text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500
                     bg-red-500 hover:bg-red-500/70 border-none">
        Сменить курс
      </button>
    </div>
  </div>
  <div v-else class="mx-8 mt-8 text-center text-gray-400 font-semibold flex-col justify-center ">
    <p>Курс не выбран</p>
    <NuxtLink to="/courses" class="btn btn-ghost  text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mt-2">Выбрать курс</NuxtLink>
  </div>

  <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 mx-8 mt-12 p-2 rounded-t-lg">
    <h2>Последние достижения</h2>
  </div>
  <div class="mx-8 bg-orange-50 opacity-90 inset-shadow-sm rounded-b-lg shadow-md inset-shadow-red-300/60 p-4 grid md:grid-cols-2 lg:grid-cols-3 gap-4">
    <div v-for="cat in achItemsLinks" :key="cat.value" class="shadow-md rounded-lg">
      <div class="font-bold text-white text-shadow-lg/20 bg-red-500/50 mb-2 p-2 rounded-t-lg flex justify-between items-center">
        {{ cat.title }}
      </div>
      <ScrollArea class="h-30 w-full rounded-b-lg">
        <div v-for="ach in cat.ach" :key="ach.title" class="flex gap-4 p-2 inset-shadow-sm inset-shadow-red-500/50 rounded-lg mb-2 mx-4">
          <div class="avatar w-12 h-12 self-start">
            <div class="rounded shadow-sm shadow-black w-12 h-12">
              <img :src="ach.icon" class="object-top" />
            </div>
          </div>
          <div class="flex-1">
            <p class="font-semibold text-sm">{{ ach.title }}</p>
            <p class="font-normal text-xs">{{ ach.text }}</p>
          </div>
        </div>
        <div v-if="cat.ach.length === 0" class="text-xs opacity-50 p-2">Нет достижений</div>
      </ScrollArea>
    </div>
  </div>

  <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 mx-8 mt-12 p-2 rounded-t-lg ">
    <h2>Ваш персонаж</h2>
  </div>
  <div
      class="mx-8 p-6 inset-shadow-sm rounded-b-lg shadow-md inset-shadow-red-300/60 bg-orange-50 opacity-90 flex flex-col md:flex-row items-center gap-6 justify-center mb-12"
      v-if="selectedCharacter"
  >
    <img :src="selectedCharacter.avatar || '/mascot/mascot.png'" class="w-32 h-32 rounded-box object-cover shadow-md object-top"  />
    <div class="font-semibold text-shadow-lg text-center md:text-left">
      <p class="text-lg">{{ selectedCharacter.name }}</p>
      <p class="text-sm">Уровень {{ selectedCharacter.level ?? '—' }}</p>
      <p class="text-xs opacity-70">Опыт {{ selectedCharacter.exp ?? 0 }} / {{ selectedCharacter.totalExp ?? 100 }}</p>
      <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60">
        <Progress :model-value="characterPercent" class="progress progress-error w-full"></Progress>
      </div>
    </div>
  </div>
  <div v-else class="mx-8 mt-8 text-center text-gray-400 font-semibold mb-12">Персонаж не выбран</div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useUsersStore } from '~/stores/users_store'
import { useCoursesStore } from '~/stores/courses_store'
import { useCharactersStore } from '~/stores/characters_store'
import { useUserAchievementsStore } from '~/stores/user_achievements_store'
import { useAchievementListsStore } from '~/stores/achievement_lists_store'
import { useRouter } from 'vue-router'

const userStore = useUsersStore()
const coursesStore = useCoursesStore()
const charactersStore = useCharactersStore()
const userAchievementsStore = useUserAchievementsStore()
const achievementListsStore = useAchievementListsStore()
const router = useRouter()


const activeCourseId = computed(() => userStore.currentUser?.activeCourseId)
const activeCourse = computed(() => coursesStore.allCourses.find(
    c => String(c.id) === String(activeCourseId.value)
) || null)

const totalModules = computed(() => activeCourse.value?.modules?.length || 0)
const completedModules = computed(() =>
    activeCourse.value?.completedModules ?? 0
)
const coursePercent = computed(() =>
    totalModules.value > 0 ? Math.round((completedModules.value / totalModules.value) * 100) : 0
)

const selectedCharacterId = computed(() => userStore.currentUser?.selectedCharacterId)
const selectedCharacter = computed(() => charactersStore.characters.find(
    c => String(c.id) === String(selectedCharacterId.value)
) || null)
const characterPercent = computed(() =>
    selectedCharacter.value?.totalExp
        ? Math.round((selectedCharacter.value.exp / selectedCharacter.value.totalExp) * 100)
        : 0
)

const userAchievements = computed(() => userAchievementsStore.userAchievements)
const achievementLists = computed(() => achievementListsStore.lists)

const achItemsLinks = computed(() => {
  return achievementLists.value.map(list => ({
    value: list.id,
    title: list.name,
    ach: userAchievements.value
        .filter(a => a.listId === list.id)
        .slice(-3)
        .map(a => ({
          value: a.id,
          icon: a.icon || '/img/ach-placeholder.png',
          title: a.name,
          text: a.description,
          points: a.points
        }))
  }))
      .filter(cat => cat.ach.length > 0)
})


function changeCourse() {
  router.push('/courses')
}

function logout() {
  userStore.logout()
  router.push('/login')
}

onMounted(async () => {
  if (!userStore.currentUser) await userStore.loadSession?.()
  if (!coursesStore.allCourses.length) await coursesStore.fetchAll?.()
  if (!charactersStore.characters.length) await charactersStore.fetchCharacters?.()
  if (!achievementListsStore.lists.length) await achievementListsStore.fetchLists()
  if (userStore.currentUser?.id) {
    await userAchievementsStore.fetchUserAchievements(userStore.currentUser.id)
  }
})
</script>
