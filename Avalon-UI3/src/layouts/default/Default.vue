<template>
  <v-app>
    <v-btn size="small" @click="getStarted" color="orange">Get </v-btn>
    <default-bar />
    <v-btn size="small" @click="getStarted" color="orange"> Started</v-btn>
    <default-view />
    <v-btn size="small" @click="getStarted" color="orange">HEllo world</v-btn>
  </v-app>
</template>

<script setup>
  import DefaultBar from './AppBar.vue'
  import DefaultView from './View.vue'
</script>
