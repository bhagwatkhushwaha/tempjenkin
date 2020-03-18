import { AbpModule } from '@abp/abp.module';
import * as ngCommon from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClientJsonpModule } from '@angular/common/http';

import { FileUploadModule } from 'ng2-file-upload';
import { ModalModule, PopoverModule, TabsModule, TooltipModule, BsDropdownModule } from 'ngx-bootstrap';
import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { AutoCompleteModule, EditorModule, FileUploadModule as PrimeNgFileUploadModule, InputMaskModule, PaginatorModule } from 'primeng/primeng';
import { TableModule } from 'primeng/table';
import { TreeModule } from 'primeng/tree';
import { DragDropModule } from 'primeng/dragdrop';
import { TreeDragDropService } from 'primeng/api';
import { ContextMenuModule } from 'primeng/contextmenu';

import { NgxChartsModule } from '@swimlane/ngx-charts';
import { CountoModule } from 'angular2-counto';
import { TextMaskModule } from 'angular2-text-mask';
import { ImageCropperModule } from 'ngx-image-cropper';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';

import { CommonModule } from '@shared/common/common.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { RecaptchaModule } from 'ng-recaptcha';
import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account.component';
import { AccountRouteGuard } from './auth/account-route-guard';
import { ConfirmEmailComponent } from './email-activation/confirm-email.component';
import { EmailActivationComponent } from './email-activation/email-activation.component';
import { LanguageSwitchComponent } from './language-switch.component';
import { LoginComponent } from './login/login.component';
import { LoginService } from './login/login.service';
import { SendTwoFactorCodeComponent } from './login/send-two-factor-code.component';
import { ValidateTwoFactorCodeComponent } from './login/validate-two-factor-code.component';
import { ForgotPasswordComponent } from './password/forgot-password.component';
import { ResetPasswordComponent } from './password/reset-password.component';
import { PayPalPurchaseComponent } from './payment/paypal/paypal-purchase.component';
import { StripePurchaseComponent } from './payment/stripe/stripe-purchase.component';
import { StripeSubscribeComponent } from './payment/stripe/stripe-subscribe.component';
import { StripeUpdateSubscriptionComponent } from './payment/stripe/stripe-update-subscription.component';
import { BuyEditionComponent } from './payment/buy.component';
import { UpgradeEditionComponent } from './payment/upgrade.component';
import { ExtendEditionComponent } from './payment/extend.component';
import { RegisterTenantResultComponent } from './register/register-tenant-result.component';
import { RegisterTenantComponent } from './register/register-tenant.component';
import { RegisterComponent } from './register/register.component';
import { SelectEditionComponent } from './register/select-edition.component';
import { TenantRegistrationHelperService } from './register/tenant-registration-helper.service';
import { TenantChangeModalComponent } from './shared/tenant-change-modal.component';
import { TenantChangeComponent } from './shared/tenant-change.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { PaymentHelperService } from './payment/payment-helper.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { OwlModule } from 'ngx-owl-carousel';
import { RegistrationComponent, OnlynumberDirective, SeriesPipe } from './registration/registration.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatFormFieldModule } from '@angular/material';
import { MatIconModule } from '@angular/material/icon';
import { OnboardComponent } from './on-board/on-board.component';
import { OnboardService } from './on-board/on-board.service';
import { SaveProgressModalComponent } from './registration/save-progress-modal.component';
import { ConfirmSaveProgressModalComponent } from './registration/confirm-save-progress-modal.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AppCommonModule } from '@app/shared/common/app-common.module';

@NgModule({
    imports: [
        ngCommon.CommonModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        RecaptchaModule.forRoot(),
        AbpModule,
        CommonModule,
        ServiceProxyModule,
        AccountRoutingModule,
        MatCheckboxModule,
        OwlModule,
        OAuthModule.forRoot(),
        FormsModule,
        ReactiveFormsModule,
        FileUploadModule,
        ModalModule.forRoot(),
        TabsModule.forRoot(),
        TooltipModule.forRoot(),
        PopoverModule.forRoot(),
        BsDropdownModule.forRoot(),
        BsDatepickerModule.forRoot(),
        UtilsModule,
        TableModule,
        TreeModule,
        DragDropModule,
        ContextMenuModule,
        PaginatorModule,
        PrimeNgFileUploadModule,
        AutoCompleteModule,
        EditorModule,
        InputMaskModule,
        NgxChartsModule,
        CountoModule,
        TextMaskModule,
        ImageCropperModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        MatRadioModule,
        MatAutocompleteModule,
        MatFormFieldModule,
        NgxMatSelectSearchModule,
        MatIconModule,
        NgApexchartsModule,
        AppCommonModule
    ],
    declarations: [
        AccountComponent,
        TenantChangeComponent,
        TenantChangeModalComponent,
        LoginComponent,
        OnboardComponent,
        RegistrationComponent,
        DashboardComponent,
        RegisterComponent,
        RegisterTenantComponent,
        RegisterTenantResultComponent,
        SelectEditionComponent,
        ForgotPasswordComponent,
        ResetPasswordComponent,
        EmailActivationComponent,
        ConfirmEmailComponent,
        SendTwoFactorCodeComponent,
        ValidateTwoFactorCodeComponent,
        LanguageSwitchComponent,
        BuyEditionComponent,
        UpgradeEditionComponent,
        ExtendEditionComponent,
        PayPalPurchaseComponent,
        StripePurchaseComponent,
        StripeSubscribeComponent,
        StripeUpdateSubscriptionComponent,
        OnlynumberDirective,
        SaveProgressModalComponent,
        ConfirmSaveProgressModalComponent,
        SeriesPipe
    ],
    providers: [
        LoginService,
        OnboardService,
        TenantRegistrationHelperService,
        PaymentHelperService,
        AccountRouteGuard,
        TreeDragDropService,
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class AccountModule {

}
