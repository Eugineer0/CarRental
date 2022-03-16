import { Component, OnInit } from '@angular/core';
import { formatDate, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { RentalCenterService } from '../_services/rental-center.service';
import { RentalCenter } from '../_models/center/rental-center';
import { Car } from '../_models/car/car';
import { CarFilter } from '../_models/car/car-filter';

@Component({
    selector: 'app-rental-center-info',
    templateUrl: './rental-center-info.component.html',
    styleUrls: ['./rental-center-info.component.css']
})
export class RentalCenterInfoComponent implements OnInit {
    public cars: Car[] = [];
    public center: RentalCenter | undefined;

    public centerName: string | null = null;

    public startRentDate: string | undefined = undefined;
    public finishRentDate: string | undefined = undefined;
    public startRentTime: string | undefined = undefined;
    public finishRentTime: string | undefined = undefined;

    private filter: CarFilter = {
        startRent: undefined,
        finishRent: undefined
    };

    constructor(
        private location: Location,
        private rentalCenterService: RentalCenterService,
        private route: ActivatedRoute
    ) {
    }

    ngOnInit(): void {
        this.centerName = this.route.snapshot.paramMap.get('name');

        const startDateString = this.route.snapshot.queryParamMap.get('start');
        const finishDateString = this.route.snapshot.queryParamMap.get('finish');

        if (startDateString && finishDateString) {
            this.startRentDate = formatDate(startDateString, 'yyyy-MM-dd', 'en-us');
            this.startRentTime = formatDate(startDateString, 'HH:mm', 'en-us');

            this.finishRentDate = formatDate(finishDateString, 'yyyy-MM-dd', 'en-us');
            this.finishRentTime = formatDate(finishDateString, 'HH:mm', 'en-us');
        }

        this.getCenter();
        this.getCars();
    }

    goBack(): void {
        this.location.back();
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

    public onSubmit(): void {
        console.log(new Date(this.startRentDate + ' ' + this.startRentTime));
    }

    private processDates(): void {
        this.filter.startRent = new Date(this.startRentDate + ' ' + this.startRentTime);
        this.filter.finishRent = new Date(this.finishRentDate + ' ' + this.finishRentTime);
    }
}
