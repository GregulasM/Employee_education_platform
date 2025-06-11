<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Персонажи</h2>
    </div>
    <div class="flex justify-center p-4 gap-4">
      <button
          class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-green-500/90 hover:bg-green-700/90 font-semibold"
          @click="createMode = !createMode; showFilters = false"
      >
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
          @click="charactersStore.fetchCharacters"
      >
        <span v-if="charactersStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink
          draggable="false"
          @click="go_back"
          class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <!-- Создание персонажа -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать персонажа</h3>
      <form @submit.prevent="submitCreateCharacter" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newCharacter.name" class="input input-bordered" placeholder="Имя персонажа" />
        <input v-model="newCharacter.avatar" class="input input-bordered" placeholder="Ссылка на аватарку" />
        <input v-model="newCharacter.rarity" class="input input-bordered" placeholder="Редкость" />
        <input v-model="newCharacter.unlockCond" class="input input-bordered" placeholder="Условие разблокировки" />
        <input v-model="newCharacter.baseStats" class="input input-bordered" placeholder="BaseStats (json)" />
        <input v-model="newCharacter.cosmetics" class="input input-bordered" placeholder="Cosmetics (json)" />
        <input v-model="newCharacter.skills" class="input input-bordered" placeholder="Skills (json)" />
        <textarea v-model="newCharacter.description" class="textarea textarea-bordered col-span-1 md:col-span-2" placeholder="Описание"></textarea>
        <label class="flex items-center gap-2">
          <input type="checkbox" v-model="newCharacter.default" class="checkbox" /> По умолчанию?
        </label>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтры -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.name" class="input input-sm" placeholder="Фильтр по имени" />
      <input v-model="filter.rarity" class="input input-sm" placeholder="Фильтр по редкости" />
      <input v-model="filter.description" class="input input-sm" placeholder="Фильтр по описанию" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="name">Имя</option>
          <option value="rarity">Редкость</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {name:'', rarity:'', description:''})">Сбросить фильтры</button>
    </div>

    <!-- Таблица персонажей -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[1200px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Имя</th>
              <th class="whitespace-nowrap px-2 py-3">Аватар</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Редкость</th>
              <th class="whitespace-nowrap px-2 py-3">Default</th>
              <th class="whitespace-nowrap px-2 py-3">Условие разблокировки</th>
              <th class="whitespace-nowrap px-2 py-3">BaseStats</th>
              <th class="whitespace-nowrap px-2 py-3">Cosmetics</th>
              <th class="whitespace-nowrap px-2 py-3">Skills</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="char in filteredCharacters" :key="char.id">
              <!-- Не в режиме редактирования -->
              <template v-if="editingId !== char.id && confirmingDeleteId !== char.id">
                <td class="whitespace-nowrap px-2 py-2">{{ char.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ char.name }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="char.avatar" :src="char.avatar" class="w-10 h-10 rounded object-cover" alt="avatar" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ char.description }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ char.rarity }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ char.default ? 'Да' : 'Нет' }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ char.unlockCond }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <span class="truncate block max-w-[140px]" :title="char.baseStats">{{ char.baseStats }}</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <span class="truncate block max-w-[120px]" :title="char.cosmetics">{{ char.cosmetics }}</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <span class="truncate block max-w-[120px]" :title="char.skills">{{ char.skills }}</span>
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(char.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(char.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Режим редактирования -->
              <template v-else-if="editingId === char.id">
                <td class="whitespace-nowrap px-2 py-2">{{ char.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.name" class="input input-xs w-28" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.avatar" class="input input-xs w-24" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.description" class="input input-xs w-48" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.rarity" class="input input-xs w-16" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input type="checkbox" v-model="editCharacter.default" class="checkbox" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.unlockCond" class="input input-xs w-28" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.baseStats" class="input input-xs w-28" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.cosmetics" class="input input-xs w-20" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">
                  <input v-model="editCharacter.skills" class="input input-xs w-20" />
                </td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(char.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Режим подтверждения удаления -->
              <template v-else-if="confirmingDeleteId === char.id">
                <td colspan="11" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить персонажа #{{ char.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(char.id)">Точно удалить</button>
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
import { useCharactersStore } from '~/stores/characters_store'

const router = useRouter()
const charactersStore = useCharactersStore()

const editingId = ref<number | null>(null)
const editCharacter = ref<any>({})
const confirmingDeleteId = ref<number | null>(null)

function go_back() {
  router.back()
}

function startEdit(id: number) {
  editingId.value = id
  const char = charactersStore.characters.find(c => c.id === id)
  editCharacter.value = { ...char }
}
function cancelEdit() {
  editingId.value = null
  editCharacter.value = {}
}
async function saveEdit(id: number) {
  await charactersStore.updateCharacter(id, editCharacter.value)
  editingId.value = null
  editCharacter.value = {}
  await charactersStore.fetchCharacters()
}
function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await charactersStore.deleteCharacter(id)
    confirmingDeleteId.value = null
    await charactersStore.fetchCharacters()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}
const showFilters = ref(false)
const filter = reactive({
  name: '',
  rarity: '',
  description: '',
})
const sortKey = ref<'id' | 'name' | 'rarity'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredCharacters = computed(() => {
  let arr = [...charactersStore.characters]
  if (filter.name) arr = arr.filter(c => (c.name || '').toLowerCase().includes(filter.name.toLowerCase()))
  if (filter.rarity) arr = arr.filter(c => (c.rarity || '').toLowerCase().includes(filter.rarity.toLowerCase()))
  if (filter.description) arr = arr.filter(c => (c.description || '').toLowerCase().includes(filter.description.toLowerCase()))
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
const newCharacter = ref({
  name: '', avatar: '', description: '', baseStats: '', cosmetics: '', skills: '', default: false, rarity: '', unlockCond: ''
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newCharacter.value, {
    name: '', avatar: '', description: '', baseStats: '', cosmetics: '', skills: '', default: false, rarity: '', unlockCond: ''
  })
}
async function submitCreateCharacter() {
  try {
    createError.value = null
    await charactersStore.createCharacter({ ...newCharacter.value })
    closeCreateForm()
    await charactersStore.fetchCharacters()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания персонажа'
  }
}
onMounted(() => charactersStore.fetchCharacters())
</script>
