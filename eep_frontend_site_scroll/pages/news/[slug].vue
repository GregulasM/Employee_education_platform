<template>
  <div>
    <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90 h-min">

      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
        <h2>Новость: {{ article.title }}</h2>
        <button class="drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888" d="M17.707..."/></svg>
        </button>
      </div>
      <div class="flex-col p-4 font-semibold text-shadow-lg rounded-box shadow-md text-black space-y-6">

        <h3 class="text-lg">Введение</h3>
        <p class="whitespace-pre-line">{{ article.content.intro }}</p>

        <h3 class="text-lg">Основной материал</h3>
        <p class="whitespace-pre-line">{{ article.content.body }}</p>

        <div class="carousel w-full rounded-box">
          <div v-for="(img, idx) in article.images" :key="idx" :id="`slide${idx+1}`" class="carousel-item relative w-full">
            <img :src="img" class="w-full object-cover h-64 rounded-md" />
            <div class="absolute left-5 right-5 top-1/2 flex justify-between transform -translate-y-1/2">
              <a :href="`#slide${idx===0 ? article.images.length : idx}`" class="btn btn-circle">❮</a>
              <a :href="`#slide${idx+2 > article.images.length ? 1 : idx+2}`" class="btn btn-circle">❯</a>
            </div>
          </div>
        </div>

        <div class="flex items-center space-x-4 pt-4 border-t">
          <img :src="article.author.avatar" class="w-12 h-12 rounded-full" />
          <div>
            <NuxtLink :to="article.author.profile" class="font-bold hover:text-red-500">
              {{ article.author.name }}
            </NuxtLink>
            <p class="italic text-sm opacity-70">"{{ article.author.comment }}"</p>
          </div>
        </div>
      </div>
      <div class="mt-8"></div>
    </div>
    <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90 h-min">
      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
        <h2>Комментарии:</h2>
        <button class="drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
        </button>
      </div>
      <div class="flex-col p-4 font-semibold text-shadow-lg rounded-box shadow-md text-black space-y-6">
        <div class="chat chat-start">
          <div class="chat-image avatar">
            <div class="w-10 rounded-full">
              <img
                  alt="Tailwind CSS chat bubble component"
                  src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
            </div>
          </div>
          <div class="chat-header">
            Валера
            <time class="text-xs opacity-50">12:45</time>
          </div>
          <div class="chat-bubble">Тестовая штука</div>
        </div>
        <div class="chat chat-start">
          <div class="chat-image avatar">
            <div class="w-10 rounded-full">
              <img
                  alt="Tailwind CSS chat bubble component"
                  src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
            </div>
          </div>
          <div class="chat-header">
            Валера
            <time class="text-xs opacity-50">12:45</time>
          </div>
          <div class="chat-bubble">Тестовая штука</div>
        </div>
        <div class="chat chat-start">
          <div class="chat-image avatar">
            <div class="w-10 rounded-full">
              <img
                  alt="Tailwind CSS chat bubble component"
                  src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
            </div>
          </div>
          <div class="chat-header">
            Валера
            <time class="text-xs opacity-50">12:45</time>
          </div>
          <div class="chat-bubble">Тестовая штука</div>
        </div>
      </div>
    </div>
    <div class="mt-8"></div>
  </div>
</template>

<script setup>
import { useRoute } from 'vue-router'
import { ref } from 'vue'

const route = useRoute()
const slug = route.params.slug

// Буду заменять на fetch с API
const allNews = [
  {
    slug: 'js-course',
    title: 'Новый курс по JS',
    content: {
      intro: 'Lorem ipsum dolor sit amet...',
      body: 'Phasellus nec felis nibh. Pellentesque ullamcorper urna...'
    },
    images: [
      'https://img.daisyui.com/images/stock/photo-1625726411847-8cbb60cc71e6.webp',
      'https://img.daisyui.com/images/stock/photo-1609621838510-5ad474b7d25d.webp'
    ],
    author: {
      name: 'Валера Лесник',
      avatar: 'https://img.daisyui.com/images/avatar/large/paul.jpg',
      profile: '/profile',
      comment: 'Какой же крутой!'
    }
  },
]

const article = ref(allNews.find(n => n.slug === slug) || allNews[0])
</script>