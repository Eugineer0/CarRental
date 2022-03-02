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
  public filter: FilterRequest = {
    country: undefined,
    city: undefined,
    minimumAvailableCarsNumber: undefined,
    startRent: undefined,
    finishRent: undefined
  }

  public startRentTime: Time = {
    hours: 0,
    minutes: 0
  }
  public finishRentTime: Time = {
    hours: 23,
    minutes: 59
  }

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
            if(!this.countries.includes(country)) {
              this.countries.push(country);
            }
          }
        }
      )
  }

  onSubmit() {
    this.filter.startRent?.setMinutes(this.startRentTime.minutes);
    this.filter.startRent?.setHours(this.startRentTime.hours);

    this.filter.finishRent?.setMinutes(this.finishRentTime.minutes);
    this.filter.finishRent?.setHours(this.finishRentTime.hours);

    this.rentalCenterService.getFilteredRentalCenters(this.filter)
      .subscribe(
        centers => {
          this.centers = centers;
        }
      );
  }
}
