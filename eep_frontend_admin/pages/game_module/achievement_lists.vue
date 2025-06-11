<template>
  <div class="pb-4">
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Листы достижений</h2>
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
          @click="achievementListsStore.fetchLists"
      >
        <span v-if="achievementListsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-3xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать лист достижений</h3>
      <form @submit.prevent="submitCreateList" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newList.name"        class="input input-bordered" placeholder="Название" />
        <input v-model="newList.description" class="input input-bordered" placeholder="Описание" />
        <input v-model="newList.icon"        class="input input-bordered" placeholder="Ссылка на иконку" />
        <input v-model.number="newList.sortType" class="input input-bordered" placeholder="Тип сортировки (число)" type="number"/>
        <label class="flex items-center gap-2">
          <input type="checkbox" v-model="newList.isHidden" class="checkbox" /> Скрыт?
        </label>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.name"        class="input input-sm" placeholder="Фильтр по названию" />
      <input v-model="filter.description" class="input input-sm" placeholder="Фильтр по описанию" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="name">Название</option>
          <option value="createdAt">Дата создания</option>
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
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Иконка</th>
              <th class="whitespace-nowrap px-2 py-3">Тип сортировки</th>
              <th class="whitespace-nowrap px-2 py-3">Скрыт?</th>
              <th class="whitespace-nowrap px-2 py-3">Достижений</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="list in filteredLists" :key="list.id">
              <template v-if="editingId !== list.id && confirmingDeleteId !== list.id">
                <td class="whitespace-nowrap px-2 py-2">{{ list.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ list.name }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ list.description }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="list.icon" :src="list.icon" class="w-10 h-10 rounded object-cover" alt="icon" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ list.sortType }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <span v-if="list.isHidden">Да</span>
                  <span v-else>Нет</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  {{ countAchievementsForList(list.id) }}
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(list.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(list.id)">Удалить 🗑️</button>
                </td>
              </template>
              <template v-else-if="editingId === list.id">
                <td class="whitespace-nowrap px-2 py-2">{{ list.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editList.name" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editList.description" class="input input-xs w-40" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editList.icon" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editList.sortType" type="number" class="input input-xs w-16" /></td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input type="checkbox" v-model="editList.isHidden" class="checkbox" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ list.achievements?.length || 0 }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(list.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <template v-else-if="confirmingDeleteId === list.id">
                <td colspan="8" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить лист достижений #{{ list.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(list.id)">Точно удалить</button>
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
import { useAchievementListsStore } from '~/stores/achievement_lists_store'
const router = useRouter()
const achievementListsStore = useAchievementListsStore()

const editingId = ref<number|null>(null)
const editList = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

import { useAchievementsStore } from '~/stores/achievements_store'
const achievementsStore = useAchievementsStore()

function countAchievementsForList(listId: number) {
  return achievementsStore.allAchievements.filter(a => a.listId === listId).length
}

function startEdit(id: number) {
  editingId.value = id
  const list = achievementListsStore.lists.find(l => l.id === id)
  editList.value = { ...list }
}
function cancelEdit() {
  editingId.value = null
  editList.value = {}
}
async function saveEdit(id: number) {
  await achievementListsStore.updateList(id, editList.value)
  editingId.value = null
  editList.value = {}
  await achievementListsStore.fetchLists()
}
function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await achievementListsStore.deleteList(id)
    confirmingDeleteId.value = null
    await achievementListsStore.fetchLists()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}
const showFilters = ref(false)
const filter = reactive({
  name: '',
  description: '',
})
const sortKey = ref<'id' | 'name' | 'createdAt'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredLists = computed(() => {
  let arr = [...achievementListsStore.lists]
  if (filter.name) arr = arr.filter(l => (l.name || '').toLowerCase().includes(filter.name.toLowerCase()))
  if (filter.description) arr = arr.filter(l => (l.description || '').toLowerCase().includes(filter.description.toLowerCase()))
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
const newList = ref({
  name: '', description: '', icon: '', sortType: null, isHidden: false
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newList.value, {
    name: '', description: '', icon: '', sortType: null, isHidden: false
  })
}
async function submitCreateList() {
  try {
    createError.value = null
    await achievementListsStore.createList({ ...newList.value })
    closeCreateForm()
    await achievementListsStore.fetchLists()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания листа'
  }
}
onMounted(() => {
  achievementListsStore.fetchLists()
  achievementsStore.fetchAllAchievements()
})
</script>
