<template>
      <div class="bg-radial-[at_25%_25%] bg-gradient-to-r from-red-50 via-orange-100 to-red-300 inset-0 text-black"> <!-- Убрать, если нужен скролл!-->
        <div class="flex" v-if="userStore.isAuthenticated && ready">
          <!--  Левая секция-->
          <div class="w-1/4 ">
            <div class=" drop-shadow-2xl sticky top-0">
              <ScrollArea class="h-screen rounded-md p-2 ">
                <draggable v-model="blocks" item-key="id" tag="div" handle=".drag-handle">
                  <template #item="{ element }">
                    <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
                      <div class="relative">
                        <div v-show="element.type === 'block11'" class="block">
                          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
                            <h2>Добро пожаловать, {{ userStore.currentUser?.firstName || 'Пользователь' }}!</h2>
                            <button class="justify-between drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
                              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
                            </button>
                          </div>
                          <div class="flex-col p-4 font-semibold text-shadow-lg text-black">
                            <p class="">Добро пожаловать!</p>
                            <p class="">Желаем вам хорошего дня!</p>
                            <div class="inset-shadow-sm rounded-lg mt-2 shadow-md inset-shadow-red-300/60 w-full overflow-hidden origin-top-left sm:scale-75 md:scale-90 lg:scale-100">
                              <ClientOnly>
                                <calendar-date class="cally shadow-lg rounded-box w-full max-w-none ">

                                  <svg aria-label="Previous" class="fill-current size-4" slot="previous" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="black" d="M15.75 19.5 8.25 12l7.5-7.5"></path></svg>
                                  <svg aria-label="Next" class="fill-current size-4" slot="next" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="black" d="m8.25 4.5 7.5 7.5-7.5 7.5"></path></svg>
                                  <NuxtLink to="/schedule" draggable="false">
                                    <calendar-month class="font-bold"></calendar-month>
                                  </NuxtLink>

                                </calendar-date>
                              </ClientOnly>

                              <!--                        <Calendar class="w-full max-w-none"/>-->
                            </div>

                            <p id="date-time"></p>
                          </div>
                        </div>
                        <div v-show="element.type === 'block12'" class="block">
                          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
                            <h2>Текущий курс: {{ activeCourse?.title || 'Не выбран' }}</h2>
                            <button class="justify-between drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
                              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
                            </button>
                          </div>
                          <div class="flex-col p-4 font-semibold text-shadow-lg text-black">
                            <p class="mb-2">
                              Пройдено модулей: {{ completedModules }} / {{ totalModules }}
                            </p>
                            <p class="">
                              Процент прохождения: {{ progressPercent }}%
                            </p>
                            <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">
                              <Progress :model-value="progressPercent" />
                            </div>
                          </div>
                        </div>
                        <div v-show="element.type === 'block13'" class="block">
                          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
                            <h2>Новости</h2>
                            <button class="justify-between drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
                              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
                            </button>
                          </div>
                          <ul class="flex-col p-2 font-semibold text-shadow-lg list rounded-box shadow-md text-black">
                            <li class="pb-2 text-xs opacity-60 tracking-wide">Что произошло на этой неделе:</li>

                            <li v-if="newsStore.loading" class="py-4 text-center text-red-400">Загрузка...</li>
                            <li v-else-if="newsStore.error" class="py-4 text-center text-red-500">{{ newsStore.error }}</li>
                            <li v-else-if="!lastThreeNews.length" class="py-4 text-center text-gray-500">Пока нет новостей</li>

                            <li
                                v-for="item in lastThreeNews"
                                :key="item.slug || item.id"
                                class="list-row shadow-md shadow-orange-200"
                            >
                              <div>
                                <img
                                    class="size-10 rounded-box"
                                    :src="item.author?.avatar || 'https://img.daisyui.com/images/profile/demo/1@94.webp'"
                                    alt="Автор"
                                />
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
                                  <button
                                      @click="goToNews(item.slug)"
                                      class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none"
                                  >
                                    <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                                      <path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/>
                                    </svg>
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
                    </div>
                  </template>
                </draggable>
                <div class="mt-8"/>
                <!--          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">-->
                <!--            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">-->
                <!--              <h2>Добро пожаловать, {имя_пользователь}!</h2>-->
                <!--            </div>-->
                <!--            <div class="flex-col p-4 font-semibold text-shadow-lg text-black">-->
                <!--              <p class="">Добро пожаловать!</p>-->
                <!--              <p class="">Желаем вам хорошего дня!</p>-->
                <!--              <div class="inset-shadow-sm rounded-lg mt-2 shadow-md inset-shadow-red-300/60 w-full overflow-hidden origin-top-left sm:scale-75 md:scale-90 lg:scale-100">-->
                <!--                <Calendar class="w-full max-w-none"/>-->
                <!--              </div>-->

                <!--              <p id="date-time"></p>-->
                <!--            </div>-->

                <!--          </div>-->
                <!--          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">-->
                <!--            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">-->
                <!--              <h2>Текущий курс: {курс_название}</h2>-->
                <!--            </div>-->
                <!--            <div class="flex-col p-4 font-semibold text-shadow-lg text-black">-->
                <!--              <p class="mb-2">Пройдено модулей: {модуль_количество}</p>-->
                <!--              <p class="">Процент прохождения: {прохождение_процент}</p>-->
                <!--              <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">-->
                <!--                <Progress :model-value="60" />-->
                <!--              </div>-->
                <!--            </div>-->

                <!--          </div>-->
                <!--          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">-->
                <!--            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">-->
                <!--              <h2>Новости</h2>-->
                <!--            </div>-->
                <!--            <ul class="flex-col p-2 font-semibold text-shadow-lg list rounded-box shadow-md text-black">-->

                <!--              <li class="pb-2 text-xs opacity-60 tracking-wide">Что произошло на этой неделе:</li>-->

                <!--              <li class="list-row shadow-md shadow-orange-200">-->
                <!--                <div><img class="size-10 rounded-box" src="https://img.daisyui.com/images/profile/demo/1@94.webp"/></div>-->
                <!--                <div>-->
                <!--                  <div>Новый курс по JS</div>-->
                <!--                  <div class="text-xs uppercase font-semibold opacity-60">Обновление</div>-->
                <!--                </div>-->
                <!--                <p class="list-col-wrap text-xs">-->
                <!--                  Мы добавили полный курс по JavaScript! Подробности в полной новости :)-->
                <!--                </p>-->
                <!--                <HoverCard>-->
                <!--                  <HoverCardTrigger>-->
                <!--                    <button class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">-->
                <!--                    <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/></svg>-->
                <!--                    </button>-->
                <!--                  </HoverCardTrigger>-->
                <!--                  <HoverCardContent class="text-sm opacity-90">-->
                <!--                    Откроется страница с новостью-->
                <!--                  </HoverCardContent>-->
                <!--                </HoverCard>-->
                <!--              </li>-->

                <!--              <li class="list-row shadow-md shadow-orange-200">-->
                <!--                <div><img class="size-10 rounded-box" src="https://img.daisyui.com/images/profile/demo/4@94.webp"/></div>-->
                <!--                <div>-->
                <!--                  <div>Обновление страницы</div>-->
                <!--                  <div class="text-xs uppercase font-semibold opacity-60">Обновление</div>-->
                <!--                </div>-->
                <!--                <p class="list-col-wrap text-xs">-->
                <!--                 В текущей версии сайта изменили главный экран и добавили скроллы! Подробности на странице обновления-->
                <!--                </p>-->
                <!--                <HoverCard>-->
                <!--                  <HoverCardTrigger>-->
                <!--                    <button class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">-->
                <!--                      <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/></svg>-->
                <!--                    </button>-->
                <!--                  </HoverCardTrigger>-->
                <!--                  <HoverCardContent class="text-sm opacity-90">-->
                <!--                    Откроется страница с новостью-->
                <!--                  </HoverCardContent>-->
                <!--                </HoverCard>-->
                <!--              </li>-->

                <!--              <li class="list-row shadow-md shadow-orange-200">-->
                <!--                <div><img class="size-10 rounded-box" src="https://img.daisyui.com/images/profile/demo/3@94.webp"/></div>-->
                <!--                <div>-->
                <!--                  <div>Размеры и компоненты</div>-->
                <!--                  <div class="text-xs uppercase font-semibold opacity-60">Изменение</div>-->
                <!--                </div>-->
                <!--                <p class="list-col-wrap text-xs">-->
                <!--                  Размерность аватарок и некоторых компонентов поменяли: смотрите в полной!-->
                <!--                </p>-->
                <!--                <HoverCard>-->
                <!--                  <HoverCardTrigger>-->
                <!--                    <button class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">-->
                <!--                      <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/></svg>-->
                <!--                    </button>-->
                <!--                  </HoverCardTrigger>-->
                <!--                  <HoverCardContent class="text-sm opacity-90">-->
                <!--                    Откроется страница с новостью-->
                <!--                  </HoverCardContent>-->
                <!--                </HoverCard>-->
                <!--              </li>-->
                <!--            </ul>-->
                <!--          </div>-->
                <!--          <div class="mt-8"/>-->
              </ScrollArea>
            </div>
          </div>

          <!--  Профиль и контент -->
          <div class="w-1/2">
            <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-auto z-20">
              <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
                <h2>Главная</h2>
              </div>
              <div class="flex p-4 justify-between ">
                <!--          Аватарка-->
                <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 size-max p-2 ">
                  <div class="avatar w-24">
                    <div class="w-full h-full object-cover rounded-xl inset-shadow-sm shadow-md inset-shadow-red-300/60">
                      <img :src="userStore.currentUser?.avatar ? userStore.currentUser?.avatar : '/mascot/mascot.png'"/>
                    </div>
                  </div>

                  <p class="mt-2 text-center text-[12px]">Фото</p>
                </div>
                <!--          Кнопки перемещения-->
                <div class="w-full h-full flex flex-col justify-between">
                  <div class="self-center w-full ">
                    <Menubar class=" mr-4 ml-4 text-white text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50 justify-center">
                      <MenubarMenu>
                        <MenubarTrigger class="cursor-pointer "> Курсы</MenubarTrigger>
                        <MenubarContent class="border-red-500/50 ">
                          <NuxtLink to="/my_courses" draggable="false">
                            <MenubarItem class="cursor-pointer">Мои курсы
                              <MenubarShortcut>💼</MenubarShortcut>
                            </MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/courses" draggable="false">
                             <MenubarItem class="cursor-pointer ">Все курсы <MenubarShortcut>🦉</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/useful_links" draggable="false">
                             <MenubarItem class="cursor-pointer">Полезные ссылки <MenubarShortcut>🖥</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/tests" draggable="false">
                             <MenubarItem class="cursor-pointer">Тестирование <MenubarShortcut>📑</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                        </MenubarContent>
                      </MenubarMenu>
                      <MenubarMenu>
                        <MenubarTrigger class="cursor-pointer">Главная</MenubarTrigger>
                        <MenubarContent class="border-red-500/50">
                          <NuxtLink to="/profile" draggable="false">
                             <MenubarItem class="cursor-pointer">Профиль <MenubarShortcut>👤</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/profile/user_settings" draggable="false">
                             <MenubarItem class="cursor-pointer">Настройки <MenubarShortcut>⚙️</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/profile/achievements" draggable="false">
                            <MenubarItem class="cursor-pointer">Достижения <MenubarShortcut>🏆</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/profile/character" draggable="false">
                             <MenubarItem class="cursor-pointer">Персонаж <MenubarShortcut>🎩</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                        </MenubarContent>
                      </MenubarMenu>
                      <MenubarMenu>
                        <MenubarTrigger class="cursor-pointer">Информация</MenubarTrigger>
                        <MenubarContent class="border-red-500/50">
                          <NuxtLink to="/news" draggable="false">
                            <MenubarItem class="cursor-pointer">Новости <MenubarShortcut>📢</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/schedule" draggable="false">
                            <MenubarItem class="cursor-pointer">Расписание <MenubarShortcut>🗓</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                          <MenubarSeparator />
                          <NuxtLink to="/info" draggable="false">
                            <MenubarItem class="cursor-pointer">О сайте <MenubarShortcut>❔</MenubarShortcut></MenubarItem>
                          </NuxtLink>
                        </MenubarContent>
                      </MenubarMenu>
                    </Menubar>
                  </div>
                  <div class="mt-8 ml-4 mr-4 inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 ">
                    <Alert class="bg-red-300/60">
                      <Rocket class="h-4 w-4" />
                      <AlertTitle class="text-sm text-black font-semibold">Факт дня:</AlertTitle>
                      <AlertDescription class="text-sm text-black font-normal">
                        Вы лучший.
                      </AlertDescription>
                    </Alert>
                    <!--              <div class="dropdown dropdown-hover">-->
                    <!--                <div tabindex="0" role="button" class="btn m-1">Click</div>-->
                    <!--                <ul tabindex="0" class="dropdown-content menu bg-base-100 rounded-box z-1 w-52 p-2 shadow-sm">-->
                    <!--                  <li><a>Item 1</a></li>-->
                    <!--                  <li><a>Item 2</a></li>-->
                    <!--                </ul>-->
                    <!--              </div>-->

                  </div>
                  <AlertDialog>
                    <AlertDialogTrigger class="mt-4 p-1 text-center self-center w-1/4 rounded-lg text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50 ">
                      Помощь!
                    </AlertDialogTrigger>
                    <AlertDialogContent>
                      <AlertDialogHeader>
                        <AlertDialogTitle>Смотри!</AlertDialogTitle>
                        <AlertDialogDescription>
                          Сейчас вы начнете процесс обучения.
                          Здесь перечислены базовые функции сайта и что на нем можно делать!
                        </AlertDialogDescription>
                      </AlertDialogHeader>
                      <AlertDialogFooter class="mt-4 p-1">
                        <AlertDialogAction class="text-center self-center w-1/3 rounded-lg text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50">Дальше</AlertDialogAction>
                        <AlertDialogCancel class="text-center self-center w-1/3 rounded-lg text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50">Отмена</AlertDialogCancel>
                      </AlertDialogFooter>
                    </AlertDialogContent>
                  </AlertDialog>


                </div>
                <!--          Рейтинг и кнопки-->
                <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4 max-w-min">
                  <p>Рейтинг: {{ userStore.currentUser?.rating ?? '—' }} <span class="stars">★★★★☆</span></p>
                  <p>Текущее прохождение: 60%</p>
                  <div class="flex justify-between mt-2 ">
                    <NuxtLink to="/profile" draggable="false">
                      <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50">Профиль</Button>
                    </NuxtLink>

                    <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500" v-on:click="logout">Выход</Button>
                  </div>
                </div>

              </div>


            </div>
            <slot/>
          </div>

          <!--  Правая секция-->
          <div class="w-1/4 ">
            <div class="drop-shadow-2xl sticky top-0">
              <ScrollArea class="h-screen rounded-md p-2">
                <draggable v-model="blocks" item-key="id" tag="div" handle=".drag-handle">
                  <template #item="{ element }">
                    <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
                      <div class="relative">
                        <div v-show="element.type === 'block1'" class="block">
                          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
                            <h2>Мои курсы</h2>
                            <button class="justify-between drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
                              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
                            </button>
                          </div>
                          <div class="flex-col p-4 font-semibold text-shadow-lg">
                            <div class="m-2">

                              <li>{{ activeCourse?.title || 'Не выбран' }}</li>
                            </div>
                          </div>
                        </div>
                        <div v-show="element.type === 'block2'" class="block">
                          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
                            <h2>Полезные ссылки</h2>
                            <button class="justify-between drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
                              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
                            </button>
                          </div>
                          <div class="flex-col p-2 font-semibold text-shadow-lg">
                            <ScrollArea class="h-72 w-full rounded-md">
                              <div class="flex-col mr-4 font-semibold text-shadow-lg whitespace-pre-line">
                                <div v-if="usefulLinksStore.loading" class="text-center text-red-400 text-sm">Загрузка...</div>
                                <div v-else-if="!courseLinks.length" class="text-center text-xs opacity-60 mt-4">Нет полезных ссылок для этого курса</div>
                                <div v-else>
                                  <div v-for="item in courseLinks" :key="item.value" class="collapse collapse-arrow border-b border-red-500/50 cursor-pointer bg-white rounded-lg mb-2">
                                    <input type="checkbox" :name="'accordion-'+item.value" />
                                    <div class="collapse-title font-semibold flex justify-between items-center">
                                      <span>{{ item.title }}</span>
                                    </div>
                                    <div class="collapse-content text-sm space-y-3">
                                      <div v-for="link in item.links" :key="link.id" class="flex flex-col gap-1 mb-2">
                                        <a :href="link.url" target="_blank" class="text-black hover:underline font-semibold flex items-center gap-2">
                                          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2.5" d="M11.1 3.002c-3.648.007-5.56.096-6.78 1.317C3.002 5.637 3.002 7.758 3.002 12s0 6.363 1.318 7.681s3.438 1.318 7.68 1.318s6.363 0 7.681-1.318c1.221-1.22 1.31-3.132 1.317-6.78m-.518-9.384l-5.548 5.534m5.549-5.534c-.494-.494-3.822-.448-4.525-.438m4.525.438c.494.495.448 3.826.438 4.53" color="currentColor"/></svg>
                                          {{ link.title }}
                                        </a>
                                        <span v-if="link.description" class="text-xs opacity-80 ml-6">{{ link.description }}</span>
                                        <div v-if="link.tags && link.tags.length" class="ml-6 flex flex-wrap gap-1 mt-1">
                                          <span v-for="tag in link.tags" :key="tag" class="px-2 py-0.5 rounded bg-orange-200 text-500/70 text-xs">{{ tag }}</span>
                                        </div>
                                      </div>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </ScrollArea>
                          </div>
                        </div>
                        <div v-show="element.type === 'block3'" class="block">
                          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
                            <h2>Мой персонаж</h2>
                            <button class="justify-between drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
                              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
                            </button>
                          </div>
                          <div class="flex-col p-4 font-semibold text-shadow-lg">
                            <div v-if="charactersStore.loading" class="py-4 text-center text-red-400">Загрузка персонажа...</div>
                            <div v-else-if="charactersStore.error" class="py-4 text-center text-red-500">{{ charactersStore.error }}</div>
                            <div v-else>
                              <div v-if="selectedCharacter">
                                <img
                                    class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 size-max p-2 mx-auto max-h-60"
                                    :src="selectedCharacter.avatar || '/mascot/mascot.png'"
                                    alt="Персонаж"
                                >
                                <p class="mb-1 text-center text-lg">{{ selectedCharacter.name }}</p>
                                <p v-if="selectedCharacter.description" class="mb-2 text-xs text-center opacity-80">{{ selectedCharacter.description }}</p>
                                <p>Уровень: {{ selectedCharacter.level ?? '—' }}</p>
                                <p>Опыт: {{ selectedCharacter.exp ?? '—' }}<span v-if="selectedCharacter.totalExp">/{{ selectedCharacter.totalExp }}</span></p>
                                <p>Редкость: <span class="uppercase font-bold">{{ selectedCharacter.rarity || '—' }}</span></p>
                                <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60"></div>
                              </div>
                              <div v-else>
                                <div class="text-center text-gray-400 text-sm">Персонаж не выбран</div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </template>
                </draggable>
                <div class="mt-8"/>
                <!--          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">-->
                <!--            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">-->
                <!--              <h2>Мои курсы</h2>-->
                <!--            </div>-->
                <!--            <div class="flex-col p-4 font-semibold text-shadow-lg">-->
                <!--              <div class="m-2">-->
                <!--                <li>Введение в веб-разработку</li>-->
                <!--                <li>Основы JavaScript</li>-->
                <!--                <li>Tailwind CSS</li>-->
                <!--              </div>-->
                <!--            </div>-->

                <!--          </div>-->
                <!--          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">-->
                <!--            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">-->
                <!--              <h2>Полезные ссылки</h2>-->
                <!--            </div>-->
                <!--            <div class="flex-col p-4 font-semibold text-shadow-lg">-->
                <!--              <ScrollArea class="h-72 w-full rounded-md">-->
                <!--                <div class="flex-col mr-4 font-semibold text-shadow-lg whitespace-pre-line">-->
                <!--                  <Accordion type="multiple" collapsible>-->
                <!--                    <AccordionItem v-for="item in accordionItemsLinks" :key="item.value" :value="item.value" class="border-red-500/50">-->
                <!--                      <AccordionTrigger class="font-semibold text-shadow-lg text-md ">{{ item.title }} </AccordionTrigger>-->
                <!--                      <AccordionContent class="ml-8 ">-->
                <!--                        {{ item.content }}-->
                <!--                      </AccordionContent>-->
                <!--                    </AccordionItem>-->
                <!--                  </Accordion>-->
                <!--                  <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">-->
                <!--                  </div>-->
                <!--                </div>-->


                <!--              </ScrollArea>-->
                <!--            </div>-->

                <!--          </div>-->
                <!--          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">-->
                <!--            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">-->
                <!--              <h2>Мой персонаж</h2>-->
                <!--            </div>-->
                <!--            <div class="flex-col p-4 font-semibold text-shadow-lg">-->
                <!--              <img class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 size-max p-2 " src="https://steamuserimages-a.akamaihd.net/ugc/1691653893917179883/91673BB8FC3051DCF7F8AEC808FA18D40D40FB4A/?imw=512&imh=590&ima=fit&impolicy=Letterbox&imcolor=%23000000&letterbox=true" alt="Персонаж">-->
                <!--              <p>Уровень: 5</p>-->
                <!--              <p>Опыт: 250/500</p>-->
                <!--              <p>Пройдено курсов: 2</p>-->
                <!--              <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">-->
                <!--              </div>-->
                <!--            </div>-->
                <!--          </div>-->
                <!--          <div class="mt-8"/>-->
              </ScrollArea>
  <!--            <div class="block settings-block draggable" id="settings-block">-->
  <!--              <h2>Настройки сайта</h2>-->
  <!--              <ul>-->
  <!--                <button>Темы</button>-->
  <!--                <button>Модули</button>-->
  <!--                <button>Персонализация</button>-->
  <!--              </ul>-->
  <!--            </div>-->
            </div>
          </div>

        </div>
        <div v-else-if="!ready" class="flex justify-center w-full h-screen">
          <span class="loading loading-spinner bg-red-500/50"></span>
        </div>
      </div>

</template>

<script setup lang="ts">
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion'
import { Rocket } from 'lucide-vue-next'


import {
  Menubar,
  MenubarContent,
  MenubarItem,
  MenubarMenu,
  MenubarSeparator,
  MenubarShortcut,
  MenubarTrigger,
} from '@/components/ui/menubar'

import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from '@/components/ui/breadcrumb'

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog'

import {
  HoverCard,
  HoverCardContent,
  HoverCardTrigger,
} from '@/components/ui/hover-card'


const defaultValue = 'item-1'

const accordionItems = [
  { value: 'item-1', title: 'Новость 1: Новый курс по JS', content: 'Добавили раздел по JavaScript. Подробнее в разделе <NuxtLink to="/" class="text-orange-500 underline italic font-">обновление 09.05</NuxtLink>' },
  { value: 'item-2', title: 'Новость 2: Обновление рейтинга', content: 'Добавили раздел по JavaScript. <br> Подробнее в разделе <NuxtLink to="/" class="text-orange-500 underline italic font-">обновление 09.05</NuxtLink>' },
  { value: 'item-3', title: 'Новость 3: Вебинар в пятницу', content: 'Yes! You can use the transition prop to configure the animation.' },
]

const accordionItemsLinks= [
  { value: 'itemL-1', title: 'База знаний JS', content: 'Ссылка 1 \n Ссылка 2\nСсылка 3 \nСсылка 4 \nСсылка 5 \nСсылка 6' },
  { value: 'itemL-2', title: 'База знаний ASP .NET Core', content: 'Ссылка 1 \n Ссылка 2\nСсылка 3 \nСсылка 4 \nСсылка 5 \nСсылка 6'  },
  { value: 'itemL-3', title: 'Коллекция курсов по C#', content: 'Ссылка 1 \n Ссылка 2\nСсылка 3 \nСсылка 4 \nСсылка 5 \nСсылка 6'  },
]

import { ref } from 'vue'
import draggable from 'vuedraggable'


const defaultBlocks = ref([
  { id: 1, type: 'block1' },
  { id: 2, type: 'block2' },
  { id: 3, type: 'block3' },
  { id: 11, type: 'block11' },
  { id: 12, type: 'block12' },
  { id: 13, type: 'block13' },
])

const blocks = ref(defaultBlocks)

import { useUsersStore } from '~/stores/users_store'
import { useCoursesStore } from '~/stores/courses_store'
import { useUsefulLinksStore } from '@/stores/useful_links_store'
import { useNewsStore } from '~/stores/news_store'
import { useRouter } from 'vue-router'
import { useCharactersStore } from '~/stores/characters_store'

const charactersStore = useCharactersStore()
const userStore = useUsersStore()
const coursesStore = useCoursesStore()
const usefulLinksStore = useUsefulLinksStore()
const router = useRouter()
const ready = ref(false)
const newsStore = useNewsStore()

const selectedCharacter = computed(() => {
  const id = userStore.currentUser?.selectedCharacterId
  if (!id) return null
  return charactersStore.characters.find(c => c.id === id) || null
})

const activeCourse = computed(() => {
  const id = userStore.currentUser?.activeCourseId
  if (!id) return null
  return coursesStore.allCourses.find(c => Number(c.id) === Number(id)) || null
})

const activeCourseId = computed(() => userStore.currentUser?.activeCourseId)

const courseLinks = computed(() => {
  if (!activeCourseId.value) return []
  const parents = usefulLinksStore.links.filter(l => l.parentId === null && l.courseId === activeCourseId.value)
  return parents.map(parent => {
    const childLinks = usefulLinksStore.links.filter(l => l.parentId === parent.id && l.courseId === activeCourseId.value)
    return {
      value: `link-parent-${parent.id}`,
      title: parent.title,
      links: childLinks.length ? childLinks : []
    }
  }).filter(item => item.links.length > 0)
})

const totalModules = computed(() => activeCourse.value?.modules?.length || 0)
const completedModules = computed(() => {
  return 0
})
const progressPercent = computed(() => (
    totalModules.value === 0 ? 0 : Math.round((completedModules.value / totalModules.value) * 100)
))

function logout() {
  userStore.logout()
  router.push('/login')
}

const lastThreeNews = computed(() => {
  return [...newsStore.newsList]
      .filter(n => n.isActive !== false)
      .sort((a, b) => new Date(b.createdAt || b.date) - new Date(a.createdAt || a.date))
      .slice(0, 3)
})

function goToNews(slug) {
  router.push(`/news/${slug}`)
}
function tagClass(type) {
  switch ((type || '').toLowerCase()) {
    case 'обновление':
      return 'text-green-700 font-bold'
    case 'изменение':
      return 'text-blue-700 font-bold'
    case 'анонс':
      return 'text-purple-700 font-bold'
    case 'исправление':
      return 'text-orange-700 font-bold'
    default:
      return 'text-gray-600'
  }
}

onMounted(async () => {
  if (process.client) {
    const storedBlocks = localStorage.getItem('blocks')
    if (storedBlocks) {
      blocks.value = JSON.parse(storedBlocks)
    }
    watch(blocks, (newBlocks) => {
      localStorage.setItem('blocks', JSON.stringify(newBlocks))
    }, { deep: true })
  }
  await userStore.loadSession()
  if (!coursesStore.allCourses.length) await coursesStore.fetchAll()
  await usefulLinksStore.fetchLinks()
  if (newsStore.newsList.length === 0) await newsStore.fetchNews()
  await charactersStore.fetchCharacters()
  ready.value = true
})

watch(
    () => userStore.currentUser?.activeCourseId,
    async (newId, oldId) => {
      if (newId !== oldId && newId != null) {
        await coursesStore.fetchAll()
      }
    }
)

</script>