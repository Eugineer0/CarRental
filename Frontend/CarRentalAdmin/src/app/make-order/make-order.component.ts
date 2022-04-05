import { Component, OnInit } from '@angular/core';
import { Location } from "@angular/common";
import { ActivatedRoute } from "@angular/router";

import { RentalCenterService } from "../_services/rental-center.service";
import { Car } from "../_models/car/car";

@Component({
    selector: 'app-make-order',
    templateUrl: './make-order.component.html',
    styleUrls: ['./make-order.component.css']
})
export class MakeOrderComponent implements OnInit {
    public registrationNumber: string | null = null;
    public centerName: string | null = null;

    public car: Car = {
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

    constructor(
        private location: Location,
        private rentalCenterService: RentalCenterService,
        private route: ActivatedRoute
    ) {
    }

    ngOnInit(): void {
        this.centerName = this.route.snapshot.paramMap.get('name');
        this.registrationNumber = this.route.snapshot.paramMap.get('registrationNumber');
        this.getCar();
    }

    private getCar(): void {
        if (this.centerName && this.registrationNumber) {
            this.rentalCenterService.getCar(this.centerName, this.registrationNumber)
                .subscribe(car => {
                        this.car = car;
                    }
                );
        }
    }

    onSubmit() {

    }
}
