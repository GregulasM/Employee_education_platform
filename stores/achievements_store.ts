import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface AchievementList {
    id: number
    publicId: string | null
    name: string | null
    description: string | null
    icon: string | null
    sortType: number | null
    isHidden: boolean | null
    isActive: boolean | null
}

export interface Achievement {
    id: number
    publicId: string
    name: string
    description: string | null
    icon: string | null
    points: number | null
    listId: number
}

export const useAchievementsStore = defineStore('achievements', () => {
    const achievementLists = ref<AchievementList[]>([])
    const achievements = ref<Achievement[]>([])
    const allAchievements = ref<Achievement[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)
    const currentListId = ref<number | null>(null)

    async function fetchAchievementLists() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<AchievementList[]>('http://localhost:5148/api/achievementlists')
            achievementLists.value = res.filter(l => l.isActive !== false && l.isHidden !== true)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки листов достижений'
        }
        loading.value = false
    }

    async function fetchAllAchievements(forceReload = false) {
        if (allAchievements.value.length > 0 && !forceReload) return
        loading.value = true
        error.value = null
        try {
            let all: Achievement[] = []
            if (achievementLists.value.length === 0) {
                await fetchAchievementLists()
            }
            const promises = achievementLists.value.map(list =>
                $fetch<Achievement[]>(`http://localhost:5148/api/achievementlists/${list.id}/achievements`)
                    .then(achList => achList ?? [])
                    .catch(() => [])
            )
            const allResults = await Promise.all(promises)
            all = allResults.flat()
            allAchievements.value = all
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки достижений'
        }
        loading.value = false
    }

    async function fetchAchievements(listId: number) {
        if (!listId) return
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Achievement[]>(`http://localhost:5148/api/achievementlists/${listId}/achievements`)
            achievements.value = res
            currentListId.value = listId
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки достижений'
        }
        loading.value = false
    }

    return {
        achievementLists,
        achievements,
        allAchievements,
        loading,
        error,
        currentListId,
        fetchAchievementLists,
        fetchAchievements,
        fetchAllAchievements
    }
})
