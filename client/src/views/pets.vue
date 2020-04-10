<template>
  <div class="ma-2 ma-md-12">
    <PetGrid v-if="animalData" :animals="animalData.animals" />
    <v-pagination
      class="mt-8"
      v-if="animalData && animalData.pagination.total_pages > 1"
      v-model="currentPage"
      :length="this.animalData.pagination.total_pages"
      :total-visible="7"
    ></v-pagination>
    <v-overlay :value="isLoading">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { AnimalType } from "@/data/animal-types";
import SearchParameters from "@/data/search-parameters";
import AnimalData from "@/data/animal-data";
import SearchResult from "../data/search-result-types";
import PetGrid from "@/components/pet-grid.vue";

@Component({ components: { PetGrid } })
export default class Pets extends Vue {
  animalDataService: AnimalData = new AnimalData();
  animalData: SearchResult | null = null;
  currentPage: number = 1;
  isLoading: boolean = false;

  async created() {
    this.loadData();
  }

  async loadData() {
    this.isLoading = true;

    this.animalData = await this.animalDataService.getSearchedAnimals(
      this.$route.query
    );

    this.isLoading = false;
  }

  @Watch("currentPage")
  onCurrentPageChanged(value: number, oldvalue: number) {
    this.$router.push({
      name: "pets",
      query: {
        organization: this.$route.query["organization"],
        type: this.$route.query["type"],
        page: this.currentPage.toString(),
      },
    });
  }

  @Watch("$route")
  onRouteChange() {
    this.loadData();
  }
}
</script>
<style scoped></style>
