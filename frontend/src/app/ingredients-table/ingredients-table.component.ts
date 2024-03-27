import {Component, OnInit} from '@angular/core';
import {Ingredient} from "../models/Ingredient";
import {IngredientService} from "../services/ingredient.service";

@Component({
  selector: 'app-ingredients-table',
  standalone: true,
  imports: [],
  templateUrl: './ingredients-table.component.html',
  styleUrl: './ingredients-table.component.scss'
})
export class IngredientsTableComponent implements OnInit{

  ingredients: Ingredient[] = [];

  constructor(private service: IngredientService) {
  }

  ngOnInit(): void {
    this.service.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients;
    })
  }

}
