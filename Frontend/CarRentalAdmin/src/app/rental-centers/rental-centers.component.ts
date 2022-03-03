import { Component, OnInit } from '@angular/core';

import { RentalCenterService } from "../_services/rental-center.service";

import { RentalCenter } from "../_models/center/rental-center";
import { FilterRequest } from "../_models/center/filter-request";
import { Time } from "@angular/common";

@Component({
  selector: 'app-rental-centers',
  templateUrl: './rental-centers.component.html',
  styleUrls: ['./rental-centers.component.css']
})
export class RentalCentersComponent implements OnInit {
  public centers: RentalCenter[] = [];
  public cities: string[] = [];
  public countries: string[] = [];
  public filter: FilterRequest = this.initFilter();
  public finishRentDate: Date | undefined = undefined;
  public startRentDate: Date | undefined = undefined;
  public startRentTime: Time | undefined = undefined;
  public finishRentTime: Time | undefined = undefined;

  constructor(
    private rentalCenterService: RentalCenterService,
  ) {
  }

  public ngOnInit(): void {
    this.getRentalCenters();
  }

  private getRentalCenters(): void {
    this.rentalCenterService.getRentalCenters()
      .subscribe(
        centers => {
          this.centers = centers;
          this.cities = centers.map(center => center.city);
          for (let country of centers.map(center => center.country)) {
            if (!this.countries.includes(country)) {
              this.countries.push(country);
            }
          }
        }
      )
  }

  onSubmit() {
    this.processDates();
    this.rentalCenterService.getFilteredRentalCenters(this.filter)
      .subscribe(
        centers => {
          this.centers = centers;
        }
      );
  }

  private processDates(): void {
    this.filter.startRent = new Date(this.startRentDate + ' ' + this.startRentTime);
    this.filter.finishRent = new Date(this.finishRentDate + ' ' + this.finishRentTime);
  }

  public initFilter(): FilterRequest {
    return {
      country: undefined,
      city: undefined,
      minimumAvailableCarsNumber: undefined,
      startRent: undefined,
      finishRent: undefined
    }
  }

  public resetFilter(): void {
    this.filter = this.initFilter();
  }
}
