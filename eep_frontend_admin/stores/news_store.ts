import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface News {
    id: number
    publicId: string | null
    title: string | null
    slug: string | null
    excerpt: string | null
    content: JSON | null
    authorId: number | null
    author?: { id: number; firstName: string; lastName: string } | null
    type: string | null
    date: string | null
    tags: string | null
    isPinned: boolean | null
    createdAt: string | null
    updatedAt: string | null
    isActive: boolean | null
}

export const useNewsStore = defineStore('news', () => {
    const newsList: Ref<News[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

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

    async function createNews(news: Partial<News>) {
        await $fetch('http://localhost:5148/api/admin_panel/users/news', {
            method: 'POST',
            body: news,
        })
    }

    async function updateNews(slug: string, patch: Partial<News>) {
        await $fetch(`http://localhost:5148/api/admin_panel/users/news/${slug}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteNews(slug: string) {
        await $fetch(`http://localhost:5148/api/admin_panel/users/news/${slug}`, {
            method: 'DELETE',
        })
    }

    return { newsList, loading, error, fetchNews, createNews, updateNews, deleteNews }
})
