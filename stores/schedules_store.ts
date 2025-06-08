import { defineStore } from 'pinia'
import type { Ref } from 'vue'

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

export const useSchedulesStore = defineStore('schedules', () => {
    const schedules: Ref<Schedule[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

    async function fetchSchedules() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Schedule[]>('http://localhost:5148/api/users/schedules')
            schedules.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки расписаний'
        }
        loading.value = false
    }

    async function createSchedule(schedule: Partial<Schedule>) {
        await $fetch('http://localhost:5148/api/admin_panel/users/schedules', {
            method: 'POST',
            body: schedule,
        })
    }

    async function updateSchedule(id: number, patch: Partial<Schedule>) {
        await $fetch(`http://localhost:5148/api/admin_panel/users/schedules/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteSchedule(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/users/schedules/${id}`, {
            method: 'DELETE',
        })
    }

    return { schedules, loading, error, fetchSchedules, createSchedule, updateSchedule, deleteSchedule }
})
