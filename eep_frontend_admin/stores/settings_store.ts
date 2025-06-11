import { defineStore } from 'pinia'
import type { Ref } from 'vue'

export interface Setting {
    id: number
    publicId: string | null
    type: string | null
    name: string | null
    value: string | null
    icon: string | null
    isActive: boolean | null
}

export const useSettingsStore = defineStore('settings', () => {
    const settings: Ref<Setting[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const themes = computed(() => settings.value.filter(s => s.type === 'theme'))
    const fonts = computed(() => settings.value.filter(s => s.type === 'font'))
    const error: Ref<string | null> = ref(null)

    async function fetchThemes() {
        themes.value = await $fetch('http://localhost:5148/api/admin_panel/settings?type=theme')
    }
    async function fetchFonts() {
        fonts.value = await $fetch('http://localhost:5148/api/admin_panel/settings?type=font')
    }

    async function fetchSettings() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Setting[]>('http://localhost:5148/api/admin_panel/settings')
            settings.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки настроек'
        }
        loading.value = false
    }

    async function createSetting(setting: Partial<Setting>) {
        await $fetch('http://localhost:5148/api/admin_panel/settings', {
            method: 'POST',
            body: setting,
        })
    }

    async function updateSetting(id: number, patch: Partial<Setting>) {
        await $fetch(`http://localhost:5148/api/admin_panel/settings/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function deleteSetting(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/settings/${id}`, {
            method: 'DELETE',
        })
    }

    return { settings, loading, error, themes, fonts, fetchThemes, fetchFonts, fetchSettings, createSetting, updateSetting, deleteSetting }
})
