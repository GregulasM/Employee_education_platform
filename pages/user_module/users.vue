<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Пользователи</h2>
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
          @click="usersStore.fetchUsers"
      >
        <span v-if="usersStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать пользователя</h3>
      <form @submit.prevent="submitCreateUser" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <label>
          <span>Логин:</span>
          <input v-model="newUser.login" class="input input-bordered w-full mt-1" placeholder="Логин" />
        </label>
        <label>
          <span>Пароль:</span>
          <input v-model="newUser.password" class="input input-bordered w-full mt-1" placeholder="Пароль" type="password"/>
        </label>
        <label>
          <span>Имя:</span>
          <input v-model="newUser.firstName" class="input input-bordered w-full mt-1" placeholder="Имя" />
        </label>
        <label>
          <span>Отчество:</span>
          <input v-model="newUser.secondName" class="input input-bordered w-full mt-1" placeholder="Отчество" />
        </label>
        <label>
          <span>Фамилия:</span>
          <input v-model="newUser.lastName" class="input input-bordered w-full mt-1" placeholder="Фамилия" />
        </label>
        <label>
          <span>Эл. почта:</span>
          <input v-model="newUser.email" class="input input-bordered w-full mt-1" placeholder="Эл. почта" type="email"/>
        </label>
        <label>
          <span>Номер телефона:</span>
          <input v-model="newUser.phoneNumber" class="input input-bordered w-full mt-1" placeholder="Номер телефона"/>
        </label>
        <label>
          <span>Аватарка (URL):</span>
          <input v-model="newUser.avatar" class="input input-bordered w-full mt-1" placeholder="Ссылка на аватарку"/>
        </label>
        <label>
          <span>Рейтинг:</span>
          <input v-model.number="newUser.rating" class="input input-bordered w-full mt-1" placeholder="Рейтинг" type="number"/>
        </label>

        <label>
          <span>Тема:</span>
          <select v-model.number="newUser.themeId" class="input input-bordered w-full mt-1">
            <option value="" disabled selected>Выберите тему</option>
            <option v-for="theme in settingsStore.themes" :key="theme.id" :value="theme.id">
              {{ theme.name }}
            </option>
          </select>
        </label>

        <label>
          <span>Шрифт:</span>
          <select v-model.number="newUser.fontId" class="input input-bordered w-full mt-1">
            <option value="" disabled selected>Выберите шрифт</option>
            <option v-for="font in settingsStore.fonts" :key="font.id" :value="font.id">
              {{ font.name }}
            </option>
          </select>
        </label>

        <label>
          <span>Активный курс:</span>
          <select v-model.number="newUser.activeCourseId" class="input input-bordered w-full mt-1">
            <option value="" disabled selected>Выберите курс</option>
            <option v-for="c in coursesStore.courses" :key="c.id" :value="c.id">
              {{ c.title }}
            </option>
          </select>
        </label>

        <label>
          <span>Персонаж:</span>
          <select v-model.number="newUser.selectedCharacterId" class="input input-bordered w-full mt-1">
            <option value="" disabled selected>Выберите персонажа</option>
            <option v-for="char in charactersStore.characters" :key="char.id" :value="char.id">
              {{ char.name }}
            </option>
          </select>
        </label>

        <label>
          <span>Отдел:</span>
          <select v-model.number="newUser.departmentId" class="input input-bordered w-full mt-1">
            <option value="" disabled selected>Выберите отдел</option>
            <option v-for="dep in departmentsStore.departments" :key="dep.id" :value="dep.id">
              {{ dep.name }} (id:{{ dep.id }})
            </option>
          </select>
        </label>

        <label>
          <span>Роль:</span>
          <select v-model.number="newUser.roleId" class="input input-bordered w-full mt-1">
            <option value="" disabled selected>Выберите роль</option>
             <option v-for="role in userRolesStore.roles" :key="role.id" :value="role.id">{{ role.name }}</option>
          </select>
        </label>

        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.login"      class="input input-sm" placeholder="Фильтр по логину" />
      <input v-model="filter.firstName"  class="input input-sm" placeholder="Фильтр по имени" />
      <input v-model="filter.role"       class="input input-sm" placeholder="Фильтр по роли" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="login">Логин</option>
          <option value="firstName">Имя</option>
          <option value="createdAt">Дата создания</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {login:'', firstName:'', role:''})">Сбросить фильтры</button>
    </div>
    <!-- Прокрутка! -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[1200px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Логин</th>
              <th class="whitespace-nowrap px-2 py-3">Имя</th>
              <th class="whitespace-nowrap px-2 py-3">Отчество</th>
              <th class="whitespace-nowrap px-2 py-3">Фамилия</th>
              <th class="whitespace-nowrap px-2 py-3">Эл. почта</th>
              <th class="whitespace-nowrap px-2 py-3">Номер телефона</th>
              <th class="whitespace-nowrap px-2 py-3">Аватарка</th>
              <th class="whitespace-nowrap px-2 py-3">Рейтинг</th>
              <th class="whitespace-nowrap px-2 py-3">Тема</th>
              <th class="whitespace-nowrap px-2 py-3">Шрифт</th>
              <th class="whitespace-nowrap px-2 py-3">Активный (тек.) курс</th>
              <th class="whitespace-nowrap px-2 py-3">Выбранные курсы</th>
              <th class="whitespace-nowrap px-2 py-3">Отдел</th>
              <th class="whitespace-nowrap px-2 py-3">Роль</th>
              <th class="whitespace-nowrap px-2 py-3">Достижения</th>
              <th class="whitespace-nowrap px-2 py-3">Персонаж</th>
              <th class="whitespace-nowrap px-2 py-3">Дата создания</th>
              <th class="whitespace-nowrap px-2 py-3">Дата обновления</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="user in filteredUsers" :key="user.id">
              <!-- Не в режиме редактирования -->
              <template v-if="editingId !== user.id && confirmingDeleteId !== user.id">
                <td class="whitespace-nowrap px-2 py-2">{{ user.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.login }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.firstName }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.secondName }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.lastName }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.email }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.phoneNumber }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="user.avatar" :src="user.avatar" class="w-10 h-10 rounded-full object-cover" alt="avatar" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.rating }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.theme?.name || user.themeId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.font?.name || user.fontId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.activeCourse?.title || user.activeCourseId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.chosenCourses?.length || 0 }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.department?.name || user.departmentId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.role?.name || user.roleId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.achievements?.length || 0 }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.selectedCharacter?.name || user.selectedCharacterId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center align-centre items-center place-content-center place-self-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(user.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(user.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Режим редактирования -->
              <template v-else-if="editingId === user.id">
                <td class="whitespace-nowrap px-2 py-2">{{ user.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.login" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.firstName" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.secondName" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.lastName" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.email" class="input input-xs w-36" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.phoneNumber" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.avatar" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editUser.rating" type="number" class="input input-xs w-16" /></td>
                <!-- Theme -->
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editUser.themeId" class="input input-xs w-28">
                    <option value="">Нет</option>
                    <option
                        v-for="theme in settingsStore.themes"
                        :key="theme.id"
                        :value="theme.id"
                    >
                      {{ theme.name }}
                    </option>
                  </select>
                </td>

                <!-- Font -->
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editUser.fontId" class="input input-xs w-28">
                    <option value="">Нет</option>
                    <option
                        v-for="font in settingsStore.fonts"
                        :key="font.id"
                        :value="font.id"
                    >
                      {{ font.name }}
                    </option>
                  </select>
                </td>
                <!-- Активный курс -->
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editUser.activeCourseId" class="input input-xs w-32">
                    <option value="">Нет</option>
                    <option v-for="c in coursesStore.courses" :key="c.id" :value="c.id">
                      {{ c.title }}
                    </option>
                  </select>
                </td>
                <!-- Выбранные курсы (если надо — просто число или как-то иначе) -->
                <td class="whitespace-nowrap px-2 py-2">—</td>
                <!-- Отдел -->
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editUser.departmentId" class="input input-xs w-32">
                    <option value="" disabled>Выберите отдел</option>
                    <option v-for="dep in departmentsStore.departments" :key="dep.id" :value="dep.id">
                      {{ dep.name }} (id:{{ dep.id }})
                    </option>
                  </select>
                </td>
                <!-- Роль -->
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editUser.roleId" class="input input-xs w-32">
                    <option value="" disabled>Выберите роль</option>
                    <option v-for="role in userRolesStore.roles" :key="role.id" :value="role.id">
                      {{ role.name }}
                    </option>
                  </select>
                </td>
                <!-- Достижения -->
                <td class="whitespace-nowrap px-2 py-2">—</td>
                <!-- Персонаж -->
                <td class="whitespace-nowrap px-2 py-2">
                  <select v-model.number="editUser.selectedCharacterId" class="input input-xs w-28">
                    <option value="">Нет</option>
                    <option v-for="char in charactersStore.characters" :key="char.id" :value="char.id">
                      {{ char.name }}
                    </option>
                  </select>
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ user.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(user.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Режим подтверждения удаления -->
              <template v-else-if="confirmingDeleteId === user.id">
                <td colspan="20" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить пользователя #{{ user.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(user.id)">Точно удалить</button>
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
import { useUsersStore } from '~/stores/users_store'
import { useDepartmentsStore } from '~/stores/departments_store'
import { useUserRolesStore } from '~/stores/user_roles_store'
import { useCharactersStore } from '~/stores/characters_store'
import { useCoursesStore } from '~/stores/courses_store'
import { useSettingsStore } from '~/stores/settings_store'



const router = useRouter()

const usersStore = useUsersStore()
const departmentsStore = useDepartmentsStore()
const charactersStore = useCharactersStore()
const coursesStore = useCoursesStore()
const settingsStore = useSettingsStore()
const userRolesStore = useUserRolesStore()

const editingId = ref<number|null>(null)
const editUser = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

console.log(usersStore.users)

function go_back () {
  router.back()
}

function startEdit(id: number) {
  editingId.value = id
  const user = usersStore.users.find(u => u.id === id)
  editUser.value = { ...user }
}

function cancelEdit() {
  editingId.value = null
  editUser.value = {}
}

async function saveEdit(id: number) {

    await usersStore.updateUser(id, editUser.value)
    editingId.value = null
    editUser.value = {}
    await usersStore.fetchUsers()

}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await usersStore.deleteUser(id)
    confirmingDeleteId.value = null
    await usersStore.fetchUsers()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

const showFilters = ref(false)
const filter = reactive({
  login: '',
  firstName: '',
  role: '',
})
const sortKey = ref<'id' | 'login' | 'firstName' | 'createdAt'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredUsers = computed(() => {
  let arr = [...usersStore.users]
  // Фильтрация
  if (filter.login) arr = arr.filter(u => (u.login || '').toLowerCase().includes(filter.login.toLowerCase()))
  if (filter.firstName) arr = arr.filter(u => (u.firstName || '').toLowerCase().includes(filter.firstName.toLowerCase()))
  if (filter.role) arr = arr.filter(u => (u.role?.name || '').toLowerCase().includes(filter.role.toLowerCase()))
  // Сортировка
  arr.sort((a, b) => {
    let aVal = a[sortKey.value]
    let bVal = b[sortKey.value]
    // null guard
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
const newUser = ref({
  login: '', password: '', firstName: '', secondName: '', lastName: '', email: '', phoneNumber: '', avatar: '',
  rating: null, themeId: null, fontId: null, activeCourseId: null, selectedCharacterId: null, departmentId: null, roleId: null
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newUser.value, {
    login: '', password: '', firstName: '', secondName: '', lastName: '', email: '', phoneNumber: '', avatar: '',
    rating: null, themeId: null, fontId: null, activeCourseId: null, selectedCharacterId: null, departmentId: null, roleId: null
  })
}

async function submitCreateUser() {
  try {
    createError.value = null
    await usersStore.createUser({ ...newUser.value })
    closeCreateForm()
    await usersStore.fetchUsers()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания пользователя'
  }
}

onMounted(async () => {
  await Promise.all([
    usersStore.fetchUsers(),
    departmentsStore.fetchDepartments(),
    userRolesStore.fetchRoles(),
    charactersStore.fetchCharacters(),
    settingsStore.fetchSettings(),
    coursesStore.fetchCourses(),
  ])
})
</script>
