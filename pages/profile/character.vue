<template>
  <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">

    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center mb-4">
      <h2>Персонаж</h2>
    </div>
    <div class="p-2 font-semibold text-shadow-lg p-4 mb-4 ">
      <div class="carousel w-full ">
        <div :id="item.href_id" class="carousel-item relative w-full flex-col  p-8" v-for="item in characters_list" :key="item.value" :value="item.value">
          <img
              :src=item.photo
              class="w-50 h-50" />
          <p>{{item.title}}</p>
          <p>{{item.text}}</p>
          <div class="absolute left-5 right-20 top-1/2 flex -translate-y-1/2 transform justify-between">
            <a :href="'#' + (parseInt(item.href_id)-1)" class="btn btn-circle">❮</a>
            <a :href="'#' + (parseInt(item.href_id)+1)" class="btn btn-circle">❯</a>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>
<script setup lang="ts">
const characters_list= [
  { value: 'pers-1', href_id:"1", photo: "https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13", title: 'Имя персонажа 1', text: 'персонаж 1 \n ого текст' },
  { value: 'pers-2',href_id:"2", photo: "https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13", title: 'Имя персонажа 2', text: 'персонаж 2 \n ого текст' },
  { value: 'pers-3',href_id:"3", photo: "https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13", title: 'Имя персонажа 3', text: 'персонаж 3 \n ого текст' },
  { value: 'pers-4',href_id:"4", photo: "https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13", title: 'Имя персонажа 4', text: 'персонаж 4 \n ого текст' },
  { value: 'pers-5',href_id:"5", photo: "https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13", title: 'Имя персонажа 5', text: 'персонаж 5 \n ого текст' },
  { value: 'pers-6',href_id:"6", photo: "https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13", title: 'Имя персонажа 6', text: 'персонаж 6 \n ого текст' },
  { value: 'pers-7',href_id:"7", photo: "https://avatars.mds.yandex.net/i?id=daffd78d9e0355271866a9b11579ae4d_l-5233530-images-thumbs&n=13", title: 'Имя персонажа 7', text: 'персонаж 7 \n ого текст' },
]
</script>