import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface Department {
    id: number
    publicId: string | null
    name: string | null
    description: string | null
    isActive: boolean | null
}

export const useDepartmentsStore = defineStore('departments', () => {
    const departments: Ref<Department[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchDepartments() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Department[]>('http://localhost:5148/api/admin_panel/departments')
            departments.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки отделов'
        }
        loading.value = false
    }

    async function createDepartment(department: Partial<Department>) {
        await $fetch('http://localhost:5148/api/admin_panel/departments', {
            method: 'POST',
            body: department,
        })
    }

    async function updateDepartment(id: number, patch: Partial<Department>) {
        await $fetch(`http://localhost:5148/api/admin_panel/departments/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteDepartment(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/departments/${id}`, {
            method: 'DELETE',
        })
    }

    return { departments, loading, error, fetchDepartments, createDepartment, updateDepartment, deleteDepartment }
})
