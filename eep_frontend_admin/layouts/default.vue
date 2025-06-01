<template>
  <div>
    <div>
      <div v-if="!site_store.active_page">
        <NuxtLink to="/game_module" v-on:click="site_store.switch_vision()"
                  class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4">
          Модуль геймификации</NuxtLink>
        <NuxtLink to="/course_module" v-on:click="site_store.switch_vision()"
                  class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4">
          Модуль статей</NuxtLink>
        <NuxtLink to="/user_module" v-on:click="site_store.switch_vision()"
                  class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4">
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