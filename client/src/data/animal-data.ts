import axios from "axios";
import API from "./config";
import { AnimalTypes, AnimalType } from "./animal-types";

export default class AnimalData {
  public async getFilterData() {
    let filterData = await axios.get<AnimalTypes>(`${API}/types`);

    return filterData.data.types;
  }
}
