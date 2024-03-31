import { Component } from '@angular/core';
import {MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";

@Component({
  selector: 'dialog-content-example-dialog',
  templateUrl: 'delete-ingredient-dialog.component.html',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
})
export class DeleteIngredientDialogComponent {
}
