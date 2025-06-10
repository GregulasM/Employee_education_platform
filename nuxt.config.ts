// https://nuxt.com/docs/api/configuration/nuxt-config
import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  css: ['~/assets/css/main.css'],
  vite: {
    plugins: [
      tailwindcss(),

    ],
  },
  vue: {
    compilerOptions: {
      isCustomElement: (tag) => ['calendar-'].some(prefix => tag.startsWith(prefix)),
    },
  },

  modules: [
    '@nuxt/icon',
    '@nuxt/image',
    '@nuxt/test-utils',
    '@nuxt/content',
    'shadcn-nuxt',
    '@pinia/nuxt',
    'nuxt-tiptap-editor'
  ],
  shadcn: {
    /**
     * Prefix for all the imported component
     */
    prefix: '',
    /**
     * Directory that the component lives in.
     * @default "./components/ui"
     */
    componentDir: './components/ui'
  },

  components: [
    {
      path: '~/components/editor',
      pathPrefix: false,
    },
  ],

  hooks: {
    'pages:extend'(pages) {
      for (const page of pages) {
        if (page.path.startsWith('/profile')) {
          page.meta = page.meta || {}
          page.meta.layout = 'profile'
        }
      }
    }
  }
})