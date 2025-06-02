<template>
  <div>
    <div>
      <div class="w-full flex justify-center pt-2">
        <h2 class="text-2xl text-white font-bold text-shadow-lg">Модуль пользователей</h2>
      </div>
      <div class="flex justify-center p-4 gap-4">
        <NuxtLink draggable="false" to="/user_module/users"
            class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Пользователи</NuxtLink>
        <NuxtLink draggable="false" to="/user_module/settings"
            class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Настройки</NuxtLink>
        <NuxtLink draggable="false" v-on:click="go_back()"
                  class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
        >
          Назад</NuxtLink>
      </div>

    </div>

  </div>
</template>
<script setup lang="ts">

import { useSiteStore } from '~/stores/site_store'
import {useRoute} from "vue-router"

const site_store = useSiteStore()

const router = useRouter()

function go_back () {
  router.back()
}

</script>