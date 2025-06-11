<template>
  <div class="shadow-md shadow-orange-200 mt-8 mx-8 rounded-lg bg-orange-50 opacity-90">
    <div class="text-xl text-white font-bold text-shadow-lg/20 bg-red-500/50 p-2 rounded-t-lg flex justify-between items-center">
      <h2>Курсы</h2>
    </div>

    <ul class="p-4 space-y-4 font-semibold text-shadow-lg text-black">
      <li v-if="loading" class="text-center text-lg text-red-400">Загрузка...</li>
      <li v-if="error" class="text-center text-lg text-red-600">{{ error }}</li>

      <li v-for="c in paged" :key="c.slug"
          class="collapse collapse-arrow bg-white border border-red-500/50 rounded-lg">

        <input type="checkbox" />

        <div class="collapse-title flex items-center gap-4">
          <img :src="c.icon" class="w-10 h-10 rounded-box object-cover shadow">
          <span class="flex-1">{{ c.title }}</span>
          <button  v-on:click.stop="openCoursePage(c.slug)" class="btn btn-square btn-ghost hover:bg-red-500/50 border-none z-20">
            <svg xmlns="http://www.w3.org/2000/svg" class="size-[1.5em]" viewBox="0 0 24 24"><!-- Icon from Huge Icons by Hugeicons - undefined --><path fill="none" stroke="#888888" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M11 2C7.229 2 5.343 2 4.172 3.129C3 4.257 3 6.074 3 9.708v8.273c0 2.306 0 3.459.773 3.871c1.496.8 4.304-1.867 5.637-2.67c.773-.465 1.16-.698 1.59-.698s.817.233 1.59.698c1.333.803 4.14 3.47 5.637 2.67c.773-.412.773-1.565.773-3.871V12.5M3.5 7H10m3-2h8" color="#888888"/></svg>
          </button>

          <button  v-on:click.stop="openCoursePage(c.slug)" class="btn btn-square btn-ghost hover:bg-red-500/50 border-none z-20">
            <svg xmlns="http://www.w3.org/2000/svg" class="size-[1.5em]" viewBox="0 0 24 24"><!-- Icon from Huge Icons by Hugeicons - undefined --><path fill="none" stroke="#888888" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M11 2C7.229 2 5.343 2 4.172 3.129C3 4.257 3 6.074 3 9.708v8.273c0 2.306 0 3.459.773 3.871c1.496.8 4.304-1.867 5.637-2.67c.773-.465 1.16-.698 1.59-.698s.817.233 1.59.698c1.333.803 4.14 3.47 5.637 2.67c.773-.412.773-1.565.773-3.871V12.5M3.5 7H10m7 3V2m-4 4h8" color="#888888"/></svg>
          </button>

          <button  v-on:click.stop="openCoursePage(c.slug)" class="btn btn-square btn-ghost hover:bg-red-500/50 border-none z-20">
            <svg xmlns="http://www.w3.org/2000/svg" class="size-[1.5em]" viewBox="0 0 24 24"><!-- Icon from Huge Icons by Hugeicons - undefined --><g fill="none" stroke="#888888" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" color="#888888"><path d="M16.613 16.085C13.98 17.568 12.477 20.64 12 21.5V8c.415-.746 1.602-2.884 3.632-4.336c.855-.612 1.282-.918 1.825-.64c.543.28.543.896.543 2.127v8.84c0 .666 0 .999-.137 1.232c-.136.234-.508.443-1.25.862"/><path d="M12 7.806c-.687-.722-2.678-2.436-6.02-3.036c-1.692-.305-2.538-.457-3.26.126C2 5.48 2 6.426 2 8.321v6.809c0 1.732 0 2.598.463 3.139c.462.54 1.48.724 3.518 1.09c1.815.326 3.232.847 4.258 1.37c1.01.514 1.514.771 1.761.771s.752-.257 1.76-.771c1.027-.523 2.444-1.044 4.26-1.37c2.036-.366 3.055-.55 3.517-1.09c.463-.541.463-1.407.463-3.14V8.322c0-1.894 0-2.841-.72-3.425C20.557 4.313 19 4.77 18 5.5"/></g></svg>
          </button>
        </div>

        <div class="collapse-content">
          <div v-for="m in c.modules" :key="m.id"
               class="collapse collapse-arrow bg-orange-50/60 border border-red-300/50 rounded-lg mb-3 ml-4">

            <input type="checkbox" />
            <div class="collapse-title flex items-center gap-3">
              <img :src="m.icon" class="w-8 h-8 rounded-box object-cover shadow">
              {{ m.num }}. {{ m.title }}
            </div>

            <div class="collapse-content">
              <ul class="space-y-2 ml-4">
                <li v-for="a in m.articles" :key="a.slug"
                    class="flex items-center gap-2 inset-shadow-sm inset-shadow-red-500/50 p-2 rounded-lg">
                  <span class="text-xs opacity-60 w-12">{{ a.num }}</span>
                  <button class="text-left flex-1 hover:underline"
                          v-on:click="openArticle(c.slug, m.id, a.slug)">
                    {{ a.title }}
                  </button>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </li>

      <li class="text-xs self-center opacity-60 tracking-wide">
        <div class="join">
          <button v-on:click="prev" :disabled="page===1"
                  class="join-item btn btn-sm bg-red-500/50 btn-square btn-ghost hover:bg-red-500/50 border-none">«</button>
          <button v-for="n in totalPages" :key="n"  v-on:click="page=n"
                  class="join-item btn btn-sm btn-square btn-ghost hover:bg-red-500/50 border-none">{{ n }}</button>
          <button v-on:click="next" :disabled="page===totalPages"
                  class="join-item btn btn-sm bg-red-500/50 btn-square btn-ghost hover:bg-red-500/50 border-none">»</button>
        </div>
      </li>
    </ul>
    <div class="mt-8"></div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const allCourses = ref<any[]>([])
const loading = ref(true)
const error = ref<string|null>(null)

const page     = ref(1)
const perPage  = 5
const totalPages = computed(() => Math.ceil(allCourses.value.length / perPage))
const paged = computed(() => {
  const start = (page.value-1)*perPage
  return allCourses.value.slice(start, start+perPage)
})

function prev(){ if(page.value>1) page.value-- }
function next(){ if(page.value<totalPages.value) page.value++ }

const router = useRouter()
function openArticle(courseSlug: string, moduleId: number, articleSlug: string) {

  const courseObj = allCourses.value.find(c => c.slug === courseSlug)
  if (!courseObj) return

  const mod = courseObj.modules.find(m => m.id === moduleId)
  if (!mod) return

  const art = mod.articles.find(a => a.slug === articleSlug)
  if (!art) return

  router.push(
      `/courses/${courseSlug}/${moduleId}/${encodeURIComponent(art.title)}`
  )
}
function openCoursePage(slug:string){
  router.push(`/courses/${slug}`)
}

onMounted(async () => {
  loading.value = true
  error.value = null
  try {
    const courses = await $fetch('http://localhost:5148/api/courses')
    const modules = await $fetch('http://localhost:5148/api/modules')
    const articlesByModuleId: Record<number, any[]> = {}
    await Promise.all(modules.map(async (m:any) => {
      try {
        const articles = await $fetch(`http://localhost:5148/api/modules/${m.id}/articles`)
        articlesByModuleId[m.id] = articles || []
      } catch {
        articlesByModuleId[m.id] = []
      }
    }))
    allCourses.value = (courses || []).map((course:any) => {
      const slug = course.publicId || course.id || course.title?.toLowerCase().replace(/\s+/g, '-')
      const courseModules = modules.filter((m:any) => m.courseId === course.id)
          .sort((a, b) => (a.order ?? 0) - (b.order ?? 0))
          .map((mod:any, idx:number) => ({
            id: mod.id,
            num: idx+1,
            title: mod.title,
            icon: mod.image || '/mascot/mascot.png',
            articles: (articlesByModuleId[mod.id] || []).map((art:any, idx2:number) => ({
              slug: art.id || art.title?.toLowerCase().replace(/\s+/g, '-'),
              num: `${idx+1}.${idx2+1}`,
              title: art.title
            }))
          }))
      return {
        slug,
        icon: course.image || '/mascot/mascot.png',
        title: course.title,
        modules: courseModules
      }
    })
  } catch (e:any) {
    error.value = e?.message || 'Ошибка загрузки курсов'
  }
  loading.value = false
})
</script>
