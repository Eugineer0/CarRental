import { Component, Input, OnInit } from '@angular/core';
import { User } from '../_models/user/user';

@Component({
    selector: 'app-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
    @Input() user: User | undefined;

    constructor() {
    }

    ngOnInit(): void {
    }

}
