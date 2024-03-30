import {Component, OnInit} from '@angular/core';
import {Ingredient} from "../models/Ingredient";
import {IngredientService} from "../services/ingredient.service";
import {DecimalPipe, NgForOf, NgIf} from "@angular/common";
import {IngredientType} from "../models/IngredientType";
import {routes} from "../app.routes";
import {Router} from "@angular/router";

@Component({
  selector: 'app-ingredients-table',
  standalone: true,
  imports: [
    DecimalPipe,
    NgForOf,
    NgIf
  ],
  templateUrl: './ingredients-table.component.html',
  styleUrl: './ingredients-table.component.scss'
})
export class IngredientsTableComponent implements OnInit{

  ingredients: Ingredient[] = [];

  constructor(private service: IngredientService, private router: Router) {
  }

  ngOnInit(): void {
    this.service.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients;
    })
  }

  hideIngredients() {
    this.ingredients = [];
  }

  getIngredientType(type: number): string {
    return IngredientType[type];
  }

  goToIngredientAdd() {
    this.router.navigate(['/addIngredient']);
  }

  protected readonly routes = routes;
}
