import { Car } from '../car/car';
import { CarService } from '../car/car-service';
import { RentalCenter } from '../center/rental-center';

export interface Order {
    overallPrice: number;
    startRent: Date;
    finishRent: Date;
    orderCarServices: CarService[];
    car: Car;
    rentalCenter: RentalCenter;
}