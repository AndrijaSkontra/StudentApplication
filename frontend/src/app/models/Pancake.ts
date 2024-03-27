import {Ingredient} from "./Ingredient";

export interface Pancake {
  id: number;
  price: number;
  ingredients: Ingredient[];
}
