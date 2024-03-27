import {Component, OnInit} from '@angular/core';
import {PancakeService} from "../pancake.service";
import {Pancake} from "../models/Pancake";
import {DecimalPipe, NgForOf} from "@angular/common";
import {IngredientType} from "../models/IngredientType";

@Component({
  selector: 'app-pancake-table',
  standalone: true,
  imports: [
    NgForOf,
    DecimalPipe
  ],
  templateUrl: './pancake-table.component.html',
  styleUrl: './pancake-table.component.scss'
})
export class PancakeTableComponent implements OnInit{

  service: PancakeService;
  pancakes: Pancake[] = [];

  constructor(service: PancakeService) {
    this.service = service;
  }

  ngOnInit(): void {
    this.service.getPancakes().subscribe(pancakes => {
      console.log(pancakes);
      this.pancakes = pancakes;
    });
  }

  hidePancakes() {
    this.pancakes = [];
  }

  getIngredientType(type: number): string {
  return IngredientType[type];
  }

}
