import { Component, OnInit, TemplateRef } from '@angular/core';
import { formatDate, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { RentalCenterService } from '../_services/rental-center.service';
import { RentalCenter } from '../_models/center/rental-center';
import { Car } from '../_models/car/car';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { dateConsistencyValidator } from '../date-consistency.directive';

@Component({
    selector: 'app-rental-center-info',
    templateUrl: './rental-center-info.component.html',
    styleUrls: ['./rental-center-info.component.css']
})
export class RentalCenterInfoComponent implements OnInit {
    public dateForm: FormGroup = new FormGroup({
            startDate: new FormControl(
                formatDate(Date.now(), 'yyyy-MM-dd', 'en-us'),
                Validators.required
            ),
            startTime: new FormControl(
                formatDate(Date.now(), 'HH:mm', 'en-us'),
                Validators.required
            ),
            finishDate: new FormControl(
                formatDate(Date.now(), 'yyyy-MM-dd', 'en-us'),
                Validators.required
            ),
            finishTime: new FormControl(
                formatDate(Date.now(), 'HH:mm', 'en-us'),
                Validators.required
            )
        },
        { validators: dateConsistencyValidator }
    );

    public cars: Car[] = [];
    public center: RentalCenter | undefined;
    public centerName: string | null = null;

    constructor(
        private rentalCenterService: RentalCenterService,
        private location: Location,
        private route: ActivatedRoute,
        private modalService: NgbModal
    ) {
    }

    ngOnInit(): void {
        this.centerName = this.route.snapshot.paramMap.get('name');

        const startDate = this.route.snapshot.queryParamMap.get('startRent');
        const finishDate = this.route.snapshot.queryParamMap.get('finishRent');

        if (startDate && finishDate) {
            this.dateForm.controls.startDate.setValue(formatDate(startDate, 'yyyy-MM-dd', 'en-us'));
            this.dateForm.controls.startTime.setValue(formatDate(startDate, 'HH:mm', 'en-us'));

            this.dateForm.controls.finishDate.setValue(formatDate(finishDate, 'yyyy-MM-dd', 'en-us'));
            this.dateForm.controls.finishTime.setValue(formatDate(finishDate, 'HH:mm', 'en-us'));
        }

        this.getCenter();
        this.getCars();
    }

    goBack(): void {
        this.location.back();
    }

    public open(content: TemplateRef<any>): void {
        if(this.dateForm.valid) {
            this.modalService.open(content);
        }
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
    }

    public getWidth(): string {
        return "width: 15%";
    }

    public getStartWidth(car: Car): string {
        return "width: 15%";
    }
}
