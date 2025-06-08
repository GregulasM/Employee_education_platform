<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Отделы</h2>
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
              @click="departmentsStore.fetchDepartments">
        <span v-if="departmentsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold">
        Назад
      </NuxtLink>
    </div>
    <!-- Создание -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-3xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать отдел</h3>
      <form @submit.prevent="submitCreateDepartment" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newDepartment.name" class="input input-bordered" placeholder="Название отдела" />
        <input v-model="newDepartment.description" class="input input-bordered" placeholder="Описание" />
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>
    <!-- Фильтр -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.name" class="input input-sm" placeholder="Фильтр по названию" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="name">Название</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {name:''})">Сбросить фильтры</button>
    </div>
    <!-- Таблица -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[700px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="department in filteredDepartments" :key="department.id">
              <!-- Не в режиме редактирования -->
              <template v-if="editingId !== department.id && confirmingDeleteId !== department.id">
                <td class="whitespace-nowrap px-2 py-2">{{ department.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ department.name }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ department.description }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center items-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(department.id)">✏️ Изм.</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(department.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Редактирование -->
              <template v-else-if="editingId === department.id">
                <td class="whitespace-nowrap px-2 py-2">{{ department.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editDepartment.name" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editDepartment.description" class="input input-xs w-40" /></td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(department.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Подтверждение удаления -->
              <template v-else-if="confirmingDeleteId === department.id">
                <td colspan="4" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить отдел #{{ department.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(department.id)">Точно удалить</button>
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
import { useDepartmentsStore } from '~/stores/departments_store'

const router = useRouter()
const departmentsStore = useDepartmentsStore()

const editingId = ref<number|null>(null)
const editDepartment = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

function startEdit(id: number) {
  editingId.value = id
  const department = departmentsStore.departments.find(d => d.id === id)
  editDepartment.value = { ...department }
}

function cancelEdit() {
  editingId.value = null
  editDepartment.value = {}
}

async function saveEdit(id: number) {
  await departmentsStore.updateDepartment(id, editDepartment.value)
  editingId.value = null
  editDepartment.value = {}
  await departmentsStore.fetchDepartments()
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  await departmentsStore.deleteDepartment(id)
  confirmingDeleteId.value = null
  await departmentsStore.fetchDepartments()
}

const showFilters = ref(false)
const filter = reactive({
  name: '',
})
const sortKey = ref<'id' | 'name'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredDepartments = computed(() => {
  let arr = [...departmentsStore.departments]
  if (filter.name) arr = arr.filter(d => (d.name || '').toLowerCase().includes(filter.name.toLowerCase()))
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
const newDepartment = ref({
  name: '', description: ''
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newDepartment.value, { name: '', description: '' })
}

async function submitCreateDepartment() {
  try {
    createError.value = null
    await departmentsStore.createDepartment({ ...newDepartment.value })
    closeCreateForm()
    await departmentsStore.fetchDepartments()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания отдела'
  }
}

onMounted(() => departmentsStore.fetchDepartments())
</script>
