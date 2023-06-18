<template>
  <v-row class="d-flex justify-center ma-6">
    <v-col cols="7">
      <file-pond name="test" ref="pond" label-idle="Drop files here or Browse" v-bind:allow-multiple="false"
        accepted-file-types="image/jpeg, image/png" v-on:init="handleFilePondInit" v-on:addfile="addFile"
        v-on:removefile="removeFile" />
    </v-col>
    <v-col cols="5">
      <v-expansion-panels variant="popout">
        <v-expansion-panel>
          <v-expansion-panel-title color="white">
            <template v-slot:default="{ expanded }">
              <v-row no-gutters>
                <v-col cols="4" class="d-flex justify-start">
                  Description
                </v-col>
                <v-col cols="8" class="text-grey">
                  <v-fade-transition leave-absolute>
                    <span v-if="expanded" key="0">
                      Enter your description
                    </span>
                    <span v-else key="1">
                      {{ photo.description }}
                    </span>
                  </v-fade-transition>
                </v-col>
              </v-row>
            </template>
          </v-expansion-panel-title>
          <v-expansion-panel-text>
            <v-text-field v-model="photo.description" hide-details placeholder="Bla bla bla"></v-text-field>
          </v-expansion-panel-text>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-title color="white">
            <template v-slot:default="{ expanded }">
              <v-row no-gutters>
                <v-col cols="4" class="d-flex justify-start">
                  Hastag
                </v-col>
                <v-col cols="8" class="text-grey">
                  <v-fade-transition leave-absolute>
                    <span v-if="expanded" key="0">
                      Enter your hashtag
                    </span>
                    <span v-else key="1">
                      {{ photo.hashtag }}
                    </span>
                  </v-fade-transition>
                </v-col>
              </v-row>
            </template>
          </v-expansion-panel-title>
          <v-expansion-panel-text>
            <v-text-field prefix="#" v-model="photo.hashtag" hide-details placeholder="holidays"></v-text-field>
          </v-expansion-panel-text>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-col>
  </v-row>
  <v-row class="d-flex justify-end ma-6">
    <v-col cols="6">
      <v-btn :disabled="isDisabled" @click="uploadPhoto" block append-icon="mdi-upload" color="orange">upload</v-btn>
    </v-col>
  </v-row>
  <v-snackbar v-model="snackbar" color="red" variant="tonal" :timeout="timeout">
    {{ text }}

    <template v-slot:actions>
      <v-btn color="red" variant="text" @click="snackbar = false">
        Close
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script>
// Import Vue FilePond
import vueFilePond from "vue-filepond";
import "filepond/dist/filepond.min.css";


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
  data() {
    return {
      date: null,
      photo: {
        description: '',
        hashtag: '',
        image: ''
      },
      base64String: '',
      hasFile: false,

      snackbar: false,
      text: 'No more upload available',
      timeout: 3000,
    };
  },

  methods: {
    handleFilePondInit() {
      console.log("FilePond has initialized");
    },

    addFile(error, file) {
      this.hasFile = true
      this.convertFileToBase64(file.file)
    },

    removeFile(error, file) {
      this.hasFile = false
    },

    convertFileToBase64(file) {
      const reader = new FileReader();
      reader.onload = () => {
        const base64String = reader.result.split(',')[1];
        this.photo.image = base64String;
      };
      reader.readAsDataURL(file);
    },

    uploadPhoto() {
      if (this.$root.remainingPhoto > 0) {
        this.fetchAPIData();
      } else {
        this.snackbar = true
      }
    },

    fetchAPIData() {
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          authorId: this.$root.userId,
          publicationDate: Date().toString().slice(0, 10).replace(/-/g, ""),
          hashtags: '#' + this.photo.hashtag,
          description: this.photo.description,
          imageData: this.photo.image
        })
      };
      fetch('http://localhost:9001/api/PhotoItems', requestOptions)
        .then(async response => {
          const data = await response.json();

          if (!response.ok) {
            const error = (data && data.message) || response.status;
            return Promise.reject(error);
          }
          else {
            location.reload();
          }
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },
  },

  computed: {
    isDisabled() {
      // evaluate whatever you need to determine disabled here...
      return !this.hasFile
    }
  },

  components: {
    FilePond,
  },
};
</script>

<style></style>
