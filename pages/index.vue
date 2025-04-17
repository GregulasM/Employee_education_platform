<template>
  <div class="flex bg-radial-[at_25%_25%] bg-gradient-to-r from-red-50 via-orange-100 to-red-300 ">

    <!--  Левая секция-->
    <div class="w-1/4 mb-8">
      <div class=" drop-shadow-2xl">
        <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
            <h2>Добро пожаловать, {имя_пользователь}!</h2>
          </div>
          <div class="flex-col p-4 font-semibold text-shadow-lg">
            <p class="">Добро пожаловать!</p>
            <p class="">Желаем вам хорошего дня!</p>
            <div class="inset-shadow-sm rounded-lg mt-2 shadow-md inset-shadow-red-300/60">
              <Calendar />
            </div>

            <p id="date-time"></p>
          </div>

        </div>
        <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
          <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
            <h2>Текущий курс: {курс_название}</h2>
          </div>
          <div class="flex-col p-4 font-semibold text-shadow-lg">
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
          <div class="flex-col p-4 font-semibold text-shadow-lg">
            <Accordion type="single" collapsible>
              <AccordionItem v-for="item in accordionItems" :key="item.value" :value="item.value" >
                <AccordionTrigger class="font-semibold text-shadow-lg text-md">{{ item.title }} </AccordionTrigger>
                <AccordionContent class="ml-8">
                  {{ item.content }}
                </AccordionContent>
              </AccordionItem>
            </Accordion>
            <div class="inset-shadow-sm rounded-lg mt-4 shadow-md inset-shadow-red-300/60 ">
            </div>
          </div>
        </div>
      </div>
    </div>

    <!--  Профиль и контент -->
    <div class="w-1/2">
      <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">
        <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg">
          <h2>Профиль</h2>
        </div>
        <div class="flex p-4">
          <div class="mr-4 inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 h-1/5 w-1/5 p-2">
            <Avatar class="w-auto h-auto inset-shadow-sm shadow-md inset-shadow-red-300/60">
              <AvatarImage src="https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13" alt="Аватар" />
              <AvatarFallback>RU</AvatarFallback>
            </Avatar>
            <p class="text-center text-sm">Фото</p>
          </div>
          <div class="inset-shadow-sm rounded-lg shadow-md inset-shadow-red-300/60 p-4">
            <p>Рейтинг: 4.5 <span class="stars">★★★★☆</span></p>
            <p>Текущее прохождение: 60%</p>
            <button>Профиль</button>
            <button>Выход</button>
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
      <div class="right-section">
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

<!--<style src="assets/css/main.css"></style>-->

<script setup lang="ts">
  import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion'

  const defaultValue = 'item-1'

  const accordionItems = [
  { value: 'item-1', title: 'Новость 1: Новый курс по JS', content: 'Добавили раздел по JavaScript. Подробнее в разделе <NuxtLink to="/" class="text-orange-500 underline italic font-">обновление 09.05</NuxtLink>' },
  { value: 'item-2', title: 'Новость 2: Обновление рейтинга', content: 'Добавили раздел по JavaScript. <br> Подробнее в разделе <NuxtLink to="/" class="text-orange-500 underline italic font-">обновление 09.05</NuxtLink>' },
  { value: 'item-3', title: 'Новость 3: Вебинар в пятницу', content: 'Yes! You can use the transition prop to configure the animation.' },
  ]
</script>