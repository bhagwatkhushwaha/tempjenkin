import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
//import { TenantDashboardServiceProxy, SalesSummaryDatePeriod } from '@shared/service-proxies/service-proxies';

import * as _ from 'lodash';
import { CreateOrEditNetworkModalComponent } from './create-or-edit-familyNetwork-modal.component';

@Component({
    templateUrl: './familyAndNetwork.component.html',
    styleUrls: ['./familyAndNetwork.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class FamilyAndNetworkComponent extends AppComponentBase {
    @ViewChild('createOrEditNetworkModal') createOrEditNetworkModal: CreateOrEditNetworkModalComponent;

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    addFamilyMember(): void {
        this.createOrEditNetworkModal.show();
    }
}