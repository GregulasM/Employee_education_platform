import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface UsefulLink {
    id: number
    publicId: string | null
    title: string | null
    url: string | null
    description: string | null
    tags: string[] | string | null
    courseId: number | null
    parentId: number | null
    isActive: boolean | null
    course?: { id: number; title: string } | null
}

export const useUsefulLinksStore = defineStore('useful_links', () => {
    const links = ref<UsefulLink[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchLinks() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<UsefulLink[]>('http://localhost:5148/api/useful_links')
            for (const l of res) {
                if (typeof l.tags === 'string') {
                    try { l.tags = JSON.parse(l.tags) } catch { l.tags = [] }
                }
            }
            links.value = res.filter(l => l.isActive)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки ссылок'
        }
        loading.value = false
    }

    return { links, loading, error, fetchLinks }
})
