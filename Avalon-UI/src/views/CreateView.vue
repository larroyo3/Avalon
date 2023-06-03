<template>
  <v-row class="d-flex justify-center ma-6">
    <v-col cols="2">
      <v-sheet>
        Hello
      </v-sheet>
    </v-col>
    <v-col cols="8">
      <file-pond name="test" ref="pond" label-idle="Drop files here or Browse" v-bind:allow-multiple="false"
        accepted-file-types="image/jpeg, image/png" v-on:init="handleFilePondInit" v-on:addfile="addFile"	/>
    </v-col>
    <v-col cols="2">
      <v-sheet>
        Hello
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
// Import Vue FilePond
import vueFilePond from "vue-filepond";
import "filepond/dist/filepond.min.css";

// Import FilePond plugins
// Please note that you need to install these plugins separately

// Import image preview plugin styles
import "filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css";

// Import image preview and file type validation plugins
import FilePondPluginFileValidateType from "filepond-plugin-file-validate-type";
import FilePondPluginImagePreview from "filepond-plugin-image-preview";

// Create component
const FilePond = vueFilePond(
  FilePondPluginFileValidateType,
  FilePondPluginImagePreview
);

export default {
  name: "create",
  data: function () {
    return { myFiles: ["cat.jpeg"] };
  },
  methods: {
    handleFilePondInit: function () {
      console.log("FilePond has initialized");

      // FilePond instance methods are available on `this.$refs.pond`
    },

    addFile: function (error, file) {
      console.log("FilePond has upload");
      console.log(file)

      // FilePond instance methods are available on `this.$refs.pond`
    },
  },
  components: {
    FilePond,
  },
};
</script>

<style>
.div-2 {
  background-color: #ABBAEA;
}
</style>
