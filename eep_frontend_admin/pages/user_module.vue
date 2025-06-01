<template>
  <div>
    <div>
      <div>

        <NuxtLink to="/"
            class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4"
            v-on:click="site_store.switch_vision()">
          Назад</NuxtLink>
      </div>

    </div>

  </div>
</template>
<script setup lang="ts">

import { useSiteStore } from '~/stores/site_store'
import {useRoute} from "vue-router"

const site_store = useSiteStore()

const router = useRoute()


</script>