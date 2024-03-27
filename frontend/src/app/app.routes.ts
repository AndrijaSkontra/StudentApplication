import { Routes } from '@angular/router';
import {IngredientsTableComponent} from "./ingredients-table/ingredients-table.component";
import {PancakeTableComponent} from "./pancake-table/pancake-table.component";

export const routes: Routes = [
  {path: "ingredients", component: IngredientsTableComponent},
  {path: "", pathMatch: "full", redirectTo: "pancakes"},
  {path: "pancakes", component: PancakeTableComponent}
];
