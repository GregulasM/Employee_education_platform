<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Роли пользователей</h2>
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
          @click="rolesStore.fetchRoles"
      >
        <span v-if="rolesStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <!-- Создание -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-2xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать роль</h3>
      <form @submit.prevent="submitCreateRole" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <label class="md:col-span-1">
          <span>Название роли:</span>
          <input v-model="newRole.name" class="input input-bordered w-full mt-1" placeholder="Название" />
        </label>
        <label class="md:col-span-1">
          <span>Описание:</span>
          <input v-model="newRole.description" class="input input-bordered w-full mt-1" placeholder="Описание" />
        </label>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтры и сортировка -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.name" class="input input-sm" placeholder="Фильтр по названию" />
      <input v-model="filter.description" class="input input-sm" placeholder="Фильтр по описанию" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="name">Название</option>
          <option value="description">Описание</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {name:'', description:''})">Сбросить фильтры</button>
    </div>

    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[700px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Действия</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="role in filteredRoles" :key="role.id">
              <!-- Не в режиме редактирования -->
              <template v-if="editingId !== role.id && confirmingDeleteId !== role.id">
                <td class="px-2 py-2">{{ role.id }}</td>
                <td class="px-2 py-2">{{ role.name }}</td>
                <td class="px-2 py-2">{{ role.description }}</td>
                <td class="px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-warning" @click="startEdit(role)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(role.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Редактирование -->
              <template v-else-if="editingId === role.id">
                <td class="px-2 py-2">{{ role.id }}</td>
                <td class="px-2 py-2"><input v-model="editRole.name" class="input input-xs w-28" /></td>
                <td class="px-2 py-2"><input v-model="editRole.description" class="input input-xs w-40" /></td>
                <td class="px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(role.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Подтверждение удаления -->
              <template v-else-if="confirmingDeleteId === role.id">
                <td colspan="4" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить роль #{{ role.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(role.id)">Точно удалить</button>
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
import { useUserRolesStore } from '~/stores/user_roles_store'
const rolesStore = useUserRolesStore()
const router = useRouter()

const createMode = ref(false)
const createError = ref<string | null>(null)
const newRole = ref({
  name: '',
  description: ''
})

const editingId = ref<number | null>(null)
const editRole = ref<any>({})
const confirmingDeleteId = ref<number | null>(null)

function go_back() {
  router.back()
}

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newRole.value, { name: '', description: '' })
}

async function submitCreateRole() {
  try {
    createError.value = null
    await rolesStore.createRole({ ...newRole.value })
    closeCreateForm()
    await rolesStore.fetchRoles()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания роли'
  }
}

function startEdit(role: any) {
  editingId.value = role.id
  editRole.value = { ...role }
}
function cancelEdit() {
  editingId.value = null
  editRole.value = {}
}
async function saveEdit(id: number) {
  await rolesStore.updateRole(id, editRole.value)
  editingId.value = null
  editRole.value = {}
  await rolesStore.fetchRoles()
}
function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await rolesStore.deleteRole(id)
    confirmingDeleteId.value = null
    await rolesStore.fetchRoles()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

// Фильтры и сортировка
const showFilters = ref(false)
const filter = reactive({
  name: '',
  description: '',
})
const sortKey = ref<'id' | 'name' | 'description'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredRoles = computed(() => {
  let arr = [...rolesStore.roles]
  if (filter.name) arr = arr.filter(r => (r.name || '').toLowerCase().includes(filter.name.toLowerCase()))
  if (filter.description) arr = arr.filter(r => (r.description || '').toLowerCase().includes(filter.description.toLowerCase()))
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

onMounted(async () => {
  await rolesStore.fetchRoles()
})
</script>
