export interface AnimalTypes {
    types: AnimalType[];
}

export interface AnimalType {
    name:    string;
    coats:   string[];
    colors:  string[];
    genders: Gender[];
    _links:  Links;
}

export interface Links {
    self:   Breeds;
    breeds: Breeds;
}

export interface Breeds {
    href: string;
}

export enum Gender {
    Female = "Female",
    Male = "Male",
    Unknown = "Unknown",
}
