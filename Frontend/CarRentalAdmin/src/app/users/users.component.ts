import { Component, OnInit } from '@angular/core';

import { UserMinimal } from "../_models/user-minimal";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  public users: UserMinimal[] = [];
  public selectedUser: UserMinimal = {
    dateOfBirth: new Date(),
    email: 'adad@gmail.com',
    name: 'pidor',
    passportNumber: 'string',
    surname: 'string',
    username: 'string'
  };

  constructor() {
  }

  ngOnInit(): void {
    this.users = [
      {
        dateOfBirth: new Date(),
        email: 'adad@gmail.com',
        name: 'pidor',
        passportNumber: 'string',
        surname: 'string',
        username: 'Qwer'
      },
      {
        dateOfBirth: new Date(),
        email: 'adad@gmail.com',
        name: 'pedofil',
        passportNumber: 'string',
        surname: 'string',
        username: 'Qwer1'
      },
      {
        dateOfBirth: new Date(),
        email: 'adad@gmail.com',
        name: 'traher',
        passportNumber: 'string',
        surname: 'string',
        username: 'Sadmin'
      }
    ]
  }

  onSelect(user: UserMinimal): void {
    this.selectedUser = user;
  }
}
