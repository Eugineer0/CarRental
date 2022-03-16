import { Component, Input, OnInit } from '@angular/core';

import { Car, GearBoxTypes } from "../_models/car/car";

@Component({
  selector: 'app-car',
  templateUrl: './car.component.html',
  styleUrls: ['./car.component.css']
})
export class CarComponent implements OnInit {
  @Input() car: Car = {
    registrationNumber: '',
    brand: '',
    model: '',
    seatPlaces: 0,
    averageConsumption: 0,
    gearboxType: 0,
    weight: 0,
    length: 0,
    power: 0,
    pricePerMinute: 0,
    pricePerHour: 0,
    pricePerDay: 0,
    availableServices: []
  }


  constructor() { }

  public ngOnInit(): void {
  }

  public showGearboxType(car: Car): string {
    return GearBoxTypes[car.gearboxType];
  }
}
