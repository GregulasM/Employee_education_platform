export interface Article {
    id: number
    slug: string
    title: string
    num: string
}

export interface Module {
    id: number
    num: number
    title: string
    icon: string
    articles: Article[]
}

export interface Course {
    id: number
    slug: string
    icon: string
    title: string
    modules: Module[]
}

import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCoursesStore = defineStore('courses', () => {
    const allCourses = ref<any[]>([])
    const loading = ref(false)
    const error = ref<string|null>(null)

    const page = ref(1)
    const perPage = 5
    const totalPages = computed(() => Math.ceil(allCourses.value.length / perPage))
    const paged = computed(() => {
        const start = (page.value-1)*perPage
        return allCourses.value.slice(start, start+perPage)
    })

    async function fetchAll() {
        loading.value = true
        error.value = null
        try {
            const courses = await $fetch('http://localhost:5148/api/courses')
            const modules = await $fetch('http://localhost:5148/api/modules')
            const articlesByModuleId: Record<number, any[]> = {}
            await Promise.all(modules.map(async (m:any) => {
                try {
                    const articles = await $fetch(`http://localhost:5148/api/modules/${m.id}/articles`)
                    articlesByModuleId[m.id] = articles || []
                } catch {
                    articlesByModuleId[m.id] = []
                }
            }))
            allCourses.value = (courses || []).map((course:any) => {
                const slug = course.publicId || course.id || course.title?.toLowerCase().replace(/\s+/g, '-')
                const courseModules = modules.filter((m:any) => m.courseId === course.id)
                    .sort((a, b) => (a.order ?? 0) - (b.order ?? 0))
                    .map((mod:any, idx:number) => ({
                        id: mod.id,
                        num: idx+1,
                        title: mod.title,
                        icon: mod.image || '/mascot/mascot.png',
                        articles: (articlesByModuleId[mod.id] || []).map((art:any, idx2:number) => ({
                            slug: art.id || art.title?.toLowerCase().replace(/\s+/g, '-'),
                            num: `${idx+1}.${idx2+1}`,
                            title: art.title
                        }))
                    }))
                return {
                    id: course.id,
                    slug,
                    icon: course.image || '/mascot/mascot.png',
                    title: course.title,
                    modules: courseModules
                }
            })
        } catch (e:any) {
            error.value = e?.message || 'Ошибка загрузки курсов'
        }
        loading.value = false
    }

    function prev(){ if(page.value>1) page.value-- }
    function next(){ if(page.value<totalPages.value) page.value++ }
    function goToPage(n:number) { page.value = n }

    function findCourseBySlug(slug:string) {
        return allCourses.value.find((c:any) => c.slug === slug)
    }
    function findModule(courseSlug:string, moduleId:number) {
        const course = findCourseBySlug(courseSlug)
        return course?.modules.find((m:any) => m.id === moduleId)
    }
    function findArticle(courseSlug:string, moduleId:number, articleSlug:string) {
        const module = findModule(courseSlug, moduleId)
        return module?.articles.find((a:any) => a.slug === articleSlug)
    }

    return {
        allCourses, loading, error,
        page, perPage, totalPages, paged,
        prev, next, goToPage,
        fetchAll,
        findCourseBySlug, findModule, findArticle,
    }
})
