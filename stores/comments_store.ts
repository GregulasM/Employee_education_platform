import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Comment {
    id: number
    publicId: string | null
    newsId: number | null
    userId: number | null
    text: string | null
    createdAt: string | null
    updatedAt: string | null
    isActive: boolean | null
}

export const useCommentsStore = defineStore('comments', () => {
    const comments = ref<Comment[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchComments() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Comment[]>('http://localhost:5148/api/admin_panel/comments')
            comments.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки комментариев'
        }
        loading.value = false
    }

    async function fetchCommentsByNews(newsId: number) {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Comment[]>(`http://localhost:5148/api/admin_panel/comments`)
            comments.value = res
            return comments.value.filter(c => c.newsId === newsId)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки комментариев'
            return []
        } finally {
            loading.value = false
        }
    }

    async function getCommentById(id: number) {
        if (!comments.value.length) await fetchComments()
        return comments.value.find(c => c.id === id) || null
    }

    async function createComment(comment: Partial<Comment>) {
        loading.value = true
        error.value = null
        try {
            await $fetch('http://localhost:5148/api/admin_panel/comments', {
                method: 'POST',
                body: comment,
            })
            await fetchComments()
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка создания комментария'
        }
        loading.value = false
    }

    async function updateComment(id: number, patch: Partial<Comment>) {
        loading.value = true
        error.value = null
        try {
            await $fetch(`http://localhost:5148/api/admin_panel/comments/${id}`, {
                method: 'PATCH',
                body: patch,
            })
            await fetchComments()
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка обновления комментария'
        }
        loading.value = false
    }

    async function deleteComment(id: number) {
        loading.value = true
        error.value = null
        try {
            await $fetch(`http://localhost:5148/api/admin_panel/comments/${id}`, {
                method: 'DELETE',
            })
            comments.value = comments.value.filter(c => c.id !== id)
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка удаления комментария'
        }
        loading.value = false
    }

    return {
        comments, loading, error,
        fetchComments, fetchCommentsByNews, getCommentById,
        createComment, updateComment, deleteComment
    }
})
