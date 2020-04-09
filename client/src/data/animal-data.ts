import axios from "axios";
import API from "./config";
import { AnimalTypes } from "./animal-types";
import SearchResult, { Animal } from "./search-result-types";

type Dictionary<T> = { [key: string]: T };

export default class AnimalData {
  public async getFilterData() {
    let filterData = await axios.get<AnimalTypes>(`${API}/types`);

    return filterData.data.types;
  }

  public async getSearchedAnimals(
    params: Dictionary<string | (string | null)[]>
  ) {
    let query = new URLSearchParams();
    for (const property in params) {
      if (params[property]) {
        query.append(`${property}`, `${params[property]}`);
      }
    }

    let animals = await axios.get(
      `${API}/animals?${query.toString()}/&limit=100`
    );

    return animals.data as SearchResult;
  }

  public async getAnimal(animalId: number) {
    let animal = await axios.get(`${API}/animals/${animalId}`);

    return animal.data as Animal;
  }
}
