import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Test {
    id: number
    publicId: string
    title: string
    description: string
    courseId: number
    course?: { id: number; title: string }
    moduleId: number | null
    module?: { id: number; title: string }
    questions: string
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
    const tests = ref<Test[]>([])
    const courses = ref<Course[]>([])
    const modules = ref<Module[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

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

    return {
        tests, courses, modules, loading, error,
        fetchCourses, fetchModules, fetchTests
    }
})
