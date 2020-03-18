import { AfterViewChecked, Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { FormBuilder, Validators, FormControl, FormGroupDirective, NgForm, NgControl } from '@angular/forms';
import { MatSelect, ErrorStateMatcher } from '@angular/material';

import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'addGoalModal',
    templateUrl: './add-goal-modal.component.html',
    styleUrls: ['./add-goal-modal.component.scss']
})
export class AddGoalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    addGoalForm = this.fb.group({
        name: [null, Validators.required],
        propertyType: [null, Validators.required],
        purchaseYear: [null, Validators.required],
        purchaseValue: [null, Validators.required]
        //countryCtrl: [null, Validators.required]
    });

    active = false;
    saving = false;

    constructor(
        injector: Injector, private fb: FormBuilder
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.modal.show();
    }

    save(): void {
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
