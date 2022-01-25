import { Component, OnInit } from '@angular/core';
import { AuthResponse } from "../_models/auth-responce";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-secret',
  templateUrl: './secret.component.html',
  styleUrls: ['./secret.component.css']
})
export class SecretComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  public getData() {
    this.http.get<AuthResponse>('/api/data')
      .subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.error(error);
        }
      )
    ;
  }
}
