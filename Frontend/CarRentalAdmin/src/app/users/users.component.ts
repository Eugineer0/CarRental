import { Component, OnInit } from '@angular/core';

import { UserMinimal } from "../_models/user/user-minimal";
import { UserService } from "../_services/user.service";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  public users: UserMinimal[] = [];

  constructor(
    private userService: UserService,
  ) {
  }

  public ngOnInit(): void {
    this.getUsers()
  }

  private getUsers(): void {
    this.userService.getUsers()
      .subscribe(
        users => this.users = users
      )
  }
}
