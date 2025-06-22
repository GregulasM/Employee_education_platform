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
        <div v-if="author" class="flex items-center space-x-4 pt-4 border-t">
          <img :src="author.avatar || defaultAvatar" class="w-12 h-12 rounded-full" />
          <div>
            <NuxtLink :to="author.profile" class="font-bold hover:text-red-500">
              {{ authorName }}
            </NuxtLink>
            <p v-if="author.comment" class="italic text-sm opacity-70">"{{ author.comment }}"</p>
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
        <template v-if="commentsLoading">
          <div class="text-lg text-red-400">Загрузка комментариев...</div>
        </template>
        <template v-else-if="commentsList.length === 0">
          <div class="italic text-gray-400">Комментариев пока нет.</div>
        </template>
        <template v-else>
          <div v-for="comment in commentsList" :key="comment.id" class="chat chat-start">
            <div class="chat-image avatar">
              <div class="w-10 rounded-full">
                <img
                    :alt="comment.userName || 'User'"
                    :src="comment.avatar || defaultAvatar"
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
import { useNewsStore } from '@/stores/news_store'
import { useUsersStore } from '@/stores/users_store'
import { useCommentsStore } from '@/stores/comments_store'

const route = useRoute()
const slug = route.params.slug as string

const newsStore = useNewsStore()
const usersStore = useUsersStore()
const commentsStore = useCommentsStore()

const article = ref<any>(null)
const tiptapContent = ref(null)
const author = ref<any>(null)
const newCommentText = ref('')
const sendingComment = ref(false)
const defaultAvatar = 'https://img.daisyui.com/images/avatar/large/paul.jpg'

// Комментарии
const commentsList = ref<any[]>([])
const commentsLoading = ref(true)

const authorName = computed(() => {
  if (!author.value) return ''
  return [author.value.firstName, author.value.lastName].filter(Boolean).join(' ')
})
const dateString = computed(() =>
    article.value?.date
        ? new Date(article.value.date).toLocaleDateString('ru-RU')
        : ''
)

function formatDateTime(str: string) {
  if (!str) return ''
  const date = new Date(str)
  return date.toLocaleDateString('ru-RU') + ' ' + date.toLocaleTimeString('ru-RU', { hour: '2-digit', minute: '2-digit' })
}

async function loadArticle() {
  // Загрузка новости
  await newsStore.fetchNews()
  article.value = newsStore.newsList.find(
      n => n.slug === slug
  )
  if (!article.value) return

  // Загрузка tiptap контента
  try {
    if (typeof article.value.content === 'string') {
      tiptapContent.value = JSON.parse(article.value.content)
    } else {
      tiptapContent.value = article.value.content
    }
  } catch (e) {
    tiptapContent.value = null
  }

  // Загрузка автора
  if (article.value.authorId) {
    await usersStore.fetchUsers()
    const user = usersStore.users.find(u => u.id === article.value.authorId)
    author.value = user
        ? {
          ...user,
          profile: `/users/${user.publicId || user.id}`,
        }
        : null
  } else if (article.value.author) {
    // Если автор уже пришёл
    author.value = {
      ...article.value.author,
      profile: `/users/${article.value.author.publicId || article.value.author.id || ''}`
    }
  } else {
    author.value = null
  }
}

async function loadComments() {
  commentsLoading.value = true
  await commentsStore.fetchComments()
  const newsId = article.value?.id
  let raw = commentsStore.comments.filter(c => c.newsId === newsId)
  await usersStore.fetchUsers()
  commentsList.value = raw.map(c => {
    const user = usersStore.users.find(u => u.id === c.userId)
    return {
      id: c.id,
      userName: user ? [user.firstName, user.lastName].filter(Boolean).join(' ') : 'Пользователь',
      avatar: user?.avatar || defaultAvatar,
      text: c.text,
      time: formatDateTime(c.createdAt || '')
    }
  })
  commentsLoading.value = false
}

async function sendComment() {
  if (!newCommentText.value.trim() || !article.value?.id) return
  try {
    sendingComment.value = true
    // тут нужно передать userId — сейчас пример с userId: 1, замените на текущего пользователя, если авторизация есть!
    await commentsStore.createComment({
      newsId: article.value.id,
      userId: 1,
      text: newCommentText.value.trim()
    })
    newCommentText.value = ''
    await loadComments()
  } catch (e) {
    alert('Не удалось отправить комментарий')
  } finally {
    sendingComment.value = false
  }
}

onMounted(async () => {
  await loadArticle()
  await loadComments()
})
</script>
