<template>
  <div>
    <div class="d-flex flex-row mb-6 bg-surface-variant">
      <v-sheet class="ma-2 pa-2">Flex item 1</v-sheet>
      <v-sheet class="ma-2 pa-2">Flex item 2</v-sheet>
      <v-sheet class="ma-2 pa-2">Flex item 3</v-sheet>
    </div>

    <div class="d-flex flex-row-reverse mb-6 bg-surface-variant">
      <v-sheet class="ma-2 pa-2">Fcqfvcervzergv</v-sheet>
      <v-sheet class="ma-2 pa-2">Flex item 2</v-sheet>
      <v-sheet class="ma-2 pa-2">Flex item 3</v-sheet>
    </div>
  </div>
  <v-btn block color="orange" @click="logout">Log out</v-btn>
</template>

<script>

export default {
  name: "account",

  methods: {
    logout() {
      this.$root.userId = 0;
      localStorage.clear();
      this.$router.push({ path: '/' });
    }
  }

}
</script>

<style></style>
