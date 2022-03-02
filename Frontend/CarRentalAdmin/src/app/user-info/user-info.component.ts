import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";

import { UserService } from "../_services/user.service";

import { UserFull, Roles } from "../_models/user/user-full";

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {
  public user: UserFull | undefined;
  public allRoles: Roles[] = Object.keys(Roles).filter(value => isNaN(Number(value))).map(key => Roles[key as keyof typeof Roles]);

  private roles: Roles[] = [];
  private username: string | null = null;

  constructor(
    private location: Location,
    private userService: UserService,
    private route: ActivatedRoute
  ) {
  }

  public ngOnInit(): void {
    console.log(Object.values(Roles));
    this.getUser()
  }

  public goBack(): void {
    this.location.back();
  }

  private getUser(): void {
    this.username = this.route.snapshot.paramMap.get('username');

    if (this.username) {
      this.userService.getUser(this.username)
        .subscribe(user => {
            this.user = user;
            this.copyRoles(user.roles, this.roles);
          }
        );
    }
  }

  public checkUserRole(role: Roles): boolean {
    return this.roles.includes(role);
  }

  public isNoneRole(role: Roles): boolean {
    return role === Roles.None;
  }

  public onSubmit(): void {
    if (this.username) {
      this.userService.putRoles(this.username, this.roles)
        .subscribe();
      if (this.user) {
        this.copyRoles(this.roles, this.user.roles);
      }
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
  }

  public resetRoles(): void {
    if (this.user) {
      this.copyRoles(this.user.roles, this.roles);
    }
  }

  public showRole(role: Roles): string {
    return Roles[role];
  }

  private copyRoles(source: Roles[], dest: Roles[]): void {
    dest.splice(0);
    for (let role of source) {
      dest.push(role);
    }
  }
}
