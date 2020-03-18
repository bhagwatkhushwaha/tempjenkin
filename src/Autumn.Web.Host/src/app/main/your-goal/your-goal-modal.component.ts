import { AfterViewChecked, Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { ChooseGoalModalComponent } from '../../main/choose-goal/choose-goal-modal.component';

import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { LifeGoalsServiceProxy, LifeGoalsDto } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';

@Component({
    selector: 'yourGoalModal',
    templateUrl: './your-goal-modal.component.html',
    styleUrls: ['./your-goal-modal.component.scss']
})
export class YourGoalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('chooseGoalModal') chooseGoalModal: ChooseGoalModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    goals: LifeGoalsDto[] = [];

    constructor(
        injector: Injector,
        private _lifeGoalService: LifeGoalsServiceProxy,
        private _router: Router
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.modal.show();
        this.getGoals();
    }

    save(): void {
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    getGoals() {
        abp.ui.setBusy();
        this._lifeGoalService.getGoals()
            .pipe(finalize(() => abp.ui.clearBusy()))
            .subscribe(res => {
                this.goals = res;
                abp.ui.clearBusy();
            });
    }
}
