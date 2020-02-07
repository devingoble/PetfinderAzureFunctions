export default interface SearchResult {
  animals: Animal[];
  pagination: Pagination;
}

export interface Animal {
  id: number;
  organization_id: string;
  url: string;
  type: string;
  species: string;
  breeds: Breeds;
  colors: Colors;
  age: string;
  gender: string;
  size: string;
  coat: null;
  attributes: Attributes;
  environment: Environment;
  tags: string[];
  name: string;
  description: string;
  photos: Photo[];
  status: string;
  status_changed_at: string;
  published_at: string;
  distance: null;
  contact: Contact;
  _links: AnimalLinks;
}

export interface AnimalLinks {
  self: string;
  type: string;
  organization: string;
}

export interface Attributes {
  spayed_neutered: boolean;
  house_trained: boolean;
  declawed: boolean | null;
  special_needs: boolean;
  shots_current: boolean;
}

export interface Breeds {
  primary: string;
  secondary: null | string;
  mixed: boolean;
  unknown: boolean;
}

export interface Colors {
  primary: string;
  secondary: string;
  tertiary: string;
}

export interface Contact {
  email: string;
  phone: string;
  address: Address;
}

export interface Address {
  address1: string;
  address2: string;
  city: string;
  state: string;
  postcode: string;
  country: string;
}

export interface Environment {
  children: boolean | null;
  dogs: boolean | null;
  cats: boolean | null;
}

export interface Photo {
  small: string;
  medium: string;
  large: string;
  full: string;
  [key: string]: string;
}

export interface Pagination {
  count_per_page: number;
  total_count: number;
  current_page: number;
  total_pages: number;
  _links: PaginationLinks;
}

export interface PaginationLinks {
  next: string;
}
