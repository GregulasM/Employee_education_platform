import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface News {
    id: number
    publicId: string | null
    title: string | null
    slug: string | null
    excerpt: string | null
    content: JSON | null
    authorId: number | null
    author?: { id: number; firstName: string; lastName: string; avatar?: string } | null
    type: string | null
    date: string | null
    tags: string | null
    isPinned: boolean | null
    createdAt: string | null
    updatedAt: string | null
    isActive: boolean | null
}

export const useNewsStore = defineStore('news', () => {
    const newsList = ref<News[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchNews() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<News[]>('http://localhost:5148/api/users/news')
            newsList.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки новостей'
        }
        loading.value = false
    }

    async function fetchNewsBySlug(slug: string) {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<News>(`http://localhost:5148/api/users/news/${slug}`)
            loading.value = false
            return res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки новости'
            loading.value = false
            return null
        }
    }

    async function createNews(news: Partial<News>) {
        try {
            await $fetch('http://localhost:5148/api/admin_panel/users/news', {
                method: 'POST',
                body: news,
            })
            await fetchNews()
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка создания новости'
        }
    }

    async function updateNews(slug: string, patch: Partial<News>) {
        try {
            await $fetch(`http://localhost:5148/api/admin_panel/users/news/${slug}`, {
                method: 'PATCH',
                body: patch,
            })
            await fetchNews()
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка обновления новости'
        }
    }

    async function deleteNews(slug: string) {
        try {
            await $fetch(`http://localhost:5148/api/admin_panel/users/news/${slug}`, {
                method: 'DELETE',
            })
            newsList.value = newsList.value.filter(n => n.slug !== slug)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка удаления новости'
        }
    }

    return {
        newsList, loading, error,
        fetchNews, fetchNewsBySlug,
        createNews, updateNews, deleteNews
    }
})
