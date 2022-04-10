import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
    selector: 'app-registration-modal',
    templateUrl: './registration-modal.component.html',
    styleUrls: ['./registration-modal.component.css']
})
export class RegistrationModalComponent implements OnInit {
    @Output()
    private readonly close: EventEmitter<void> = new EventEmitter();

    @Output()
    private readonly submit: EventEmitter<void> = new EventEmitter();

    constructor() {
    }

    public ngOnInit(): void {
    }

    public onClose() {
        this.close.emit();
    }

    public onSubmit() {
        this.submit.emit();
    }
}