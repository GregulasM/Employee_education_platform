<template>
  <div>
    <div>
      <div class="w-full flex justify-center pt-4 px-4 gap-4">
        <h2 class=" text-center  text-2xl text-white font-bold text-shadow-lg">Модуль курсов</h2>
      </div>
      <div class="grid grid-cols-3 gap-4 w-full max-w-6xl mx-auto p-4 ">
        <NuxtLink draggable="false" to="/course_module/courses"
            class="btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Курсы</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/modules"
            class="btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Модули</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/articles"
                  class="btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
                  >
          Статьи</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/tests"
                  class="btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
                  >
          Тесты</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/test_results"
                  class="btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
        >
          Результаты тестов</NuxtLink>
        <NuxtLink draggable="false" to="/course_module/useful_links"
                  class="btn btn-ghost max-w-lg h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
                  >
          Полезные ссылки</NuxtLink>
        <NuxtLink draggable="false" v-on:click="go_back()"
                  class="col-start-3  max-w-lg h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
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