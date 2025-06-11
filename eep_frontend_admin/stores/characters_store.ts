import { defineStore } from 'pinia'
import type { Ref } from 'vue'

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
    const characters: Ref<Character[]> = ref([])
    const loading: Ref<boolean> = ref(false)
    const error: Ref<string | null> = ref(null)

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

    async function createCharacter(character: Partial<Character>) {
        await $fetch('http://localhost:5148/api/admin_panel/characters', {
            method: 'POST',
            body: character,
        })
    }

    async function updateCharacter(id: number, patch: Partial<Character>) {
        await $fetch(`http://localhost:5148/api/admin_panel/characters/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }

    async function replaceCharacter(id: number, character: Character) {
        await $fetch(`http://localhost:5148/api/admin_panel/characters/${id}`, {
            method: 'PUT',
            body: character,
        })
    }

    async function deleteCharacter(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/characters/${id}`, {
            method: 'DELETE',
        })
    }

    return {
        characters, loading, error,
        fetchCharacters, createCharacter, updateCharacter, replaceCharacter, deleteCharacter,
    }
})
