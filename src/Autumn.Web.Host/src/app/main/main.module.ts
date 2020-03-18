import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule, TabsModule, TooltipModule, BsDropdownModule, PopoverModule } from 'ngx-bootstrap';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';

import { FamilyAndNetworkComponent } from './familyAndNetwork/familyAndNetwork.component';
import { CreateOrEditNetworkModalComponent } from './familyAndNetwork/create-or-edit-familyNetwork-modal.component';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material';
import { MatCheckboxModule } from '@angular/material/checkbox';
NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();
import { OverviewComponent } from './overview/overview.component';
import { YourGoalModalComponent } from './your-goal/your-goal-modal.component';
import { ChooseGoalModalComponent } from './choose-goal/choose-goal-modal.component';
import { AddGoalModalComponent } from './add-goal/add-goal-modal.component';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot(),
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        MatRadioModule,
        MatAutocompleteModule,
        MatIconModule,
        MatFormFieldModule,
        MatCheckboxModule
    ],
    declarations: [
        DashboardComponent,
        OverviewComponent,
        YourGoalModalComponent,
        ChooseGoalModalComponent,
        AddGoalModalComponent,
        FamilyAndNetworkComponent,
        CreateOrEditNetworkModalComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class MainModule { }
