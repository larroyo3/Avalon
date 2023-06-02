<template>
  <v-card theme="dark">
    <v-layout>
      <v-navigation-drawer expand-on-hover rail>
        <v-list v-if="login">
          <v-list-item prepend-avatar="https://randomuser.me/api/portraits/men/34.jpg" title="Sandra Adams"
            subtitle="sandra_a88@gmailcom">
          </v-list-item>
        </v-list>
        <v-list v-else>
          <v-list-item prepend-avatar="https://randomuser.me/api/portraits/women/85.jpg">
            <v-col cols="auto">
              <v-btn size="small" @click="getStarted" color="orange">Get Started</v-btn>
            </v-col>
          </v-list-item>
        </v-list>

        <v-divider></v-divider>

        <v-list density="compact" nav active-color="orange">
          <v-list-item prepend-icon="mdi-home" to="/foo" title="Home" value="home"></v-list-item>
          <v-list-item prepend-icon="mdi-plus-box" to="/bar" title="Create" value="create"></v-list-item>
          <v-list-item prepend-icon="mdi-account" title="Account" value="account"></v-list-item>
        </v-list>
      </v-navigation-drawer>

      <router-view></router-view>
    </v-layout>
  </v-card>
</template>

<script>
import Home from '@/views/Home'

export default {
  data() {
    return {
      login: false,

      foo: {
        template: '<div>Foo component!</div>'
      },

      bar: {
        template: '<div>Bar component!</div>'
      },

      routes: [
        { path: '/foo', component: Home },
        { path: '/bar', component: Home },
      ],

      router: new VueRouter({ routes })
    }
  },

  methods: {
    getStarted() {
      this.login = true
    },

    fetchAPIData() {
      fetch("http://localhost:5048/api/TodoItems", this.requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error ::', error));
    }
  },
}
</script>
