import {Component, OnInit} from '@angular/core';
import {Ingredient} from "../models/Ingredient";
import {IngredientService} from "../services/ingredient.service";
import {DecimalPipe, NgForOf} from "@angular/common";

@Component({
  selector: 'app-ingredients-table',
  standalone: true,
    imports: [
        DecimalPipe,
        NgForOf
    ],
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
