<template>
  <div class="min-h-screen bg-gradient-to-br from-orange-100 to-red-100 flex items-center justify-center" >
    <div class="w-full max-w-md mx-auto">
      <slot v-if="ready && !userStore.isAuthenticated"/>
      <div v-else-if="ready && userStore.isAuthenticated" class="flex justify-center">
        <span class="loading loading-spinner bg-red-500/50"></span>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import { useUsersStore } from '~/stores/users_store'
import { ref, onMounted } from 'vue'

const userStore = useUsersStore()
const ready = ref(false)

onMounted(() => {
  if (process.client) {
    userStore.loadSession()
  }
  setTimeout(() => { ready.value = true }, 0)
})
</script>