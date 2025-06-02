<template>
  <div class="bg-gray-700 h-screen">
    <div>
      <div v-if="!site_store.active_page" class="flex justify-center p-4 gap-4">
        <NuxtLink to="/game_module" v-on:click="site_store.switch_vision()"
                  class="btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4">
          Модуль геймификации</NuxtLink>
        <NuxtLink to="/course_module" v-on:click="site_store.switch_vision()"
                  class="btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4">
          Модуль статей</NuxtLink>
        <NuxtLink to="/user_module" v-on:click="site_store.switch_vision()"
                  class="btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4">
          Модуль пользователей</NuxtLink>
<!--        router.push('/game_module')-->
      </div>
    </div>
    <slot v-if="site_store.active_page && (router.name === 'course_module' || router.name === 'game_module' || router.name === 'user_module')"/>
  </div>
</template>
<script setup lang="ts">

import { useSiteStore } from '~/stores/site_store'
import {useRoute} from "vue-router"

const site_store = useSiteStore()

const router = useRoute()



</script>