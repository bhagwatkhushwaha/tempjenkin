import { AfterViewChecked, Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { AutumnUserDto, AutumnUserServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'saveProgressModal',
    templateUrl: './save-progress-modal.component.html',
    styleUrls: ['./save-progress-modal.component.scss']
})

export class SaveProgressModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    error1: boolean = false;
    error2: boolean = false;
    userInfo: AutumnUserDto = new AutumnUserDto();
    aboutYouForm = this.forBuilder.group({
        password: [null, Validators.required],
        confirmPassword: [null, Validators.required],
    });

    constructor(
        injector: Injector,
        private _route: Router,
        private forBuilder: FormBuilder,
        private _autumnservice: AutumnUserServiceProxy
    ) {
        super(injector);
    }

    show(input: AutumnUserDto): void {
        this.userInfo = input;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    onChange() {
        this.error1 = false;
        this.error2 = false;
    }

    save() {
        let password = this.aboutYouForm.controls['password'].value;
        let confirmPassword = this.aboutYouForm.controls['confirmPassword'].value;
        if (password == undefined || password == null || password.trim() == "" || confirmPassword == undefined || confirmPassword == null || confirmPassword.trim() == "") {
            this.error1 = true;
            return;
        }
        else if (password != confirmPassword) {
            this.error2 = true;
            return;
        }
        this.userInfo.password = password;
        abp.ui.setBusy();

        this._autumnservice.createUser(this.userInfo)
            .pipe(finalize(() => abp.ui.clearBusy())).subscribe(result => {
                abp.ui.clearBusy();
                abp.message.success("User created successfully.")
                this._route.navigate(["/account/home"]);
            });

        this.active = false;
        this.modal.hide();
    }

}
