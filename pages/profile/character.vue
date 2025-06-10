<!--profile/character-->
<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">

    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Персонаж</h2>
    </div>

    <div class="p-6 flex flex-col items-center">
      <!--        max-w-xl-->
      <div class="relative w-full ">


        <div class="flex flex-col  items-center gap-6 justify-center">
          <img :src="current.photo" class="w-40 h-40 rounded-box object-cover shadow-md object-top" />
          <div class="space-y-2 font-semibold text-shadow-lg text-center md:text-left max-h-40 overflow-y-auto pr-2">
            <h3 class="text-lg">{{ current.name }}</h3>
            <p class="text-sm whitespace-pre-line">{{ current.about }}</p>
            <div class="text-xs opacity-70">Уровень {{ current.stats.level }} • Опыт {{ current.stats.xp }}</div>
          </div>
        </div>

        <button @click="prev" class="btn text-shadow-lg/20 bg-red-500/50 border-none absolute left-2 top-1/2 z-10">❮</button>
        <button @click="next" class="btn text-shadow-lg/20 border-none bg-red-500/50 absolute right-2 top-1/2 z-10">❯</button>
      </div>

      <div class="text-center mt-6">
        <button
            @click="toggleChoose"
            class="btn text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 border-none"
            :class="isChosen ? 'bg-red-500/70 hover:bg-red-500/60' : 'bg-red-500/50 hover:bg-red-500/70'">

          {{ isChosen ? 'Удалить персонажа' : 'Выбрать персонажа' }}
        </button>
      </div>

      <p v-if="chosenId" class="text-xs mt-4">
        Вы выбрали: <span class="font-semibold">{{ characters.find(c => c.id === chosenId)?.name }}</span>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">


const characters = [
  { id:'pers-1', photo:'https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13', name:'Цветок из PVZ', about:'Фронтенд‑маг', stats:{level:4,xp:1200} },
  { id:'pers-2', photo:'/mascot/mascot.png', name:'Пчелка',   about:'Главный маскот, мастер меда и холодкааа',  stats:{level:3,xp:900}  },
  { id:'pers-3', photo:'https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13', name:'Макс',  about:'Питонистер',  stats:{level:5,xp:1800} },
  { id:'pers-4', photo:'https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13', name:'Максим',  about:'Годжо С#', stats:{level:2,xp:500} },
  { id:'pers-5', photo:'https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13', name:'Валера',   about:'Androido', stats:{level:3,xp:780}  },
  { id:'pers-6', photo:'https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13', name:'Виктор',  about:'Data‑саентисто', stats:{level:4,xp:1400} },
  { id:'pers-7', photo:'https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13', name:'Вадим',  about:'Грызуны',    stats:{level:1,xp:200}  },
]

const slide = ref(0)
const chosenId = ref<string|null>(null)

const current   = computed(() => characters[slide.value])
const isChosen  = computed(() => chosenId.value === current.value.id)

function prev() { slide.value = (slide.value + characters.length - 1) % characters.length }
function next() { slide.value = (slide.value + 1) % characters.length }
function toggleChoose() { chosenId.value = isChosen.value ? null : current.value.id }
</script>
