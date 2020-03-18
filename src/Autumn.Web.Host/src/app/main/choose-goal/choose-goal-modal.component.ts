import { AfterViewChecked, Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { AddGoalModalComponent } from '../../main/add-goal/add-goal-modal.component';

import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'chooseGoalModal',
    templateUrl: './choose-goal-modal.component.html',
    styleUrls: ['./choose-goal-modal.component.scss']
})
export class ChooseGoalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('addGoalModal') addGoalModal: AddGoalModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    constructor(
        injector: Injector
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
