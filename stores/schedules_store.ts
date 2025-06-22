import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Schedule {
    id: number
    publicId: string | null
    dayOfWeek: string | null
    timeSlot: string | null
    subject: string | null
    teacher: string | null
    details: string | null
    courseId: number | null
    course?: { id: number; title: string } | null
    moduleId: number | null
    module?: { id: number; name: string } | null
    departmentId: number | null
    department?: { id: number; name: string } | null
    userId: number | null
    user?: { id: number; firstName: string } | null
    isActive: boolean | null
}

export const useScheduleStore = defineStore('schedule', () => {
    const schedules = ref<Schedule[]>([])
    const loading = ref<boolean>(false)
    const error = ref<string | null>(null)

    async function fetchSchedules() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Schedule[]>('http://localhost:5148/api/users/schedules')
            console.log(res)
            if (Array.isArray(res)) {
                schedules.value = res
            } else {
                schedules.value = []
                error.value = 'Некорректный формат ответа API'
            }
        } catch (err: any) {
            error.value = err?.message ?? 'Ошибка загрузки расписания'
            schedules.value = []
        } finally {
            loading.value = false
        }
    }

    function resetSchedules() {
        schedules.value = []
        loading.value = false
        error.value = null
    }

    return {
        schedules,
        loading,
        error,
        fetchSchedules,
        resetSchedules,
    }
})
