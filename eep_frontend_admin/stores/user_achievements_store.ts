import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface UserAchievementDto {
    id: number
    userId: number
    achievementId: number
    achievement?: AchievementDto | null
    user?: UserDto | null
}

export interface AchievementDto {
    id: number
    publicId: string
    name: string
    description: string
    icon: string
    points: number
    listId: number
}

export interface UserDto {
    id: number
    login: string
    email: string
}

export const useUserAchievementsStore = defineStore('userAchievements', () => {
    const userAchievementsList: Ref<UserAchievementDto[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function createUserAchievement(userId: number, achievementId: number) {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<UserAchievementDto>(`http://localhost:5148/api/admin_panel/users/${userId}/achievements/${achievementId}`, {
                method: 'POST'
            })
            userAchievementsList.value.push(res)
            return res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка при выдаче достижения'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function fetchAllUserAchievements() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<UserAchievementDto[]>('http://localhost:5148/api/admin_panel/user-achievements')
            userAchievementsList.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки всех достижений пользователей'
        } finally {
            loading.value = false
        }
    }

    async function fetchUserAchievements(userId: number) {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<AchievementDto[]>(`http://localhost:5148/api/admin_panel/users/${userId}/achievements`)
            return res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки достижений пользователя'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function fetchUserAchievement(id: number) {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<UserAchievementDto>(`http://localhost:5148/api/admin_panel/user-achievements/${id}`)
            return res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка получения связки пользователь-достижение'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function deleteUserAchievement(userId: number, achievementId: number) {
        loading.value = true
        error.value = null
        try {
            await $fetch(`http://localhost:5148/api/admin_panel/users/${userId}/achievements/${achievementId}`, {
                method: 'DELETE'
            })
            userAchievementsList.value = userAchievementsList.value.filter(
                ua => !(ua.userId === userId && ua.achievementId === achievementId)
            )
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка при удалении достижения'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function deleteUserAchievementById(id: number) {
        loading.value = true
        error.value = null
        try {
            await $fetch(`http://localhost:5148/api/admin_panel/user_achievements/${id}`, {
                method: 'DELETE'
            })
            userAchievementsList.value = userAchievementsList.value.filter(ua => ua.id !== id)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка при удалении связки'
            throw err
        } finally {
            loading.value = false
        }
    }

    return {

        userAchievementsList,
        loading,
        error,

        createUserAchievement,
        fetchAllUserAchievements,
        fetchUserAchievements,
        fetchUserAchievement,
        deleteUserAchievement,
        deleteUserAchievementById
    }
})