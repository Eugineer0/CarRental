import { Component, OnInit } from '@angular/core';

import { RentalCenterService } from "../_services/rental-center.service";

import { RentalCenter } from "../_models/center/rental-center";

@Component({
  selector: 'app-rental-centers',
  templateUrl: './rental-centers.component.html',
  styleUrls: ['./rental-centers.component.css']
})
export class RentalCentersComponent implements OnInit {
  public centers: RentalCenter[] = [];
  public cities: string[] = [];
  public countries: string[] = [];

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

  }
}
