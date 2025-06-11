<template>
  <div>
    <div class="w-full flex justify-center pt-2">
      <h2 class="text-2xl text-white font-bold text-shadow-lg">Курсы</h2>
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
              @click="coursesStore.fetchCourses">
        <span v-if="coursesStore.loading">Обновление...</span>
        <span v-else>Обновить</span>
      </button>
      <NuxtLink draggable="false" @click="go_back"
                class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 font-semibold">
        Назад
      </NuxtLink>
    </div>

    <!-- Создание -->
    <div v-if="createMode" class="bg-gray-800/90 p-6 rounded-lg mb-6 w-full max-w-4xl mx-auto">
      <h3 class="text-lg font-bold text-white mb-4">Создать курс</h3>
      <form @submit.prevent="submitCreateCourse" class="grid grid-cols-1 md:grid-cols-2 gap-3 text-white">
        <input v-model="newCourse.title" class="input input-bordered" placeholder="Название" />
        <input v-model="newCourse.description" class="input input-bordered" placeholder="Описание" />
        <input v-model="newCourse.image" class="input input-bordered" placeholder="Ссылка на изображение" />
        <input v-model="newCourse.tags" class="input input-bordered" placeholder="Теги (через запятую)" />
        <input v-model.number="newCourse.position" class="input input-bordered" placeholder="Позиция (число)" type="number" />
        <label>
          <span>Отдел:</span>
          <select v-model.number="newCourse.departmentId" class="input input-bordered w-full mt-1" required>
            <option value="" disabled>Выберите отдел</option>
            <option v-for="dep in departmentsStore.departments" :key="dep.id" :value="dep.id">
              {{ dep.name }} (id:{{ dep.id }})
            </option>
          </select>
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
      <input v-model="filter.title" class="input input-sm" placeholder="Фильтр по названию" />
      <input v-model="filter.department" class="input input-sm" placeholder="Фильтр по отделу" />
      <input v-model="filter.tag" class="input input-sm" placeholder="Фильтр по тегу" />
      <label class="flex items-center gap-1">Сортировать по:
        <select v-model="sortKey" class="select select-sm">
          <option value="id">ID</option>
          <option value="title">Название</option>
          <option value="createdAt">Дата создания</option>
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

    <!-- Таблица -->
    <div class="w-full flex justify-center">
      <div class="w-full px-4">
        <div class="overflow-x-auto w-full">
          <table class="w-full min-w-[900px] table-auto bg-gray-900/90 text-white font-semibold shadow rounded-lg">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th class="whitespace-nowrap px-2 py-3">ID</th>
              <th class="whitespace-nowrap px-2 py-3">Название</th>
              <th class="whitespace-nowrap px-2 py-3">Описание</th>
              <th class="whitespace-nowrap px-2 py-3">Картинка</th>
              <th class="whitespace-nowrap px-2 py-3">Теги</th>
              <th class="whitespace-nowrap px-2 py-3">Позиция</th>
              <th class="whitespace-nowrap px-2 py-3">Отдел</th>
              <th class="whitespace-nowrap px-2 py-3">Модули</th>
              <th class="whitespace-nowrap px-2 py-3">Ссылки</th>
              <th class="whitespace-nowrap px-2 py-3">Тесты</th>
              <th class="whitespace-nowrap px-2 py-3">Дата создания</th>
              <th class="whitespace-nowrap px-2 py-3">Дата обновления</th>
              <th class="whitespace-nowrap px-2 py-3">Кнопки управления</th>
            </tr>
            </thead>
            <tbody class="bg-gray-900/90 text-white font-semibold">
            <tr v-for="course in filteredCourses" :key="course.id">
              <!-- Просмотр -->
              <template v-if="editingId !== course.id && confirmingDeleteId !== course.id">
                <td class="whitespace-nowrap px-2 py-2">{{ course.id }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.title }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.description }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  <img v-if="course.image" :src="course.image" class="w-12 h-12 rounded object-cover" alt="course-image" />
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.tags ? course.tags.join(', ') : '' }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.position }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.departmentName || course.departmentId }}</td>
                <td class="whitespace-nowrap px-2 py-2">
                  {{ modulesByCourse[course.id] || 0 }}
                </td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.usefulLinks?.length || 0 }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.tests?.length || 0 }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2 justify-center align-centre items-center">
                  <button class="btn btn-xs btn-warning font-bold" @click="startEdit(course.id)">✏️ Изменить</button>
                  <button class="btn btn-xs btn-error" @click="startDelete(course.id)">Удалить 🗑️</button>
                </td>
              </template>
              <!-- Редактирование -->
              <template v-else-if="editingId === course.id">
                <td class="whitespace-nowrap px-2 py-2">{{ course.id }}</td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editCourse.title" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editCourse.description" class="input input-xs w-28" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editCourse.image" class="input input-xs w-24" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editCourse.tagsString" class="input input-xs w-24" placeholder="теги, через запятую" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editCourse.position" type="number" class="input input-xs w-12" /></td>
                <td class="whitespace-nowrap px-2 py-2"><input v-model="editCourse.departmentId" type="number" class="input input-xs w-12" /></td>
                <td class="whitespace-nowrap px-2 py-2">-</td>
                <td class="whitespace-nowrap px-2 py-2">-</td>
                <td class="whitespace-nowrap px-2 py-2">-</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.createdAt }}</td>
                <td class="whitespace-nowrap px-2 py-2">{{ course.updatedAt }}</td>
                <td class="whitespace-nowrap px-2 py-2 flex gap-2">
                  <button class="btn btn-xs btn-success" @click="saveEdit(course.id)">Сохранить</button>
                  <button class="btn btn-xs btn-warning" @click="cancelEdit">Отмена</button>
                </td>
              </template>
              <!-- Удаление -->
              <template v-else-if="confirmingDeleteId === course.id">
                <td colspan="13" class="text-center text-red-400 bg-red-900/60 font-bold">
                  Удалить курс #{{ course.id }}?
                  <button class="btn btn-xs btn-error ml-3" @click="confirmDelete(course.id)">Точно удалить</button>
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
import { useCoursesStore } from '~/stores/courses_store'
import { useModulesStore } from '~/stores/modules_store'
import { useDepartmentsStore } from '~/stores/departments_store'


const router = useRouter()
const coursesStore = useCoursesStore()
const modulesStore = useModulesStore()
const departmentsStore = useDepartmentsStore()

const editingId = ref<number|null>(null)
const editCourse = ref<any>({})
const confirmingDeleteId = ref<number|null>(null)

function go_back () {
  router.back()
}

const modulesByCourse = computed(() => {
  const map: Record<number, number> = {}
  modulesStore.modules.forEach(m => {
    const courseId = m.course?.id || m.courseId
    if (courseId) map[courseId] = (map[courseId] || 0) + 1
  })
  return map
})

function startEdit(id: number) {
  editingId.value = id
  const course = coursesStore.courses.find(c => c.id === id)
  editCourse.value = {
    ...course,
    tagsString: course.tags ? course.tags.join(', ') : ''
  }
}

function cancelEdit() {
  editingId.value = null
  editCourse.value = {}
}

async function saveEdit(id: number) {

  let patch = { ...editCourse.value }
  if (typeof patch.tagsString === 'string') {
    patch.tags = patch.tagsString.split(',').map(x => x.trim()).filter(x => x)
  }
  delete patch.tagsString

  await coursesStore.updateCourse(id, patch)
  editingId.value = null
  editCourse.value = {}
  await coursesStore.fetchCourses()
}

function startDelete(id: number) {
  confirmingDeleteId.value = id
}
function cancelDelete() {
  confirmingDeleteId.value = null
}
async function confirmDelete(id: number) {
  try {
    await coursesStore.deleteCourse(id)
    confirmingDeleteId.value = null
    await coursesStore.fetchCourses()
  } catch (e) {
    alert('Ошибка удаления!')
  }
}

const showFilters = ref(false)
const filter = reactive({
  title: '',
  department: '',
  tag: '',
})
const sortKey = ref<'id' | 'title' | 'createdAt'>('id')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredCourses = computed(() => {
  let arr = [...coursesStore.courses]
  if (filter.title) arr = arr.filter(c => (c.title || '').toLowerCase().includes(filter.title.toLowerCase()))
  if (filter.department) arr = arr.filter(c => (c.departmentName || '').toLowerCase().includes(filter.department.toLowerCase()))
  if (filter.tag) arr = arr.filter(c => (c.tags || []).join(', ').toLowerCase().includes(filter.tag.toLowerCase()))
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

function resetFilters() {
  filter.title = ''
  filter.department = ''
  filter.tag = ''
}

const createMode = ref(false)
const createError = ref<string | null>(null)
const newCourse = ref({
  title: '', description: '', image: '', tags: '', position: null, departmentId: null
})

function closeCreateForm() {
  createMode.value = false
  createError.value = null
  Object.assign(newCourse.value, {
    title: '', description: '', image: '', tags: '', position: null, departmentId: null
  })
}

async function submitCreateCourse() {
  try {
    createError.value = null
    let payload = {
      ...newCourse.value,
      tags: newCourse.value.tags
          ? newCourse.value.tags.split(',').map((t: string) => t.trim()).filter((t: string) => t)
          : []
    }
    await coursesStore.createCourse(payload)
    closeCreateForm()
    await coursesStore.fetchCourses()
  } catch (e: any) {
    createError.value = e?.message || 'Ошибка создания курса'
  }
}

onMounted(async() => {
  await coursesStore.fetchCourses()
  await modulesStore.fetchModules()
  await departmentsStore.fetchDepartments()
})
</script>
