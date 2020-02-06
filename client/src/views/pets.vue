<template>
  <div>
    <div>Hello Pets!</div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { AnimalType } from "@/data/animal-types";
import SearchParameters from "@/data/search-parameters";
import AnimalData from "@/data/animal-data";

@Component
export default class Pets extends Vue {
  searchParameters: SearchParameters = new SearchParameters();
  animalData: AnimalData = new AnimalData();
  data: any;
  async created() {
    this.searchParameters.organization =
      (this.$route.query.organization as string) ?? "";

    this.data = await this.animalData.getSearchedAnimals(this.searchParameters);
  }
}
</script>
<style scoped></style>
