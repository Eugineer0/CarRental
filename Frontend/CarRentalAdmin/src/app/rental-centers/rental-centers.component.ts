import { Component, OnInit } from '@angular/core';

import { RentalCenterService } from "../_services/rental-center.service";

import { RentalCenter } from "../_models/center/rental-center";
import { RentalCenterFilter } from "../_models/center/rental-center-filter";

@Component({
  selector: 'app-rental-centers',
  templateUrl: './rental-centers.component.html',
  styleUrls: ['./rental-centers.component.css']
})
export class RentalCentersComponent implements OnInit {
  public centers: RentalCenter[] = [];
  public cities: string[] = [];
  public countries: string[] = [];
  public maxAvailableCarsNumber: number = 0;
  public filter: RentalCenterFilter = this.initFilter();

  public startRentDate: string | undefined = undefined;
  public finishRentDate: string | undefined = undefined;
  public startRentTime: string | undefined = undefined;
  public finishRentTime: string | undefined = undefined;

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
          this.maxAvailableCarsNumber = Math.max(...(centers.map(center => center.availableCarsNumber)));
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
    if (this.dateFillInRequired()) {
      this.setFilterDates();
    }
    this.rentalCenterService.getFilteredRentalCenters(this.filter)
      .subscribe(
        centers => {
          this.centers = centers;
        }
      );
  }

  private setFilterDates(): void {
    this.filter.startRent = new Date(this.startRentDate + ' ' + this.startRentTime);
    this.filter.finishRent = new Date(this.finishRentDate + ' ' + this.finishRentTime);
  }

  public dateFillInRequired(): boolean {
    return !!(this.startRentDate
      || this.startRentTime
      || this.finishRentDate
      || this.finishRentTime);
  }

  public initFilter(): RentalCenterFilter {
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
    this.finishRentDate = undefined;
    this.startRentDate = undefined;
    this.startRentTime = undefined;
    this.finishRentTime = undefined;

    this.getRentalCenters();
  }

  public getCurrentDate(): string {
    return new Date(Date.now()).toISOString().slice(0, 10);
  }

  public getCurrentDateD(): Date {
    return new Date(Date.now());
  }

  getMinTime() {
    if (this.startRentDate) {
      if (this.getCurrentDate() === this.startRentDate) {
        return new Date(Date.now()).toISOString().slice(11, 16);
      }
    }

    return '00-00';
  }
}
