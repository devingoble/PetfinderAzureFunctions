<template v-slot="{ hover }">
  <div class="white justified">
    <v-img
      v-if="isCompact"
      position="top"
      :lazy-src="getLazyPhoto"
      :src="getPhoto"
      max-height="300px"
      min-height="300px"
    ></v-img>
    <PhotoGallery v-else :photos="animal.photos" />
    <div>
      <v-card-title>{{ animal.name }}</v-card-title>
      <v-card-text>
        <v-tooltip top>
          <template v-slot:activator="{ on }">
            <div v-on="on" class="text-truncate">{{ buildBreeds }}</div>
          </template>
          <span>{{ buildBreeds }}</span>
        </v-tooltip>
        <div>{{ getDescription }}</div>
        <div v-if="animal.gender !== 'Unknown'">{{ getSpayNeuter }}</div>
      </v-card-text>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Watch, Vue } from "vue-property-decorator";
import { Animal } from "@/data/search-result-types";
import AnimalData from "@/data/animal-data";
import PhotoGallery from "@/components/photo-gallery.vue";

@Component({ components: { PhotoGallery } })
export default class PetDetail extends Vue {
  @Prop()
  animal?: Animal;
  @Prop()
  isCompact?: boolean;
  localAnimal?: Animal;

  get getPhoto() {
    if (!this.animal) return this.getLazyPhoto();

    if (this.animal.photos.length > 0) {
      if (this.isCompact === undefined) {
        return this.animal.photos[0]["medium"];
      }

      if (this.isCompact === true) {
        return this.animal.photos[0]["medium"];
      } else {
        return this.animal.photos[0]["large"];
      }
    }

    return "";
  }

  get getLazyPhoto() {
    return require("@/assets/300.png");
  }

  get buildBreeds() {
    if (!this.animal) return "";
    if (this.animal.breeds.unknown) {
      return "unknown";
    }

    let breed = this.animal.breeds.primary;
    breed += this.animal.breeds.secondary
      ? ", " + this.animal.breeds.secondary
      : "";
    breed += this.animal.breeds.mixed ? ", mixed" : "";

    return breed;
  }

  get getSpayNeuter() {
    if (!this.animal) return "";
    let status = "";
    if (
      this.animal.gender === "Male" &&
      this.animal.attributes.spayed_neutered
    ) {
      return "Neutered";
    } else if (
      this.animal.gender === "Female" &&
      this.animal.attributes.spayed_neutered
    ) {
      return "Spayed";
    } else {
      return "";
    }
  }

  get getDescription() {
    if (!this.animal) return "";
    let desc = this.animal.age;

    if (this.animal.gender) {
      desc += ", " + this.animal.gender;
    }

    if (this.animal.size) {
      desc += ", " + this.animal.size + " size";
    }

    return desc;
  }
}
</script>

<style lang="scss" scoped>
.justified {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100%;
}
</style>
