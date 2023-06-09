<template>
  <v-dialog v-model="dialog" persistent width="1024">
    <template v-slot:activator="{ props }">
      <v-card theme="dark">
        <v-layout>
          <v-navigation-drawer expand-on-hover rail :key="navBarPinned" :mini-variant="closed"
            :expand-on-hover="!navBarPinned" permanent clipped>
            <v-list v-if="userId != 0">
              <v-list-item :prepend-avatar="profilePhoto" color="orange" :title="name"
                :subtitle="'Upload available : ' + remainingPhoto">
              </v-list-item>
            </v-list>
            <v-list v-else>
              <v-list-item prepend-icon="mdi-account-outline">
                <v-col cols="auto">
                  <v-btn size="small" v-bind="props" color="orange">Get Started</v-btn>
                </v-col>
              </v-list-item>
            </v-list>

            <v-divider></v-divider>

            <v-list density="compact" nav active-color="orange">
              <v-list-item prepend-icon="mdi-home" to="/" title="Home" value="home"></v-list-item>
              <v-list-item v-if="userId != 0" prepend-icon="mdi-plus-box" to="/create" title="Create"
                value="create"></v-list-item>
              <v-list-item v-if="userId != 0" prepend-icon="mdi-account" to="/account" title="Account"
                value="account"></v-list-item>
            </v-list>
          </v-navigation-drawer>
          <v-main>
            <RouterView @setupUserInfo="setupUserInfo" :remainingPhoto="remainingPhoto" :userId="userId" />
          </v-main>
        </v-layout>
      </v-card>
    </template>

    <v-card theme="dark">
      <v-tabs fixed-tabs bg-color="orange" v-model="tab">
        <v-tab value="registration">
          Registration
        </v-tab>
        <v-tab value="login">
          Log in
        </v-tab>
      </v-tabs>

      <v-window v-model="tab">
        <v-window-item value="registration">
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12" sm="6" md="6">
                  <v-text-field v-model="name" :rules="rules" label="Name*" required></v-text-field>
                </v-col>
                <v-col cols="12" md="6">
                  <v-text-field v-model="password" :rules="rules" label="Password*" type="password"
                    required></v-text-field>
                </v-col>
                <v-col cols="12" sm="8">
                  <v-select :items="packages" v-model="package" item-title="package" label="Package*" required></v-select>
                </v-col>
              </v-row>
            </v-container>
            <small>*indicates required field</small>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red" variant="text" @click="dialog = false">
              Cancel
            </v-btn>
            <v-btn color="green" :disabled="isDisabled" variant="text" @click="register">
              Save
            </v-btn>
          </v-card-actions>
        </v-window-item>

        <v-window-item value="login">
          <v-card-text>
            <v-form ref="form">
              <v-container>
                <v-row>
                  <v-col cols="12" sm="6" md="6">
                    <v-text-field label="Name*" v-model="name" :rules="rules" required></v-text-field>
                  </v-col>
                  <v-col cols="12" md="6">
                    <v-text-field label="Password*" type="password" v-model="password" :rules="rules"
                      required></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-form>
            <small>*indicates required field</small>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="red" variant="text" @click="dialog = false">
              Cancel
            </v-btn>
            <v-btn color="green" :disabled="isDisabled" variant="text" @click="login">
              Save
            </v-btn>
          </v-card-actions>
        </v-window-item>
      </v-window>
    </v-card>

    <v-snackbar v-model="snackbar" color="red" variant="tonal" :timeout="timeout">
      {{ text }}

      <template v-slot:actions>
        <v-btn color="red" variant="text" @click="snackbar = false">
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-dialog>
</template>

<script>
import { RouterView } from 'vue-router'

export default {
  name: 'App',

  data() {
    return {
      dialog: false,
      tab: null,
      userId: 0,

      snackbar: false,
      text: 'Error during log in',
      timeout: 3000,

      valid: false,
      name: '',
      password: '',
      remainingPhoto: 3,
      profilePhoto: null,
      package: 'Free (3 daily upload)',
      packages: ['Free (3 daily upload)', 'Pro (10 daily upload)', 'Gold (10000 daily upload)'],
      rules: [
        value => {
          if (value) return true

          return 'Field cannot be empty.'
        },
      ],
    }
  },

  methods: {
    login() {
      this.loginApi()
    },

    register() {
      this.dialog = false;

      if (this.package == 'Free (3 daily upload)') {
        this.remainingPhoto = 3
      } else if (this.package == 'Pro (10 daily upload)') {
        this.remainingPhoto = 10
      } else {
        this.remainingPhoto = 10000
      }
      this.registerApi()
    },

    registerApi() {
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: this.name,
          profilePhoto: this.profilePhoto,
          remainingPhoto: this.remainingPhoto,
          package: this.package,
          password: this.password
        })
      };
      fetch('http://localhost:5048/api/Users', requestOptions)
        .then(async response => {
          const data = await response.json();

          if (!response.ok) {
            const error = (data && data.message) || response.status;
            return Promise.reject(error);
          }
          else {
            this.userId = data.id
            localStorage.setItem('userId', data.id);
          }
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    loginApi() {
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: this.name,
          password: this.password
        })
      };
      fetch('http://localhost:5048/login', requestOptions)
        .then(async response => {
          const data = await response.json();

          if (!response.ok) {
            const error = (data && data.message) || response.status;
            this.snackbar = true
            return Promise.reject(error);
          }
          else {
            this.dialog = false;

            this.userId = data.id
            this.package = data.package
            this.remainingPhoto = data.remainingPhoto
            this.profilePhoto = data.profilePhoto
            localStorage.setItem('userId', data.id);
          }
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    fetchGetUserById(userId) {
      const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
      };
      fetch(`http://localhost:5048/api/Users/${userId}`, requestOptions)
        .then(async response => {
          const data = await response.json();

          if (!response.ok) {
            const error = (data && data.message) || response.status;
            return Promise.reject(error);
          }
          else {
            this.name = data.name;
            this.profilePhoto = data.profilePhoto;
            this.package = data.package
            this.remainingPhoto = data.remainingPhoto
          }
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    loadImageFromPath(path) {
      return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open('GET', path);
        xhr.responseType = 'blob';

        xhr.onload = () => {
          if (xhr.status === 200) {
            resolve(xhr.response);
          } else {
            reject(new Error('Impossible de charger l\'image.'));
          }
        };

        xhr.onerror = () => {
          reject(new Error('Erreur lors du chargement de l\'image.'));
        };

        xhr.send();
      });
    },

    convertImageToBase64(imageBlob) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onloadend = () => {
          resolve(reader.result);
        };

        reader.onerror = () => {
          reject(new Error('Erreur lors de la conversion de l\'image en base64.'));
        };

        reader.readAsDataURL(imageBlob);
      });
    },

    async loadImageAndConvertToBase64(path) {
      try {
        const imageBlob = await this.loadImageFromPath(path);
        const base64Image = await this.convertImageToBase64(imageBlob);

        if (this.profilePhoto == null)
          this.profilePhoto = base64Image

      } catch (error) {
        console.error(error);
      }
    },

    setupUserInfo() {
      this.userId = localStorage.getItem('userId') || '0';
      console.log("setup User info")
      this.loadImageAndConvertToBase64("./src/assets/blank_account.jpg")

      if (this.userId != 0) {
        this.fetchGetUserById(this.userId)
      } 
    }
  },

  computed: {
    isDisabled() {
      if (this.name.length > 0 && this.password.length > 0)
        return false

      return true
    }
  },

  mounted() {
    this.setupUserInfo()
  }
}
</script>

<style scoped></style>
