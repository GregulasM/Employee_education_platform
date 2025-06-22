import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface UserAchievement {
    id: number
    userId: number
    achievementId: number
    achievement: {
        id: number
        publicId: string
        name: string
        description: string | null
        icon: string | null
        points: number | null
        listId: number
    }
    isActive?: boolean
}

export const useUserAchievementsStore = defineStore('userAchievements', () => {
    const userAchievements = ref<UserAchievement[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchUserAchievements(userId: number) {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch(`http://localhost:5148/api/admin_panel/users/${userId}/achievements`)
            userAchievements.value = res
        } catch (err: any) {
            error.value = err.message || 'Ошибка загрузки достижений'
        }
        loading.value = false
    }

    return { userAchievements, loading, error, fetchUserAchievements }
})
