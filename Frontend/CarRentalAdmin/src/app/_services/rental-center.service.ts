import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";

import { Observable } from "rxjs";

import { RentalCenter } from "../_models/center/rental-center";
import { Car } from "../_models/car/car";

@Injectable({
  providedIn: 'root'
})
export class RentalCenterService {
  private rentalCentersUrl: string = 'api/rental-centers';

  constructor(
    private http: HttpClient
  ) {
  }

  public getRentalCenters(): Observable<RentalCenter[]> {
    return this.http.get<RentalCenter[]>(this.rentalCentersUrl);
  }

  public getCenterAccessibleCars(name: string, startRent: Date, finishRent: Date): Observable<ArrayBuffer> {
    return this.http.get<Car[]>(
      `${this.rentalCentersUrl}/${name}`,
      {queryParams: {start: startRent}, {finish: finishRent}}
    );
  }
}
