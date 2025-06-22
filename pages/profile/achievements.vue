<!-- profile/achievements.vue -->
<template>
  <div>
    <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">

      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center mb-4">
        <h2>Достижения</h2>
      </div>
      <div class="p-2 font-semibold text-shadow-lg p-4 mb-4">
        <ScrollArea class="h-180 w-full rounded-md ">
          <div class="flex-col mr-4 font-semibold text-shadow-lg whitespace-pre-line grid grid-cols-3 gap-6 mb-2">

            <!-- Загрузка -->
            <div v-if="listsLoading" class="col-span-3 flex items-center justify-center text-red-500 text-lg py-10">Загрузка...</div>
            <div v-else-if="listsError" class="col-span-3 flex items-center justify-center text-red-600 text-lg py-10">{{ listsError }}</div>

            <div v-else v-for="item in achievementLists" :key="item.id" :value="item.id" class="shadow-md rounded-lg ml-4 mb-4">
              <div class="font-bold text-white text-shadow-lg/20 bg-red-500/50 mb-2 p-2 rounded-t-lg flex justify-between items-center">
                <h2>{{ item.name }}</h2>
              </div>
              <div class="flex-col font-semibold text-shadow-lg">
                <ScrollArea class="h-72 w-full rounded-md mb-2">
                  <!-- Лоадер по конкретному листу -->
                  <div v-if="achievementsLoading[item.id]" class="text-center text-xs text-red-400 py-8">Загрузка достижений...</div>
                  <template v-else>
                    <div
                        v-for="ach in achievementsByList[item.id]"
                        :key="ach.id"
                        :value="ach.id"
                        class="flex-col mr-4 font-semibold text-shadow-lg whitespace-pre-line ml-4"
                    >
                      <div class="flex gap-4 p-2 inset-shadow-sm inset-shadow-red-500/50 rounded-lg mb-2 ">
                        <div class="avatar self-center ml-2">
                          <div class="w-12 h-12 rounded shadow-sm shadow-black">
                            <img :src="ach.icon || '/img/ach-placeholder.png'" />
                          </div>
                        </div>
                        <div class="flex-col">
                          <p class="font-semibold text-sm">{{ ach.name }}</p>
                          <p class="font-normal text-xs">{{ ach.description }}</p>
                          <p v-if="ach.points" class="font-normal text-xs text-red-700">+{{ ach.points }} очков</p>
                        </div>
                      </div>
                    </div>
                    <div v-if="!achievementsByList[item.id]?.length" class="text-xs opacity-50 p-4">Нет достижений</div>
                  </template>
                </ScrollArea>
              </div>
            </div>
          </div>
        </ScrollArea>
      </div>
      <div class="grid"></div>
      <div class="mt-8"/>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useAchievementListsStore } from '@/stores/achievement_lists_store'

const achievementListsStore = useAchievementListsStore()

const achievementLists = ref<any[]>([])
const achievementsByList = reactive<{ [key: number]: any[] }>({})
const achievementsLoading = reactive<{ [key: number]: boolean }>({})
const listsLoading = ref(true)
const listsError = ref<string | null>(null)


onMounted(async () => {
  listsLoading.value = true
  listsError.value = null
  try {
    await achievementListsStore.fetchLists()
    achievementLists.value = achievementListsStore.lists

    for (const item of achievementLists.value) {
      achievementsLoading[item.id] = true
      try {
        const res = await $fetch<any[]>(`http://localhost:5148/api/achievementlists/${item.id}/achievements`)
        achievementsByList[item.id] = res
      } catch {
        achievementsByList[item.id] = []
      }
      achievementsLoading[item.id] = false
    }
  } catch (e: any) {
    listsError.value = e?.message || 'Ошибка загрузки'
  }
  listsLoading.value = false
})
</script>
