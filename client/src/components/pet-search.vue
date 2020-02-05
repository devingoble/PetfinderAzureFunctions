<template>
  <div>search</div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { AnimalType } from '@/data/animal-types';
import AnimalData from '@/data/animal-data';

@Component( { name: 'pet-search'})
export default class PetSearch extends Vue {
  private animalTypes: AnimalType[] = [];
  private animalData = new AnimalData();

  async created(){
    await this.loadAnimalTypes();
  }

  public async loadAnimalTypes() {
    this.animalTypes = await this.animalData.getFilterData();
  }
}
</script>
<style scoped></style>
