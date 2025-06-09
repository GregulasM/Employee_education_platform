<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Полезные ссылки</h2>
    </div>
    <div class="flex justify-center p-4 gap-4">
      <button class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-green-500/90 hover:bg-green-700/90 font-semibold"
              @click="createMode = !createMode; showFilters = false">
        Создать
      </button>
      <button
          class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 font-semibold"
          @click="showFilters = !showFilters; createMode = false"
      >
        Сортировка/Фильтрация
      </button>
      <button
          class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-amber-500/90 hover:bg-amber-700/90 font-semibold"
          @click="refreshLinks"
      >
        <span v-if="linksStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <div v-if="coursesStore.courses.length === 0" class="text-red-400 text-center my-6">
      Не удалось загрузить курсы. Работа с полезными ссылками невозможна.
    </div>

    <!-- Создание ссылки -->
    <div v-if="createMode && coursesStore.courses.length > 0" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-3xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать полезную ссылку</h3>
      <form @submit.prevent="submitCreateLink" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <label class="md:col-span-2">
          <span>Курс:</span>
          <select v-model.number="newLink.courseId" class="input input-bordered w-full mt-1" required>
            <option v-for="course in coursesStore.courses" :key="course.id" :value="course.id">
              {{ course.title }}
            </option>
          </select>
        </label>
        <input v-model="newLink.title" class="input input-bordered" placeholder="Название ссылки" required />
        <input v-model="newLink.url" class="input input-bordered" placeholder="URL" required />
        <input v-model="newLink.description" class="input input-bordered" placeholder="Описание" />
        <label class="md:col-span-2">
          <span>Родительская ссылка (заголовок):</span>
          <select v-model.number="newLink.parentId" class="input input-bordered w-full mt-1">
            <option value="">Без родителя</option>
            <option
                v-for="link in parentLinksForCourse(newLink.courseId)"
                :key="link.id"
                :value="link.id"
            >
              {{ link.title }}
            </option>
          </select>
        </label>
        <input v-model="tagsInput" class="input input-bordered col-span-1 md:col-span-2" placeholder="Теги (через запятую)" />
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтрация/Сортировка -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <select v-model="filter.courseId" class="input input-sm">
        <option value="">Все курсы</option>
        <option v-for="c in coursesStore.courses" :key="c.id" :value="c.id">{{ c.title }}</option>
      </select>
      <input v-model="filter.title" class="input input-sm" placeholder="Поиск по названию" />
      <input v-model="filter.url" class="input input-sm" placeholder="Поиск по URL" />
      <input v-model="filter.tags" class="input input-sm" placeholder="Поиск по тегам" />
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {courseId:'', title:'', url:'', tags:''})">Сбросить фильтры</button>
    </div>

    <!-- Таблица -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">URL</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Курс</th>
              <th class="whitespace-nowrap px-2 py-3">Родительская ссылка</th>
              <th class="whitespace-nowrap px-2 py-3">Теги</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="link in filteredLinks" :key="link.id">
              <template v-if="editingId !== link.id && confirmingDeleteId !== link.id">
                <td class="whitespace-nowrap px-2 py-2">{{ link.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ link.title }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <a :href="link.url" target="_blank" class="underline text-blue-300 hover:text-blue-500">{{ link.url }}</a>
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ link.description }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ courseTitleById(link.courseId) }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ parentLinkTitle(link.parentId) }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <span v-for="t in (parseTags(link.tags))" :key="t" class="badge badge-info mr-1">{{ t }}</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center items-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(link.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(link.id)">Удалить 🗑</button>
                </td>
              </template>
              <template v-else-if="editingId === link.id">
                <td class="whitespace-nowrap px-2 py-2">{{ link.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editLink.title" class="input input-xs w-28" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editLink.url" class="input input-xs w-40" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editLink.description" class="input input-xs w-40" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editLink.courseId" class="input input-xs w-32">
                    <option v-for="course in coursesStore.courses" :key="course.id" :value="course.id">{{ course.title }}</option>
                  </select>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editLink.parentId" class="input input-xs w-32">
                    <option value="">Без родителя</option>
                    <option v-for="pl in parentLinksForCourse(editLink.courseId, link.id)" :key="pl.id" :value="pl.id">{{ pl.title }}</option>
                  </select>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editTagsInput" class="input input-xs w-32" />
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(link.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <template v-else-if="confirmingDeleteId === link.id">
                <td colspan="8" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить ссылку #{{ link.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(link.id)">Точно удалить</button>
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
import { useUsefulLinksStore } from '~/stores/useful_links_store'
import { useCoursesStore } from '~/stores/courses_store'

const router = useRouter()

const linksStore = useUsefulLinksStore()
const coursesStore = useCoursesStore()

const createMode = ref(false)
const showFilters = ref(false)
const createError = ref<string | null>(null)
const newLink = ref({
  title: '',
  url: '',
  description: '',
  tags: '',
  parentId: null as number | null,
  courseId: null as number | null,
})
const tagsInput = ref('')
const editTagsInput = ref('')

const editingId = ref<number | null>(null)
const editLink = ref<any>({})
const confirmingDeleteId = ref<number | null>(null)

const filter = reactive({
  courseId: '',
  title: '',
  url: '',
  tags: '',
})

function go_back () {
  router.back()
}

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newLink.value, {
    title: '', url: '', description: '', tags: '', parentId: null, courseId: null
  })
  tagsInput.value = ''
}

async function submitCreateLink() {
  try {
    createError.value = null
    if (!newLink.value.courseId) {
      createError.value = 'Выберите курс!'
      return
    }
    const payload = {
      ...newLink.value,
      tags: tagsInput.value.split(',').map(s => s.trim()).filter(Boolean)
    }
    await linksStore.createLink(payload)
    closeCreateForm()
    await linksStore.fetchLinks()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания ссылки'
  }
}

function refreshLinks() {
  linksStore.fetchLinks()
  coursesStore.fetchCourses()
}

function courseTitleById(courseId: number | null) {
  if (!courseId) return '—'
  const c = coursesStore.courses.find(c => c.id === courseId)
  return c ? c.title : `ID ${courseId}`
}
function parentLinkTitle(parentId: number | null) {
  if (!parentId) return '—'
  const l = linksStore.links.find(l => l.id === parentId)
  return l ? l.title : `ID ${parentId}`
}
function parseTags(tags: string | string[] | null) {
  if (!tags) return []
  if (Array.isArray(tags)) return tags
  try {
    return JSON.parse(tags)
  } catch {
    return tags.split(',').map(t => t.trim())
  }
}
function parentLinksForCourse(courseId: number | null, excludeId: number | null = null) {
  return linksStore.links
      .filter(l => l.courseId === courseId && (!excludeId || l.id !== excludeId) && !l.parentId)
}

const filteredLinks = computed(() => {
  let arr = [...linksStore.links]
  if (filter.courseId) arr = arr.filter(l => String(l.courseId) === String(filter.courseId))
  if (filter.title) arr = arr.filter(l => (l.title || '').toLowerCase().includes(filter.title.toLowerCase()))
  if (filter.url) arr = arr.filter(l => (l.url || '').toLowerCase().includes(filter.url.toLowerCase()))
  if (filter.tags) {
    arr = arr.filter(l => {
      const tags = parseTags(l.tags)
      return tags.some((t: string) => t.toLowerCase().includes(filter.tags.toLowerCase()))
    })
  }
  // Только активные!
  arr = arr.filter(l => l.isActive !== false)
  arr.sort((a, b) => b.id - a.id)
  return arr
})

function startEdit(id: number) {
  editingId.value = id
  const link = linksStore.links.find(l => l.id === id)
  if (!link) return
  editLink.value = { ...link }
  editTagsInput.value = Array.isArray(link.tags)
      ? link.tags.join(', ')
      : parseTags(link.tags).join(', ')
}
function cancelEdit() {
  editingId.value = null
  editLink.value = {}
  editTagsInput.value = ''
}
async function saveEdit(id: number) {
  await linksStore.updateLink(id, {
    ...editLink.value,
    tags: editTagsInput.value.split(',').map(s => s.trim()).filter(Boolean)
  })
  editingId.value = null
  editLink.value = {}
  editTagsInput.value = ''
  await linksStore.fetchLinks()
}
function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await linksStore.deleteLink(id)
    confirmingDeleteId.value = null
    await linksStore.fetchLinks()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

onMounted(async () => {
  await coursesStore.fetchCourses()
  await linksStore.fetchLinks()
})
</script>
