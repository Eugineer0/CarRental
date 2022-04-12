import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-secret',
    templateUrl: './secret.component.html',
    styleUrls: ['./secret.component.css']
})
export class SecretComponent implements OnInit {

    constructor(
        private http: HttpClient
    ) {
    }

    ngOnInit(): void {

    }

    public click(): void {
        this.http.get('/api/users')
            .subscribe(
                response => {
                    console.log(JSON.stringify(response));
                },
                error => {
                    console.log(JSON.stringify(error));
                }
            )

        this.http.get('/api/users')
            .subscribe(
                response => {
                    console.log(JSON.stringify(response));
                },
                error => {
                    console.log(JSON.stringify(error));
                }
            )

        this.http.get('/api/users')
            .subscribe(
                response => {
                    console.log(JSON.stringify(response));
                },
                error => {
                    console.log(JSON.stringify(error));
                }
            )

        this.http.get('/api/users/Qwert/orders')
            .subscribe(
                response => {
                    console.log(JSON.stringify(response));
                },
                error => {
                    console.log(JSON.stringify(error));
                }
            )
    }
}
