<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Статьи</h2>
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
              @click="refreshArticles">
        <span v-if="articlesStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <div v-if="articlesStore.modules.length === 0" class="text-red-400 text-center my-6">
      Не удалось загрузить модули. Создание статей невозможно.
    </div>

    <!-- Создание статьи -->
    <div v-if="createMode && articlesStore.modules.length > 0" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать статью</h3>
      <form @submit.prevent="submitCreateArticle" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <label class="md:col-span-2">
          <span>Модуль:</span>
          <select v-model.number="newArticle.moduleId" class="input input-bordered w-full mt-1" required>
            <option v-for="m in articlesStore.modules" :key="m.id" :value="m.id">
              {{ m.title }}
            </option>
          </select>
        </label>
        <input v-model="newArticle.title" class="input input-bordered" placeholder="Название статьи" required />
        <input v-model="newArticle.image" class="input input-bordered" placeholder="Ссылка на картинку"/>
        <input v-model="newArticle.tags" class="input input-bordered" placeholder="Теги (json-строка)" />
        <textarea v-model="newArticle.content" class="textarea textarea-bordered col-span-1 md:col-span-2" placeholder="Контент (json-строка)"></textarea>
        <input v-model.number="newArticle.rating" class="input input-bordered" placeholder="Рейтинг" type="number"/>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтры -->
    <div v-if="showFilters && articlesStore.modules.length > 0" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <select v-model="filter.moduleId" class="input input-sm">
        <option value="">Все модули</option>
        <option v-for="m in articlesStore.modules" :key="m.id" :value="m.id">{{ m.title }}</option>
      </select>
      <input v-model="filter.title" class="input input-sm" placeholder="Фильтр по названию"/>
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

    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[1200px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Модуль</th>
              <th class="whitespace-nowrap px-2 py-3">Картинка</th>
              <th class="whitespace-nowrap px-2 py-3">Теги</th>
              <th class="whitespace-nowrap px-2 py-3">Рейтинг</th>
              <th class="whitespace-nowrap px-2 py-3">Дата создания</th>
              <th class="whitespace-nowrap px-2 py-3">Дата обновления</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="a in filteredArticles" :key="a.id">
              <template v-if="editingId !== a.id && confirmingDeleteId !== a.id">
                <td class="whitespace-nowrap px-2 py-2">{{ a.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ a.title }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ moduleNameById(a.moduleId) }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="a.image" :src="a.image" class="w-10 h-10 rounded object-cover" alt="img"/>
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ a.tags }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ a.rating }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ a.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ a.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center align-centre items-center place-content-center place-self-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(a.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(a.id)">Удалить 🗑️</button>
                </td>
              </template>
              <template v-else-if="editingId === a.id">
                <td class="whitespace-nowrap px-2 py-2">{{ a.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editArticle.title" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editArticle.moduleId" class="input input-xs w-24">
                    <option v-for="m in articlesStore.modules" :key="m.id" :value="m.id">{{ m.title }}</option>
                  </select>
                </td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editArticle.image" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editArticle.tags" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editArticle.rating" type="number" class="input input-xs w-16" /></td>
                <td class="whitespace-nowrap px-2 py-2">{{ a.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ a.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(a)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <template v-else-if="confirmingDeleteId === a.id">
                <td colspan="10" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить статью #{{ a.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(a)">Точно удалить</button>
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
import { useArticlesStore } from '~/stores/articles_store'
const router = useRouter()
const articlesStore = useArticlesStore()


const editingId = ref<number|null>(null)
const editArticle = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

const createMode = ref(false)
const showFilters = ref(false)
const createError = ref<string | null>(null)
const newArticle = ref({
  title: '', moduleId: null, image: '', content: '', tags: '', rating: null
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newArticle.value, {
    title: '', moduleId: null, image: '', content: '', tags: '', rating: null
  })
}

async function submitCreateArticle() {
  try {
    createError.value = null
    if (!newArticle.value.title || !newArticle.value.moduleId) {
      createError.value = 'Выберите модуль и введите название статьи!'
      return
    }
    await articlesStore.createArticle(newArticle.value.moduleId, { ...newArticle.value })
    closeCreateForm()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания статьи'
  }
}

const filter = reactive({
  moduleId: '',
  title: '',
})
const sortKey = ref<'id' | 'title' | 'createdAt'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredArticles = computed(() => {
  let arr = [...articlesStore.articles]
  if (filter.moduleId) arr = arr.filter(a => a.moduleId === +filter.moduleId)
  if (filter.title) arr = arr.filter(a => (a.title || '').toLowerCase().includes(filter.title.toLowerCase()))
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

function moduleNameById(moduleId: number | null) {
  const m = articlesStore.modules.find(m => m.id === moduleId)
  return m ? m.title : '—'
}

// CRUD (редактирование/удаление)
function startEdit(id: number) {
  editingId.value = id
  const art = articlesStore.articles.find(a => a.id === id)
  editArticle.value = { ...art }
}
function cancelEdit() {
  editingId.value = null
  editArticle.value = {}
}
async function saveEdit() {
  await articlesStore.updateArticle(editArticle.value.id, { ...editArticle.value })
  editingId.value = null
  editArticle.value = {}
}
function startDelete(id: number) {
  confirmingDeleteId.value = id
  const art = articlesStore.articles.find(a => a.id === id)
  editArticle.value = { ...art }
}
function cancelDelete() {
  confirmingDeleteId.value = null
  editArticle.value = {}
}
async function confirmDelete() {
  await articlesStore.deleteArticle(editArticle.value.id)
  confirmingDeleteId.value = null
  editArticle.value = {}
}

function refreshArticles() {
  if (filter.moduleId) {
    articlesStore.fetchArticles(+filter.moduleId)
  } else {
    articlesStore.fetchArticles()
  }
}
function resetFilters() {
  filter.moduleId = ''
  filter.title = ''
  refreshArticles()
}

onMounted(async () => {
  await articlesStore.fetchModules()
  await articlesStore.fetchArticles()
})
</script>
