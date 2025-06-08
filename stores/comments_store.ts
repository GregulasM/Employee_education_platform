import { defineStore } from 'pinia'
import type { Ref } from 'vue'

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
    const comments: Ref<Comment[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

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

    async function createComment(comment: Partial<Comment>) {
        await $fetch('http://localhost:5148/api/admin_panel/comments', {
            method: 'POST',
            body: comment,
        })
    }

    async function updateComment(id: number, patch: Partial<Comment>) {
        await $fetch(`http://localhost:5148/api/admin_panel/comments/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteComment(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/comments/${id}`, {
            method: 'DELETE',
        })
    }

    return { comments, loading, error, fetchComments, createComment, updateComment, deleteComment }
})
