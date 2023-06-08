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
                  <v-card-title class="text-white" v-text="item.hashtags"></v-card-title>
                </v-img>
                <v-card-title v-text="item.description">
                </v-card-title>

                <v-card-subtitle class="text-orange" v-text="item.publicationDate">
                </v-card-subtitle>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn size="small" :color="item.isLike ? 'red' : 'surface-variant'" variant="text" icon="mdi-heart"
                    @click="toggleLike(item)"></v-btn>
                  <v-btn size="small" color="surface-variant" variant="text" icon="mdi-download"
                    @click="downloadImage(item.imageData)"></v-btn>
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
</template>

<script>
export default {
  name: "home",

  data() {
    return {
      photos: [],

      isModalOpen: false,
      selectedImage: ''
    };
  },

  methods: {
    fetchAPIData() {
      const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
      };
      fetch('http://localhost:5048/api/PhotoItems', requestOptions)
        .then(async response => {
          const data = await response.json();

          if (!response.ok) {
            const error = (data && data.message) || response.status;
            return Promise.reject(error);
          }
          else {
            data.forEach(item => {
              this.photos.push()
            });

            // Parcourez chaque élément du tableau d'origine
            data.forEach(item => {
              // Ajoutez "BONJOUR" au début de la valeur de la description
              const modifiedItem = {
                ...item,
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
    this.fetchAPIData();
  }
}
</script>
