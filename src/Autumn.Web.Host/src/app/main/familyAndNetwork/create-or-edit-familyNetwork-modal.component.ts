import { AfterViewChecked, Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import * as _ from 'lodash';
import { FormBuilder, Validators, FormControl, FormGroupDirective, NgForm, NgControl } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material';

export interface networkType {
    value: number;
    viewValue: string;
}

export interface accessLevel {
    value: string;
    descr: string;
    checked: false;
}

export interface roles {
    value: number;
    descr: string;
}


/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
    isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
        const isSubmitted = form && form.submitted;
        return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
    }
}


@Component({
    selector: 'createOrEditNetworkModal',
    templateUrl: './create-or-edit-familyNetwork-modal.component.html',
})
export class CreateOrEditNetworkModalComponent extends AppComponentBase {
    @ViewChild('createOrEditNetworkModal') modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    memberForm = this.fb.group({
        networkType: [null, Validators.required],
        contactEmail: [null, [Validators.required, Validators.pattern('^[a-zA-Z]{1}[a-zA-Z0-9.\-_]*@[a-zA-Z0-9]{1}[a-zA-Z0-9.-]*[a-zA-Z0-9]{1}[.][a-zA-Z]{2,}$')]],
        role: [null, Validators.required],
        accessLevel: [null, Validators.required]
    });

    networkTypes: networkType[] = [
        { value: 1, viewValue: 'Family Member' },
        { value: 2, viewValue: 'Trusted Network' }];

    roles: roles[] = [
        { value: 1, descr: 'Parent' },
        { value: 2, descr: 'Spouse' },
        { value: 3, descr: 'Child' },
        { value: 4, descr: 'Other Family' },
        { value: 5, descr: 'Financial Advisor' }];

    accessLevel: string;

    accessLevels: accessLevel[] = [
        { value: 'Goals-only', descr: 'Family member will only be able to see your life goals', checked: false },
        { value: 'Limited access', descr: 'Family member will only be able to see your goals and insurance policies', checked: false },
        { value: 'Full access', descr: 'Family member will be able to see your goals, insurance policies and finances', checked: false }
    ];

    seasons: string[] = ['Winter', 'Spring', 'Summer', 'Autumn'];

    constructor(
        injector: Injector, private fb: FormBuilder
    ) {
        super(injector);
    }

    show(userId?: number): void {
        this.modal.show();
    }

    onShown(): void {
        //document.getElementById('Name').focus();
    }

    close(): void {
        this.modal.hide();
    }

    saveMember(): void {
        debugger;
        var d = this.memberForm; 
    }
}