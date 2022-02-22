import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-secret',
  templateUrl: './secret.component.html',
  styleUrls: ['./secret.component.css']
})
export class SecretComponent implements OnInit {

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
  }

  public getData() {
    this.http.get('/api/data')
      .subscribe(
        _ => {
          console.log();
        },
        error => {
          console.error(error);
        }
      )
    ;
  }
}
