import axios from "axios";
import API from "./config";
import { AnimalTypes } from "./animal-types";
import SearchParameters from "./search-parameters";
import SearchResult from "./search-result-types";

export default class AnimalData {
  public async getFilterData() {
    let filterData = await axios.get<AnimalTypes>(`${API}/types`);

    return filterData.data.types;
  }

  public async getSearchedAnimals(params: SearchParameters) {
    let query = new URLSearchParams();
    for (const property in params) {
      if (params[property]) {
        query.append(`${property}`, `${params[property]}`);
      }
    }

    let animals = await axios.get(`${API}/animals?${query.toString()}`);

    return animals.data as SearchResult;
  }
}
