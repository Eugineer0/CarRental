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
  public allRoles = Object.keys(Roles);

  private roles: Roles[] = [];
  private username: string | null = null;

  constructor(
    private location: Location,
    private userService: UserService,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit(): void {
    this.getUser()
  }

  goBack(): void {
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

  checkUserRole(role: string): boolean {
    return this.roles.includes(Roles[role as keyof typeof Roles]);
  }

  processNoneRole(role: string): boolean {
    return role === Roles.None
  }

  public onSubmit(): void {
    if(this.username) {
      this.userService.putRoles(this.username, this.roles)
        .subscribe();
      if(this.user) {
        this.copyRoles(this.roles, this.user.roles);
      }
    }
  }

  public editRole(role: string): void {
    const enumRole = Roles[role as keyof typeof Roles];
    const index = this.roles.indexOf(enumRole);

    if (index > -1) {
      this.roles.splice(index, 1);

      if (this.roles.length < 1) {
        this.roles.push(Roles.None);
      }
    } else {
      this.roles.push(enumRole);

      if (this.roles.length > 1) {
        const index = this.roles.indexOf(Roles.None);
        if (index > -1) {
          this.roles.splice(index, 1);
        }
      }
    }

    console.log(this.user?.roles);
  }

  public resetRoles(): void {
    if (this.user) {
      console.log(this.user.roles);

      this.copyRoles(this.user.roles, this.roles);

      console.log(this.user.roles);
    }
  }

  private copyRoles(source: Roles[], dest: Roles[]): void {
    for(let role of source) {
      dest.push(role);
    }
  }
}
