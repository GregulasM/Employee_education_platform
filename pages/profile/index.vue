<template>
  <div>
    <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-auto z-20">
      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
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
        <div class="w-full h-full flex flex-col ">
          <div class="ml-4 mr-4 inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 ">
            <p class=" mr-4 ml-4 text-black ">Текст</p>
            <p class=" mr-4 ml-4 text-black ">Текст</p>
            <p class=" mr-4 ml-4 text-black ">Текст</p>
            <p class=" mr-4 ml-4 text-black ">Текст</p>
            <p class=" mr-4 ml-4 text-black ">Текст</p>
            <!--              <div class="dropdown dropdown-hover">-->
            <!--                <div tabindex="0" role="button" class="btn m-1">Click</div>-->
            <!--                <ul tabindex="0" class="dropdown-content menu bg-base-100 rounded-box z-1 w-52 p-2 shadow-sm">-->
            <!--                  <li><a>Item 1</a></li>-->
            <!--                  <li><a>Item 2</a></li>-->
            <!--                </ul>-->
            <!--              </div>-->

          </div>


        </div>
        <!--          Рейтинг и кнопки-->
        <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4 max-w-min">
          <p>Рейтинг: 4.5 <span class="stars">★★★★☆</span></p>
          <p>Текущее прохождение: 60%</p>
          <div class="flex justify-between mt-2 ">
            <NuxtLink to="/profile" draggable="false">
              <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500/50">Профиль</Button>
            </NuxtLink>

            <Button class="m-2 text-sm text-white font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 bg-red-500">Выход</Button>
          </div>
        </div>

      </div>



    </div>
    <div class="mt-8"/>

    <div class="mt-8 ml-8 mr-8 text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center mb-4">
      <h2>Последние достижения</h2>
    </div>
    <div class="p-2 font-semibold text-shadow-lg p-4 mb-4 ">

    </div>
    <div class="mt-8 ml-8 mr-8 text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center mb-4">
      <h2>Ваш персонаж</h2>
    </div>
    <div class="p-2 font-semibold text-shadow-lg p-4 mb-4 ">

    </div>
  </div>
</template>
