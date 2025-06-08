import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface User {
    id: number
    publicId: string | null
    login: string | null
    password: string | null
    phoneNumber: string | null
    firstName: string | null
    secondName: string | null
    lastName: string | null
    email: string | null
    avatar: string | null
    rating: number | null
    themeId: number | null
    theme?: { id: number; name: string } | null
    fontId: number | null
    font?: { id: number; name: string } | null
    activeCourseId: number | null
    activeCourse?: { id: number; title: string } | null
    selectedCharacterId: number | null
    selectedCharacter?: { id: number; name: string } | null
    departmentId: number | null
    department?: { id: number; name: string } | null
    roleId: number | null
    role?: { id: number; name: string } | null
    chosenCourses?: { id: number; title: string }[] | null
    achievements?: { id: number; name: string }[] | null
    userCharacters?: any[] | null
    createdAt: string | null
    updatedAt: string | null
    isActive: boolean | null
}

export const useUsersStore = defineStore('users', () => {
    const users: Ref<User[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchUsers() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<User[]>('http://localhost:5148/api/admin_panel/users')
            users.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки пользователей'
        }
        loading.value = false
    }

    async function createUser(user: Partial<User>) {
        await $fetch('http://localhost:5148/api/admin_panel/users', {
            method: 'POST',
            body: user,
        })
    }

    async function updateUser(id: number, patch: Partial<User>) {
        await $fetch(`http://localhost:5148/api/users/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteUser(id: number) {
        await $fetch(`http://localhost:5148/api/users/${id}`, {
            method: 'DELETE',
        })
    }

    return { users, loading, error, fetchUsers, createUser, updateUser, deleteUser }
})
