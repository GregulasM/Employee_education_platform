<template>
  <div class="flex bg-radial-[at_25%_25%] bg-gradient-to-r from-red-50 via-orange-100 to-red-300 inset-0 text-black">
    <div class="w-1/4">
      <div class="drawer lg:drawer-open w-full">
        <input id="profile-drawer" type="checkbox" class="drawer-toggle" />
        <div class="drawer-content flex flex-col items-center pt-4 lg:pt-0">
          <label
              for="profile-drawer"
              class="btn btn-primary text-white bg-red-500/50 hover:bg-red-500/70
                   hover:border-none border-none font-semibold text-shadow-lg/20
                   shadow-sm shadow-neutral-500 mb-4 lg:hidden drawer-button swap">
            <span class="swap-off">Открыть меню</span>
            <span class="swap-on">Закрыть меню</span>
          </label>
        </div>

        <div class="drawer-side">
          <label for="profile-drawer" class="drawer-overlay"></label>
          <ul for="profile-drawer" class=" menu text-white min-h-full w-80 p-4 bg-white relative">
            <div class="flex justify-between">
              <NuxtLink
                to="/"
                class="btn lg:btn-block text-white bg-red-500/50 hover:bg-red-500/70
                   hover:border-none border-none font-semibold text-shadow-lg/20
                   shadow-sm shadow-neutral-500 mb-4">
                <svg class="" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><!-- Icon from MingCute Icon by MingCute Design - https://github.com/Richard9394/MingCute/blob/main/LICENSE --><g fill="none"><path d="m12.593 23.258l-.011.002l-.071.035l-.02.004l-.014-.004l-.071-.035q-.016-.005-.024.005l-.004.01l-.017.428l.005.02l.01.013l.104.074l.015.004l.012-.004l.104-.074l.012-.016l.004-.017l-.017-.427q-.004-.016-.017-.018m.265-.113l-.013.002l-.185.093l-.01.01l-.003.011l.018.43l.005.012l.008.007l.201.093q.019.005.029-.008l.004-.014l-.034-.614q-.005-.018-.02-.022m-.715.002a.02.02 0 0 0-.027.006l-.006.014l-.034.614q.001.018.017.024l.015-.002l.201-.093l.01-.008l.004-.011l.017-.43l-.003-.012l-.01-.01z"/><path fill="white" d="M10.772 2.688a2 2 0 0 1 2.317-.099l.139.1l8.384 6.52c.721.561.37 1.692-.499 1.785l-.116.006H20v8a2 2 0 0 1-1.85 1.995L18 21H6a2 2 0 0 1-1.994-1.85L4 19v-8h-.997C2.09 11 1.671 9.892 2.3 9.285l.088-.076zM12 4.267L5.625 9.225c.229.185.375.468.375.785V19h3v-5a3 3 0 1 1 6 0v5h3v-8.99c0-.317.146-.6.375-.785zM12 13a1 1 0 0 0-1 1v5h2v-5a1 1 0 0 0-1-1"/></g></svg>
              </NuxtLink>

              <label for="profile-drawer" class="btn btn-square top-2 right-2 bg-red-500/50 hover:bg-red-500/70 border-none text-white lg:hidden mb-4 self-end">✕</label></div>


            <NuxtLink
                to="/profile"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70
                   hover:border-none border-none font-semibold text-shadow-lg/20
                   shadow-sm shadow-neutral-500 mb-4"
            >Профиль</NuxtLink>

            <NuxtLink
                to="/profile/user_settings"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70
                   hover:border-none border-none font-semibold text-shadow-lg/20
                   shadow-sm shadow-neutral-500 mb-4"
            >Настройки</NuxtLink>

            <NuxtLink
                to="/profile/achievements"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70
                   hover:border-none border-none font-semibold text-shadow-lg/20
                   shadow-sm shadow-neutral-500 mb-4"
            >Все достижения</NuxtLink>

            <NuxtLink
                to="/profile/character"
                class="btn btn-block text-white bg-red-500/50 hover:bg-red-500/70
                   hover:border-none border-none font-semibold text-shadow-lg/20
                   shadow-sm shadow-neutral-500 mb-4"
            >Персонаж</NuxtLink>

          </ul>
        </div>
      </div>
    </div>

    <main class="w-1/2 p-4">
      <slot />
    </main>

    <div class="w-1/4 ">
      <div class="drop-shadow-2xl sticky top-0">
        <ScrollArea class="h-screen rounded-md p-2">
          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
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
          <div class="mt-8"/>
        </ScrollArea>
      </div>
    </div>

  </div>
</template>

<script setup>
const lastAchievements = [
  'Пройден курс «Vue 3»',
  'Пройден курс «JS»',
  'Пройден курс «Nuxt 3»',
]
</script>
