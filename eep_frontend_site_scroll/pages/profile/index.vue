<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg">
      <h2>Профиль</h2>
    </div>

    <div class="flex flex-col lg:flex-row p-6 gap-6">

      <div class="flex flex-col items-center self-start inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4">
        <img :src="user.avatar" class="w-24 h-24 object-cover rounded-xl shadow object-top" />
        <p class="mt-2 text-sm font-semibold">{{ user.username }}</p>
      </div>

      <div class="flex-1 inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4 space-y-1">
        <p><span class="font-semibold">ФИО:</span> {{ user.fullname }}</p>
        <p><span class="font-semibold">Почта:</span> {{ user.email }}</p>
        <p><span class="font-semibold">Тема:</span> default</p>
        <p><span class="font-semibold">Шрифт:</span> sans</p>
      </div>

      <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4 text-center w-60 shrink-0">
        <p class="font-semibold mb-1">Рейтинг: {{ user.rating }}</p>
        <p class="text-yellow-500 text-lg select-none">
          ★<span v-if="user.rating>1">★</span><span v-if="user.rating>2">★</span><span v-if="user.rating>3">★</span><span v-if="user.rating>4">★</span>
        </p>
        <p class="text-sm mt-2">Прохождение: {{ (user.progress*100).toFixed(0) }}%</p>

        <button v-on:click="logout"
                class="btn w-full mt-4 text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500
                 bg-red-500 hover:bg-red-500/70 border-none">
          Выход
        </button>
      </div>
    </div>
  </div>

  <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 mx-8 mt-12 p-2 rounded-t-lg">
    <h2>Текущий курс</h2>
  </div>
  <div class="mx-8 p-6 inset-shadow-sm rounded-b-lg shadow-md inset-shadow-red-300/60 bg-orange-50 opacity-90 space-y-4">

    <h3 class="text-lg font-semibold text-shadow-lg">{{ course.title }}</h3>
    <p class="text-sm">{{ course.about }}</p>

    <div class="flex-col p-4 font-semibold text-shadow-lg text-black">
      <p class="mb-2">Пройдено модулей: {{ course.modulesDone }} / {{ course.modulesTotal }}</p>
      <p>Процент прохождения: {{ percent }}%</p>
      <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60">
        <Progress :model-value="percent" class="progress progress-error w-full"></Progress>
      </div>
    </div>

    <!-- кнопки -->
    <div class="flex flex-col sm:flex-row gap-4">
      <NuxtLink :to="`/courses/${course.id}`" class="flex-1">
        <button class="btn w-full text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500
                       bg-red-500/50 hover:bg-red-500/70 border-none">
          Перейти к курсу
        </button>
      </NuxtLink>

      <button v-on:click="changeCourse"
              class="btn flex-1 text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500
                     bg-red-500 hover:bg-red-500/70 border-none">
        Сменить курс
      </button>
    </div>
  </div>

  <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 mx-8 mt-12 p-2 rounded-t-lg">
    <h2>Последние достижения</h2>
  </div>
  <div class="mx-8 bg-orange-50 opacity-90 inset-shadow-sm rounded-b-lg shadow-md inset-shadow-red-300/60 p-4 grid md:grid-cols-2 lg:grid-cols-3 gap-4">
    <div v-for="cat in achItemsLinks" :key="cat.value" class="shadow-md rounded-lg">
      <div class="font-bold text-white text-shadow-lg/20 bg-red-500/50 mb-2 p-2 rounded-t-lg flex justify-between items-center">
        {{ cat.title }}
      </div>

      <ScrollArea class="h-30 w-full rounded-b-lg">
        <div v-for="ach in cat.ach" :key="ach.title" class="flex gap-4 p-2 inset-shadow-sm inset-shadow-red-500/50 rounded-lg mb-2 mx-4">
          <div class="avatar w-12 h-12 self-start">
            <div class="rounded shadow-sm shadow-black w-12 h-12">
              <img :src="ach.icon" class="object-top" />
            </div>
          </div>
          <div class="flex-1">
            <p class="font-semibold text-sm">{{ ach.title }}</p>
            <p class="font-normal text-xs">{{ ach.text }}</p>
          </div>
        </div>
      </ScrollArea>
    </div>
  </div>

  <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 mx-8 mt-12 p-2 rounded-t-lg ">
    <h2>Ваш персонаж</h2>
  </div>
  <div class="mx-8 p-6 inset-shadow-sm rounded-b-lg shadow-md inset-shadow-red-300/60 bg-orange-50 opacity-90
              flex flex-col md:flex-row items-center gap-6 justify-center mb-12">
    <img :src="character.photo" class="w-32 h-32 rounded-box object-cover shadow-md object-top"  />
    <div class="font-semibold text-shadow-lg text-center md:text-left">
      <p class="text-lg">{{ character.name }}</p>
      <p class="text-sm">Уровень {{ character.level }}</p>
      <p class="text-xs opacity-70">Опыт {{ character.xp }} / {{ character.next }}</p>
      <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60">
        <Progress :model-value="percent_character" class="progress progress-error w-full"></Progress>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">

const user = reactive({
  username: 'Gregulas',
  fullname: 'Грег Улаз',
  email: 'greg@mail.com',
  avatar: '/mascot/mascot.png',
  rating: 4.5,
  progress: 0.6,
})

const achItemsLinks = [
  { value:'cat‑1', title:'JavaScript',      ach:[{ value: 'achL-1', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },{ value: 'achL-2', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },{ value: 'achL-3', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' }] },
  { value:'cat‑2', title:'Vue',             ach:[{ value: 'achL-1', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' }] },
  { value:'cat‑3', title:'Nuxt',            ach:[{ value: 'achL-1', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' }] },

]

const character = {
  name:'Пчелка',
  photo:'/mascot/mascot.png',
  level:4,
  xp:500,
  next:1500,
}

const course = reactive({
  id: 'course_js_base',
  title: 'Основы JavaScript',
  about: '42 модулей, Синтаксис, Асинхронность, Тесты',
  modulesDone: 28,
  modulesTotal: 42,
})

const percent = computed(() =>
    Math.round((course.modulesDone / course.modulesTotal) * 100))

const percent_character = computed(() =>
    Math.round((character.xp / character.next) * 100))


function logout () { alert('Вы вышли из аккаунта') }

</script>