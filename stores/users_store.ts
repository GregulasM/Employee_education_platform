import { defineStore } from 'pinia'
import { ref } from 'vue'

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
    const users = ref<User[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)
    const currentUser = ref<User | null>(null)
    const isAuthenticated = ref(false)
    const token = ref<string | null>(null)

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

    function saveSession(user: User, receivedToken: string) {
        currentUser.value = user
        isAuthenticated.value = true
        token.value = receivedToken
        if (typeof window !== 'undefined') {
            localStorage.setItem('user_session', JSON.stringify({ user, token: receivedToken }))
        }
    }

    function isTokenExpired(token: string): boolean {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]))
            return payload.exp * 1000 < Date.now()
        } catch {
            return true
        }
    }

    function loadSession() {
        if (typeof window === 'undefined') return
        const raw = localStorage.getItem('user_session')
        if (raw) {
            try {
                const { user, token: storedToken } = JSON.parse(raw)
                if (storedToken && !isTokenExpired(storedToken)) {
                    currentUser.value = user
                    token.value = storedToken
                    isAuthenticated.value = true
                    return
                } else {
                    clearSession()
                }
            } catch {
                clearSession()
            }
        }
    }

    function clearSession() {
        currentUser.value = null
        token.value = null
        isAuthenticated.value = false
        if (typeof window !== 'undefined') {
            localStorage.removeItem('user_session')
        }
    }

    async function login(login: string, password: string) {
        loading.value = true
        error.value = null
        try {
            const response = await $fetch<{ token: string, user: User }>('http://localhost:5148/api/auth/login', {
                method: 'POST',
                body: { login, password }
            })
            saveSession(response.user, response.token)
        } catch (err: any) {
            error.value = err?.data?.message || err?.message || 'Ошибка входа'
            clearSession()
            throw err
        } finally {
            loading.value = false
        }
    }

    function logout() {
        clearSession()
    }

    async function getUserById(id: number) {
        if (!users.value.length) await fetchUsers()
        return users.value.find(u => u.id === id) || null
    }

    async function getUserByPublicId(publicId: string) {
        if (!users.value.length) await fetchUsers()
        return users.value.find(u => u.publicId === publicId) || null
    }

    async function updateUser(id: number, patch: Partial<User>) {
        try {
            await $fetch(`http://localhost:5148/api/users/${id}`, {
                method: 'PATCH',
                body: patch,
            })
            await fetchUsers()
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка обновления пользователя'
        }
    }

    if (typeof window !== 'undefined') {
        loadSession()
    }

    return {
        users, loading, error, currentUser, isAuthenticated, token,
        login, logout, saveSession, clearSession, fetchUsers, getUserById, getUserByPublicId, updateUser, loadSession
    }
})
