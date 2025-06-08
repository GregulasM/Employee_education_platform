import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface Test {
    id: number
    publicId: string
    title: string
    description: string
    courseId: number
    course?: { id: number; title: string }
    moduleId: number | null
    module?: { id: number; title: string }
    questions: string // JSON строка
    isActive: boolean | null
}

export interface Course {
    id: number
    title: string
}

export interface Module {
    id: number
    title: string
    courseId: number
}

export const useTestsStore = defineStore('tests', () => {
    const tests: Ref<Test[]> = ref([])
    const courses: Ref<Course[]> = ref([])
    const modules: Ref<Module[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchCourses() {
        try {
            const res = await $fetch<Course[]>('http://localhost:5148/api/courses')
            courses.value = res
        } catch (err: any) {
            courses.value = []
            error.value = err.message ?? 'Ошибка загрузки курсов'
        }
    }

    async function fetchModules() {
        try {
            const res = await $fetch<Module[]>('http://localhost:5148/api/modules')
            modules.value = res
        } catch (err: any) {
            modules.value = []
            error.value = err.message ?? 'Ошибка загрузки модулей'
        }
    }

    async function fetchTests() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Test[]>('http://localhost:5148/api/admin_panel/tests')
            tests.value = res
        } catch (err: any) {
            tests.value = []
            error.value = err.message ?? 'Ошибка загрузки тестов'
        }
        loading.value = false
    }

    async function createTest(test: Partial<Test>) {
        // Находим название курса
        const course = courses.value.find(c => c.id === +test.courseId)
        if (!course) throw new Error('Выберите курс')
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(course.title)}/tests`, {
            method: 'POST',
            body: {
                ...test,
                courseId: test.courseId ? +test.courseId : null,
                moduleId: test.moduleId ? +test.moduleId : null,
            },
        })
    }

    async function updateTest(id: number, patch: Partial<Test>) {
        // Новый вариант — PATCH по id, всегда работает
        await $fetch(`http://localhost:5148/api/admin_panel/tests/${id}`, {
            method: 'PATCH',
            body: patch
        })
    }

    async function deleteTest(id: number) {
        const t = tests.value.find(t => t.id === id)
        if (!t) throw new Error('Тест не найден')
        const course = courses.value.find(c => c.id === t.courseId)
        if (!course) throw new Error('Выберите курс')
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(course.title)}/tests/${encodeURIComponent(t.title)}`, {
            method: 'DELETE'
        })
    }

    return {
        tests, courses, modules, loading, error,
        fetchCourses, fetchModules, fetchTests,
        createTest, updateTest, deleteTest
    }
})
