<template>
  <div class="ma-2 ma-md-12">
    <PetGrid v-if="animalData" :animals="animalData.animals" />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { AnimalType } from "@/data/animal-types";
import SearchParameters from "@/data/search-parameters";
import AnimalData from "@/data/animal-data";
import SearchResult from "../data/search-result-types";
import PetGrid from "@/components/pet-grid.vue";

@Component({ components: { PetGrid } })
export default class Pets extends Vue {
  animalDataService: AnimalData = new AnimalData();
  animalData: SearchResult | null = null;
  async created() {
    this.animalData = await this.animalDataService.getSearchedAnimals(
      this.$route.query
    );
  }
}
</script>
<style scoped></style>
