<template>
  <div>
    <div>
      <div class="w-full flex justify-center pt-4 px-4 gap-4">
        <h2 class=" text-center  text-2xl text-white font-bold text-shadow-lg">Модуль курсов</h2>
      </div>
      <div class="flex justify-center p-4 gap-4 w-full">
        <NuxtLink draggable="false" to="/course_module/courses"
            class="flex-1 btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Курсы</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/modules"
            class="flex-1 btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Модули</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/articles"
                  class="flex-1 btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
                  >
          Статьи</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/tests"
                  class="flex-1 btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
                  >
          Тесты</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/useful_links"
                  class="flex-1 btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
                  >
          Полезные ссылки</NuxtLink>
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