<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Настройки (Settings)</h2>
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
              @click="settingsStore.fetchSettings">
        <span v-if="settingsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold">
        Назад
      </NuxtLink>
    </div>

    <!-- Создание Setting -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать настройку</h3>
      <form @submit.prevent="submitCreateSetting" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newSetting.type"     class="input input-bordered" placeholder="Тип (type)" />
        <input v-model="newSetting.name"     class="input input-bordered" placeholder="Название (name)" />
        <input v-model="newSetting.value"    class="input input-bordered" placeholder="Значение (value)" />
        <input v-model="newSetting.icon"     class="input input-bordered" placeholder="Иконка (icon)" />
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтр -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.type"   class="input input-sm" placeholder="Фильтр по типу" />
      <input v-model="filter.name"   class="input input-sm" placeholder="Фильтр по названию" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="type">Тип</option>
          <option value="name">Название</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {type:'', name:''})">Сбросить фильтры</button>
    </div>

    <!-- Таблица -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Тип</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Значение</th>
              <th class="whitespace-nowrap px-2 py-3">Иконка</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="setting in filteredSettings" :key="setting.id">
              <template v-if="editingId !== setting.id && confirmingDeleteId !== setting.id">
                <td class="whitespace-nowrap px-2 py-2">{{ setting.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ setting.type }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ setting.name }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <pre class="whitespace-pre-line break-words text-xs">{{ setting.value }}</pre>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="setting.icon" :src="setting.icon" class="w-8 h-8 object-contain" alt="icon" />
                  <span v-else>-</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center items-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(setting.id)">✏️ Изм.</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(setting.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Режим редактирования -->
              <template v-else-if="editingId === setting.id">
                <td class="whitespace-nowrap px-2 py-2">{{ setting.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSetting.type" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSetting.name" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2   py-2"><input v-model="editSetting.value" class="input input-xs w-40" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSetting.icon" class="input input-xs w-16" /></td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(setting.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Подтверждение удаления -->
              <template v-else-if="confirmingDeleteId === setting.id">
                <td colspan="7" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить настройку #{{ setting.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(setting.id)">Точно удалить</button>
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
import { useSettingsStore } from '~/stores/settings_store'

const router = useRouter()
const settingsStore = useSettingsStore()

const editingId = ref<number|null>(null)
const editSetting = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

function startEdit(id: number) {
  editingId.value = id
  const setting = settingsStore.settings.find(s => s.id === id)
  editSetting.value = { ...setting }
}

function cancelEdit() {
  editingId.value = null
  editSetting.value = {}
}

async function saveEdit(id: number) {
  await settingsStore.updateSetting(id, editSetting.value)
  editingId.value = null
  editSetting.value = {}
  await settingsStore.fetchSettings()
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  await settingsStore.deleteSetting(id)
  confirmingDeleteId.value = null
  await settingsStore.fetchSettings()
}

const showFilters = ref(false)
const filter = reactive({
  type: '',
  name: '',
})
const sortKey = ref<'id' | 'type' | 'name'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredSettings = computed(() => {
  let arr = [...settingsStore.settings]
  if (filter.type) arr = arr.filter(s => (s.type || '').toLowerCase().includes(filter.type.toLowerCase()))
  if (filter.name) arr = arr.filter(s => (s.name || '').toLowerCase().includes(filter.name.toLowerCase()))
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
const newSetting = ref({
  type: '', name: '', value: '', icon: '', isActive: true
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newSetting.value, { type: '', name: '', value: '', icon: ''})
}

async function submitCreateSetting() {
  try {
    createError.value = null
    await settingsStore.createSetting({ ...newSetting.value })
    closeCreateForm()
    await settingsStore.fetchSettings()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания настройки'
  }
}

onMounted(() => settingsStore.fetchSettings())
</script>
