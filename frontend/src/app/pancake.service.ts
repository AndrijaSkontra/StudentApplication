import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Pancake} from "./models/Pancake";

@Injectable({
  providedIn: 'root'
})
export class PancakeService {

  constructor(private http: HttpClient) { }

  public getPancakes(): Observable<Pancake[]> {
    return this.http.get<Pancake[]>('http://localhost:5013/api/Pancakes');
  }
}
