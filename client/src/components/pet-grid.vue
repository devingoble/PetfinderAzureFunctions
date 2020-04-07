<template>
  <div class="d-flex flex-wrap justify-center">
    <v-hover v-for="animal in animals" :key="animal.id">
      <template v-slot="{ hover }">
        <v-card
          class="pa-2 ma-2 flex-grow-1 flex-md-grow-0"
          height="460px"
          width="300px"
          :elevation="hover ? 24 : 6"
          @click.stop="showDetail(animal)"
        >
          <PetDetail :animal="animal" :is-compact="true" />
        </v-card>
      </template>
    </v-hover>
    <v-dialog v-model="dialog" max-width="600px">
      <PetDetail :animal="selectedAnimal" :is-compact="false" class="pa-2" />
    </v-dialog>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Animal } from "@/data/search-result-types";
import PetDetail from "@/components/pet-detail.vue";

@Component({ components: { PetDetail } })
export default class PetGrid extends Vue {
  @Prop()
  animals?: Animal[];
  dialog: boolean = false;
  selectedAnimal?: Animal | null = null;

  showDetail(animal: Animal) {
    this.dialog = true;
    this.selectedAnimal = animal;
  }
}
</script>

<style lang="scss" scoped></style>
