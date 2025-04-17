<template>
  <div class="flex bg-radial-[at_25%_25%] bg-gradient-to-r from-red-50 via-orange-100 to-red-300">

    <!--  Левая секция-->
    <div class="w-1/3">
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
        <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 ">
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

        <div class="block course-block draggable" id="course-block">
          <h2>Текущий курс</h2>
          <p>Название курса: Введение в веб-разработку</p>
          <p>Степень прохождения: 60%</p>
          <progress value="60" max="100"></progress>
        </div>
        <div class="block news-block draggable" id="news-block">
          <h2>Новости</h2>
          <ul>
            <li>Новость 1: Новый курс по JS</li>
            <li>Новость 2: Обновление рейтинга</li>
            <li>Новость 3: Вебинар в пятницу</li>
          </ul>
        </div>
      </div>
    </div>

    <!--  Профиль и контент -->
    <div class="w-1/2">
      <div class="center-section">
        <div class="profile-rectangle draggable" id="profile-block">
          <img src="https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13" alt="Аватар" class="avatar">
          <div class="profile-info">
            <p>Рейтинг: 4.5 <span class="stars">★★★★☆</span></p>
            <p>Текущее прохождение: 60%</p>
            <button>Профиль</button>
            <button>Выход</button>
          </div>
        </div>
        <div class="popup-area" id="popup-area"></div>
      </div>
    </div>

    <!--  Правая секция-->
    <div class="w-1/3">
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
<script>
import Progress from "../components/ui/progress/Progress.vue";
export default {
  components: {Progress}
}
</script>