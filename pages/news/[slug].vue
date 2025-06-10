<template>
  <div v-if="article" class="mx-auto mt-8 mb-8">
    <div class="shadow-md shadow-orange-200 rounded-lg bg-orange-50 opacity-90 h-min mb-8">

      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
        <h2>Новость: {{ article.title }}</h2>
        <span class="text-sm opacity-60">{{ dateString }}</span>
      </div>


      <div class="flex-col p-4 font-semibold text-shadow-lg rounded-box shadow-md text-black space-y-6">

        <div>
          <TiptapViewer class="tiptap-content" v-if="tiptapContent" :content="tiptapContent" />
        </div>

        <div v-if="article.author" class="flex items-center space-x-4 pt-4 border-t">
          <img :src="article.author.avatar" class="w-12 h-12 rounded-full" />
          <div>
            <NuxtLink :to="article.author.profile" class="font-bold hover:text-red-500">
              {{ authorName }}
            </NuxtLink>
            <p v-if="article.author.comment" class="italic text-sm opacity-70">"{{ article.author.comment }}"</p>
          </div>
        </div>
      </div>
    </div>

    <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90 h-min">
      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
        <h2>Комментарии:</h2>
        <button class="drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
        </button>
      </div>
      <div class="flex-col p-4 font-semibold text-shadow-lg rounded-box shadow-md text-black space-y-6">
        <div class="pt-4 border-t space-y-3">
            <textarea
                v-model="newCommentText"
                rows="3"
                placeholder="Напишите комментарий…"
                class="textarea textarea-bordered w-full resize-y bg-white"
            />
          <div class="flex gap-2 justify-between">
            <button
                class="btn btn-ghost text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500"
                @click="sendComment"
                :disabled="sendingComment || !newCommentText.trim()"
            >
              {{ sendingComment ? 'Отправка…' : 'Отправить' }}
            </button>

            <button
                class="btn btn-ghost text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500"
                @click="newCommentText = ''"
                :disabled="sendingComment"
            >
              Стереть
            </button>
          </div>
        </div>
        <template v-if="loadingComments">
          <div class="text-lg text-red-400">Загрузка комментариев...</div>
        </template>
        <template v-else-if="comments.length === 0">
          <div class="italic text-gray-400">Комментариев пока нет.</div>
        </template>
        <template v-else>

          <div v-for="comment in comments" :key="comment.id" class="chat chat-start">
            <div class="chat-image avatar">
              <div class="w-10 rounded-full">
                <img
                    :alt="comment.userName || 'User'"
                    :src="comment.avatar"
                    class="object-cover"
                />
              </div>
            </div>
            <div class="chat-header">
              {{ comment.userName || 'Пользователь' }}
              <time class="text-xs opacity-50 ml-2">{{ comment.time }}</time>
            </div>
            <div class="chat-bubble">{{ comment.text }}</div>
          </div>
        </template>
      </div>
    </div>
    <div class="mt-8"></div>
  </div>
  <div v-else class="flex justify-center items-center h-32 text-2xl text-red-500/50 font-bold">Загрузка...</div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const slug = route.params.slug

const article = ref<any>(null)
const tiptapContent = ref(null)
const newCommentText = ref('')
const sendingComment = ref(false)


async function sendComment() {
  if (!newCommentText.value.trim()) return
  try {
    sendingComment.value = true
    await $fetch('http://localhost:5148/api/admin_panel/comments', {
      method: 'POST',
      body: {
        newsId: article.value.id,
        userId: 1,               // СДЕЛАТЬ ПОТОМ
        text: newCommentText.value.trim()
      }
    })
    newCommentText.value = ''
    await loadComments()
  } catch (e) {
    alert('Не удалось отправить комментарий')
  } finally {
    sendingComment.value = false
  }
}

async function loadComments() {
  try {
    loadingComments.value = true
    const data = await $fetch('http://localhost:5148/api/admin_panel/comments')
    comments.value = (data || [])
        .filter(c => c.newsId === article.value.id)
        .map(c => ({
          id: c.id,
          userName: c.userName,
          text: c.text,
          time: formatDateTime(c.createdAt),
          avatar: c.userAvatar || 'https://img.daisyui.com/images/avatar/large/paul.jpg'
        }))
  } catch {
    errorComments.value = 'Ошибка загрузки комментариев'
  } finally {
    loadingComments.value = false
  }
}

const authorName = computed(() =>
    article.value?.author
        ? [article.value.author.firstName, article.value.author.lastName].filter(Boolean).join(' ')
        : ''
)
const dateString = computed(() =>
    article.value?.date ? new Date(article.value.date).toLocaleDateString('ru-RU') : ''
)

const comments = ref<any[]>([])
const loadingComments = ref(true)
const errorComments = ref<string|null>(null)

function formatDateTime(str: string) {
  if (!str) return ''
  const date = new Date(str)
  return date.toLocaleDateString('ru-RU') + ' ' + date.toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' })
}

onMounted(async () => {
  const response = await $fetch(`http://localhost:5148/api/admin_panel/users/news/${useRoute().params.slug}`)
  article.value = response
  try {
    if (typeof article.value.content === 'string') {
      tiptapContent.value = JSON.parse(article.value.content)
    } else {
      tiptapContent.value = article.value.content
    }
  } catch (e) {
    tiptapContent.value = null
  }

  await loadComments()
})
</script>
