<template>
 <div class="flex bg-radial-[at_25%_25%] bg-gradient-to-r from-red-50 via-orange-100 to-red-300 fixed inset-0 text-black"> <!-- Убрать, если нужен скролл!-->

    <!--  Левая секция-->
    <div class="w-1/4 mb-8">
      <div class=" drop-shadow-2xl ">
        <ScrollArea class="h-screen rounded-md p-2 ">
          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
              <h2>Добро пожаловать, {имя_пользователь}!</h2>
            </div>
            <div class="flex-col p-4 font-semibold text-shadow-lg text-black">
              <p class="">Добро пожаловать!</p>
              <p class="">Желаем вам хорошего дня!</p>
              <div class="inset-shadow-sm rounded-lg mt-2 shadow-md inset-shadow-red-300/60 w-full overflow-hidden origin-top-left sm:scale-75 md:scale-90 lg:scale-100">
                <Calendar class="w-full max-w-none"/>
              </div>

              <p id="date-time"></p>
            </div>

          </div>
          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
              <h2>Текущий курс: {курс_название}</h2>
            </div>
            <div class="flex-col p-4 font-semibold text-shadow-lg text-black">
              <p class="mb-2">Пройдено модулей: {модуль_количество}</p>
              <p class="">Процент прохождения: {прохождение_процент}</p>
              <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">
                <Progress :model-value="60" />
              </div>
            </div>

          </div>
          <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
            <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
              <h2>Новости</h2>
            </div>

              <ul class="flex-col p-2 font-semibold text-shadow-lg list rounded-box shadow-md text-black">

                <li class="pb-2 text-xs opacity-60 tracking-wide">Что произошло на этой неделе:</li>

                <li class="list-row shadow-md shadow-orange-200">
                  <div><img class="size-10 rounded-box" src="https://img.daisyui.com/images/profile/demo/1@94.webp"/></div>
                  <div>
                    <div>Новый курс по JS</div>
                    <div class="text-xs uppercase font-semibold opacity-60">Обновление</div>
                  </div>
                  <p class="list-col-wrap text-xs">
                    Мы добавили полный курс по JavaScript! Подробности в полной новости :)
                  </p>
                  <HoverCard>
                    <HoverCardTrigger>
                      <button class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">
                      <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/></svg>
                      </button>
                    </HoverCardTrigger>
                    <HoverCardContent class="text-sm opacity-90">
                      Откроется страница с новостью
                    </HoverCardContent>
                  </HoverCard>
                </li>

                <li class="list-row shadow-md shadow-orange-200">
                  <div><img class="size-10 rounded-box" src="https://img.daisyui.com/images/profile/demo/4@94.webp"/></div>
                  <div>
                    <div>Обновление страницы</div>
                    <div class="text-xs uppercase font-semibold opacity-60">Обновление</div>
                  </div>
                  <p class="list-col-wrap text-xs">
                   В текущей версии сайта изменили главный экран и добавили скроллы! Подробности на странице обновления
                  </p>
                  <HoverCard>
                    <HoverCardTrigger>
                      <button class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">
                        <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/></svg>
                      </button>
                    </HoverCardTrigger>
                    <HoverCardContent class="text-sm opacity-90">
                      Откроется страница с новостью
                    </HoverCardContent>
                  </HoverCard>
                </li>

                <li class="list-row shadow-md shadow-orange-200">
                  <div><img class="size-10 rounded-box" src="https://img.daisyui.com/images/profile/demo/3@94.webp"/></div>
                  <div>
                    <div>Размеры и компоненты</div>
                    <div class="text-xs uppercase font-semibold opacity-60">Изменение</div>
                  </div>
                  <p class="list-col-wrap text-xs">
                    Размерность аватарок и некоторых компонентов поменяли: смотрите в полной!
                  </p>
                  <HoverCard>
                    <HoverCardTrigger>
                      <button class="btn btn-square btn-ghost hover:bg-red-500/50 hover:border-none border-none">
                        <svg class="size-[2em]" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path fill="#888888" d="M12 21.5c-1.35-.85-3.8-1.5-5.5-1.5c-1.65 0-3.35.3-4.75 1.05c-.1.05-.15.05-.25.05c-.25 0-.5-.25-.5-.5V6c.6-.45 1.25-.75 2-1c1.11-.35 2.33-.5 3.5-.5c1.95 0 4.05.4 5.5 1.5c1.45-1.1 3.55-1.5 5.5-1.5c1.17 0 2.39.15 3.5.5c.75.25 1.4.55 2 1v14.6c0 .25-.25.5-.5.5c-.1 0-.15 0-.25-.05c-1.4-.75-3.1-1.05-4.75-1.05c-1.7 0-4.15.65-5.5 1.5m-1-14c-1.36-.6-3.16-1-4.5-1c-1.2 0-2.4.15-3.5.5v11.5c1.1-.35 2.3-.5 3.5-.5c1.34 0 3.14.4 4.5 1zM13 19c1.36-.6 3.16-1 4.5-1c1.2 0 2.4.15 3.5.5V7c-1.1-.35-2.3-.5-3.5-.5c-1.34 0-3.14.4-4.5 1zm1-2.65c.96-.35 2.12-.52 3.5-.52c1.04 0 1.88.08 2.5.24v-1.5a13.9 13.9 0 0 0-6 .19zm0-2.66c.96-.35 2.12-.53 3.5-.53c1.04 0 1.88.08 2.5.24v-1.5c-.87-.16-1.71-.23-2.5-.23c-1.28 0-2.45.15-3.5.45zM14 11c.96-.33 2.12-.5 3.5-.5c.91 0 1.76.09 2.5.28V9.23c-.87-.15-1.71-.23-2.5-.23c-1.32 0-2.5.15-3.5.46z"/></svg>
                      </button>
                    </HoverCardTrigger>
                    <HoverCardContent class="text-sm opacity-90">
                      Откроется страница с новостью
                    </HoverCardContent>
                  </HoverCard>
                </li>
              </ul>
          </div>
          <div class="mt-8"/>
        </ScrollArea>
      </div>
    </div>

    <!--  Профиль и контент -->
    <div class="w-1/2">
      <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-auto">
        <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
          <h2>Главная</h2>
        </div>
        <div class="flex p-4 justify-between ">
<!--          Аватарка-->
          <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 size-max p-2 ">
            <Avatar class="w-full h-full object-cover rounded-full inset-shadow-sm shadow-md inset-shadow-red-300/60">
              <AvatarImage src="https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13" alt="Аватар" />
              <AvatarFallback>RU</AvatarFallback>
            </Avatar>
            <p class="mt-2 text-center text-[12px]">Фото</p>
          </div>
<!--          Кнопки перемещения-->
          <div class="w-full h-full flex flex-col justify-between">
            <div class="self-center">
              <Menubar class=" mr-4 ml-4 text-sm text-white text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50 ">
              <MenubarMenu>
                <MenubarTrigger class="cursor-pointer"> Курсы</MenubarTrigger>
                <MenubarContent class="border-red-500/50">
                  <NuxtLink to="/da" >
                    <MenubarItem class="cursor-pointer">Мои курсы
                      <MenubarShortcut>💼</MenubarShortcut>
                    </MenubarItem>
                  </NuxtLink>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">Все курсы <MenubarShortcut>🦉</MenubarShortcut></MenubarItem>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">Полезные ссылки <MenubarShortcut>🖥</MenubarShortcut></MenubarItem>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">Тестирование <MenubarShortcut>📑</MenubarShortcut></MenubarItem>
                </MenubarContent>
              </MenubarMenu>
              <MenubarMenu>
                <MenubarTrigger class="cursor-pointer">Главная</MenubarTrigger>
                <MenubarContent class="border-red-500/50">
                  <MenubarItem class="cursor-pointer">
                    Профиль <MenubarShortcut>👤</MenubarShortcut>
                  </MenubarItem>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">Настройки <MenubarShortcut>⚙️</MenubarShortcut></MenubarItem>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">Достижения <MenubarShortcut>🏆</MenubarShortcut></MenubarItem>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">Персонаж <MenubarShortcut>🎩</MenubarShortcut></MenubarItem>
                </MenubarContent>
              </MenubarMenu>
              <MenubarMenu>
                <MenubarTrigger class="cursor-pointer">Информация</MenubarTrigger>
                <MenubarContent class="border-red-500/50">
                  <MenubarItem class="cursor-pointer">Новости <MenubarShortcut>📢</MenubarShortcut></MenubarItem>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">Расписание <MenubarShortcut>🗓</MenubarShortcut></MenubarItem>
                  <MenubarSeparator />
                  <MenubarItem class="cursor-pointer">О сайте <MenubarShortcut>❔</MenubarShortcut></MenubarItem>
                </MenubarContent>
              </MenubarMenu>
            </Menubar>
            </div>
            <div class="mt-8 ml-2 mr-2 text-shadow-lg/20 inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 ">
              <Alert class="bg-red-300/60">
                <Rocket class="h-4 w-4" />
                <AlertTitle class="text-sm text-black font-medium">Факт дня:</AlertTitle>
                <AlertDescription class="text-sm text-black font-light">
                  Вы пидарас.
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
              <AlertDialogTrigger class="mt-4 p-1 text-center self-center w-1/2 rounded-lg text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50 ">
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
          <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4">
            <p>Рейтинг: 4.5 <span class="stars">★★★★☆</span></p>
            <p>Текущее прохождение: 60%</p>
            <div class="flex justify-between mt-2">
              <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50">Профиль</Button>
              <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500">Выход</Button>
            </div>
          </div>
        </div>


      </div>
      <div class="center-section">
        <div class="profile-rectangle draggable" id="profile-block">

          <div class="profile-info">

          </div>
        </div>
        <div class="popup-area" id="popup-area"></div>
      </div>
    </div>

    <!--  Правая секция-->
    <div class="w-1/4">
      <div class="drop-shadow-2xl">
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
                      <li>Введение в веб-разработку</li>
                      <li>Основы JavaScript</li>
                      <li>Tailwind CSS</li>
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
                  <div class="flex-col p-4 font-semibold text-shadow-lg">
                    <ScrollArea class="h-72 w-full rounded-md">
                      <div class="flex-col mr-4 font-semibold text-shadow-lg whitespace-pre-line">
                        <Accordion type="multiple" collapsible>
                          <AccordionItem v-for="item in accordionItemsLinks" :key="item.value" :value="item.value" class="border-red-500/50">
                            <AccordionTrigger class="font-semibold text-shadow-lg text-md ">{{ item.title }} </AccordionTrigger>
                            <AccordionContent class="ml-8 ">
                              {{ item.content }}
                            </AccordionContent>
                          </AccordionItem>
                        </Accordion>
                        <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">
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
                    <img class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 size-max p-2 " src="https://steamuserimages-a.akamaihd.net/ugc/1691653893917179883/91673BB8FC3051DCF7F8AEC808FA18D40D40FB4A/?imw=512&imh=590&ima=fit&impolicy=Letterbox&imcolor=%23000000&letterbox=true" alt="Персонаж">
                    <p>Уровень: 5</p>
                    <p>Опыт: 250/500</p>
                    <p>Пройдено курсов: 2</p>
                    <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">
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
        <div class="block courses-block draggable" id="courses-block">
          <h2>Мои курсы</h2>
          <ul>
            <li>Введение в веб-разработку</li>
            <li>Основы JavaScript</li>
            <li>Tailwind CSS</li>
          </ul>
        </div>
        <div class="block links-block draggable" id="links-block">
          <h2>Полезные ссылки</h2>
          <ul>
            <li><a href="#">Документация JS</a></li>
            <li><a href="#">Tailwind CSS</a></li>
            <li><a href="#">ASP.NET Core</a></li>
          </ul>
        </div>
        <div class="block character-block draggable" id="character-block">
          <h2>Мой персонаж</h2>
          <img src="https://steamuserimages-a.akamaihd.net/ugc/1691653893917179883/91673BB8FC3051DCF7F8AEC808FA18D40D40FB4A/?imw=512&imh=590&ima=fit&impolicy=Letterbox&imcolor=%23000000&letterbox=true" alt="Персонаж" class="character">
          <p>Уровень: 5</p>
          <p>Опыт: 250/500</p>
          <p>Пройдено курсов: 2</p>
        </div>
        <div class="block settings-block draggable" id="settings-block">
          <h2>Настройки сайта</h2>
          <ul>
            <button>Темы</button>
            <button>Модули</button>
            <button>Персонализация</button>
          </ul>
        </div>
      </div>
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
  ])

  const blocks = ref(defaultBlocks)

  onMounted(() => {
    if (process.client) {
      const storedBlocks = localStorage.getItem('blocks')
      if (storedBlocks) {
        blocks.value = JSON.parse(storedBlocks)
      }
      watch(blocks, (newBlocks) => {
        localStorage.setItem('blocks', JSON.stringify(newBlocks))
      }, { deep: true })
    }
  })

</script>