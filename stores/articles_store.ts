import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface Article {
    id: number
    publicId: string
    title: string
    moduleId: number | null
    module?: { id: number; title: string } | null
    image: string | null
    content: string | null
    tags: string | null
    rating: number | null
    createdAt: string | null
    updatedAt: string | null
    isActive: boolean | null
}

export interface ModuleShort {
    id: number
    title: string
}

export const useArticlesStore = defineStore('articles', () => {
    const articles: Ref<Article[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)
    const modules: Ref<ModuleShort[]> = ref([])

    // Получить все модули для dropdown'а (используем /api/modules)
    async function fetchModules() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<ModuleShort[]>('http://localhost:5148/api/modules')
            modules.value = res.filter(m => m.isActive !== false)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки модулей'
        }
        loading.value = false
    }

    // Получить все статьи по выбранному модулю (или все, если moduleId не передан)
    async function fetchArticles(moduleId?: number) {
        loading.value = true
        error.value = null
        try {
            let res: Article[] = []
            if (moduleId) {
                res = await $fetch<Article[]>(`http://localhost:5148/api/modules/${moduleId}/articles`)
            } else {
                // Получить все статьи по всем модулям
                const mods = modules.value.length ? modules.value : await fetchModules()
                let all: Article[] = []
                const promises = mods.map(m =>
                    $fetch<Article[]>(`http://localhost:5148/api/modules/${m.id}/articles`).catch(() => [])
                )
                all = (await Promise.all(promises)).flat()
                res = all
            }
            articles.value = res.filter(a => a.isActive !== false)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки статей'
        }
        loading.value = false
    }

    // Создать статью (title, moduleId — обязательны)
    async function createArticle(moduleId: number, article: Partial<Article>) {
        await $fetch(`http://localhost:5148/api/admin_panel/modules/${moduleId}/articles`, {
            method: 'POST',
            body: article,
        })
        await fetchArticles(moduleId)
    }

    // Обновить (PATCH)
    async function updateArticle(id: number, patch: Partial<Article>) {
        const article = articles.value.find(a => a.id === id)
        if (!article) throw new Error('Статья не найдена')
        await $fetch(`http://localhost:5148/api/admin_panel/modules/${article.moduleId}/articles/${encodeURIComponent(article.title)}`, {
            method: 'PATCH',
            body: patch,
        })
        await fetchArticles()
    }

    // Soft delete (DELETE)
    async function deleteArticle(id: number) {
        const article = articles.value.find(a => a.id === id)
        if (!article) throw new Error('Статья не найдена')
        await $fetch(`http://localhost:5148/api/admin_panel/modules/${article.moduleId}/articles/${encodeURIComponent(article.title)}`, {
            method: 'DELETE',
        })
        await fetchArticles()
    }

    return { articles, loading, error, modules, fetchModules, fetchArticles, createArticle, updateArticle, deleteArticle }
})
