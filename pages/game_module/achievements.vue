<template>
  <div class="pb-4">
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Достижения</h2>
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
          @click="refreshAchievements"
      >
        <span v-if="achievementsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <div v-if="achievementsStore.achievementLists.length === 0" class="text-red-400 text-center my-6">
      Не удалось загрузить листы достижений. Создание достижений невозможно.
    </div>

    <!-- Создание достижения -->
    <div v-if="createMode && achievementsStore.achievementLists.length > 0" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-3xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать достижение</h3>
      <form @submit.prevent="submitCreateAchievement" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <label class="md:col-span-2">
          <span>Лист достижений:</span>
          <select v-model="selectedListId" class="input input-bordered w-full mt-1">
            <option v-for="list in achievementsStore.achievementLists" :key="list.id" :value="list.id">
              {{ list.name }}
            </option>
          </select>
        </label>
        <input v-model="newAchievement.name"        class="input input-bordered" placeholder="Название достижения" required />
        <input v-model="newAchievement.icon"        class="input input-bordered" placeholder="Ссылка на иконку"/>
        <input v-model.number="newAchievement.points" class="input input-bordered" placeholder="Баллы" type="number"/>
        <textarea v-model="newAchievement.description" class="textarea textarea-bordered col-span-1 md:col-span-2" placeholder="Описание"></textarea>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтры -->
    <div v-if="showFilters && achievementsStore.achievementLists.length > 0" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <select v-model="filter.listId" class="input input-sm">
        <option value="">Все листы</option>
        <option v-for="list in achievementsStore.achievementLists" :key="list.id" :value="list.id">{{ list.name }}</option>
      </select>
      <input v-model="filter.name"      class="input input-sm" placeholder="Фильтр по названию" />
      <input v-model="filter.description" class="input input-sm" placeholder="Фильтр по описанию" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="name">Название</option>
          <option value="points">Баллы</option>
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

    <!-- Таблица достижений -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Лист</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Иконка</th>
              <th class="whitespace-nowrap px-2 py-3">Баллы</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="ach in filteredAchievements" :key="ach.id">
              <!-- Не в режиме редактирования -->
              <template v-if="editingId !== ach.id && confirmingDeleteId !== ach.id">
                <td class="whitespace-nowrap px-2 py-2">{{ ach.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  {{ listNameById(ach.listId) }}
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ ach.name }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ ach.description }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="ach.icon" :src="ach.icon" class="w-10 h-10 rounded object-cover" alt="icon" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ ach.points }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(ach.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(ach.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Режим редактирования -->
              <template v-else-if="editingId === ach.id">
                <td class="whitespace-nowrap px-2 py-2">{{ ach.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ listNameById(editAchievement.listId) }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editAchievement.name" class="input input-xs w-32" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editAchievement.description" class="input input-xs w-48" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editAchievement.icon" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editAchievement.points" type="number" class="input input-xs w-14" /></td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(ach.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Режим подтверждения удаления -->
              <template v-else-if="confirmingDeleteId === ach.id">
                <td colspan="7" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить достижение #{{ ach.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(ach.id)">Точно удалить</button>
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
import { useAchievementsStore } from '~/stores/achievements_store'

const router = useRouter()
const achievementsStore = useAchievementsStore()

const createMode = ref(false)
const showFilters = ref(false)
const createError = ref<string | null>(null)
const selectedListId = ref<number|null>(null)
const newAchievement = ref({
  name: '', description: '', icon: '', points: null
})

const editingId = ref<number|null>(null)
const editAchievement = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

const filter = reactive({
  listId: '',
  name: '',
  description: '',
})

const sortKey = ref<'id'|'name'|'points'>('id')
const sortDir = ref<'asc'|'desc'>('asc')

function go_back() {
  router.back()
}

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newAchievement.value, {
    name: '', description: '', icon: '', points: null
  })
  selectedListId.value = null
}

async function submitCreateAchievement() {
  try {
    createError.value = null
    if (!selectedListId.value) {
      createError.value = 'Выберите лист достижений!'
      return
    }
    await achievementsStore.createAchievement(selectedListId.value, { ...newAchievement.value })
    closeCreateForm()
    // selectedListId не трогаем, после создания сразу обновляется список этого листа через store
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания достижения'
  }
}

const filteredAchievements = computed(() => {
  let arr = [...achievementsStore.allAchievements]

  // Если форма создания активна — фильтруем по выбранному списку в форме создания!
  if (createMode.value && selectedListId.value) {
    arr = arr.filter(a => a.listId === +selectedListId.value)
  }
  // Если форма создания не активна — фильтруем по фильтру
  else if (!createMode.value && filter.listId) {
    arr = arr.filter(a => a.listId === +filter.listId)
  }
  if (filter.name) arr = arr.filter(a => (a.name || '').toLowerCase().includes(filter.name.toLowerCase()))
  if (filter.description) arr = arr.filter(a => (a.description || '').toLowerCase().includes(filter.description.toLowerCase()))
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

function listNameById(listId: number) {
  const list = achievementsStore.achievementLists.find(l => l.id === listId)
  return list ? list.name : '—'
}

// CRUD (редактирование/удаление)
function startEdit(id: number) {
  editingId.value = id
  const ach = achievementsStore.achievements.find(a => a.id === id)
  editAchievement.value = { ...ach }
}
function cancelEdit() {
  editingId.value = null
  editAchievement.value = {}
}
async function saveEdit(id: number) {
  const listId = editAchievement.value.listId
  await achievementsStore.updateAchievement(listId, id, editAchievement.value)
  editingId.value = null
  editAchievement.value = {}
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  const ach = achievementsStore.achievements.find(a => a.id === id)
  if (!ach) return
  await achievementsStore.deleteAchievement(ach.listId, id)
  confirmingDeleteId.value = null
}

// Загрузка листов и автоустановка selectedListId
onMounted(async () => {
  await achievementsStore.fetchAchievementLists()
  await achievementsStore.fetchAllAchievements()
  await achievementsStore.fetchAchievementLists()
  await achievementsStore.fetchAllAchievements()
  if (achievementsStore.achievementLists.length > 0) {
    if (!selectedListId.value || !achievementsStore.achievementLists.some(l => l.id === selectedListId.value)) {
      selectedListId.value = achievementsStore.achievementLists[0].id
    }
  }
})
watch(selectedListId, async (val) => {
  if (val) {
    await achievementsStore.fetchAchievements(val)
  }
})
</script>
