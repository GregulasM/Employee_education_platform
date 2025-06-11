<template>
  <div class="pb-4">
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Тесты</h2>
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
              @click="refreshTests">
        <span v-if="testsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <!-- Создание теста -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-3xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать тест</h3>
      <form @submit.prevent="submitCreateTest" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <label class="md:col-span-2">
          <span>Курс <span class="text-red-500">*</span>:</span>
          <select v-model="newTest.courseId" class="input input-bordered w-full mt-1" required>
            <option disabled value="">Выберите курс</option>
            <option v-for="course in testsStore.courses" :key="course.id" :value="course.id">{{ course.title }}</option>
          </select>
        </label>
        <label class="md:col-span-2">
          <span>Модуль (необязательно):</span>
          <select v-model="newTest.moduleId" class="input input-bordered w-full mt-1">
            <option :value="null">Без модуля</option>
            <option v-for="module in filteredModules" :key="module.id" :value="module.id">{{ module.title }}</option>
          </select>
        </label>
        <input v-model="newTest.title"        class="input input-bordered" placeholder="Название теста" required />
        <input v-model="newTest.description"  class="input input-bordered" placeholder="Описание"/>
        <textarea v-model="newTest.questions" class="textarea textarea-bordered md:col-span-2" rows="2"
                  placeholder="Вопросы (JSON-строка, например: [{&quot;question&quot;: &quot;...&quot;,&quot;answers&quot;:[...]}, ...])"></textarea>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтрация/Сортировка -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <label>
        <span>Курс:</span>
        <select v-model="filter.courseId" class="select select-sm ml-1">
          <option value="">Все</option>
          <option v-for="course in testsStore.courses" :key="course.id" :value="course.id">{{ course.title }}</option>
        </select>
      </label>
      <label>
        <span>Модуль:</span>
        <select v-model="filter.moduleId" class="select select-sm ml-1">
          <option value="">Все</option>
          <option v-for="module in filteredModules" :key="module.id" :value="module.id">{{ module.title }}</option>
        </select>
      </label>
      <input v-model="filter.title"      class="input input-sm" placeholder="Название" />
      <input v-model="filter.description" class="input input-sm" placeholder="Описание" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="title">Название</option>
          <option value="courseTitle">Курс</option>
          <option value="moduleTitle">Модуль</option>
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

    <!-- Таблица тестов -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Курс</th>
              <th class="whitespace-nowrap px-2 py-3">Модуль</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Вопросы</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="test in filteredTests" :key="test.id">
              <!-- Не в режиме редактирования -->
              <template v-if="editingId !== test.id && confirmingDeleteId !== test.id">
                <td class="whitespace-nowrap px-2 py-2">{{ test.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ test.title }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ courseNameById(test.courseId) }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ moduleNameById(test.moduleId) }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ test.description }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <pre class="max-w-xs whitespace-pre-wrap break-all">{{ test.questions }}</pre>
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center align-centre items-center place-content-center place-self-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(test.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(test.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Режим редактирования -->
              <template v-else-if="editingId === test.id">
                <td class="whitespace-nowrap px-2 py-2">{{ test.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editTest.title" class="input input-xs w-36" /></td>
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model="editTest.courseId" class="select select-xs w-32">
                    <option disabled value="">Выберите</option>
                    <option v-for="course in testsStore.courses" :key="course.id" :value="course.id">{{ course.title }}</option>
                  </select>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model="editTest.moduleId" class="select select-xs w-28">
                    <option value="">Без модуля</option>
                    <option v-for="module in filteredModules" :key="module.id" :value="module.id">{{ module.title }}</option>
                  </select>
                </td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editTest.description" class="input input-xs w-36" /></td>
                <td class="whitespace-nowrap px-2 py-2"><textarea v-model="editTest.questions" class="textarea textarea-xs w-44" rows="1"></textarea></td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(test.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Режим подтверждения удаления -->
              <template v-else-if="confirmingDeleteId === test.id">
                <td colspan="7" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить тест #{{ test.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(test.id)">Точно удалить</button>
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
import { useTestsStore } from '~/stores/tests_store'
const router = useRouter()
const testsStore = useTestsStore()

const createMode = ref(false)
const showFilters = ref(false)
const createError = ref<string | null>(null)
const editingId = ref<number|null>(null)
const editTest = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)
const newTest = ref({
  title: '', description: '', courseId: '', moduleId: '', questions: ''
})

const filter = reactive({
  courseId: '',
  moduleId: '',
  title: '',
  description: ''
})

const sortKey = ref<'id'|'title'|'courseTitle'|'moduleTitle'>('id')
const sortDir = ref<'asc'|'desc'>('asc')

const filteredModules = computed(() => {
  if (!testsStore.modules.length || !newTest.value.courseId && !filter.courseId && !editTest.value.courseId)
    return testsStore.modules
  const courseId = +(
      createMode.value
          ? newTest.value.courseId
          : (editTest.value.courseId || filter.courseId)
  )
  return testsStore.modules.filter(m => m.courseId === courseId)
})

function go_back () { router.back() }
function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newTest.value, { title: '', description: '', courseId: '', moduleId: '', questions: '' })
}
function resetFilters() {
  Object.assign(filter, { courseId: '', moduleId: '', title: '', description: '' })
}

function courseNameById(id: number) {
  const c = testsStore.courses.find(c => c.id === id)
  return c ? c.title : '—'
}
function moduleNameById(id: number) {
  const m = testsStore.modules.find(m => m.id === id)
  return m ? m.title : '—'
}

async function refreshTests() {
  await testsStore.fetchCourses()
  await testsStore.fetchModules()
  await testsStore.fetchTests()
}
async function submitCreateTest() {
  try {
    createError.value = null
    if (!newTest.value.courseId) {
      createError.value = 'Выберите курс!'
      return
    }
    await testsStore.createTest({ ...newTest.value })
    closeCreateForm()
    await testsStore.fetchTests()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания теста'
  }
}

const filteredTests = computed(() => {
  let arr = [...testsStore.tests]
  if (filter.courseId) arr = arr.filter(t => String(t.courseId) === String(filter.courseId))
  if (filter.moduleId) arr = arr.filter(t => String(t.moduleId) === String(filter.moduleId))
  if (filter.title) arr = arr.filter(t => (t.title || '').toLowerCase().includes(filter.title.toLowerCase()))
  if (filter.description) arr = arr.filter(t => (t.description || '').toLowerCase().includes(filter.description.toLowerCase()))
  arr.sort((a, b) => {
    let aVal, bVal
    if (sortKey.value === 'courseTitle') {
      aVal = courseNameById(a.courseId)
      bVal = courseNameById(b.courseId)
    } else if (sortKey.value === 'moduleTitle') {
      aVal = moduleNameById(a.moduleId)
      bVal = moduleNameById(b.moduleId)
    } else {
      aVal = a[sortKey.value]
      bVal = b[sortKey.value]
    }
    if (aVal == null) return 1
    if (bVal == null) return -1
    if (aVal < bVal) return sortDir.value === 'asc' ? -1 : 1
    if (aVal > bVal) return sortDir.value === 'asc' ? 1 : -1
    return 0
  })
  return arr
})

function startEdit(id: number) {
  editingId.value = id
  const t = testsStore.tests.find(t => t.id === id)
  editTest.value = { ...t }
}
function cancelEdit() { editingId.value = null; editTest.value = {} }
async function saveEdit(id: number) {
  await testsStore.updateTest(id, editTest.value)
  editingId.value = null
  editTest.value = {}
  await testsStore.fetchTests()
}
function startDelete(id: number) { confirmingDeleteId.value = id }
function cancelDelete() { confirmingDeleteId.value = null }
async function confirmDelete(id: number) {
  await testsStore.deleteTest(id)
  confirmingDeleteId.value = null
  await testsStore.fetchTests()
}

onMounted(async () => {
  await testsStore.fetchCourses()
  await testsStore.fetchModules()
  await testsStore.fetchTests()
})
</script>
