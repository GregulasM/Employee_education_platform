<template>
  <div class="bg-white shadow-md rounded-xl p-8 space-y-6 max-w-md mx-auto mt-16">
    <h1 class="text-2xl font-bold text-center mb-2 text-red-500/50">Вход в аккаунт</h1>
    <form @submit.prevent="submit">
      <div class="mb-4">
        <label class="block text-gray-700 mb-1">Логин или Email</label>
        <input v-model="login" type="text" class="w-full input input-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " required/>
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 mb-1">Пароль</label>
        <input v-model="password" type="password" class="w-full input input-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " required/>
      </div>
      <button :disabled="loading" class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4">
        {{ loading ? 'Вход...' : 'Войти' }}
      </button>
    </form>
    <div v-if="error" class="text-red-500 text-center">{{ error }}</div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useUsersStore } from '~/stores/users_store'

const login = ref('')
const password = ref('')
const loading = ref(false)
const error = ref('')

const userStore = useUsersStore()
const router = useRouter()

async function submit() {
  loading.value = true
  error.value = ''
  try {
    await userStore.login(login.value, password.value)
    await router.push('/')
  } catch (e: any) {
    error.value = e?.message || e?.data?.message || 'Неверный логин или пароль'
    password.value = ''
  } finally {
    loading.value = false
  }
}
</script>
