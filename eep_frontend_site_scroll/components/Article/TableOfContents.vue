<template>
  <nav v-if="headings.length > 1" class="mb-10">
    <div class="text-sm font-semibold mb-3 opacity-80">Содержание</div>
    <ul class="space-y-2">
      <li v-for="h in headings" :key="h.idx">
        <a
            href="#"
            class="hover:underline opacity-90 pl-2"
            :class="h.level === 2 ? 'font-bold' : 'pl-6'"
            @click.prevent="scrollTo(h.idx)"
        >
          {{ h.text }}
        </a>
      </li>
    </ul>
  </nav>
</template>

<script setup lang="ts">
import { nextTick } from 'vue'
const props = defineProps<{ blocks: Array<any> }>()
const headings = computed(() =>
    props.blocks
        .map((b, i) =>
            b.type === 'heading'
                ? { idx: i, level: b.level, text: b.text }
                : null
        )
        .filter(Boolean)
)

function scrollTo(idx: number) {
  nextTick(() => {
    const el = document.getElementById(`heading-${idx}`)
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'start' })
    }
  })
}
</script>
