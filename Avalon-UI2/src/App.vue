<script lang="ts">
import HelloWorld from './components/HelloWorld.vue'
import TheWelcome from './components/TheWelcome.vue'

export default {
  components: {
    HelloWorld,
    TheWelcome
  },

  data() {
    return {
      count: 0,
      result: " ",
      responseAvailable: false,

      requestOptions: {
        method: 'GET',
        redirect: 'follow'
      }
    }
  },
  methods: {
    increment() {
      this.count++
    },

    fetchAPIData() {
      fetch("http://localhost:5048/api/TodoItems", this.requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error ::', error));
    }
  },

  async mounted() {
  }
}
</script>

<template>
  <header>
    <img alt="Vue logo" class="logo" src="./assets/logo.svg" width="125" height="125" />

    <div class="wrapper">
      <HelloWorld msg="You did it Lucas!" />
      <button @click="increment">Count is: {{ count }}</button>
      <button type="button" id="get-joke" @click="fetchAPIData">Get a Joke!!</button>
    </div>
    <div v-if="responseAvailable == true">
      <hr>
      <p>
        <i>{{ result }}</i>
      </p>
      <hr>
    </div>
  </header>

  <main>
    <TheWelcome />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
