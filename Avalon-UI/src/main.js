import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import mitt from 'mitt';

// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

import "vuetify/dist/vuetify.min.css";
import '@mdi/font/css/materialdesignicons.css'

const vuetify = createVuetify({
  components,
  directives,
})

const emitter = mitt();

const app = createApp(App)

app.use(vuetify)
app.use(router)
app.provide('emitter', emitter);
app.mount('#app')
