
<template>
  <div>
    <EditorContent :editor="editor" />
  </div>
</template>

<script setup lang="ts">
import { Editor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Heading from '@tiptap/extension-heading'
import Image from '@tiptap/extension-image'
import Link from '@tiptap/extension-link'
import Color from '@tiptap/extension-color'
import TextAlign from '@tiptap/extension-text-align'
import Youtube from '@tiptap/extension-youtube'

const props = defineProps({
  content: { type: Object, required: true },
})

const editor = new Editor({
  editable: false,
  content: props.content,
  extensions: [
    StarterKit,
    Heading,
    Image,
    Link,
    Color,
    TextAlign,
    Youtube,
  ],
})
</script>
