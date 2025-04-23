<template>
  <div class="mt-52 text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
    <h2>Главная</h2>
  </div>
  <div class="flex p-4 justify-between ">
    <!--          Аватарка-->
    <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 size-max p-2 ">
      <div class="avatar w-24">
        <div class="w-full h-full object-cover rounded-xl inset-shadow-sm shadow-md inset-shadow-red-300/60">
          <img src="https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13" />
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
              <NuxtLink to="/da" >
                <MenubarItem class="cursor-pointer">Мои курсы
                  <MenubarShortcut>💼</MenubarShortcut>
                </MenubarItem>
              </NuxtLink>
              <MenubarSeparator />
              <MenubarItem class="cursor-pointer ">Все курсы <MenubarShortcut>🦉</MenubarShortcut></MenubarItem>
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
      <p>Рейтинг: 4.5 <span class="stars">★★★★☆</span></p>
      <p>Текущее прохождение: 60%</p>
      <div class="flex justify-between mt-2 ">
        <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50">Профиль</Button>
        <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500">Выход</Button>
      </div>
    </div>

  </div>


</template>

<script>

</script>