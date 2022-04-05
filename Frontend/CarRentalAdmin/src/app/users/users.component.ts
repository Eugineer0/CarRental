import { Component, OnInit } from '@angular/core';

import { UserService } from '../_services/user.service';
import { User } from '../_models/user/user';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
    public users: User[] = [];

    constructor(
        private userService: UserService
    ) {
    }

    public ngOnInit(): void {
        this.getUsers();
    }

    public approveClient(user: User): void {
        this.userService.approveClient(user.username)
            .subscribe(
                _ => {
                    user.approvalRequested = false;
                }
            );
    }

    private getUsers(): void {
        this.userService.getUsers()
            .subscribe(
                users => {
                    this.users = users;
                }
            );
    }
}
