import { defineStore } from 'pinia'
import type { Ref } from 'vue'
// Импортируй стор курсов, если используешь его напрямую:
import { useCoursesStore } from '~/stores/courses_store'

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
}

export const useUsefulLinksStore = defineStore('useful_links', () => {
    const links: Ref<UsefulLink[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchLinks() {
        loading.value = true
        error.value = null
        try {
            // Грузим все ссылки по всем курсам
            const res = await $fetch<UsefulLink[]>('http://localhost:5148/api/useful_links')
            for (const l of res) {
                if (typeof l.tags === 'string') {
                    try {
                        l.tags = JSON.parse(l.tags)
                    } catch { l.tags = [] }
                }
            }
            links.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки ссылок'
        }
        loading.value = false
    }

    async function createLink(link: Partial<UsefulLink>) {
        if (!link.courseId) throw new Error('Не выбран курс')
        const body = {
            ...link,
            tags: JSON.stringify(link.tags || []),
            parentId: link.parentId || null,
            isActive: true
        }
        const courseTitle = await getCourseTitle(link.courseId)
        await $fetch(`http://localhost:5148/api/admin_panel/courses/${encodeURIComponent(courseTitle)}/usefullinks`, {
            method: 'POST',
            body,
        })
        await fetchLinks()
    }

    async function updateLink(id: number, patch: Partial<UsefulLink>) {
        await $fetch(`http://localhost:5148/api/admin_panel/useful_links/${id}`, {
            method: 'PATCH',
            body: {
                ...patch,
                tags: JSON.stringify(patch.tags || []),
                parentId: patch.parentId || null
            }
        })
        await fetchLinks()
    }

    async function deleteLink(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/useful_links/${id}`, {
            method: 'DELETE'
        })
        await fetchLinks()
    }

    // Получить title курса по id (ищет сначала в coursesStore, иначе делает fetch)
    async function getCourseTitle(courseId: number | null): Promise<string> {
        if (!courseId) throw new Error('courseId required')
        // Пытаемся получить из стора (если уже загружен)
        try {
            const coursesStore = useCoursesStore()
            const found = coursesStore.courses.find(c => c.id === courseId)
            if (found && found.title) return found.title
        } catch {}
        // Если не найден — делаем запрос
        const res = await $fetch<any[]>('http://localhost:5148/api/courses')
        const course = res.find((c: any) => c.id === courseId)
        if (!course || !course.title) throw new Error('Курс не найден')
        return course.title
    }

    return { links, loading, error, fetchLinks, createLink, updateLink, deleteLink }
})
