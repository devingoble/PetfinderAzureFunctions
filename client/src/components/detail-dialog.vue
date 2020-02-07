<template>
  
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Animal } from "@/data/search-result-types";

@Component
export default class DetailDialog extends Vue {
  @Prop()
  animal?: Animal;
}
</script>

<style lang="scss" scoped>
</style>