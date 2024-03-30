import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Ingredient} from "../models/Ingredient";
import {CreateIngredientDto} from "../models/CreateIngredientDto";

@Injectable({
  providedIn: 'root'
})
export class IngredientService {

  constructor(private http: HttpClient) { }

  getIngredients(): Observable<Ingredient[]> {
    return this.http.get<Ingredient[]>('http://localhost:5013/api/Ingredients');
  }

  addIngredient(ingredient: CreateIngredientDto) {
    console.log("Adding ingredient 123");
    this.http.post<Ingredient>('http://localhost:5013/api/Ingredients', ingredient).subscribe();
  }
}
