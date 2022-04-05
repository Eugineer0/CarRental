import { Component, Input, OnInit } from '@angular/core';

import { Order } from "../_models/user/order";

@Component({
    selector: 'app-order',
    templateUrl: './order.component.html',
    styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
    @Input() order: Order | undefined;

    constructor() {
    }

    public ngOnInit(): void {
    }
}
