import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface UserRole {
    id: number
    name: string | null
    description: string | null
    isActive: boolean | null
}

export const useUserRolesStore = defineStore('user_roles', () => {
    const roles: Ref<UserRole[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchRoles() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<UserRole[]>('http://localhost:5148/api/admin_panel/user_roles')
            roles.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки ролей'
        }
        loading.value = false
    }

    async function createRole(role: Partial<UserRole>) {
        await $fetch('http://localhost:5148/api/admin_panel/user_roles', {
            method: 'POST',
            body: role,
        })
    }

    async function updateRole(id: number, patch: Partial<UserRole>) {
        await $fetch(`http://localhost:5148/api/admin_panel/user_roles/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteRole(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/user_roles/${id}`, {
            method: 'DELETE',
        })
    }

    return { roles, loading, error, fetchRoles, createRole, updateRole, deleteRole }
})
