<!-- components/TiptapEditor.vue -->
<template>
  <div>
    <div class="flex flex-wrap gap-2 mb-2 " v-if="editor">
      <!-- Bold -->
      <button
          class="btn btn-xs bg-black"
          :class="{ 'bg-gray-700': editor.isActive('bold') }"
          @click.prevent="editor.chain().focus().toggleBold().run()"
          title="Жирный"
      >
        <b>B</b>
      </button>
      <!-- Italic -->
      <button
          class="btn btn-xs bg-black"
          :class="{ 'bg-gray-700': editor.isActive('italic') }"
          @click.prevent="editor.chain().focus().toggleItalic().run()"
          title="Курсив"
      >
        <i>I</i>
      </button>
      <!-- Heading -->
      <button
          class="btn btn-xs bg-black"
          :class="{ 'bg-orange-600': editor.isActive('heading', { level: 2 }) }"
          @click.prevent="editor.chain().focus().toggleHeading({ level: 2 }).run()"
          title="Заголовок H2"
      >
        H2
      </button>
      <button
          class="btn btn-xs bg-black"
          :class="{ 'bg-orange-600': editor.isActive('heading', { level: 3 }) }"
          @click.prevent="editor.chain().focus().toggleHeading({ level: 3 }).run()"
          title="Заголовок H3"
      >
        H3
      </button>
      <!-- Bullet List -->
      <button
          class="btn btn-xs bg-black"
          :class="{ 'bg-green-600': editor.isActive('bulletList') }"
          @click.prevent="editor.chain().focus().toggleBulletList().run()"
          title="Список"
      >
        • Список
      </button>
      <!-- Ordered List -->
      <button
          class="btn btn-xs bg-black"
          :class="{ 'bg-green-800': editor.isActive('orderedList') }"
          @click.prevent="editor.chain().focus().toggleOrderedList().run()"
          title="Нумерованный список"
      >
        1. Список
      </button>
      <!-- Quote -->
      <button
          class="btn btn-xs bg-black"
          :class="{ 'bg-purple-600': editor.isActive('blockquote') }"
          @click.prevent="editor.chain().focus().toggleBlockquote().run()"
          title="Цитата"
      >
        “Цитата”
      </button>
      <!-- Image -->
      <button
          class="btn btn-xs btn-warning"
          @click.prevent="addImage"
          title="Картинка"
      >
        🖼️
      </button>
      <!-- Очистить -->
      <button
          class="btn btn-xs btn-error"
          @click.prevent="editor.chain().focus().clearContent().run()"
          title="Очистить"
      >
        🧹
      </button>
    </div>

    <EditorContent :editor="editor" class="rounded border border-none bg-gray-900 text-white min-h-[250px] px-2 py-2" />
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Heading from '@tiptap/extension-heading'
import Image from '@tiptap/extension-image'
import Link from '@tiptap/extension-link'
import Color from '@tiptap/extension-color'
import TextAlign from '@tiptap/extension-text-align'

const props = defineProps({
  modelValue: { type: Object, default: null },
})

const emit = defineEmits(['update:modelValue'])
const editor = useEditor({
  content: props.modelValue,
  extensions: [
    StarterKit,
    Heading.configure({ levels: [1, 2, 3] }),
    Image,
    Link,
    Color,
    TextAlign,
  ],
  autofocus: false,
  onUpdate({ editor }) {
    emit('update:modelValue', editor.getJSON())
  },
})

// Watch for external value changes (optional)
watch(
    () => props.modelValue,
    (val) => {
      if (!val) return
      if (editor.value && JSON.stringify(editor.value.getJSON()) !== JSON.stringify(val)) {
        editor.value.commands.setContent(val)
      }
    }
)

function addImage() {
  const url = prompt('Вставьте ссылку на картинку')
  if (url) {
    editor.value?.chain().focus().setImage({ src: url }).run()
  }
}
</script>
