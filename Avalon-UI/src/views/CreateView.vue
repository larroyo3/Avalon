<template>
  <div>
    <div class="d-flex flex-row mb-6 bg-surface-variant">
      <v-sheet class="ma-2 pa-2">Flex item 1</v-sheet>
      <v-sheet class="ma-2 pa-2">Flex item 2</v-sheet>
      <v-sheet class="ma-2 pa-2">cfezgztegtg</v-sheet>
    </div>

    <div class="d-flex flex-row-reverse mb-6 bg-surface-variant">
      <v-sheet class="ma-2 pa-2">Flex item 1</v-sheet>
      <v-sheet class="ma-2 pa-2">Flex item 2</v-sheet>
      <v-sheet class="ma-2 pa-2">cfezgztegtg</v-sheet>
    </div>
  </div>
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
