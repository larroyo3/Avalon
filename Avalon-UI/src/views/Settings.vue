<template>
  <v-card theme="dark">
    <v-card-title class="text-h6 text-md-h5 text-lg-h4 ma-5 text-orange">Contact</v-card-title>
    <chipsLabel class="ma-2"></chipsLabel>
    <v-textarea class="mx-14" clearable label="More details" variant="outlined" v-model="message"></v-textarea>
    <v-btn @click="sendMessage" class="ma-5" color="orange">
      Send
    </v-btn>
  </v-card>
</template>

<script>
import chipsLabel from "../components/ChipsLabel.vue"
import { ref, onMounted, inject } from 'vue';

export default {
  components: {
    chipsLabel,
  },

  setup() {
    const emitter = inject('emitter');
    const selection = ref([]);

    onMounted(() => {
      emitter.on('countChanged', (newValue) => {
        selection.value = newValue;
      });
    });

    return {
      selection,
    };
  },

  data() {
    return {
      message: ""
    };
  },

  methods: {
    sendMessage() {
      console.log(this.message)
      console.log(this.selection)
    }
  }
}
</script>