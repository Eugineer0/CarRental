import { CarService } from "./car-service";

export interface Car {
  registrationNumber: string;
  brand: string;
  model: string;
  seatPlaces: number;
  averageConsumption: number;
  gearboxType: GearBoxTypes;
  weight: number;
  length: number;
  power: number;
  pricePerMinute: number;
  pricePerHour: number;
  pricePerDay: number;
  services: CarService[];
}

export enum GearBoxTypes {
  Mechanical,
  Automatic,
  Robotic
}
