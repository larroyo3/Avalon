<template>
  <v-card theme="dark">
    <v-layout>
      <v-navigation-drawer expand-on-hover rail :key="navBarPinned" :mini-variant="closed" :expand-on-hover="!navBarPinned" permanent clipped>
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
          <v-list-item prepend-icon="mdi-home" to="/" title="Home" value="home"></v-list-item>
          <v-list-item prepend-icon="mdi-plus-box" to="/create" title="Create" value="create"></v-list-item>
          <v-list-item v-if="login" prepend-icon="mdi-account" to="/account" title="Account"
            value="account"></v-list-item>
        </v-list>
      </v-navigation-drawer>
      <v-main style="height: 100vh; width: 100vw;">
        <RouterView />
      </v-main>
    </v-layout>
  </v-card>
</template>

<script>
import { RouterView } from 'vue-router'

export default {
  data() {
    return {
      login: false,

      responseAvailable: false,

      requestOptions: {
        method: 'GET',
        redirect: 'follow'
      }
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

<style scoped></style>
