import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {IngredientType} from "../models/IngredientType";
import {JsonPipe, NgIf} from "@angular/common";
import {CreateIngredientDto} from "../models/CreateIngredientDto";
import {IngredientService} from "../services/ingredient.service";

@Component({
  selector: 'app-add-ingredient',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, JsonPipe],
  templateUrl: './add-ingredient.component.html',
  styleUrl: './add-ingredient.component.scss'
})
export class AddIngredientComponent implements OnInit{

  ingredient: CreateIngredientDto = {
    name: "",
    price: 0,
    isHealthy: true,
    ingredientType: IngredientType.Base
  }

  addIngredientForm: FormGroup = new FormGroup({});

  constructor(private service: IngredientService) {
  }

  ngOnInit(): void {
    this.addIngredientForm = new FormGroup({
      name: new FormControl('', Validators.required),
      price: new FormControl('', [Validators.required, Validators.min(0), Validators.max(1000)]),
      isHealthy: new FormControl("true"),
      ingredientType: new FormControl(IngredientType.Base)
    });
  }

  onSubmit() {
    if (this.addIngredientForm.valid) {
      console.log(typeof this.addIngredientForm.value.isHealthy);

      let isHealthy = this.addIngredientForm.value.isHealthy === "true";
      let ingredientType: number = parseInt(this.addIngredientForm.value.ingredientType);


      this.ingredient = {
        name: this.addIngredientForm.value.name,
        price: this.addIngredientForm.value.price,
        isHealthy: isHealthy,
        ingredientType: ingredientType
      }

      this.service.addIngredient(this.ingredient);

      this.addIngredientForm.reset();
    }
  }
}
