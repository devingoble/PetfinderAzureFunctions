<template>
  <div>
    <div>Hello Pets!</div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { AnimalType } from "@/data/animal-types";
import SearchParameters from "@/data/search-parameters";

@Component
export default class Pets extends Vue {
  searchParameters: SearchParameters = {};
}
</script>
<style scoped></style>
