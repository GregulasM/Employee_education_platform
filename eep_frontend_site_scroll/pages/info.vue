<template>
  <div>
    <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
        <h2>О сайте</h2>
        <button class="drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
          <svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="16"
              viewBox="0 0 24 24"
          >
            <path
                fill="#888888"
                d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"
            />
          </svg>
        </button>
      </div>

      <div class="flex-col p-4 font-semibold text-shadow-lg rounded-box shadow-md text-black space-y-8">
        <section class="space-y-3">
          <h3 class="text-lg">Платформа</h3>
          <p class="text-md font-medium whitespace-no-wrap">
            Наша платформа для обучения сотрудников компании — это место, где работник может изучать
            структурированные курсы, интерактивные задания и различные кейсы в уютной обстановке!
            <br>
            Удобство и развитие в комплекте :D
            <br><br>
            Платформа доступна бесплатно на GitHub.
          </p>
        </section>

        <section class="space-y-3">
          <h3 class="text-lg">Наша команда</h3>
          <ul class="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            <li
                v-for="person in team"
                :key="person.name"
                class="flex items-center space-x-3 p-3 rounded-box shadow-md shadow-red-300/60 bg-white/70"
            >
              <img
                  :src="person.avatar"
                  class="size-12 rounded-box shadow-sm"
                  :alt="person.name"
              />
              <div class="flex-1">
                <div>{{ person.name }}</div>
                <div class="text-xs uppercase opacity-60">
                  {{ person.role }}
                </div>
              </div>
            </li>
          </ul>
        </section>

        <section class="space-y-3">
          <h3 class="text-lg">Наши ценности</h3>
          <div class="grid gap-4 md:grid-cols-3">
            <div
                v-for="value in values"
                :key="value.title"
                class="p-4 rounded-box shadow-md shadow-red-300/60  bg-orange-50/70 hover:bg-red-200 transition">
              <h4 class="mb-1">{{ value.title }}</h4>
              <p class="text-sm opacity-80">{{ value.text }}</p>
            </div>
          </div>
        </section>

        <section class="space-y-3">
          <h3 class="text-lg">Контакты</h3>
          <div class="space-y-1 text-md ">
            <div class="flex gap-4 ">
              <div class="card-side bg-white rounded-box shadow-md shadow-red-300/60">
                <figure >
                  <img
                      src="public/mascot/TelegramMaskot.png"
                      alt="Movie" />
                </figure>
                <div class="card-body ">
                  <h2 class="card-title font-bold">Профиль Telegram Gregulas</h2>
                  <p>Мой профиль в Telegram! Кликай на кнопку <span class="whitespace-nowrap">(ﾉ◕ヮ◕)ﾉ</span></p>
                  <div class="card-actions justify-end mt-2">
                    <button class="btn btn-ghost text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500">Telegram</button>
                  </div>
                </div>
              </div>
              <div class="card-side bg-white rounded-box shadow-md shadow-red-300/60">
                <figure>
                  <img
                      src="public/mascot/DiscordMaskot.png"
                      alt="Movie" />
                </figure>
                <div class="card-body">
                  <h2 class="card-title font-bold">Профиль Discord Gregulas</h2>
                  <p >Мой профиль в Discord! Кликай на кнопку <span class="whitespace-nowrap">(✧u✧)</span></p>
                  <div class="card-actions justify-end mt-2">
                    <button class="btn btn-ghost text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500">Discord</button>
                  </div>
                </div>
              </div>
              <div class="card-side bg-white rounded-box shadow-md shadow-red-300/60">
                <figure>
                  <img
                      src="public/mascot/GithubMaskot.png"
                      alt="Movie" />
                </figure>
                <div class="card-body">
                  <h2 class="card-title font-bold">Профиль GitHub Gregulas</h2>
                  <p>Мой профиль на GitHub! Кликай на кнопку <span class="whitespace-nowrap">( ✧≖ ͜ʖ≖)</span></p>
                  <div class="card-actions justify-end mt-2">
                    <NuxtLink href="https://github.com/GregulasM" external target="_blank" rel="noopener">
                       <button class="btn btn-ghost text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500">GitHub</button>
                    </NuxtLink>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>

      </div>

      <div class="mt-8"></div>
    </div>
  </div>
</template>

<script setup>

// Буду заменять на fetch с API
const team = [
  {
    name: 'Михаил Gregulas',
    role: 'Создатель всего',
    avatar: 'https://img.daisyui.com/images/profile/demo/1@94.webp',
  },
  {
    name: 'Валера',
    role: 'Тот, кто замещает профили',
    avatar: 'https://img.daisyui.com/images/profile/demo/2@94.webp',
  },
  {
    name: 'Вадим',
    role: 'Дарова',
    avatar: 'https://img.daisyui.com/images/profile/demo/3@94.webp',
  },
  {
    name: 'Годжо Сатору',
    role: 'Тот самый',
    avatar: 'https://img.daisyui.com/images/profile/demo/4@94.webp',
  },
  {
    name: 'Цветок из PVZ',
    role: 'Завод солнца',
    avatar: 'https://img.daisyui.com/images/profile/demo/5@94.webp',
  },
  {
    name: 'Морозострел',
    role: 'Пф-пук',
    avatar: 'https://img.daisyui.com/images/profile/demo/5@94.webp',
  },
]

const values = [
  {
    title: 'Доступность',
    text: 'Репозиторий на Github бесплатен и готов к вашим модификациям!',
  },
  {
    title: 'Комфорт',
    text: 'Компоненты и внешний вид делались с учетом красоты, удобства и уюта.',
  },
  {
    title: 'Развитие',
    text: 'Платформа постоянно развивается, изменяется и улучшается!',
  },
]
</script>