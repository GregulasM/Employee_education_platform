import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface AchievementList {
    id: number
    publicId: string
    name: string
    description: string | null
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
    const achievementLists: Ref<AchievementList[]> = ref([])
    const achievements: Ref<Achievement[]> = ref([])
    const allAchievements: Ref<Achievement[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)
    const currentListId: Ref<number | null> = ref(null)

    async function fetchAchievementLists() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<AchievementList[]>('http://localhost:5148/api/admin_panel/achievementlists')
            achievementLists.value = res
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

    async function createAchievement(listId: number, achievement: Partial<Achievement>) {
        await $fetch(`http://localhost:5148/api/admin_panel/achievementlists/${listId}/achievements`, {
            method: 'POST',
            body: achievement,
        })
        await fetchAllAchievements()
    }

    async function updateAchievement(listId: number, achievementId: number, patch: Partial<Achievement>) {
        await $fetch(`http://localhost:5148/api/admin_panel/achievementlists/${listId}/achievements/${achievementId}`, {
            method: 'PATCH',
            body: patch,
        })
        await fetchAllAchievements()
    }

    async function deleteAchievement(listId: number, achievementId: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/achievementlists/${listId}/achievements/${achievementId}`, {
            method: 'DELETE',
        })
        await fetchAllAchievements()
    }

    return {
        achievementLists, achievements, allAchievements, loading, error, currentListId,
        fetchAchievementLists, fetchAchievements, fetchAllAchievements,
        createAchievement, updateAchievement, deleteAchievement
    }
})
