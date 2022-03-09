import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";

import { Observable } from "rxjs";

import { RentalCenter } from "../_models/center/rental-center";
import { FilterRequest } from "../_models/center/filter-request";
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

  public getRentalCenter(name: string): Observable<RentalCenter> {
    return this.http.get<RentalCenter>(`${ this.rentalCentersUrl }/${ name }`);
  }

  public getRentalCenters(): Observable<RentalCenter[]> {
    return this.http.get<RentalCenter[]>(this.rentalCentersUrl);
  }

  public getFilteredRentalCenters(filter: FilterRequest): Observable<RentalCenter[]> {
    return this.http.post<RentalCenter[]>(
      `${ this.rentalCentersUrl }/filtered`,
      filter
    );
  }

  public getRentalCenterCars(centerName: string): Observable<Car[]> {
    return this.http.get<Car[]>(
      `${ this.rentalCentersUrl }/${ centerName }/cars`,
    );
  }
}
