import { defineStore } from 'pinia'

export const useSiteStore = defineStore('SiteStore', {
    state: () => ({
        active_page: false,
    }),
    actions: {
        switch_vision() {
            this.active_page = !this.active_page;
        },
    },
})