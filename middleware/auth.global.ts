import { defineNuxtRouteMiddleware, navigateTo } from '#app'
import { useUsersStore } from '~/stores/users_store'

export default defineNuxtRouteMiddleware(async (to) => {
    const publicPages = ['/login', '/register', '/reset-password']
    const userStore = useUsersStore()
    if (process.server) return

    userStore.loadSession()

    if (publicPages.includes(to.path)) {
        if (userStore.isAuthenticated) {
            window.location.replace('/')
        }
        return
    }

    if (!userStore.isAuthenticated) {
        window.location.replace('/login')
    }
})