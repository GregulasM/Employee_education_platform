<template>
  <div>
    <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">

      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
        <h2>Новости</h2>
        <button class="justify-between drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
        </button>
      </div>

      <div class="flex-col p-2 font-semibold text-shadow-lg rounded-box shadow-md text-black">
        <div>
          <ul class="flex-col p-2 font-semibold list rounded-box shadow-md text-black">

            <li class="pb-2 text-xs self-center opacity-60 tracking-wide">
              <div class="join">
                <button v-on:click="prevPage" :disabled="page === 1"
                        class="join-item btn btn-sm bg-red-500/50 btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">«</button>

                <button v-for="n in totalPages" :key="n"
                        v-on:click="page = n"
                        class="join-item btn btn-sm btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none" >{{ n }}</button>

                <button v-on:click="nextPage" :disabled="page === totalPages"
                        class="join-item btn btn-sm bg-red-500/50 btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">»</button>
              </div>
            </li>

            <li v-for="item in pagedNews" :key="item.slug" class="list-row shadow-md shadow-orange-200">
              <div>
                <img class="size-10 rounded-box" :src="item.image" alt="preview"/>
              </div>
              <div>
                <div>{{ item.title }}</div>
                <div class="text-xs uppercase font-semibold opacity-60">{{ item.type }}</div>
              </div>
              <p class="list-col-wrap text-xs">
                {{ item.excerpt }}
              </p>

              <HoverCard>
                <HoverCardTrigger>
                  <button v-on:click="goToNews(item.slug)" class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">
                    <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/></svg>
                  </button>
                </HoverCardTrigger>
                <HoverCardContent class="text-sm opacity-90">
                  Откроется страница с новостью
                </HoverCardContent>
              </HoverCard>
            </li>

          </ul>
        </div>
      </div>

      <div class="mt-8"/>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

// Буду заменять на fetch с API
const newsItems = ref([
  { slug: 'js-course', title: 'Новый курс по JS', type: 'Обновление', excerpt: 'Мы добавили полный курс по JavaScript!', image: 'https://img.daisyui.com/images/profile/demo/1@94.webp' },
  { slug: 'site-update', title: 'Обновление страницы', type: 'Обновление', excerpt: 'Изменили главный экран и добавили скроллы!', image: 'https://img.daisyui.com/images/profile/demo/4@94.webp' },
  { slug: 'components-change', title: 'Размеры и компоненты', type: 'Изменение', excerpt: 'Поменяли размер аватарок и компонентов.', image: 'https://img.daisyui.com/images/profile/demo/3@94.webp' },
  { slug: 'new-event', title: 'Анонс нового мероприятия', type: 'Анонс', excerpt: 'Приглашаем на конференцию разработчиков.', image: 'https://img.daisyui.com/images/profile/demo/2@94.webp' },
  { slug: 'bugfixes', title: 'Мелкие исправления', type: 'Исправление', excerpt: 'Устранили несколько ошибок в верстке.', image: 'https://img.daisyui.com/images/profile/demo/5@94.webp' },
])

const router = useRouter()
const page = ref(1)
const perPage = 5
const totalPages = computed(() => Math.ceil(newsItems.value.length / perPage))
const pagedNews = computed(() => {
  const start = (page.value - 1) * perPage
  return newsItems.value.slice(start, start + perPage)
})

function prevPage() {
  if (page.value > 1) page.value--
}
function nextPage() {
  if (page.value < totalPages.value) page.value++
}
function goToNews(slug) {
  router.push(`/news/${slug}`)
}
</script>
