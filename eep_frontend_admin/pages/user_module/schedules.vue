<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Расписание</h2>
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
          @click="schedulesStore.fetchSchedules"
      >
        <span v-if="schedulesStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold"
      >Назад</NuxtLink>
    </div>
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать расписание</h3>
      <form @submit.prevent="submitCreateSchedule" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newSchedule.dayOfWeek" class="input input-bordered" placeholder="День недели" />
        <input v-model="newSchedule.timeSlot" class="input input-bordered" placeholder="Время (например 10:00-11:30)" />
        <input v-model="newSchedule.subject" class="input input-bordered" placeholder="Предмет" />
        <input v-model="newSchedule.teacher" class="input input-bordered" placeholder="Преподаватель" />
        <input v-model="newSchedule.details" class="input input-bordered" placeholder="Детали (опционально)" />
        <input v-model.number="newSchedule.courseId" class="input input-bordered" placeholder="ID курса (опц.)" type="number"/>
        <input v-model.number="newSchedule.moduleId" class="input input-bordered" placeholder="ID модуля (опц.)" type="number"/>
        <input v-model.number="newSchedule.departmentId" class="input input-bordered" placeholder="ID отдела (опц.)" type="number"/>
        <input v-model.number="newSchedule.userId" class="input input-bordered" placeholder="ID пользователя (опц.)" type="number"/>
        <div class="flex col-span-1 md:col-span-2 gap-3 mt-4 justify-end">
          <button class="btn btn-success" type="submit">Создать</button>
          <button class="btn btn-warning" type="button" @click="closeCreateForm">Отмена</button>
        </div>
      </form>
      <div v-if="createError" class="text-red-400 mt-2">{{ createError }}</div>
    </div>
    <div v-if="showFilters" class="bg-gray-800/80 px-4 py-4 mb-3 rounded-lg flex flex-wrap gap-4 text-white">
      <input v-model="filter.dayOfWeek" class="input input-sm" placeholder="Фильтр по дню недели" />
      <input v-model="filter.subject" class="input input-sm" placeholder="Фильтр по предмету" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="dayOfWeek">День</option>
          <option value="timeSlot">Время</option>
          <option value="subject">Предмет</option>
        </select>
      </label>
      <label class="flex items-center gap-1">Направление:
        <select v-model="sortDir" class="select select-sm">
          <option value="asc">↑</option>
          <option value="desc">↓</option>
        </select>
      </label>
      <button class="btn btn-xs btn-warning ml-2" @click="Object.assign(filter, {dayOfWeek:'', subject:''})">Сбросить фильтры</button>
    </div>
    <!-- Таблица -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">День</th>
              <th class="whitespace-nowrap px-2 py-3">Время</th>
              <th class="whitespace-nowrap px-2 py-3">Предмет</th>
              <th class="whitespace-nowrap px-2 py-3">Преподаватель</th>
              <th class="whitespace-nowrap px-2 py-3">Детали</th>
              <th class="whitespace-nowrap px-2 py-3">Курс</th>
              <th class="whitespace-nowrap px-2 py-3">Модуль</th>
              <th class="whitespace-nowrap px-2 py-3">Отдел</th>
              <th class="whitespace-nowrap px-2 py-3">Пользователь</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="sch in filteredSchedules" :key="sch.id">
              <template v-if="editingId !== sch.id && confirmingDeleteId !== sch.id">
                <td class="whitespace-nowrap px-2 py-2">{{ sch.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.dayOfWeek }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.timeSlot }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.subject }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.teacher }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.details }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.course?.title || sch.courseId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.module?.name || sch.moduleId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.department?.name || sch.departmentId }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ sch.user?.firstName || sch.userId }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center align-center items-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(sch.id)">✏️ Изм.</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(sch.id)">Удалить 🗑️</button>
                </td>
              </template>
              <template v-else-if="editingId === sch.id">
                <td class="whitespace-nowrap px-2 py-2">{{ sch.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.dayOfWeek" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.timeSlot" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.subject" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.teacher" class="input input-xs w-20" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.details" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.courseId" type="number" class="input input-xs w-14" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.moduleId" type="number" class="input input-xs w-14" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.departmentId" type="number" class="input input-xs w-14" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editSchedule.userId" type="number" class="input input-xs w-14" /></td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(sch.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <template v-else-if="confirmingDeleteId === sch.id">
                <td colspan="11" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить запись расписания #{{ sch.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(sch.id)">Точно удалить</button>
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
import { useSchedulesStore } from '~/stores/schedules_store'
const router = useRouter()
const schedulesStore = useSchedulesStore()

const editingId = ref<number|null>(null)
const editSchedule = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

function startEdit(id: number) {
  editingId.value = id
  const sch = schedulesStore.schedules.find(s => s.id === id)
  editSchedule.value = { ...sch }
}

function cancelEdit() {
  editingId.value = null
  editSchedule.value = {}
}

async function saveEdit(id: number) {
  await schedulesStore.updateSchedule(id, editSchedule.value)
  editingId.value = null
  editSchedule.value = {}
  await schedulesStore.fetchSchedules()
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  await schedulesStore.deleteSchedule(id)
  confirmingDeleteId.value = null
  await schedulesStore.fetchSchedules()
}

const showFilters = ref(false)
const filter = reactive({
  dayOfWeek: '',
  subject: '',
})
const sortKey = ref<'id' | 'dayOfWeek' | 'timeSlot' | 'subject'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredSchedules = computed(() => {
  let arr = [...schedulesStore.schedules]
  if (filter.dayOfWeek) arr = arr.filter(s => (s.dayOfWeek || '').toLowerCase().includes(filter.dayOfWeek.toLowerCase()))
  if (filter.subject) arr = arr.filter(s => (s.subject || '').toLowerCase().includes(filter.subject.toLowerCase()))
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
const newSchedule = ref({
  dayOfWeek: '', timeSlot: '', subject: '', teacher: '', details: '',
  courseId: null, moduleId: null, departmentId: null, userId: null
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newSchedule.value, {
    dayOfWeek: '', timeSlot: '', subject: '', teacher: '', details: '',
    courseId: null, moduleId: null, departmentId: null, userId: null
  })
}

async function submitCreateSchedule() {
  try {
    createError.value = null
    await schedulesStore.createSchedule({ ...newSchedule.value })
    closeCreateForm()
    await schedulesStore.fetchSchedules()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания записи расписания'
  }
}

onMounted(() => schedulesStore.fetchSchedules())
</script>
