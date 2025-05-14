<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg">
      <h2>{{ course.title }}</h2>
    </div>

    <div class="p-4">
      <div v-for="m in course.modules" :key="m.id"
           class="collapse collapse-arrow bg-white border border-red-500/50 rounded-lg mb-4">
        <input type="checkbox">
        <div class="collapse-title font-semibold flex items-center gap-4">
          <img :src="m.icon" class="w-8 h-8 rounded-box object-cover">
          {{ m.num }}. {{ m.title }}
        </div>

        <div class="collapse-content">
          <ul class="space-y-2">
            <li v-for="a in m.articles" :key="a.slug"
                class="flex items-center gap-2 inset-shadow-sm inset-shadow-red-500/50 p-2 rounded-lg">
              <span class="text-xs opacity-60 w-12">{{ a.num }}</span>
              <button class="link-hover flex-1 text-left" @click="openArticle(m.id,a.slug)">
                {{ a.title }}
              </button>
            </li>
          </ul>
        </div>

      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const courseSlug = route.params.course as string

const course = reactive({
  title: 'Основы ASP.NET Core',
  modules: [
    {
      id:'intro', num:1,  title:'Введение в ASP.NET Core', icon:'/mascot/mascot.png',
      articles:[
        { slug:'1-1', num:'1.1', title:'Установка и настройка ASP.NET Core' },
        { slug:'1-2', num:'1.2', title:'Hello World‑приложение' }
      ]
    },
    {
      id:'mvc', num:2,  title:'MVC', icon:'/mascot/mascot.png',
      articles:[
        { slug:'2-1', num:'2.1', title:'Контроллеры и маршруты' },
        { slug:'2-2', num:'2.2', title:'Views и Razor' }
      ]
    }
  ]
})

function openArticle(mod:string, art:string){
  router.push(`/courses/${courseSlug}/${mod}/${art}`)
}
</script>