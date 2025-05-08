
<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Настройки пользователя</h2>
      <button class="drag-handle p-2 rounded-full hover:bg-red-500/50 cursor-move select-none" title="Перетащить">
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><path fill="#888" d="M17.707 8.293a.999.999 0 1 0-1.414 1.414L17.586 11H13V6.414l1.293 1.293a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L12 2.586L8.293 6.293a.999.999 0 1 0 1.414 1.414L11 6.414V11H6.414l1.293-1.293a.999.999 0 1 0-1.414-1.414L2.586 12l3.707 3.707a.997.997 0 0 0 1.414 0a1 1 0 0 0 0-1.414L6.414 13H11v4.586l-1.293-1.293a.999.999 0 1 0-1.414 1.414L12 21.414l3.707-3.707a.999.999 0 1 0-1.414-1.414L13 17.586V13h4.586l-1.293 1.293a.999.999 0 1 0 1.414 1.414L21.414 12z"/></svg>
      </button>
    </div>

    <form @submit.prevent="saveSettings" class="p-6 space-y-6 text-black">

      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Логин</label>
        <Input v-model="form.username" placeholder="Логин" class="w-full input input-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>

      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">ФИО</label>
        <Input v-model="form.fullname" placeholder="Валера Валера" class="w-full input input-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>

      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Почта</label>
        <Input v-model="form.email" type="email" placeholder="name@mail.com" class="w-full input input-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>

      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Пароль</label>
        <Input v-model="form.password" type="password" autocomplete="new-password" class="w-full input input-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>

      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32 ">Повторный пароль</label>
        <div class="w-full space-y-1">
          <Input v-model="form.confirm" type="password" autocomplete="new-password" class="w-full input input-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold" />
<!--          <p v-if="form.password !== form.confirm || form.password === ''" class="text-xs text-red-600">Пароли не совпадают</p>-->
        </div>
      </div>

      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Тема</label>
        <Select v-model="form.theme" class="w-full select select-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold ">
          <option value="default">По умолчанию</option>
          <option value="dark">Dark</option>
          <option value="light">Light</option>
          <option value="cupcake">Cupcake</option>
        </Select>
      </div>

      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Шрифт</label>
        <Select v-model="form.font" class="w-full select select-bordered bg-white
        inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold ">
          <option value="sans">Sans‑Serif</option>
          <option value="serif">Serif</option>
          <option value="mono">Monospace</option>
        </Select>
      </div>

      <div class="text-right">
        <button
            type="submit"
            class="btn text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500
         disabled:bg-gray-400 disabled:cursor-not-allowed">
          {{ saving ? 'Сохраняю…' : 'Сохранить' }}
        </button>
      </div>
    </form>

    <div class="mt-8"></div>
  </div>
</template>
<script setup lang="ts">
const form = reactive({
  username: '',
  fullname: '',
  email: '',
  password: '',
  confirm: '',
  theme: 'default',
  font: 'sans',
})

const password_conf = ref(false)

const saving = ref(false)

async function saveSettings() {
  if (!password_conf.value) return
  saving.value = true

  setTimeout(() => saving.value = false, 700)
}
</script>
