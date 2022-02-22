import { Component, OnInit } from '@angular/core';
import { Location } from "@angular/common";
import { UserService } from "../_services/user.service";
import { UserFull } from "../_models/user-full";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {
  public user: UserFull | undefined;

  constructor(
    private location: Location,
    private userService: UserService,
    private route: ActivatedRoute,
  ) {
  }

  ngOnInit(): void {
    this.getUser()
  }

  goBack(): void {
    this.location.back();
  }

  private getUser(): void {
    const username = this.route.snapshot.paramMap.get('username');

    if (username) {
      this.userService.getUser(username)
        .subscribe(user => this.user = user);
    }
  }
}
