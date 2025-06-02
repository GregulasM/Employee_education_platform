<template>
  <div>
    <div>
      <div class="w-full flex justify-center pt-2">
        <h2 class="text-2xl text-white font-bold text-shadow-lg">Настройки</h2>
      </div>
      <div class="flex justify-center p-4 gap-4">
        <button
            class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-green-500/90 hover:bg-green-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Создать</button>
        <button
            class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-cyan-500/90 hover:bg-cyan-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Сортировка/Фильтрация</button>
        <button
            class="flex-1 btn btn-ghost w-1/4 h-12 text-white bg-amber-500/90 hover:bg-amber-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
            v-on:click="">
          Обновить</button>

        <NuxtLink draggable="false" v-on:click="go_back()"
                  class="flex-1 h-12 btn btn-ghost text-white bg-red-500/90 hover:bg-red-700/90 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-black/10 mb-4"
        >
          Назад</NuxtLink>
      </div>
      <div class="pl-10 pr-10 self-center items-center flex justify-center ">
        <ScrollArea class="h-100 w-full rounded-md ">
          <table class="table w-full shadow rounded-lg ">
            <thead class="sticky top-0 bg-gray-800/90 text-white/80">
            <tr>
              <th>ID</th><th>*Колонка_название*</th><th>*Колонка_название*</th><th>*Колонка_название*</th><th>*Колонка_название*</th><th>*Колонка_название*</th>
            </tr>
            </thead>

            <tbody>

            <tr>
              <td></td>
              <td></td>
              <td></td>
              <td></td>
              <td>
                <ul>
                  <li>

                  </li>
                </ul>
              </td>
              <td>
                <button class="btn btn-xs btn-ghost text-white text-shadow-sm self-center bg-red-500/80 border-none hover:shadow-none hover:bg-red-900 m-1 active:bg-red-900">Удалить 🗑️</button>
              </td>
            </tr>

            </tbody>

          </table>
        </ScrollArea>
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