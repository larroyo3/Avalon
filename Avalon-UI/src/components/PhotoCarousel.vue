<template>
    <div class="ma-3">
        <v-row class="d-flex justify-center pa-3">
            <v-container fluid>
                <v-row dense class="mb-2" no-gutters>
                    <v-col v-for="item in photos" :key="item.id" :cols="3" class="pa-2">
                        <v-card>
                            <v-img :src="item.imageData" alt="Image" gradient="to bottom, rgba(0,0,0,.0), rgba(0,0,0,.3)"
                                class="align-end" height="300px" cover>
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
                                            <v-btn v-if="this.$root.userId == 1" size="small" variant="text"
                                                icon="mdi-delete" color="red" @click="fetchDeletePhoto(item)"></v-btn>
                                        </div>
                                    </template>
                                </v-list-item>
                            </v-card-actions>
                        </v-card>
                    </v-col>
                </v-row>
            </v-container>
        </v-row>
        <v-btn size="small" variant="text" icon="mdi-delete" color="red" @click="test"></v-btn>
    </div>
</template>

<script>

export default {
    name: "photoCarousel",

    data() {
        return {
            photos: [],
        };
    },

    methods: {
        handleEvent(data) {
            // Gérez l'événement personnalisé
            console.log('Événement personnalisé reçu:', data);
            this.photos.push(data)
        },

        test() {
            console.log("log :")
            console.log(this.photos)
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
    },

    created() {
    },

    destroyed() {
    },

}
</script>