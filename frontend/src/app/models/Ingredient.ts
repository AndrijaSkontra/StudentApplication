import {Pancake} from "./Pancake";
import {IngredientType} from "./IngredientType";

export interface Ingredient {

  id: number;
  ingredientType: IngredientType;
  isHealthy: boolean;
  name: string;
  price: number;
  pancakes: Pancake[];
}

