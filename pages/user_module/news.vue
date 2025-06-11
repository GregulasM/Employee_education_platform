<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Новости</h2>
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
          @click="newsStore.fetchNews"
      >
        <span v-if="newsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>
    <!-- Форма создания новости -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <TiptapEditor v-model="tiptapContent" class="bg-gray-800/90" />
      <h3 class="text-lg font-bold text-white mb-4">Создать новость</h3>
      <form @submit.prevent="submitCreateNews" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newNews.title" class="input input-bordered" placeholder="Заголовок (title)" />
        <input v-model="newNews.slug" class="input input-bordered" placeholder="Slug (на англ.)" />
        <input v-model="newNews.excerpt" class="input input-bordered" placeholder="Краткое описание (excerpt)" />

        <input v-model.number="newNews.authorId" class="input input-bordered" placeholder="ID автора (authorId)" type="number"/>
        <input v-model="newNews.type" class="input input-bordered" placeholder="Тип (type, опционально)" />
        <input v-model="newNews.date" class="input input-bordered" placeholder="Дата (YYYY-MM-DD, опционально)" />
        <input v-model="newNews.tags" class="input input-bordered" placeholder="Теги (JSON-массив, напр. ['событие'])" />
        <label class="flex items-center gap-2">
          <input v-model="newNews.isPinned" type="checkbox" class="checkbox" />
          Важная (isPinned)
        </label>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>
    <!-- Фильтрация -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.title" class="input input-sm" placeholder="Фильтр по заголовку" />
      <input v-model="filter.type" class="input input-sm" placeholder="Фильтр по типу" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="title">Заголовок</option>
          <option value="date">Дата</option>
          <option value="isPinned">Важная</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {title:'', type:''})">Сбросить фильтры</button>
    </div>
    <!-- Таблица новостей -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[1100px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Заголовок</th>
              <th class="whitespace-nowrap px-2 py-3">Slug</th>
              <th class="whitespace-nowrap px-2 py-3">Кратко</th>
              <th class="whitespace-nowrap px-2 py-3">Тип</th>
              <th class="whitespace-nowrap px-2 py-3">Дата</th>
              <th class="whitespace-nowrap px-2 py-3">Важная</th>
              <th class="whitespace-nowrap px-2 py-3">Теги</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="n in filteredNews" :key="n.id">
              <template v-if="editingSlug !== n.slug && confirmingDeleteSlug !== n.slug">
                <td class="whitespace-nowrap px-2 py-2">{{ n.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ n.title }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ n.slug }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <pre class="whitespace-pre-line break-words text-xs">{{ n.excerpt }}</pre>
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ n.type }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ n.date ? n.date.slice(0, 10) : '' }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <span v-if="n.isPinned" class="badge badge-success">Да</span>
                  <span v-else class="badge badge-error">Нет</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <span v-if="n.tags">{{ n.tags }}</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center align-center items-center">
                  <button class="btn btn-xs btn-info" @click="viewContent(n)">👁️ Подробнее</button>
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(n.slug)">✏️ Изм.</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(n.slug)">Удалить 🗑️</button>
                </td>
              </template>
              <template v-else-if="editingSlug === n.slug">
                <td class="whitespace-nowrap px-2 py-2">{{ n.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editNews.title" class="input input-xs w-32" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editNews.slug" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editNews.excerpt" class="input input-xs w-36" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editNews.type" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editNews.date" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editNews.isPinned" type="checkbox" class="checkbox" />
                </td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editNews.tags" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(n.slug)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <template v-else-if="confirmingDeleteSlug === n.slug">
                <td colspan="9" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить новость "{{ n.title }}"?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(n.slug)">Точно удалить</button>
                  <button class="btn btn-xs btn-warning ml-1" @click="cancelDelete">Отмена</button>
                </td>
              </template>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Модальное окно для отображения content -->
    <div v-if="showContentModal" class="fixed inset-0 bg-black/50 flex items-center justify-center z-50" @click="closeContentModal">
      <div class="bg-gray-800 p-6 rounded-lg max-w-4xl w-full mx-4" @click.stop>
        <h3 class="text-lg font-bold text-white mb-4">Контент новости</h3>
        <div v-if="selectedNews" class="text-white">
          <pre class="whitespace-pre-line break-words bg-gray-700 p-2 rounded">{{ selectedNews.content }}</pre>
        </div>
        <div class="flex justify-end mt-4">
          <button class="btn btn-primary" @click="closeContentModal">Закрыть</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useNewsStore } from '~/stores/news_store'
import type { NewsDto } from '~/stores/news_store'

const tiptapContent = ref(null)

const router = useRouter()
const newsStore = useNewsStore()

const editingSlug = ref<string | null>(null)
const editNews = ref<any>({})
const confirmingDeleteSlug = ref<string | null>(null)
const showContentModal = ref(false)
const selectedNews = ref<NewsDto | null>(null)

function go_back() {
  router.back()
}

function startEdit(slug: string) {
  editingSlug.value = slug
  const n = newsStore.newsList.find(x => x.slug === slug)
  editNews.value = { ...n }
}

function cancelEdit() {
  editingSlug.value = null
  editNews.value = {}
}

async function saveEdit(slug: string) {
  await newsStore.updateNews(slug, editNews.value)
  editingSlug.value = null
  editNews.value = {}
  await newsStore.fetchNews()
}

function startDelete(slug: string) {
  confirmingDeleteSlug.value = slug
}

function cancelDelete() {
  confirmingDeleteSlug.value = null
}

async function confirmDelete(slug: string) {
  await newsStore.deleteNews(slug)
  confirmingDeleteSlug.value = null
  await newsStore.fetchNews()
}

function viewContent(news: NewsDto) {
  selectedNews.value = news
  showContentModal.value = true
}

function closeContentModal() {
  showContentModal.value = false
  selectedNews.value = null
}

const showFilters = ref(false)
const filter = reactive({
  title: '',
  type: '',
})
const sortKey = ref<'id' | 'title' | 'date' | 'isPinned'>('id')
const sortDir = ref<'asc' | 'desc'>('desc')

const filteredNews = computed(() => {
  let arr = [...newsStore.newsList]
  if (filter.title) arr = arr.filter(n => (n.title || '').toLowerCase().includes(filter.title.toLowerCase()))
  if (filter.type) arr = arr.filter(n => (n.type || '').toLowerCase().includes(filter.type.toLowerCase()))
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

const createMode = ref(false)
const createError = ref<string | null>(null)
const newNews = ref({
  title: '', slug: '', excerpt: '', content: '', authorId: null, type: '', date: '', tags: '', isPinned: false
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newNews.value, {
    title: '', slug: '', excerpt: '', content: '', authorId: null, type: '', date: '', tags: '', isPinned: false
  })
}

async function submitCreateNews() {
  try {
    createError.value = null

    const payload = { ...newNews.value }
    payload.content = JSON.stringify(tiptapContent.value)

    if (payload.date) {
      payload.date = new Date(payload.date).toISOString()
    } else {
      delete payload.date
    }

    await newsStore.createNews(payload)
    closeCreateForm()
    await newsStore.fetchNews()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания новости'
  }
}

watch(
    tiptapContent,
    (newVal) => {
      if (newVal) {
        localStorage.setItem('news-draft', JSON.stringify(newVal))
      }
    },
    { deep: true }
)

onMounted(() => {
  newsStore.fetchNews()
  const draft = localStorage.getItem('news-draft')
  if (draft) tiptapContent.value = JSON.parse(draft)
})

</script>