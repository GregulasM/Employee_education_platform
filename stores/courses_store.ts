import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface DepartmentRef {
    id: number
    name: string
}

export interface ModuleRef {
    id: number
    title: string
}

export interface LinkRef {
    id: number
    title: string
}

export interface TestRef {
    id: number
    title: string
}

export interface Course {
    id: number
    publicId: string | null
    title: string | null
    description: string | null
    image: string | null
    tags: string[] | null
    createdAt: string | null
    updatedAt: string | null
    position: number | null
    departmentId: number | null
    department?: DepartmentRef | null
    modules?: ModuleRef[] | null
    usefulLinks?: LinkRef[] | null
    tests?: TestRef[] | null
    isActive: boolean | null
}

export const useCoursesStore = defineStore('courses', () => {
    const courses: Ref<Course[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchCourses() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Course[]>('http://localhost:5148/api/courses')
            // Преобразуем tags, если надо (если с бэка приходит строкой)
            for (const c of res) {
                if (typeof c.tags === 'string') {
                    try {
                        c.tags = JSON.parse(c.tags)
                    } catch { c.tags = [] }
                }
            }
            courses.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки курсов'
        }
        loading.value = false
    }

    async function createCourse(course: Partial<Course>) {
        // Теги в JSON-строку (если требуется)
        const body = { ...course, tags: JSON.stringify(course.tags || []) }
        await $fetch('http://localhost:5148/api/admin_panel/courses', {
            method: 'POST',
            body,
        })
    }

    async function updateCourse(id: number, patch: Partial<Course>) {
        // PATCH по title, но мы ищем курс по id, поэтому найдем title по id
        const course = courses.value.find(c => c.id === id)
        if (!course || !course.title) throw new Error('Курс не найден')
        const body = { ...patch, tags: JSON.stringify(patch.tags || course.tags || []) }
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(course.title)}`, {
            method: 'PATCH',
            body,
        })
    }

    async function deleteCourse(id: number) {
        // Soft-delete через PATCH, либо через DELETE
        const course = courses.value.find(c => c.id === id)
        if (!course || !course.title) throw new Error('Курс не найден')
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(course.title)}`, {
            method: 'DELETE',
        })
    }

    return { courses, loading, error, fetchCourses, createCourse, updateCourse, deleteCourse }
})
