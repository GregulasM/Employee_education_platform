import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface AchievementRef {
    id: number
    name: string
}
export interface TestRef {
    id: number
    title: string
}
export interface CourseRef {
    id: number
    title: string
}

export interface Module {
    id: number
    publicId: string | null
    title: string | null
    courseId: number | null
    course?: CourseRef | null
    tests?: TestRef[] | null
    description: string | null
    image: string | null
    order: number | null
    hiddenAchievementId: number | null
    hiddenAchievement?: AchievementRef | null
    createdAt: string | null
    updatedAt: string | null
    tags: string[] | null
    isActive: boolean | null
}

export const useModulesStore = defineStore('modules', () => {
    const modules: Ref<Module[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchModules() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Module[]>('http://localhost:5148/api/modules')
            for (const m of res) {
                if (typeof m.tags === 'string') {
                    try { m.tags = JSON.parse(m.tags) } catch { m.tags = [] }
                }
            }
            modules.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки модулей'
        }
        loading.value = false
    }

    async function createModule(module: Partial<Module> & { courseTitle: string }) {
        const body = { ...module, tags: JSON.stringify(module.tags || []) }
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(module.courseTitle)}/modules`, {
            method: 'POST',
            body,
        })
    }

    async function updateModule(id: number, patch: Partial<Module>) {
        // PATCH по названию. Нужно знать title курса и модуля (по id ищем)
        const module = modules.value.find(m => m.id === id)
        if (!module || !module.title || !module.course?.title) throw new Error('Модуль или курс не найден')
        const body = { ...patch, tags: JSON.stringify(patch.tags || module.tags || []) }
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(module.course.title)}/modules/${encodeURIComponent(module.title)}`, {
            method: 'PATCH',
            body,
        })
    }

    async function deleteModule(id: number) {
        const module = modules.value.find(m => m.id === id)
        if (!module || !module.title || !module.course?.title) throw new Error('Модуль или курс не найден')
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(module.course.title)}/modules/${encodeURIComponent(module.title)}`, {
            method: 'DELETE',
        })
    }

    return { modules, loading, error, fetchModules, createModule, updateModule, deleteModule }
})
