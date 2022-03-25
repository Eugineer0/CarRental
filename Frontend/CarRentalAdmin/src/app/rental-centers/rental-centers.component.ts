import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { RentalCenterService } from "../_services/rental-center.service";

import { RentalCenter } from "../_models/center/rental-center";
import { RentalCenterFilter } from "../_models/center/rental-center-filter";
import { dateConsistencyValidator } from '../date-consistency.directive';
import { RentPeriodFilter } from "../_models/center/rent-period-filter";

@Component({
  selector: 'app-rental-centers',
  templateUrl: './rental-centers.component.html',
  styleUrls: ['./rental-centers.component.css']
})
export class RentalCentersComponent implements OnInit {
  public dateForm: FormGroup = new FormGroup({
      startDate: new FormControl(undefined),
      startTime: new FormControl(undefined),
      finishDate: new FormControl(undefined),
      finishTime: new FormControl(undefined)
    },
    { validators: dateConsistencyValidator }
  );

  public filterForm: FormGroup = new FormGroup({
      country: new FormControl(undefined),
      city: new FormControl(undefined),
      minimumCarsAvailable: new FormControl(0)
    }
  )

  public centers: RentalCenter[] = [];
  public cities: string[] = [];
  public countries: string[] = [];
  public maxAvailableCarsNumber: number = 0;

  constructor(
    private rentalCenterService: RentalCenterService
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
    const filter : RentalCenterFilter = this.getFilter();
    console.log(filter);
    this.rentalCenterService.getFilteredRentalCenters(filter)
      .subscribe(
        centers => {
          this.centers = centers;
        }
      );
  }

  public resetFiltration(): void {
    this.filterForm.reset();
    this.dateForm.reset();

    this.getRentalCenters();
  }

  public getRentPeriodFilter(): RentPeriodFilter | undefined {
    if(this.dateForm.valid && this.dateForm.dirty) {
      return {
        startRent: new Date(this.dateForm.controls.startDate.value + ' ' + this.dateForm.controls.startTime.value),
        finishRent: new Date(this.dateForm.controls.finishDate.value + ' ' + this.dateForm.controls.finishTime.value)
      }
    }

    return undefined;
  }

  private getFilter(): RentalCenterFilter {
    return {
      country: this.filterForm.controls.country.value,
      city: this.filterForm.controls.city.value,
      minimumCarsAvailable: this.filterForm.controls.minimumCarsAvailable.value,
      rentPeriodFilter: this.getRentPeriodFilter()
    };
  }

  nothing() {

  }
}
