<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">

    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 gap-4 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Расписание</h2>
      <button class="drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
      </button>
    </div>

    <ScrollArea class="p-4">
      <table class="table w-full rounded-lg overflow-hidden shadow-md">
        <thead>
        <tr class="text-center bg-orange-100 font-semibold text-black ">
          <th class="shadow-xs shadow-red-500/50 sticky left-0 z-10 bg-orange-100 opacity-75">
            <p class="ml-4">День</p>
            <p class="mr-4">Время</p>
          </th>
          <th v-for="day in weekDaysWithFlags" :key="day.name" class="py-2 px-4 shadow-xs shadow-red-500/50"
              :class="{'bg-red-500/50 text-white': day.isToday}">
            {{ day.name }}
            <p v-if="day.isToday" class="text-xs ">(текущий день)</p>
          </th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="slot in timeSlots" :key="slot" class="text-center border-b border-red-300/50">
          <td class="bg-orange-100 font-semibold sticky left-0 z-10 opacity-75 ">{{ slot }}</td>
          <td v-for="day in weekDaysWithFlags" :key="day.name"  class="p-2 shadow-xs shadow-red-500/50"
              :class="day.isToday ? 'bg-red-300/60  !shadow-none' : 'bg-white '">

            <div v-if="scheduleMatrix[day.name][slot] && scheduleMatrix[day.name][slot].length"
                 v-for="lesson in scheduleMatrix[day.name][slot]"
                 :key="lesson.id"
                 contenteditable="false"
                 class="hover:bg-red-300 bg-red-200 rounded-lg shadow p-2 cursor-text shadow-sm shadow-black mb-2">
              <p class="font-bold ">{{ lesson.subject }}</p>
              <p>{{ lesson.teacher }}</p>
              <p class="text-sm opacity-70">{{ lesson.details }}</p>
            </div>
          </td>
        </tr>
        </tbody>
      </table>
      <ScrollBar orientation="horizontal" />
    </ScrollArea>
    <div class="mt-8"></div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useScheduleStore } from '@/stores/schedules_store'

const store = useScheduleStore()

const weekDays = [
  'Понедельник',
  'Вторник',
  'Среда',
  'Четверг',
  'Пятница',
  'Суббота',
  'Воскресенье'
]

const timeSlots = computed(() => {
  if (!Array.isArray(store.schedules)) return []
  return Array.from(
      new Set(store.schedules.map(s => s.timeSlot).filter(Boolean))
  )
})

const scheduleMatrix = computed(() => {
  const matrix: Record<string, Record<string, any[]>> = {}
  weekDays.forEach(day => { matrix[day] = {} })
  for (const sched of store.schedules) {
    if (!sched.isActive) continue
    if (!sched.dayOfWeek || !sched.timeSlot) continue
    if (!matrix[sched.dayOfWeek]) matrix[sched.dayOfWeek] = {}
    if (!matrix[sched.dayOfWeek][sched.timeSlot]) matrix[sched.dayOfWeek][sched.timeSlot] = []
    matrix[sched.dayOfWeek][sched.timeSlot].push(sched)
  }
  return matrix
})

const today = new Date().getDay() || 7
const weekDaysWithFlags = computed(() =>
    weekDays.map((day, i) => ({
      name: day,
      isToday: i + 1 === today
    }))
)

onMounted(() => {
  store.fetchSchedules()
})
</script>
