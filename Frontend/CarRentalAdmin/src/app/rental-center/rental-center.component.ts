import { Component, Input, OnInit } from '@angular/core';
import { RentalCenter } from "../_models/center/rental-center";

@Component({
    selector: 'app-rental-center',
    templateUrl: './rental-center.component.html',
    styleUrls: ['./rental-center.component.css']
})
export class RentalCenterComponent implements OnInit {
    @Input() center: RentalCenter | undefined;

    constructor() {
    }

    ngOnInit(): void {
    }

}
