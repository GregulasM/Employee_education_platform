<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Результаты тестов</h2>
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
          @click="refreshTestResults"
      >
        <span v-if="store.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <!-- Создание результата -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-3xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Добавить результат теста</h3>
      <form @submit.prevent="submitCreate" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <label>
          <span>Пользователь:</span>
          <select v-model="newResult.userId" class="input input-bordered w-full mt-1" required>
            <option v-for="user in usersStore.users" :key="user.id" :value="user.id">
              {{ user.login }} — {{ user.firstName }} {{ user.lastName }}
            </option>
          </select>
        </label>
        <label>
          <span>Тест:</span>
          <select v-model="newResult.testId" class="input input-bordered w-full mt-1" required>
            <option v-for="test in testsStore.tests" :key="test.id" :value="test.id">
              {{ test.title }}
            </option>
          </select>
        </label>
        <input v-model.number="newResult.score" class="input input-bordered" placeholder="Баллы" type="number" required />
        <input v-model.number="newResult.maxScore" class="input input-bordered" placeholder="Максимум" type="number" />
        <textarea v-model="newResult.answers" class="textarea textarea-bordered col-span-1 md:col-span-2" placeholder="Ответы (json)"></textarea>
        <input v-model="newResult.status" class="input input-bordered" placeholder="Статус" />
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Добавить</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтры -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.user" class="input input-sm" placeholder="Пользователь" />
      <input v-model="filter.status" class="input input-sm" placeholder="Статус" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="user">Пользователь</option>
          <option value="score">Баллы</option>
          <option value="test">Тест</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {user:'', status:''})">Сбросить фильтры</button>
    </div>

    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[1100px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Пользователь</th>
              <th class="whitespace-nowrap px-2 py-3">Тест</th>
              <th class="whitespace-nowrap px-2 py-3">Баллы</th>
              <th class="whitespace-nowrap px-2 py-3">Максимум</th>
              <th class="whitespace-nowrap px-2 py-3">Статус</th>
              <th class="whitespace-nowrap px-2 py-3">Ответы</th>
              <th class="whitespace-nowrap px-2 py-3">Действия</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="result in filteredResults" :key="result.id">
              <!-- Стандартный режим -->
              <template v-if="editingId !== result.id && confirmingDeleteId !== result.id">
                <td class="whitespace-nowrap px-2 py-2">{{ result.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ result.user?.login || result.userId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ result.test?.title || result.testId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ result.score }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ result.maxScore }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ result.status }}</td>
                <td class="whitespace-nowrap px-2 py-2 ">
                  <button class="btn btn-xs btn-info " @click="viewAnswers(result.answers)">Просмотр</button>
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(result.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(result.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Режим редактирования -->
              <template v-else-if="editingId === result.id">
                <td>{{ result.id }}</td>
                <td>
                  <select v-model="editResult.userId" class="input input-xs">
                    <option v-for="user in usersStore.users" :key="user.id" :value="user.id">
                      {{ user.login }} — {{ user.firstName }} {{ user.lastName }}
                    </option>
                  </select>
                </td>
                <td>
                  <select v-model="editResult.testId" class="input input-xs">
                    <option v-for="test in testsStore.tests" :key="test.id" :value="test.id">
                      {{ test.title }}
                    </option>
                  </select>
                </td>
                <td><input v-model="editResult.score" type="number" class="input input-xs w-20" /></td>
                <td><input v-model="editResult.maxScore" type="number" class="input input-xs w-20" /></td>
                <td><input v-model="editResult.status" class="input input-xs w-20" /></td>
                <td><input v-model="editResult.answers" class="input input-xs w-24" /></td>
                <td class="flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(result.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Подтверждение удаления -->
              <template v-else-if="confirmingDeleteId === result.id">
                <td colspan="8" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить результат #{{ result.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(result.id)">Точно удалить</button>
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
import { useTestResultsStore } from '~/stores/test_results_store'
import { useUsersStore } from '~/stores/users_store'
import { useTestsStore } from '~/stores/tests_store'

const router = useRouter()
const store = useTestResultsStore()
const usersStore = useUsersStore()
const testsStore = useTestsStore()

const createMode = ref(false)
const showFilters = ref(false)
const createError = ref<string | null>(null)
const newResult = ref({
  userId: null,
  testId: null,
  score: null,
  maxScore: null,
  answers: '',
  status: '',
})

const editingId = ref<number | null>(null)
const editResult = ref<any>({})
const confirmingDeleteId = ref<number | null>(null)

const filter = reactive({
  user: '',
  status: '',
})
const sortKey = ref<'id'|'user'|'score'|'test'>('id')
const sortDir = ref<'asc'|'desc'>('asc')

// -------------------
// CRUD/Управление
// -------------------
function go_back() {
  router.back()
}
function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newResult.value, {
    userId: null,
    testId: null,
    score: null,
    maxScore: null,
    answers: '',
    status: '',
  })
}
async function submitCreate() {
  try {
    createError.value = null
    await store.createTestResult({ ...newResult.value })
    closeCreateForm()
    await store.fetchTestResults()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания результата'
  }
}

function startEdit(id: number) {
  editingId.value = id
  const result = store.testResults.find(r => r.id === id)
  editResult.value = { ...result }
}
function cancelEdit() {
  editingId.value = null
  editResult.value = {}
}
async function saveEdit(id: number) {
  await store.updateTestResult(id, editResult.value)
  editingId.value = null
  editResult.value = {}
  await store.fetchTestResults()
}
function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await store.deleteTestResult(id)
    confirmingDeleteId.value = null
    await store.fetchTestResults()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

async function refreshTestResults() {
  await store.fetchTestResults()
}

// -------------------
// Фильтрация и сортировка
// -------------------
const filteredResults = computed(() => {
  let arr = [...store.testResults]
  if (filter.user)
    arr = arr.filter(r =>
        (r.user?.login || '').toLowerCase().includes(filter.user.toLowerCase()) ||
        (r.user?.firstName || '').toLowerCase().includes(filter.user.toLowerCase()) ||
        (r.user?.lastName || '').toLowerCase().includes(filter.user.toLowerCase())
    )
  if (filter.status)
    arr = arr.filter(r => (r.status || '').toLowerCase().includes(filter.status.toLowerCase()))
  arr.sort((a, b) => {
    let aVal = a[sortKey.value]
    let bVal = b[sortKey.value]
    if (sortKey.value === 'user') {
      aVal = a.user?.login || ''
      bVal = b.user?.login || ''
    }
    if (sortKey.value === 'test') {
      aVal = a.test?.title || ''
      bVal = b.test?.title || ''
    }
    if (aVal == null) return 1
    if (bVal == null) return -1
    if (aVal < bVal) return sortDir.value === 'asc' ? -1 : 1
    if (aVal > bVal) return sortDir.value === 'asc' ? 1 : -1
    return 0
  })
  return arr
})

// -------------------
// Просмотр answers
// -------------------
function viewAnswers(answers: string) {
  alert(answers)
}

// -------------------
// Первичная загрузка данных
// -------------------
onMounted(async () => {
  await usersStore.fetchUsers()
  await testsStore.fetchTests()
  await store.fetchTestResults()
})

</script>
