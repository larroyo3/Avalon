<template>
  <v-card theme="dark">
    <v-card-title class="text-h6 text-md-h5 text-lg-h4 ma-5 text-orange">Edit my profile</v-card-title>
    <v-card-text>
      <v-row>
        <v-col cols="6">
          <file-pond name="test" ref="pond" label-idle="Drop files here or Browse" v-bind:allow-multiple="false"
            v-bind:files="imageSrc" accepted-file-types="image/jpeg, image/png" v-on:init="handleFilePondInit"
            v-on:addfile="addFile" v-on:removefile="removeFile" />
        </v-col>
        <v-col cols="6">
          <v-text-field v-model="name" :rules="rules" label="Name*" required></v-text-field>

          <v-text-field v-model="password" :rules="rules" label="Password*" type="password" required></v-text-field>

          <v-select :items="packages" v-model="package" item-title="package" label="Package*" required></v-select>

        </v-col>
      </v-row>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="red" variant="text" @click="logout">
        Log out
      </v-btn>
      <v-btn color="green" :disabled="isDisabled" variant="text" @click="saveChange">
        Save
      </v-btn>
    </v-card-actions>

    <v-card-title class="text-h6 text-md-h5 text-lg-h4 ma-5 text-orange">My upload</v-card-title>
    <photoCarousel/>
  </v-card>
</template>

<script>
import photoCarousel from "../components/PhotoCarousel.vue"
// Import Vue FilePond
import vueFilePond from "vue-filepond";
import "filepond/dist/filepond.min.css";

// Import image preview plugin styles
import "filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css";
// Import image preview and file type validation plugins
import FilePondPluginFileValidateType from "filepond-plugin-file-validate-type";
import FilePondPluginImagePreview from "filepond-plugin-image-preview";

import Singleton from '../Singleton.js';

// Create component
const FilePond = vueFilePond(
  FilePondPluginFileValidateType,
  FilePondPluginImagePreview
);

export default {
  name: "account",

  data() {
    return {
      name: Singleton.getInstance().getName(),

      profilePhoto: '',
      imageSrc: "",

      package: '',
      remainingPhoto: 0,
      password: '',

      packages: ['Free (3 daily upload)', 'Pro (10 daily upload)', 'Gold (10000 daily upload)'],

      hasFile: true,
    };
  },

  components: {
    FilePond,
    photoCarousel
  },

  methods: {
    logout() {
      this.$root.userId = 0;
      localStorage.clear();
      this.$router.push({ path: '/' });
    },

    saveChange() {
      if (this.package == 'Free (3 daily upload)') {
        this.remainingPhoto = 3
      } else if (this.package == 'Pro (10 daily upload)') {
        this.remainingPhoto = 10
      } else {
        this.remainingPhoto = 10000
      }

      this.updateUser()
    },

    updateUser() {
      const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          id: this.$root.userId,
          name: this.name,
          profilePhoto: this.profilePhoto,
          remainingPhoto: this.remainingPhoto,
          package: this.package,
          password: this.password
        })
      };
      fetch(`http://localhost:9001/api/Users/${this.$root.userId}`, requestOptions)
        .then(response => {
          console.log("appel parent")
          this.$emit('setupUserInfo');
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
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
        this.profilePhoto = "data:image/png;base64," + base64String;
      };

      reader.readAsDataURL(file);
    },

    fetchGetUserById(userId) {
      const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
      };
      fetch(`http://localhost:9001/api/Users/${userId}`, requestOptions)
        .then(async response => {
          const data = await response.json();

          if (!response.ok) {
            const error = (data && data.message) || response.status;
            return Promise.reject(error);
          }
          else {
            this.name = data.name;

            this.profilePhoto = data.profilePhoto;
            this.base64String = data.profilePhoto;
            this.convertBase64ToImage()

            this.package = data.package
            this.remainingPhoto = data.remainingPhoto
            this.password = data.password
          }
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    convertBase64ToImage() {
      if (this.profilePhoto.startsWith("data:image")) {
        this.profilePhoto = this.profilePhoto.split(",")[1];
      }

      const image = new Image();

      image.onload = () => {
        this.imageSrc = URL.createObjectURL(this.getBlobFromBase64(this.profilePhoto));
      };

      image.src = "data:image/png;base64," + this.profilePhoto;
    },

    // getBlobFromBase64_2(base64String) {
    //   const byteCharacters = atob(base64String);

    //   const byteNumbers = new Array(byteCharacters.length);
    //   for (let i = 0; i < byteCharacters.length; i++) {
    //     byteNumbers[i] = byteCharacters.charCodeAt(i);
    //   }

    //   const byteArray = new Uint8Array(byteNumbers);

    //   return new Blob([byteArray], { type: "image/png" });
    // },

    getBlobFromBase64(base64String) {
      const byteCharacters = atob(base64String);

      const byteNumbers = Array.from(byteCharacters, char => char.charCodeAt(0));

      const byteArray = new Uint8Array(byteNumbers);

      return new Blob([byteArray], { type: "image/png" });
    }

  },

  computed: {
    isDisabled() {
      // evaluate whatever you need to determine disabled here...
      if (this.hasFile && this.name.length > 0 && this.password.length > 0)
        return false
      return true
    }
  },

  mounted() {
    this.fetchGetUserById(this.$root.userId)
  }

}
</script>

<style></style>
