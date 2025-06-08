<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Модули</h2>
    </div>
    <div class="flex justify-center p-4 gap-4">
      <button class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-green-500/90 hover:bg-green-700/90 font-semibold"
              @click="createMode = !createMode; showFilters = false">
        Создать
      </button>
      <button class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 font-semibold"
              @click="showFilters = !showFilters; createMode = false">
        Сортировка/Фильтрация
      </button>
      <button class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-amber-500/90 hover:bg-amber-700/90 font-semibold"
              @click="modulesStore.fetchModules">
        <span v-if="modulesStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold">
        Назад
      </NuxtLink>
    </div>

    <!-- Создание -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать модуль</h3>
      <form @submit.prevent="submitCreateModule" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newModule.title" class="input input-bordered" placeholder="Название" />
        <input v-model="newModule.description" class="input input-bordered" placeholder="Описание" />
        <input v-model="newModule.image" class="input input-bordered" placeholder="Ссылка на изображение" />
        <input v-model="newModule.tags" class="input input-bordered" placeholder="Теги (через запятую)" />
        <input v-model.number="newModule.order" class="input input-bordered" placeholder="Порядок (число)" type="number" />
        <label class="col-span-1 md:col-span-2">
          <span>Курс:</span>
          <select v-model="newModule.courseTitle" class="input input-bordered w-full mt-1" required>
            <option value="" disabled>Выберите курс</option>
            <option v-for="course in coursesStore.courses" :key="course.id" :value="course.title">
              {{ course.title }} (id:{{ course.id }})
            </option>
          </select>
        </label>
        <input v-model.number="newModule.hiddenAchievementId" class="input input-bordered" placeholder="ID скрытого достижения" type="number" />
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтры -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.title" class="input input-sm" placeholder="Фильтр по названию" />
      <input v-model="filter.course" class="input input-sm" placeholder="Фильтр по курсу" />
      <input v-model="filter.tag" class="input input-sm" placeholder="Фильтр по тегу" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="title">Название</option>
          <option value="createdAt">Дата создания</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="resetFilters">Сбросить фильтры</button>
    </div>

    <!-- Таблица -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[1100px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Картинка</th>
              <th class="whitespace-nowrap px-2 py-3">Теги</th>
              <th class="whitespace-nowrap px-2 py-3">Порядок</th>
              <th class="whitespace-nowrap px-2 py-3">Курс</th>
              <th class="whitespace-nowrap px-2 py-3">Скрытое достижение</th>
              <th class="whitespace-nowrap px-2 py-3">Статей</th>
              <th class="whitespace-nowrap px-2 py-3">Тесты</th>
              <th class="whitespace-nowrap px-2 py-3">Дата создания</th>
              <th class="whitespace-nowrap px-2 py-3">Дата обновления</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="module in filteredModules" :key="module.id">
              <!-- Просмотр -->
              <template v-if="editingId !== module.id && confirmingDeleteId !== module.id">
                <td class="whitespace-nowrap px-2 py-2">{{ module.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.title }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.description }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="module.image" :src="module.image" class="w-12 h-12 rounded object-cover" alt="module-image" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.tags ? module.tags.join(', ') : '' }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.order }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.course?.title || module.courseId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.hiddenAchievement?.name || module.hiddenAchievementId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ articlesByModule[module.id] || 0 }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.tests?.length || 0 }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center align-centre items-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(module.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(module.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Редактирование -->
              <template v-else-if="editingId === module.id">
                <td class="whitespace-nowrap px-2 py-2">{{ module.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editModule.title" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editModule.description" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editModule.image" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editModule.tagsString" class="input input-xs w-24" placeholder="теги, через запятую" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editModule.order" type="number" class="input input-xs w-12" /></td>
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model="editModule.courseId" class="input input-xs w-32">
                    <option v-for="course in coursesStore.courses" :key="course.id" :value="course.id">
                      {{ course.title }}
                    </option>
                  </select>
                </td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editModule.hiddenAchievementId" type="number" class="input input-xs w-12" /></td>
                <td class="whitespace-nowrap px-2 py-2">-</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ module.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(module.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Удаление -->
              <template v-else-if="confirmingDeleteId === module.id">
                <td colspan="12" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить модуль #{{ module.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(module.id)">Точно удалить</button>
                  <button class="btn btn-xs btn-warning ml-1" @click="cancelDelete">Отмена</button>
                </td>
              </template>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useModulesStore } from '~/stores/modules_store'
import { useCoursesStore } from '~/stores/courses_store'
import { useArticlesStore } from '~/stores/articles_store'

const router = useRouter()
const modulesStore = useModulesStore()
const coursesStore = useCoursesStore()
const articlesStore = useArticlesStore()

const editingId = ref<number|null>(null)
const editModule = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

const articlesByModule = computed(() => {
  const map: Record<number, number> = {}
  articlesStore.articles.forEach(article => {
    if (article.moduleId) {
      map[article.moduleId] = (map[article.moduleId] || 0) + 1
    }
  })
  return map
})

function startEdit(id: number) {
  editingId.value = id
  const module = modulesStore.modules.find(m => m.id === id)
  editModule.value = {
    ...module,
    tagsString: module.tags ? module.tags.join(', ') : ''
  }
}

function cancelEdit() {
  editingId.value = null
  editModule.value = {}
}

async function saveEdit(id: number) {
  let patch = { ...editModule.value }
  if (typeof patch.tagsString === 'string') {
    patch.tags = patch.tagsString.split(',').map((t: string) => t.trim()).filter((t: string) => t)
  }
  delete patch.tagsString

  await modulesStore.updateModule(id, patch)
  editingId.value = null
  editModule.value = {}
  await modulesStore.fetchModules()
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await modulesStore.deleteModule(id)
    confirmingDeleteId.value = null
    await modulesStore.fetchModules()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

const showFilters = ref(false)
const filter = reactive({
  title: '',
  course: '',
  tag: '',
})
const sortKey = ref<'id' | 'title' | 'createdAt'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredModules = computed(() => {
  let arr = [...modulesStore.modules]
  if (filter.title) arr = arr.filter(m => (m.title || '').toLowerCase().includes(filter.title.toLowerCase()))
  if (filter.course) arr = arr.filter(m => (m.course?.title || '').toLowerCase().includes(filter.course.toLowerCase()))
  if (filter.tag) arr = arr.filter(m => (m.tags || []).join(', ').toLowerCase().includes(filter.tag.toLowerCase()))
  arr.sort((a, b) => {
    let aVal = a[sortKey.value]
    let bVal = b[sortKey.value]
    if (aVal == null) return 1
    if (bVal == null) return -1
    if (aVal < bVal) return sortDir.value === 'asc' ? -1 : 1
    if (aVal > bVal) return sortDir.value === 'asc' ? 1 : -1
    return 0
  })
  return arr
})

function resetFilters() {
  filter.title = ''
  filter.course = ''
  filter.tag = ''
}

const createMode = ref(false)
const createError = ref<string | null>(null)
const newModule = ref({
  title: '', description: '', image: '', tags: '', order: null, courseTitle: '', hiddenAchievementId: null
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newModule.value, {
    title: '', description: '', image: '', tags: '', order: null, courseTitle: '', hiddenAchievementId: null
  })
}

async function submitCreateModule() {
  try {
    createError.value = null
    if (!newModule.value.courseTitle) {
      createError.value = 'Выберите курс!'
      return
    }
    let payload = {
      ...newModule.value,
      tags: newModule.value.tags
          ? newModule.value.tags.split(',').map((t: string) => t.trim()).filter((t: string) => t)
          : [],
    }
    await modulesStore.createModule(payload)
    closeCreateForm()
    await modulesStore.fetchModules()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания модуля'
  }
}
onMounted(() => {
  modulesStore.fetchModules()
  coursesStore.fetchCourses()
})
</script>
