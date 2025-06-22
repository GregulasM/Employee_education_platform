import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Character {
    id: number
    publicId: string
    name: string | null
    avatar: string | null
    description: string | null
    baseStats: string | null
    cosmetics: string | null
    skills: string | null
    default: boolean | null
    rarity: string | null
    unlockCond: string | null
    isActive: boolean | null
}

export const useCharactersStore = defineStore('characters', () => {
    const characters = ref<Character[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchCharacters() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Character[]>('http://localhost:5148/api/characters')
            characters.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки персонажей'
        }
        loading.value = false
    }

    async function fetchCharacterById(id: number) {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<Character>(`http://localhost:5148/api/characters/${id}`)
            loading.value = false
            return res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки персонажа'
            loading.value = false
            return null
        }
    }

    return {
        characters,
        loading,
        error,
        fetchCharacters,
        fetchCharacterById,
    }
})
