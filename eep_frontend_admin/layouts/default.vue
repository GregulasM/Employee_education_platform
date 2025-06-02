<template>
  <div class="bg-gray-700 h-screen">
    <div v-if="active_main_page">
      <div class="w-full flex justify-center pt-2">
        <h2 class="text-2xl text-white font-bold text-shadow-lg">Главная страница</h2>
      </div>
      <div  class="flex justify-center p-4 gap-4">
        <NuxtLink draggable="false" to="/course_module"
                  class="btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4">
          Модуль статей</NuxtLink>
        <NuxtLink draggable="false" to="/user_module"
                  class="btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4">
          Модуль пользователей</NuxtLink>
        <NuxtLink draggable="false" to="/game_module"
                  class="btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4">
          Модуль геймификации</NuxtLink>
      </div>
    </div>
    <slot/>
  </div>
</template>
<script setup lang="ts">

import { useSiteStore } from '~/stores/site_store'
import {useRoute} from "vue-router"

const active_main_page = computed(() => router.path === '/')

const router = useRoute()



</script>