<template>
  <div class="flex-column ma-2" v-if="photos">
    <v-img
      class="m-2"
      position="top"
      :lazy-src="getLazyPhoto"
      :src="getPhoto"
      max-height="400px"
      contain
    ></v-img>
    <v-sheet class="mt-2" v-if="photos.length > 1">
      <v-slide-group
        v-model="currentPhotoIndex"
        center-active
        show-arrows
        mandatory
      >
        <v-slide-item
          v-for="(photo, index) in photos"
          :key="photo.key"
          v-slot:default="{ active, toggle }"
          class="d-flex"
        >
          <v-card
            :color="active ? 'primary' : 'grey lighten-1'"
            class="space-thumbnails"
            @click="toggle"
          >
            <v-img
              :key="index"
              :src="photo['small']"
              class="align-stretch"
              height="100px"
              width="100px"
            ></v-img>
          </v-card>
        </v-slide-item>
      </v-slide-group>
    </v-sheet>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { Photo } from "@/data/search-result-types";

@Component
export default class PhotoGallery extends Vue {
  @Prop()
  photos?: Photo[];
  currentPhotoIndex: number = 0;

  @Watch("photos")
  onPhotosUpdate(value: Photo[], oldValue: Photo[]) {
    this.currentPhotoIndex = 0;
  }

  get getPhoto() {
    if (this.photos && this.photos.length > 0) {
      console.log(this.photos);
      return this.photos[this.currentPhotoIndex]["large"];
    }

    return this.getLazyPhoto();
  }

  get getLazyPhoto() {
    return require("@/assets/300.png");
  }
}
</script>

<style lang="scss" scoped>
.space-thumbnails + .space-thumbnails {
  margin-left: 10px;
}
</style>
