import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface AchievementList {
    id: number
    publicId: string | null
    name: string | null
    description: string | null
    icon: string | null
    sortType: number | null
    isHidden: boolean | null
    achievements?: { id: number; name: string }[] | null
    isActive: boolean | null
}

export const useAchievementListsStore = defineStore('achievementlists', () => {
    const lists: Ref<AchievementList[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchLists() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<AchievementList[]>('http://localhost:5148/api/admin_panel/achievementlists')
            lists.value = res.filter(l => l.isActive !== false)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки листов достижений'
        }
        loading.value = false
    }


    async function createList(list: Partial<AchievementList>) {
        await $fetch('http://localhost:5148/api/admin_panel/achievementlists', {
            method: 'POST',
            body: list,
        })
    }

    async function updateList(id: number, patch: Partial<AchievementList>) {
        await $fetch(`http://localhost:5148/api/admin_panel/achievementlists/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteList(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/achievementlists/${id}`, {
            method: 'DELETE',
        })
    }

    return { lists, loading, error, fetchLists, createList, updateList, deleteList }
})
