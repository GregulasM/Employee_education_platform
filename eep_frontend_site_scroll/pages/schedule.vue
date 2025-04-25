<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">

    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Расписание</h2>
      <button class="drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
      </button>
    </div>

    <ScrollArea class="p-4">
<!--    <div class="p-4 overflow-x-auto">-->
      <table class="table w-full rounded-lg overflow-hidden shadow-md">
        <thead>
        <tr class="text-center bg-orange-100 font-semibold text-black">
          <th class="grid rounded-tl-lg shadow-xs shadow-red-500/50"><p class="ml-4">День</p><p class="mr-4">Время</p></th>
          <th v-for="day in weekDays" :key="day" class="py-2 px-4 shadow-xs shadow-red-500/50"
              :class="{'bg-red-500/50 text-white': day.isToday}">
            {{ day.name }}
            <p v-if="day.isToday" class="text-xs ">(текущий день)</p>
          </th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="slot in timeSlots" :key="slot" class="text-center border-b border-red-300/50">
          <td class="bg-orange-100 font-semibold ">{{ slot }}</td>
          <td v-for="day in weekDays" :key="day.name" class="p-2 shadow-xs shadow-red-500/50"
              :class="day.isToday ? 'bg-red-300/60  !shadow-none' : 'bg-white '">
            <div v-if="schedule[day.name][slot]" contenteditable
                 class="hover:bg-red-300 bg-red-200 rounded-lg shadow p-2 cursor-text shadow-sm shadow-black ">
              <p class="font-bold ">{{ schedule[day.name][slot].subject }}</p>
              <p>{{ schedule[day.name][slot].teacher }}</p>
              <p class="text-sm opacity-70">{{ schedule[day.name][slot].details }}</p>
            </div>
          </td>
        </tr>
        </tbody>
      </table>
<!--    </div>-->
      <ScrollBar orientation="horizontal" />
    </ScrollArea>

    <div class="mt-8"></div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import ScrollArea from "../components/ui/scroll-area/ScrollArea.vue";

const weekDays = computed(() => {
  const days = ['Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота', 'Воскресенье'];
  const today = new Date().getDay() || 7; // Воскресенье = 7
  return days.map((day, index) => ({ name: day, isToday: index + 1 === today }));
});

const timeSlots = ref(['8:30-10:00', '10:10-11:40', '12:00-13:30', '13:40-15:10', '15:20-16:50']);

const schedule = ref({
  'Понедельник': {
    '12:00-13:30': { subject: 'Middleware в ASP', teacher: 'Крутой Е.И.', details: 'Совещательная, каб. 21, 2 этаж' },
  },
  'Вторник': {
    '10:10-11:40': { subject: 'Практики компании', teacher: 'Лесной А.К.', details: 'Совещательная, каб. 12, 1 этаж' },
  },
  'Среда': {
    '15:20-16:50': { subject: 'AGILE. Scrum. Планерка.', teacher: 'Лесной А.К.', details: 'Совещательная, каб. 12, 1 этаж' },
  },
  'Четверг': {},
  'Пятница': {'12:00-13:30': { subject: 'Middleware в ASP', teacher: 'Крутой Е.И.', details: 'Совещательная, каб. 21, 2 этаж' },},
  'Суббота': {},
  'Воскресенье': {},
})
</script>
