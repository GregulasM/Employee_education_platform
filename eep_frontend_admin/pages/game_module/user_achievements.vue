<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Достижения пользователей</h2>
    </div>
    <div class="flex justify-center p-4 gap-4">
      <button class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-green-500/90 hover:bg-green-700/90 font-semibold"
              @click="createMode = !createMode; showFilters = false">
        Выдать достижение
      </button>
      <button
          class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 font-semibold"
          @click="showFilters = !showFilters; createMode = false"
      >
        Сортировка/Фильтрация
      </button>
      <button
          class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-amber-500/90 hover:bg-amber-700/90 font-semibold"
          @click="userAchievementsStore.fetchAllUserAchievements"
      >
        <span v-if="userAchievementsStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>

    <!-- Создание (выдача достижения) -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Выдать достижение пользователю</h3>

      <!-- Переключатель режимов -->
      <div class="flex gap-4 mb-6">
        <label class="flex items-center gap-2 text-white cursor-pointer">
          <input
              type="radio"
              v-model="selectionMode"
              value="id"
              class="radio radio-primary"
          />
          <span>По ID</span>
        </label>
        <label class="flex items-center gap-2 text-white cursor-pointer">
          <input
              type="radio"
              v-model="selectionMode"
              value="dropdown"
              class="radio radio-primary"
          />
          <span>Выбор из списка</span>
        </label>
      </div>

      <form @submit.prevent="submitGiveAchievement" class="text-white">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">

          <!-- Выбор пользователя -->
          <div class="space-y-2">
            <label class="block text-sm font-medium">Пользователь:</label>

            <!-- Режим по ID -->
            <div v-if="selectionMode === 'id'">
              <input
                  v-model.number="newUserAchievement.userId"
                  type="number"
                  class="input input-bordered w-full"
                  placeholder="ID пользователя"
                  min="1"
              />
            </div>

            <!-- Режим dropdown -->
            <div v-else class="relative">
              <div class="relative">
                <input
                    v-model="userSearchQuery"
                    @input="filterUsers"
                    @focus="showUserDropdown = true"
                    @blur="hideUserDropdown"
                    type="text"
                    class="input input-bordered w-full pr-10"
                    placeholder="Поиск пользователя..."
                    autocomplete="off"
                    v-on:click="filterUsers"
                />
                <div class="absolute inset-y-0 right-0 flex items-center pr-3">
                  <svg class="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                  </svg>
                </div>
              </div>

              <!-- Dropdown пользователей -->
              <div v-if="showUserDropdown && filteredUsers.length > 0"
                   class="absolute z-10 w-full mt-1 bg-gray-700 border border-gray-600 rounded-md shadow-lg max-h-60 overflow-y-auto">
                <div
                    v-for="user in filteredUsers"
                    :key="user.id"
                    @mousedown="selectUser(user)"
                    class="px-4 py-2 hover:bg-gray-600 cursor-pointer border-b border-gray-600 last:border-b-0"
                >
                  <div class="font-medium">{{ user.login }}</div>
                  <div class="text-sm text-gray-400">{{ user.email }} | ID: {{ user.id }}</div>
                </div>
              </div>

              <!-- Показать выбранного пользователя -->
              <div v-if="selectedUser" class="mt-2 p-2 bg-gray-700 rounded text-sm">
                <span class="text-green-400">Выбран:</span> {{ selectedUser.login }} (ID: {{ selectedUser.id }})
              </div>
            </div>
          </div>

          <!-- Выбор достижения -->
          <div class="space-y-2">
            <label class="block text-sm font-medium">Достижение:</label>

            <!-- Режим по ID -->
            <div v-if="selectionMode === 'id'">
              <input
                  v-model.number="newUserAchievement.achievementId"
                  type="number"
                  class="input input-bordered w-full"
                  placeholder="ID достижения"
                  min="1"
              />
            </div>

            <!-- Режим dropdown -->
            <div v-else class="relative">
              <div class="relative">
                <input
                    v-model="achievementSearchQuery"
                    @input="filterAchievements"
                    @focus="showAchievementDropdown = true"
                    @blur="hideAchievementDropdown"
                    type="text"
                    class="input input-bordered w-full pr-10"
                    placeholder="Поиск достижения..."
                    autocomplete="off"
                    v-on:click="filterAchievements"
                />
                <div class="absolute inset-y-0 right-0 flex items-center pr-3">
                  <svg class="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                  </svg>
                </div>
              </div>

              <!-- Dropdown достижений -->
              <div v-if="showAchievementDropdown && filteredAchievements.length > 0"
                   class="absolute z-10 w-full mt-1 bg-gray-700 border border-gray-600 rounded-md shadow-lg max-h-60 overflow-y-auto">
                <div
                    v-for="achievement in filteredAchievements"
                    :key="achievement.id"
                    @mousedown="selectAchievement(achievement)"
                    class="px-4 py-2 hover:bg-gray-600 cursor-pointer border-b border-gray-600 last:border-b-0"
                >
                  <div class="flex items-center gap-2">
                    <span v-if="achievement.icon" class="text-lg">{{ achievement.icon }}</span>
                    <div>
                      <div class="font-medium">{{ achievement.name }}</div>
                      <div class="text-sm text-gray-400">
                        {{ achievement.description || 'Без описания' }} | {{ achievement.points }} очков | ID: {{ achievement.id }}
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Показать выбранное достижение -->
              <div v-if="selectedAchievement" class="mt-2 p-2 bg-gray-700 rounded text-sm">
                <span class="text-green-400">Выбрано:</span>
                <span v-if="selectedAchievement.icon" class="mr-1">{{ selectedAchievement.icon }}</span>
                {{ selectedAchievement.name }} ({{ selectedAchievement.points }} очков)
              </div>
            </div>
          </div>
        </div>

        <!-- Кнопки -->
        <div class="flex gap-3 mt-6 justify-end">
          <button class="btn btn-success" type="submit" :disabled="!isFormValid">
            Выдать достижение
          </button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">
            Отмена
          </button>
        </div>
      </form>

      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>

    <!-- Фильтры и сортировка -->
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.userId" class="input input-sm" placeholder="Фильтр по ID пользователя" />
      <input v-model="filter.userLogin" class="input input-sm" placeholder="Фильтр по логину" />
      <input v-model="filter.achievementName" class="input input-sm" placeholder="Фильтр по названию достижения" />
      <input v-model="filter.achievementId" class="input input-sm" placeholder="Фильтр по ID достижения" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID связки</option>
          <option value="userId">ID пользователя</option>
          <option value="achievementId">ID достижения</option>
          <option value="userLogin">Логин пользователя</option>
          <option value="achievementName">Название достижения</option>
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
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID связки</th>
              <th class="whitespace-nowrap px-2 py-3">ID пользователя</th>
              <th class="whitespace-nowrap px-2 py-3">Логин</th>
              <th class="whitespace-nowrap px-2 py-3">ID достижения</th>
              <th class="whitespace-nowrap px-2 py-3">Название достижения</th>
              <th class="whitespace-nowrap px-2 py-3">Очки</th>
              <th class="whitespace-nowrap px-2 py-3">Действия</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="userAchievement in filteredUserAchievements" :key="userAchievement.id">
              <!-- Не в режиме подтверждения удаления -->
              <template v-if="confirmingDeleteId !== userAchievement.id">
                <td class="px-2 py-2">{{ userAchievement.id }}</td>
                <td class="px-2 py-2">{{ userAchievement.userId }}</td>
                <td class="px-2 py-2">{{ userAchievement.user?.login || 'N/A' }}</td>
                <td class="px-2 py-2">{{ userAchievement.achievementId }}</td>
                <td class="px-2 py-2">{{ userAchievement.achievement?.name || 'N/A' }}</td>
                <td class="px-2 py-2">{{ userAchievement.achievement?.points || 0 }}</td>
                <td class="px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-info" @click="viewDetails(userAchievement)">👁️ Подробнее</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(userAchievement.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Подтверждение удаления -->
              <template v-else>
                <td colspan="7" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Убрать достижение у пользователя (ID связки: {{ userAchievement.id }})?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(userAchievement.id)">Точно удалить</button>
                  <button class="btn btn-xs btn-warning ml-1" @click="cancelDelete">Отмена</button>
                </td>
              </template>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Модальное окно с подробностями -->
    <div v-if="showDetailsModal" class="fixed inset-0 bg-black/50 flex items-center justify-center z-50" @click="closeDetailsModal">
      <div class="bg-gray-800 p-6 rounded-lg max-w-md w-full mx-4" @click.stop>
        <h3 class="text-lg font-bold text-white mb-4">Подробности достижения</h3>
        <div v-if="selectedUserAchievement" class="text-white space-y-2">
          <p><strong>ID связки:</strong> {{ selectedUserAchievement.id }}</p>
          <p><strong>Пользователь:</strong> {{ selectedUserAchievement.user?.login }} (ID: {{ selectedUserAchievement.userId }})</p>
          <p><strong>Email:</strong> {{ selectedUserAchievement.user?.email || 'N/A' }}</p>
          <p><strong>Достижение:</strong> {{ selectedUserAchievement.achievement?.name }} (ID: {{ selectedUserAchievement.achievementId }})</p>
          <p><strong>Описание:</strong> {{ selectedUserAchievement.achievement?.description || 'N/A' }}</p>
          <p><strong>Очки:</strong> {{ selectedUserAchievement.achievement?.points || 0 }}</p>
          <p><strong>PublicId:</strong> {{ selectedUserAchievement.achievement?.publicId || 'N/A' }}</p>
          <p><strong>Иконка:</strong> {{ selectedUserAchievement.achievement?.icon || 'N/A' }}</p>
        </div>
        <div class="flex justify-end mt-4">
          <button class="btn btn-primary" @click="closeDetailsModal">Закрыть</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserAchievementsStore } from '~/stores/user_achievements_store'
import type { UserAchievementDto } from '~/stores/user_achievements_store'
import { useUsersStore } from '~/stores/users_store'
import { useAchievementsStore } from '~/stores/achievements_store'

const usersStore = useUsersStore()
const achievementsStore = useAchievementsStore()

const userSearchQuery = ref('')
const showUserDropdown = ref(false)
const selectedUser = ref(null)
const filteredUsers = ref([])

const achievementSearchQuery = ref('')
const showAchievementDropdown = ref(false)
const selectedAchievement = ref(null)
const filteredAchievements = ref([])

const isFormValid = computed(() => {
  if (selectionMode.value === 'id') {
    return newUserAchievement.value.userId > 0 && newUserAchievement.value.achievementId > 0
  } else {
    return selectedUser.value && selectedAchievement.value
  }
})

async function loadDropdownData() {
  try {
    if (usersStore.fetchUsers) {
      await usersStore.fetchUsers()
    }

    if (achievementsStore.fetchAllAchievements) {
      await achievementsStore.fetchAllAchievements()
    }
  } catch (error) {
    console.error('Ошибка загрузки данных для dropdown:', error)
  }
}

function filterUsers() {
  const query = userSearchQuery.value.toLowerCase().trim()
  if (!query) {
    filteredUsers.value = usersStore.users?.slice(0, 60) || []
    return
  }

  filteredUsers.value = (usersStore.users || [])
      .filter(user =>
          user.login?.toLowerCase().includes(query) ||
          user.email?.toLowerCase().includes(query) ||
          user.id.toString().includes(query)
      )
      .slice(0, 60) // Ограничение результатов
}

function filterAchievements() {
  const query = achievementSearchQuery.value.toLowerCase().trim()
  const allAchievements = achievementsStore.allAchievements || []

  if (!query) {
    filteredAchievements.value = allAchievements.slice(0, 60)
    return
  }

  filteredAchievements.value = allAchievements
      .filter(achievement =>
          achievement.name?.toLowerCase().includes(query) ||
          achievement.description?.toLowerCase().includes(query) ||
          achievement.id.toString().includes(query)
      )
      .slice(0, 60)
}


function selectUser(user) {
  selectedUser.value = user
  newUserAchievement.value.userId = user.id
  userSearchQuery.value = user.login
  showUserDropdown.value = false
}

function selectAchievement(achievement) {
  selectedAchievement.value = achievement
  newUserAchievement.value.achievementId = achievement.id
  achievementSearchQuery.value = achievement.name
  showAchievementDropdown.value = false
}

function hideUserDropdown() {
  setTimeout(() => {
    showUserDropdown.value = false
  }, 150)
}

function hideAchievementDropdown() {
  setTimeout(() => {
    showAchievementDropdown.value = false
  }, 150)
}

function resetForm() {
  selectedUser.value = null
  selectedAchievement.value = null
  userSearchQuery.value = ''
  achievementSearchQuery.value = ''
  showUserDropdown.value = false
  showAchievementDropdown.value = false
  Object.assign(newUserAchievement.value, { userId: 0, achievementId: 0 })
}

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  resetForm()
}

async function submitGiveAchievement() {
  try {
    createError.value = null

    let userId, achievementId

    if (selectionMode.value === 'id') {
      userId = newUserAchievement.value.userId
      achievementId = newUserAchievement.value.achievementId

      if (!userId || !achievementId || userId <= 0 || achievementId <= 0) {
        createError.value = 'Введите корректные ID пользователя и достижения'
        return
      }
    } else {
      if (!selectedUser.value || !selectedAchievement.value) {
        createError.value = 'Выберите пользователя и достижение'
        return
      }

      userId = selectedUser.value.id
      achievementId = selectedAchievement.value.id
    }

    const existingAchievement = userAchievementsStore.userAchievementsList.find(
        ua => ua.userId === userId && ua.achievementId === achievementId
    )

    if (existingAchievement) {
      createError.value = 'У данного пользователя уже есть это достижение'
      return
    }

    await userAchievementsStore.createUserAchievement(userId, achievementId)
    closeCreateForm()
    await userAchievementsStore.fetchAllUserAchievements()

  } catch (e: any) {
    createError.value = e?.message || 'Ошибка выдачи достижения'
  }
}


const selectionMode = ref<'id' | 'dropdown'>('id')

const userAchievementsStore = useUserAchievementsStore()
const router = useRouter()

const createMode = ref(false)
const createError = ref<string | null>(null)
const newUserAchievement = ref({
  userId: 0,
  achievementId: 0
})

const confirmingDeleteId = ref<number | null>(null)
const showDetailsModal = ref(false)
const selectedUserAchievement = ref<UserAchievementDto | null>(null)

function go_back() {
  router.back()
}



function viewDetails(userAchievement: UserAchievementDto) {
  selectedUserAchievement.value = userAchievement
  showDetailsModal.value = true
}

function closeDetailsModal() {
  showDetailsModal.value = false
  selectedUserAchievement.value = null
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}

function cancelDelete() {
  confirmingDeleteId.value = null
}

async function confirmDelete(id: number) {
  try {
    await userAchievementsStore.deleteUserAchievementById(id)
    confirmingDeleteId.value = null
    await userAchievementsStore.fetchAllUserAchievements()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

const showFilters = ref(false)
const filter = reactive({
  userId: '',
  userLogin: '',
  achievementName: '',
  achievementId: '',
})

const sortKey = ref<'id' | 'userId' | 'achievementId' | 'userLogin' | 'achievementName'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

function resetFilters() {
  Object.assign(filter, {
    userId: '',
    userLogin: '',
    achievementName: '',
    achievementId: ''
  })
}

const filteredUserAchievements = computed(() => {
  let arr = [...userAchievementsStore.userAchievementsList]

  if (filter.userId) {
    arr = arr.filter(ua => ua.userId.toString().includes(filter.userId))
  }
  if (filter.userLogin) {
    arr = arr.filter(ua => (ua.user?.login || '').toLowerCase().includes(filter.userLogin.toLowerCase()))
  }
  if (filter.achievementName) {
    arr = arr.filter(ua => (ua.achievement?.name || '').toLowerCase().includes(filter.achievementName.toLowerCase()))
  }
  if (filter.achievementId) {
    arr = arr.filter(ua => ua.achievementId.toString().includes(filter.achievementId))
  }

  arr.sort((a, b) => {
    let aVal: any, bVal: any

    switch (sortKey.value) {
      case 'id':
        aVal = a.id
        bVal = b.id
        break
      case 'userId':
        aVal = a.userId
        bVal = b.userId
        break
      case 'achievementId':
        aVal = a.achievementId
        bVal = b.achievementId
        break
      case 'userLogin':
        aVal = a.user?.login || ''
        bVal = b.user?.login || ''
        break
      case 'achievementName':
        aVal = a.achievement?.name || ''
        bVal = b.achievement?.name || ''
        break
      default:
        aVal = a.id
        bVal = b.id
    }

    if (aVal == null) return 1
    if (bVal == null) return -1
    if (aVal < bVal) return sortDir.value === 'asc' ? -1 : 1
    if (aVal > bVal) return sortDir.value === 'asc' ? 1 : -1
    return 0
  })

  return arr
})

watch(selectionMode, async (newMode) => {
  if (newMode === 'dropdown') {
    await loadDropdownData()
  }
  resetForm()
})


  onMounted(async () => {
    await userAchievementsStore.fetchAllUserAchievements()
    await loadDropdownData()
  })
</script>