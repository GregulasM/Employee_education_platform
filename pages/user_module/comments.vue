<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Комментарии</h2>
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
          @click="commentsStore.fetchComments"
      >
        <span v-if="commentsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать комментарий</h3>
      <form @submit.prevent="submitCreateComment" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model.number="newComment.newsId" class="input input-bordered" placeholder="ID новости (newsId)" type="number"/>
        <input v-model.number="newComment.userId" class="input input-bordered" placeholder="ID пользователя (userId)" type="number"/>
        <textarea v-model="newComment.text" class="textarea textarea-bordered md:col-span-2" placeholder="Текст комментария" rows="3"/>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.newsId" class="input input-sm" placeholder="Фильтр по ID новости" type="number"/>
      <input v-model="filter.userId" class="input input-sm" placeholder="Фильтр по ID пользователя" type="number"/>
      <input v-model="filter.text" class="input input-sm" placeholder="Фильтр по тексту" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="newsId">ID новости</option>
          <option value="userId">ID пользователя</option>
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
          <table class="w-full min-w-[1000px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">ID новости</th>
              <th class="whitespace-nowrap px-2 py-3">ID пользователя</th>
              <th class="whitespace-nowrap px-2 py-3">Текст</th>
              <th class="whitespace-nowrap px-2 py-3">Дата создания</th>
              <th class="whitespace-nowrap px-2 py-3">Дата обновления</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="comment in filteredComments" :key="comment.id">
              <!-- Обычный режим -->
              <template v-if="editingId !== comment.id && confirmingDeleteId !== comment.id">
                <td class="whitespace-nowrap px-2 py-2">{{ comment.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ comment.newsId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ comment.userId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ comment.text }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ comment.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ comment.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center items-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(comment.id)">✏️ Изм.</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(comment.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Режим редактирования -->
              <template v-else-if="editingId === comment.id">
                <td class="whitespace-nowrap px-2 py-2">{{ comment.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editComment.newsId" type="number" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editComment.userId" type="number" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editComment.text" class="input input-xs w-40" /></td>
                <td class="whitespace-nowrap px-2 py-2">{{ comment.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ comment.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(comment.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Подтверждение удаления -->
              <template v-else-if="confirmingDeleteId === comment.id">
                <td colspan="7" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить комментарий #{{ comment.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(comment.id)">Точно удалить</button>
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
import { useCommentsStore } from '~/stores/comments_store'

const router = useRouter()
const commentsStore = useCommentsStore()

const editingId = ref<number|null>(null)
const editComment = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

function startEdit(id: number) {
  editingId.value = id
  const comment = commentsStore.comments.find(c => c.id === id)
  editComment.value = { ...comment }
}

function cancelEdit() {
  editingId.value = null
  editComment.value = {}
}

async function saveEdit(id: number) {
  await commentsStore.updateComment(id, editComment.value)
  editingId.value = null
  editComment.value = {}
  await commentsStore.fetchComments()
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await commentsStore.deleteComment(id)
    confirmingDeleteId.value = null
    await commentsStore.fetchComments()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

const showFilters = ref(false)
const filter = reactive({
  newsId: '',
  userId: '',
  text: '',
})
const sortKey = ref<'id' | 'newsId' | 'userId' | 'createdAt'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

function resetFilters() {
  filter.newsId = ''
  filter.userId = ''
  filter.text = ''
}

const filteredComments = computed(() => {
  let arr = [...commentsStore.comments]
  if (filter.newsId) arr = arr.filter(c => (c.newsId || '').toString().includes(filter.newsId.toString()))
  if (filter.userId) arr = arr.filter(c => (c.userId || '').toString().includes(filter.userId.toString()))
  if (filter.text) arr = arr.filter(c => (c.text || '').toLowerCase().includes(filter.text.toLowerCase()))
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
const newComment = ref({
  newsId: null,
  userId: null,
  text: '',
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newComment.value, { newsId: null, userId: null, text: '' })
}

async function submitCreateComment() {
  try {
    createError.value = null
    await commentsStore.createComment({ ...newComment.value })
    closeCreateForm()
    await commentsStore.fetchComments()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания комментария'
  }
}

onMounted(() => commentsStore.fetchComments())
</script>
