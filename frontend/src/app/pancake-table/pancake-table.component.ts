import {Component, OnInit} from '@angular/core';
import {PancakeService} from "../services/pancake.service";
import {Pancake} from "../models/Pancake";
import {DecimalPipe, NgForOf} from "@angular/common";
import {IngredientType} from "../models/IngredientType";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {DeleteIngredientDialogComponent} from "../delete-ingredient-dialog/delete-ingredient-dialog.component";

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

  constructor(service: PancakeService, private router: Router, private dialog: MatDialog) {
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

  goToPancakesAdd() {
    this.router.navigate(["/addPancake"]);
  }

  deletePancake(id: number) {

    const dialogRef = this.dialog.open(DeleteIngredientDialogComponent, {
      autoFocus: true,
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.service.deletePancake(id);
        this.pancakes = this.pancakes.filter(pancake => pancake.id !== id);
      }
    });
  }
}
