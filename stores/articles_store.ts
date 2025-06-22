import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Article {
    id: number | string
    slug: string
    title: string
    content: any
    moduleTitle?: string
    createdAt?: string
    updatedAt?: string
}

export const useArticlesStore = defineStore('articles', () => {
    const article = ref<Article | null>(null)
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchArticle(moduleId: string | number, articleId: string | number) {
        loading.value = true
        error.value = null
        article.value = null
        try {
            const encoded = encodeURIComponent(articleId)
            const res = await $fetch<Article>(`http://localhost:5148/api/admin_panel/modules/${moduleId}/articles/${encoded}`)
            article.value = {
                ...res,
                content: typeof res.content === 'string' ? JSON.parse(res.content) : res.content,
                slug: res.slug || res.id || res.title?.toLowerCase().replace(/\s+/g, '-')
            }
        } catch (e: any) {
            error.value = e?.message ?? 'Ошибка загрузки статьи'
        }
        loading.value = false
    }

    return { article, loading, error, fetchArticle }
})
