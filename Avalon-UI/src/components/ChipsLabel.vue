<template>
  <v-row justify="left">
    <v-col cols="auto">
      <span class="subheading">Select your label(s)</span>
        <v-chip-group @click="udpateSelection" class="ma-2" multiple selected-class="text-orange" v-model="selection">
          <v-chip v-for="label in labels" :value="label" :key="label">
            {{ label }}
          </v-chip>
        </v-chip-group>
    </v-col>
  </v-row>
</template>

<script>
import { ref, watch, inject } from 'vue';

export default {
  setup() {
    const emitter = inject('emitter');
    const selection = ref([]);

    const udpateSelection = () => {
    };

    watch(selection, (newValue, oldValue) => {
      emitter.emit('countChanged', newValue);
    });

    return {
      selection,
      udpateSelection,
    };
  },

  data: () => ({
    labels: [
      'Bug',
      'Improvement',
      'Contact',
      'Suggestion',
      'Tip',
      'Security',
      'Account',
    ],

    //selection: [],
  }),

  methods: {
    // udpateSelection() {
    //   console.log(this.selection)
    // }
  }
}

</script>

<style scoped></style>
