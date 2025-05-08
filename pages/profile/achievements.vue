<template>
  <div>
    <div class="shadow-md shadow-orange-200 mt-8 ml-8 mr-8 rounded-lg bg-orange-50 opacity-90 h-min">

      <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center mb-4">
        <h2>Достижения</h2>
      </div>
      <div class="p-2 font-semibold text-shadow-lg p-4 mb-4">
        <ScrollArea class="h-180 w-full rounded-md ">
          <div class="flex-col mr-4 font-semibold text-shadow-lg whitespace-pre-line grid grid-cols-3 gap-6 mb-2">
            <div v-for="item in achItemsLinks" :key="item.value" :value="item.value" class="shadow-md rounded-lg ml-4 mb-4">
              <div class="font-bold text-white text-shadow-lg/20 bg-red-500/50 mb-2 p-2 rounded-t-lg flex justify-between items-center">
                <h2>{{item.title}}</h2>
              </div>
              <div class="flex-col font-semibold text-shadow-lg">
                <ScrollArea class="h-72 w-full rounded-md mb-2">
                  <div v-for="item_ach in achItems" :key="item_ach.value" :value="item_ach.value" class="flex-col mr-4 font-semibold text-shadow-lg whitespace-pre-line ml-4">
                    <div class="flex gap-4 p-2 inset-shadow-sm inset-shadow-red-500/50 rounded-lg mb-2 ">
                      <div class="avatar self-center ml-2">
                        <div class="w-12 h-12 rounded shadow-sm shadow-black">
                          <img :src="item_ach.icon" />
                        </div>
                      </div>
                      <div class="flex-col">
                        <p class="font-semibold text-sm">{{item_ach.title}}</p>
                        <p class="font-normal text-xs">{{item_ach.text}}</p>
                      </div>
                    </div>
                  </div>
                </ScrollArea>
              </div>
            </div>
          </div>
        </ScrollArea>
      </div>
      <div class="grid">

      </div>



      <div class="mt-8"/>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

// Буду заменять на fetch с API
const achItemsLinks= [
  { value: 'achL-1', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6' },
  { value: 'achL-2', title: 'Мастер ASP .NET Core', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-3', title: 'Мастер C#', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-4', title: 'Мастер JS', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-5', title: 'Мастер ASP .NET Core', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-6', title: 'Мастер C#', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-7', title: 'Мастер JS', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-8', title: 'Мастер ASP .NET Core', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-9', title: 'Мастер C#', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6\n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6\n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6\n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6\nДостижение 5 \nДостижение 6' },
  { value: 'achL-10', title: 'Мастер JS', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-11', title: 'Мастер ASP .NET Core', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-12', title: 'Мастер C#', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-13', title: 'Мастер JS', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-14', title: 'Мастер ASP .NET Core', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
  { value: 'achL-15', title: 'Мастер C#', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
]

const achItems= [
  { value: 'achL-1', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },
  { value: 'achL-2', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },
  { value: 'achL-3', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },
  { value: 'achL-4', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },
  { value: 'achL-5', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },
  { value: 'achL-6', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },
  { value: 'achL-7', icon: "https://img.daisyui.com/images/profile/demo/batperson@192.webp", title: 'Мастер JS', text: 'Достижение 1 \n ого текст' },
  { value: 'achL-8', title: 'Мастер ASP .NET Core', content: 'Достижение 1 \n Достижение 2\nДостижение 3 \nДостижение 4 \nДостижение 5 \nДостижение 6' },
]

</script>
