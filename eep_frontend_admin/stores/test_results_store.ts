// stores/test_results_store.ts

import { defineStore } from 'pinia'

export interface TestResult {
    id: number
    publicId: string
    userId: number | null
    user?: { id: number, login: string, firstName: string, lastName: string } | null
    testId: number | null
    test?: { id: number, title: string } | null
    score: number | null
    maxScore: number | null
    answers: string | null
    status: string | null
}

export const useTestResultsStore = defineStore('testResults', () => {
    const testResults = ref<TestResult[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)

    async function fetchTestResults() {
        loading.value = true
        error.value = null
        try {
            const res = await $fetch<TestResult[]>('http://localhost:5148/api/admin_panel/test_results')
            testResults.value = res
        } catch (err: any) {
            error.value = err.message ?? 'Ошибка загрузки результатов'
        }
        loading.value = false
    }

    async function createTestResult(dto: Partial<TestResult>) {
        await $fetch('http://localhost:5148/api/admin_panel/test_results', {
            method: 'POST',
            body: dto,
        })
    }
    async function updateTestResult(id: number, patch: Partial<TestResult>) {
        await $fetch(`http://localhost:5148/api/admin_panel/test_results/${id}`, {
            method: 'PATCH',
            body: patch,
        })
    }
    async function deleteTestResult(id: number) {
        await $fetch(`http://localhost:5148/api/admin_panel/test_results/${id}`, {
            method: 'DELETE',
        })
    }

    return { testResults, loading, error, fetchTestResults, createTestResult, updateTestResult, deleteTestResult }
})
