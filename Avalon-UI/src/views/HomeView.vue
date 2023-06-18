<template>
  <v-dialog v-model="isModalOpen" width="auto">
    <template v-slot:activator="{ on }">
      <v-row class="d-flex justify-center pa-3">
        <v-container fluid>
          <v-row dense class="mb-2" no-gutters>
            <v-col v-for="item in photos" :key="item.id" :cols="3" class="pa-2">
              <v-card>
                <v-img :src="item.imageData" alt="Image" gradient="to bottom, rgba(0,0,0,.0), rgba(0,0,0,.3)"
                  class="align-end" height="300px" cover @click="openModal(item.imageData)">
                </v-img>

                <v-list-item class="w-100">
                  <template v-slot:prepend>
                    <v-card-text v-text="item.description">
                    </v-card-text>
                  </template>
                  <template v-slot:append>
                    <div class="justify-self-end">
                      <v-col>
                        <v-card-subtitle class="text-orange" v-text="item.hashtags">
                        </v-card-subtitle>
                        <v-card-subtitle class="text-orange" v-text="item.publicationDate">
                        </v-card-subtitle>
                      </v-col>
                    </div>
                  </template>
                </v-list-item>

                <v-card-actions>
                  <v-list-item class="w-100">
                    <template v-slot:prepend>
                      <v-avatar color="grey-darken-3" :image="item.authorProfilePhoto"></v-avatar>
                      <v-card-title v-text="item.authorName"></v-card-title>
                    </template>

                    <template v-slot:append>
                      <div class="justify-self-end">
                        <v-btn size="small" :color="item.isLike ? 'red' : 'surface-variant'" variant="text"
                          icon="mdi-heart" @click="toggleLike(item)"></v-btn>
                        <v-btn v-if="item.authorId == this.$root.userId || this.$root.userId == 1" size="small"
                          variant="text" icon="mdi-pencil" color="surface-variant" @click="selectEditPost(item)"></v-btn>
                        <v-btn size="small" color="surface-variant" variant="text" icon="mdi-download"
                          @click="downloadImage(item.imageData)"></v-btn>
                        <v-btn v-if="this.$root.userId == 1" size="small" variant="text" icon="mdi-delete" color="red"
                          @click="fetchDeletePhoto(item)"></v-btn>
                      </div>
                    </template>
                  </v-list-item>
                </v-card-actions>
              </v-card>
            </v-col>
          </v-row>
        </v-container>
      </v-row>
    </template>

    <v-card>
      <!-- Contenu de la modal -->
      <v-img :src="selectedImage" alt="Image" width="100%" />
    </v-card>
  </v-dialog>

  <v-dialog v-model="isEditPostOpen">
    <v-card>
      <v-card-title>Edit photo</v-card-title>
      <v-card-text>
        <v-form ref="form">
          <v-container>
            <v-row>
              <v-col cols="12" sm="6" md="6">
                <v-text-field label="Description" v-model="editedPost.description"></v-text-field>
              </v-col>
              <v-col cols="12" md="6">
                <v-text-field label="Hashtag" v-model="editedPost.hashtags"></v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="red" variant="text" @click="isEditPostOpen = false">
          Cancel
        </v-btn>
        <v-btn color="green" variant="text" @click="updatePhoto">
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "home",

  data() {
    return {
      photos: [],
      userResult: [],
      result: [],
      editedPost: null,

      isModalOpen: false,
      isEditPostOpen: false,
      selectedImage: '',

      rules: [
        value => {
          if (value) return true

          return 'Field cannot be empty.'
        },
      ],
    };
  },

  methods: {
    selectEditPost(item) {
      console.log(item)
      this.editedPost = item
      this.isEditPostOpen = true
    },

    updatePhoto() {
      this.isEditPostOpen = false
      const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          id: this.editedPost.id,
          authorId: this.editedPost.authorId,
          publicationDate: Date.now.toString(),
          hashtags: this.editedPost.hashtags,
          description: this.editedPost.description,
          imageData: this.editedPost.imageData
        })
      };
      fetch(`http://localhost:9001/api/PhotoItems/${this.editedPost.id}`, requestOptions)
        .then(response => {
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    fetchDeletePhoto(item) {
      const requestOptions = {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
      };
      fetch(`http://localhost:9001/api/PhotoItems/${item.id}`, requestOptions)
        .then(response => {
          location.reload()
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    fetchAPIData() {
      const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
      };
      fetch('http://localhost:9001/api/PhotoItems', requestOptions)
        .then(async response => {
          const data = await response.json();

          if (!response.ok) {
            const error = (data && data.message) || response.status;
            return Promise.reject(error);
          }
          else {
            // Parcourez chaque élément du tableau d'origine
            data.forEach(async item => {

              this.fetchGetUserById(item.authorId)

              const modifiedItem = {
                ...item,
                authorName: "loading",
                authorProfilePhoto: '',
                imageData: "data:image/png;base64," + item.imageData,
                isLike: false
              };

              // Ajoutez l'élément modifié au nouveau tableau
              this.photos.push(modifiedItem);
            });
          }
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    async fetchGetUserById(userId) {
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
            this.userResult.push(data);
            this.$forceUpdate();
          }
        })
        .catch(error => {
          this.errorMessage = error;
          console.error('There was an error!', error);
        });
    },

    toggleLike(item) {
      item.isLike = !item.isLike;
    },

    downloadImage(imageData) {
      const link = document.createElement("a");
      link.href = imageData;
      link.download = "image.png";
      link.target = "_blank";
      link.click();
    },

    openModal(imageData) {
      this.selectedImage = imageData;
      this.isModalOpen = true;
    }
  },

  mounted() {
  },

  created() {
    this.fetchAPIData();
  },

  beforeUpdate() {
    this.photos.forEach(photo => {
      var matchingUser = this.userResult.find(user => user.id === photo.authorId);
      if (matchingUser) {
        photo.authorName = matchingUser.name;
        photo.authorProfilePhoto = matchingUser.profilePhoto;
      }
    });
  },
}
</script>
