import { Component, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { finalize } from 'rxjs/operators';
import { YourGoalModalComponent } from '../../main/your-goal/your-goal-modal.component';

@Component({
    templateUrl: './overview.component.html',
    styleUrls: ['./overview.component.scss'],
    animations: [appModuleAnimation()]
})
export class OverviewComponent extends AppComponentBase {

    @ViewChild('yourGoalModal') yourGoalModal: YourGoalModalComponent;

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
     }
  
}
