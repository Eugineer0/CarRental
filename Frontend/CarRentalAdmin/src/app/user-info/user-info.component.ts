import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Order } from '../_models/user/order';

import { UserService } from '../_services/user.service';
import { Roles, User } from '../_models/user/user-full';

@Component({
    selector: 'app-user-info',
    templateUrl: './user-info.component.html',
    styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {
    public user: User | null = null;
    public orders: Order[] = [];

    public errorMessage: string | null = null;
    public editRolesFailed: boolean = false;
    public rolesChanged: boolean = false;
    public allRoles: Roles[] = Object
        .keys(Roles)
        .filter(value => isNaN(Number(value)))
        .map(key => Roles[key as keyof typeof Roles]);

    private roles: Roles[] = [];

    constructor(
        private location: Location,
        private userService: UserService,
        private route: ActivatedRoute
    ) {
    }

    public ngOnInit(): void {
        const username = this.route.snapshot.paramMap.get('username');

        if (username) {
            this.getUser(username);
            this.getOrders(username);
        }
    }

    public goBack(): void {
        this.location.back();
    }

    public checkUserRole(role: Roles): boolean {
        return this.roles.includes(role);
    }

    public isNoneRole(role: Roles): boolean {
        return role === Roles.None;
    }

    public onSubmit(): void {
        if (this.user && this.rolesChanged && !this.rolesEqual(this.user.roles, this.roles)) {
            this.userService.putRoles(this.user.username, this.roles)
                .subscribe(
                    _ => {
                        if (this.user) {
                            this.copyRoles(this.roles, this.user.roles);
                        }
                    },
                    error => {
                        this.editRolesFailed = true;
                        this.errorMessage = error.error;
                    }
                );

            this.rolesChanged = false;
        }
    }

    public editRoles(role: Roles): void {
        const index = this.roles.indexOf(role);

        if (index > -1) {
            this.roles.splice(index, 1);

            if (this.roles.length < 1) {
                this.roles.push(Roles.None);
            }
        } else {
            this.roles.push(role);

            if (this.roles.length > 1) {
                const index = this.roles.indexOf(Roles.None);
                if (index > -1) {
                    this.roles.splice(index, 1);
                }
            }
        }

        this.rolesChanged = true;
        this.editRolesFailed = false;
    }

    public resetRolesChanging(): void {
        if (this.user) {
            this.copyRoles(this.user.roles, this.roles);
        }

        this.rolesChanged = false;
        this.editRolesFailed = false;
    }

    public showRole(role: Roles): string {
        return Roles[role];
    }

    private getUser(username: string): void {

        this.userService.getUser(username)
            .subscribe(user => {
                    this.user = user;
                    this.copyRoles(user.roles, this.roles);
                }
            );
    }

    private getOrders(username: string): void {
        this.userService.getOrders(username)
            .subscribe(orders => {
                    this.orders = orders;
                }
            );
    }

    private rolesEqual(target: Roles[], candidate: Roles[]): boolean {
        const length = target.length;
        if (length !== candidate.length) {
            return false;
        }

        for (let i = 0; i < length; i++) {
            if (target[i] !== candidate[i]) {
                return false;
            }
        }

        return true;
    }

    private copyRoles(source: Roles[], dest: Roles[]): void {
        dest.splice(0);
        for (let role of source) {
            dest.push(role);
        }
    }

    public showDateOfBirth(user: User): string {
        return new Date(user.dateOfBirth).toDateString().slice(4);
    }
}
