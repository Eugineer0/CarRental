import { Component, EventEmitter, Output } from '@angular/core';

@Component({
    selector: 'app-registration-modal',
    templateUrl: './registration-modal.component.html',
    styleUrls: ['./registration-modal.component.css']
})
export class RegistrationModalComponent {
    @Output()
    private readonly close: EventEmitter<void> = new EventEmitter();

    @Output()
    private readonly submit: EventEmitter<void> = new EventEmitter();

    constructor() {
    }

    public onClose(): void {
        this.close.emit();
    }

    public onSubmit(): void {
        this.submit.emit();
    }
}