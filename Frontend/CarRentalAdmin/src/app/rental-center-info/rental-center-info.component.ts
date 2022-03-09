import { Component, OnInit } from '@angular/core';
import { Location, Time } from "@angular/common";
import { ActivatedRoute } from "@angular/router";

import { RentalCenterService } from "../_services/rental-center.service";
import { RentalCenter } from "../_models/center/rental-center";
import { Car, GearBoxTypes } from "../_models/car/car";

@Component({
  selector: 'app-rental-center-info',
  templateUrl: './rental-center-info.component.html',
  styleUrls: ['./rental-center-info.component.css']
})
export class RentalCenterInfoComponent implements OnInit {
  public cars: Car[] = [];
  public center: RentalCenter | undefined;

  public centerName: string | null = null;

  public finishRentDate: Date | undefined = undefined;
  public startRentDate: Date | undefined = undefined;
  public startRentTime: Time | undefined = undefined;
  public finishRentTime: Time | undefined = undefined;
  private filter: any;


  constructor(
    private location: Location,
    private rentalCenterService: RentalCenterService,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit(): void {
    this.centerName = this.route.snapshot.paramMap.get('name');
    this.getCenter()
    this.getCars();
  }

  goBack(): void {
    this.location.back();
  }

  private processDates(): void {
    this.filter.startRent = new Date(this.startRentDate + ' ' + this.startRentTime);
    this.filter.finishRent = new Date(this.finishRentDate + ' ' + this.finishRentTime);
  }

  public dateFillInRequired(): boolean {
    return !!(this.startRentDate
      || this.startRentTime
      || this.finishRentDate
      || this.finishRentTime);
  }

  public dateIsSpecified(): boolean {
    return !!(this.startRentDate
      && this.startRentTime
      && this.finishRentDate
      && this.finishRentTime);
  }

  private getCenter(): void {
    if (this.centerName) {
      this.rentalCenterService.getRentalCenter(this.centerName)
        .subscribe(center => {
            this.center = center;
          }
        );
    }
  }

  private getCars(): void {
    if (this.centerName) {
      this.rentalCenterService.getRentalCenterCars(this.centerName)
        .subscribe(cars => {
            this.cars = cars;
          }
        );
    }
  }

  onSubmit() {

  }

  showGearboxType(car: Car) {
    return GearBoxTypes[car.gearboxType];
  }
}
