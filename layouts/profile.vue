<template>
  <div class="flex flex-col lg:flex-row min-h-screen">
    <div class="w-full lg:w-1/4">
      <div class="drawer lg:drawer-open w-full">
        <input id="profile-drawer" type="checkbox" class="drawer-toggle" />
        <div class="drawer-content flex flex-col items-center pt-4 lg:pt-0">
          <label
              for="profile-drawer"
              class="btn btn-primary text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm
              shadow-neutral-500mb-4 lg:hidden drawer-button swap"
          >
            <span class="swap-off">Открыть меню</span>
            <span class="swap-on">Закрыть меню</span>
          </label>
        </div>

        <div class="drawer-side">
          <label for="profile-drawer" class=""></label>
          <ul class="menu text-white min-h-full w-80 p-4 bg-red-500/10">
            <NuxtLink
                to="/profile"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4"
            >
              Профиль
            </NuxtLink>
            <NuxtLink
                to="/profile/settings"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4"
            >
              Настройки
            </NuxtLink>
            <NuxtLink
                to="/profile/achievements"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4"
            >
              Все достижения
            </NuxtLink>
            <NuxtLink
                to="/profile/character"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 mb-4"
            >
              Персонаж
            </NuxtLink>
          </ul>
        </div>
      </div>
    </div>

    <main class="w-full lg:w-1/2 p-4">
      <slot />
    </main>

    <aside
        class="hidden lg:block w-1/4 p-4 space-y-4 overflow-y-auto"
    >
      <div
          class="shadow-md shadow-orange-200 rounded-lg bg-orange-50 opacity-90 h-min"
      >
        <div
            class="text-lg text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg"
        >
          Последние достижения
        </div>
        <ul class="p-4 space-y-2 text-sm font-semibold">
          <li v-for="(ach, i) in lastAchievements" :key="i">
            <span class="inline-block w-2 h-2 bg-red-400 rounded-full mr-2" />
            {{ ach }}
          </li>
        </ul>
      </div>
    </aside>
  </div>
</template>

<script setup>
const lastAchievements = [
  'Пройден курс «Vue 3»',
  'Пройден курс «JS»',
  'Пройден курс «Nuxt 3»',
]
</script>
