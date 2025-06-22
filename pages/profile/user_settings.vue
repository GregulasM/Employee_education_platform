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
        <input v-model="form.login" placeholder="Логин" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">ФИО</label>
        <input v-model="form.secondName" placeholder="Фамилия" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
        <input v-model="form.firstName" placeholder="Имя" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
        <input v-model="form.lastName" placeholder="Отчество" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Почта</label>
        <input v-model="form.email" type="email" placeholder="name@mail.com" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Аватар</label>
        <input @change="onAvatarChange" type="file" accept="image/*" class="file-input file-input-ghost w-full input input-bordered bg-white focus:bg-red-500/50 focus:shadow-md active:border-none focus:border-none text-black font-semibold" />
        <input v-model="form.avatarUrl" type="text" placeholder="Ссылка на картинку" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Старый пароль</label>
<!--        <input v-model="form.old_password" type="password" autocomplete="new-password" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />-->
        <input
            v-model="form.old_password"
            type="password"
            autocomplete="new-password"
            @blur="checkOldPassword"
            class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold "
        />
        <span v-if="checkingPassword" class="text-xs text-gray-400 ml-2">Проверка...</span>
        <span v-else-if="form.old_password && !oldPasswordValid" class="text-xs text-red-600 ml-2">Пароль неверный</span>
        <span v-else-if="oldPasswordValid" class="text-xs text-green-600 ml-2">OK</span>
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Пароль</label>
        <input v-model="form.password" type="password" autocomplete="new-password" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 shadow-md shadow-orange-200 focus:shadow-none focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold " />
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32 ">Повторный пароль</label>
        <div class="w-full space-y-1">
          <input v-model="form.confirm" type="password" autocomplete="new-password" class="w-full input input-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold" />
          <p v-if="form.password !== '' && form.password !== form.confirm" class="text-xs text-red-600">Пароли не совпадают</p>
        </div>
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Тема</label>
        <select v-model="form.theme" class="w-full select select-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold ">
          <option value="default">По умолчанию</option>
          <option value="dark">Dark</option>
          <option value="light">Light</option>
          <option value="cupcake">Cupcake</option>
        </select>
      </div>
      <div class="form-control bg-white border border-red-500/50 rounded-box shadow-md p-4 flex flex-col sm:flex-row items-center gap-4">
        <label class="font-semibold shrink-0 w-32">Шрифт</label>
        <select v-model="form.font" class="w-full select select-bordered bg-white inset-shadow-xs inset-shadow-red-500/50 focus:border-none focus:inset-shadow-sm focus:inset-shadow-red-500/50 text-black font-semibold ">
          <option value="sans">Sans‑Serif</option>
          <option value="serif">Serif</option>
          <option value="mono">Monospace</option>
        </select>
      </div>
      <div class="text-right">
        <button
            type="submit"
            :disabled="!canSave"
            class="btn text-white bg-red-500/50 hover:bg-red-500/70 hover:border-none border-none font-semibold text-shadow-lg/20 shadow-sm shadow-neutral-500 disabled:bg-gray-400 disabled:cursor-not-allowed">
          {{ saving ? 'Сохраняю…' : 'Сохранить' }}
        </button>
      </div>
      <div v-if="errorMsg" class="text-red-600 font-semibold mt-2">{{ errorMsg }}</div>
      <div v-if="successMsg" class="text-green-700 font-semibold mt-2">{{ successMsg }}</div>
    </form>
    <div class="mt-8"></div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useUsersStore } from '~/stores/users_store'

const usersStore = useUsersStore()
const currentUser = computed(() => usersStore.currentUser)

const form = reactive({
  login: '',
  secondName: '',
  firstName: '',
  lastName: '',
  email: '',
  old_password: '',
  password: '',
  confirm: '',
  avatar: null as File | null,
  avatarUrl: '',
  theme: 'default',
  font: 'sans',
})
const errorMsg = ref('')
const successMsg = ref('')
const saving = ref(false)


const oldPasswordValid = ref(false)
const checkingPassword = ref(false)

async function checkOldPassword() {
  errorMsg.value = ''
  oldPasswordValid.value = false
  if (!form.login || !form.old_password) return
  checkingPassword.value = true
  try {
    await $fetch('http://localhost:5148/api/auth/login', {
      method: 'POST',
      body: { login: form.login, password: form.old_password }
    })
    oldPasswordValid.value = true
  } catch (e: any) {
    oldPasswordValid.value = false
    errorMsg.value = 'Старый пароль неверный'
  }
  checkingPassword.value = false
}


function onAvatarChange(e: Event) {
  const input = e.target as HTMLInputElement
  form.avatar = input.files && input.files[0] ? input.files[0] : null
}

const canSave = computed(() => {
  if (!form.old_password || !oldPasswordValid.value) return false
  if (form.password || form.confirm) {
    return form.password === form.confirm && form.password.length >= 4
  }
  return true
})

async function saveSettings() {
  errorMsg.value = ''
  successMsg.value = ''
  if (!currentUser.value) {
    errorMsg.value = 'Пользователь не найден'
    return
  }
  if (!form.old_password) {
    errorMsg.value = 'Введите старый пароль для подтверждения'
    return
  }
  if ((form.password || form.confirm) && form.password !== form.confirm) {
    errorMsg.value = 'Пароли не совпадают'
    return
  }
  if ((form.password && form.password.length < 4) || (form.confirm && form.confirm.length < 4)) {
    errorMsg.value = 'Новый пароль должен быть не короче 4 символов'
    return
  }

  saving.value = true

  const patch: any = {
    login: form.login,
    email: form.email,
    secondName: form.secondName,
    firstName: form.firstName,
    lastName: form.lastName,
    theme: form.theme,
    font: form.font,
    ActiveCourseId: currentUser.value?.activeCourseId ?? null,
    SelectedCharacterId: currentUser.value?.selectedCharacterId ?? null,
  }

  if (form.avatarUrl) {
    patch.avatar = form.avatarUrl
  }

  if (form.password) {
    patch.oldPassword = form.old_password
    patch.password = form.password
  } else {
    patch.oldPassword = form.old_password
  }

  if (form.avatar) {
    try {
      const fd = new FormData()
      fd.append('avatar', form.avatar)
      const uploadUrl = `http://localhost:5148/api/users/${currentUser.value.id}/avatar`
      const result = await $fetch(uploadUrl, {
        method: 'POST',
        body: fd,
      })
      if (result.avatarUrl) {
        patch.avatar = result.avatarUrl
      }
    } catch (e) {
      errorMsg.value = 'Ошибка загрузки аватара'
      saving.value = false
      return
    }
  }

  try {
    await usersStore.updateUser(currentUser.value.id, patch)
    const updated = await usersStore.getUserById(currentUser.value.id)
    if (updated && usersStore.currentUser) {
      Object.assign(usersStore.currentUser, updated)
      usersStore.saveSession(usersStore.currentUser, usersStore.token)
    }
    successMsg.value = 'Данные успешно обновлены'
    form.old_password = ''
    form.password = ''
    form.confirm = ''
    form.avatar = null
    if (currentUser.value) {
      form.login = currentUser.value.login ?? ''
      form.secondName = currentUser.value.secondName ?? ''
      form.firstName = currentUser.value.firstName ?? ''
      form.lastName = currentUser.value.lastName ?? ''
      form.email = currentUser.value.email ?? ''
      form.theme = currentUser.value.theme?.name ?? 'default'
      form.font = currentUser.value.font?.name ?? 'sans'
    }
  } catch (e: any) {
    errorMsg.value = usersStore.error || e?.message || 'Ошибка обновления'
  }
  saving.value = false
}

onMounted(() => {
  if (currentUser.value) {
    form.login = currentUser.value.login ?? ''
    form.secondName = currentUser.value.secondName ?? ''
    form.firstName = currentUser.value.firstName ?? ''
    form.lastName = currentUser.value.lastName ?? ''
    form.email = currentUser.value.email ?? ''
    form.theme = currentUser.value.theme?.name ?? 'default'
    form.font = currentUser.value.font?.name ?? 'sans'
  }
})
</script>
