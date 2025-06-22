<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Персонажи</h2>
    </div>

    <div class="p-6 flex flex-col items-center">
      <!-- Loader/Error -->
      <div v-if="loading" class="flex items-center justify-center py-10">Загрузка...</div>
      <div v-else-if="error" class="text-red-600">{{ error }}</div>
      <div v-else-if="!characters.length" class="text-gray-500">Нет персонажей для выбора</div>
      <div v-else class="relative w-full max-w-lg">
        <!-- Карусель -->
        <div class="flex flex-col items-center gap-6 justify-center">
          <img
              :src="currentCharacter?.avatar || '/mascot/mascot.png'"
              class="w-40 h-40 rounded-box object-cover shadow-md object-top bg-white"
              :alt="currentCharacter?.name || ''"
          />
          <div class="space-y-2 font-semibold text-shadow-lg text-center md:text-left max-h-40 overflow-y-auto pr-2">
            <h3 class="text-lg">{{ currentCharacter?.name }}</h3>
            <p class="text-sm whitespace-pre-line">{{ currentCharacter?.description }}</p>
            <div v-if="currentCharacter?.baseStats" class="text-xs opacity-70">{{ currentCharacter.baseStats }}</div>
            <div v-if="currentCharacter?.rarity" class="text-xs italic opacity-50">Редкость: {{ currentCharacter.rarity }}</div>
          </div>
        </div>
        <!-- Стрелки -->
        <button @click="prev" class="btn text-shadow-lg/20 bg-red-500/50 border-none absolute left-2 top-1/2 z-10">
          ❮
        </button>
        <button @click="next" class="btn text-shadow-lg/20 border-none bg-red-500/50 absolute right-2 top-1/2 z-10">
          ❯
        </button>
      </div>
      <!-- Выбор персонажа -->
      <div class="text-center mt-6">
        <button
            @click="toggleChoose"
            class="btn text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 border-none"
            :class="isChosen ? 'bg-red-500/70 hover:bg-red-500/60' : 'bg-red-500/50 hover:bg-red-500/70'"
            :disabled="!currentUser"
        >
          {{ isChosen ? 'Удалить персонажа' : 'Выбрать персонажа' }}
        </button>
      </div>
      <!-- Информация о выбранном -->
      <p v-if="chosenId" class="text-xs mt-4">
        Вы выбрали:
        <span class="font-semibold">
          {{ characters.find(c => c.id === chosenId)?.name || 'Неизвестно' }}
        </span>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useCharactersStore } from '~/stores/characters_store'
import { useUsersStore } from '~/stores/users_store'

const charactersStore = useCharactersStore()
const usersStore = useUsersStore()

const slide = ref(0)
const loading = computed(() => charactersStore.loading)
const characters = computed(() => charactersStore.characters)
const error = computed(() => charactersStore.error)

const currentUser = computed(() => usersStore.currentUser)
const chosenId = computed(() => currentUser.value?.selectedCharacterId ?? null)
const currentCharacter = computed(() => characters.value.length > 0 ? characters.value[slide.value] : null)
const isChosen = computed(() => currentCharacter.value?.id === chosenId.value)

function prev() {
  if (!characters.value.length) return
  slide.value = (slide.value + characters.value.length - 1) % characters.value.length
}
function next() {
  if (!characters.value.length) return
  slide.value = (slide.value + 1) % characters.value.length
}

async function toggleChoose() {
  if (!usersStore.currentUser?.id || !currentCharacter.value) return
  try {
    await usersStore.updateUser(usersStore.currentUser.id, {
      selectedCharacterId: isChosen.value ? null : currentCharacter.value.id,
      isSelectedCharacterIdSet: true
    })
    const updated = await usersStore.getUserById(usersStore.currentUser.id)
    if (updated && usersStore.currentUser) {
      usersStore.currentUser.selectedCharacterId = updated.selectedCharacterId
      Object.assign(usersStore.currentUser, updated)
      usersStore.saveSession(usersStore.currentUser, usersStore.token)
    }
  } catch (e) {
    alert('Ошибка выбора персонажа')
  }
}

onMounted(async () => {
  if (!characters.value.length) await charactersStore.fetchCharacters()
})
</script>