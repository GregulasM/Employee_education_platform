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

export const useAchievementListsStore = defineStore('achievementlists', () => {
    const lists = ref<AchievementList[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchLists() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<AchievementList[]>('http://localhost:5148/api/admin_panel/achievementlists')
            lists.value = res.filter(l => l.isActive !== false && l.isHidden !== true)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки листов достижений'
        }
        loading.value = false
    }

    return { lists, loading, error, fetchLists }
})
