import {Component, OnInit} from '@angular/core';
import {Ingredient} from "../models/Ingredient";
import {IngredientService} from "../services/ingredient.service";
import {DecimalPipe, NgForOf, NgIf} from "@angular/common";
import {IngredientType} from "../models/IngredientType";
import {routes} from "../app.routes";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DeleteIngredientDialogComponent} from "../delete-ingredient-dialog/delete-ingredient-dialog.component";

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

  constructor(private service: IngredientService, private router: Router, public dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.service.getIngredients().subscribe(ingredients => {
      this.ingredients = ingredients;
    });
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

  deleteIngredient(id: number) {
    // this.service.deleteIngredient(id).subscribe(() => {
    //   this.ingredients = this.ingredients.filter(ingredient => ingredient.id !== id);
    // });

    const dialogRef = this.dialog.open(DeleteIngredientDialogComponent, {
      autoFocus: true,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.service.deleteIngredient(id).subscribe(() => {
          this.ingredients = this.ingredients.filter(ingredient => ingredient.id !== id);
        });
      }
    });
  }

  protected readonly routes = routes;
}
