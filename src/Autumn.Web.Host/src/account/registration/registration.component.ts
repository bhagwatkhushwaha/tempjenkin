import { Component, OnInit, ViewChild, Directive, ElementRef, HostListener, Input, Pipe, PipeTransform } from '@angular/core';
import { CountryDataServiceProxy, CountryDto, AutumnUserDto, RetirementGoalsEnumDto, AutumnUserServiceProxy, GenderEnumDto } from '@shared/service-proxies/service-proxies';
import { FormBuilder, Validators, FormControl, FormGroupDirective, NgForm, NgControl } from '@angular/forms';
import { MatSelect, ErrorStateMatcher } from '@angular/material';
import { ReplaySubject, Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { SaveProgressModalComponent } from './save-progress-modal.component';
import { ConfirmSaveProgressModalComponent } from './confirm-save-progress-modal.component';

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
    isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
        const isSubmitted = form && form.submitted;
        return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
    }
}

@Pipe({
    name: 'series'
})
export class SeriesPipe implements PipeTransform {

    transform(value: any, args: any): any {
        if (args === 'radar') {
            return value.map(c => {
                return {
                    name: c.name,
                    data: c.data
                }
            });
        }

        return value;
    }

}

@Directive({
    selector: '[numbersOnly]'
})

export class OnlynumberDirective {

    constructor(private _el: ElementRef) { }

    @HostListener('input', ['$event']) onInputChange(event) {
        const initalValue = this._el.nativeElement.value;

        this._el.nativeElement.value = initalValue.replace(/[^0-9]*/g, '');
        if (initalValue !== this._el.nativeElement.value) {
            event.stopPropagation();
        }
    }
}

declare var $: any;

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.scss']
})

export class RegistrationComponent implements OnInit {
    aboutYouForm = this.fb.group({
        name: ['', [Validators.required, this.noWhitespaceValidator]],
        age: ['', [Validators.required, this.noWhitespaceValidator]],
        gender: [null],
        contactEmail: [null, [Validators.required, Validators.pattern('[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$')]],
        countryCtrl: [null, Validators.required]
    });

    income_Expenses = this.fb.group({
        totalMonthlyIncome: [null, Validators.required],
        totalMonthlyExpenses: [null, Validators.required],
    });

    saving_liability = this.fb.group({
        initialSaved: [null],
        initialOwed: [null],
    });

    retirementGoal = this.fb.group({
        desiredRetirementSum: [null],
        desiredRetirementIncome: [null],
        desiredLegacyAmount: [null],
        desiredRetirementAge: [null],
    });

    planGoal = this.fb.group({
        retireAge: [null],
        planToUse: [null],
    });

    series: any =
        {
            DataSeries: {
                income1: [
                    //889591
                ],
                income2: [
                    // 76010
                ],
                age: [
                ]
            },
        };

    lineChart: any;

    @ViewChild('saveProgressModal') saveProgressModal: SaveProgressModalComponent;
    @ViewChild('confirmSaveProgressModal') confirmSaveProgressModal: ConfirmSaveProgressModalComponent;

    matcher = new MyErrorStateMatcher();
    step: number = 1;
    innerStep: number = 0;
    stepTitle: string = "";
    countries: CountryDto[];
    user: AutumnUserDto = new AutumnUserDto();
    selectedRetirementGoal: RetirementGoalsEnumDto;
    public showSpecificCountryError: boolean = false;
    public continueBtnStp1: boolean = false;

    public saveAside: boolean = false;
    public canLive: boolean = false;
    public leaveBehind: boolean = false;
    public year: boolean = false;

    public retirementGoalStp1Error1: boolean = false;
    public retirementGoalStp1Error2: boolean = false;
    public retirementGoalStp2Error1: boolean = false;
    public retirementGoalStp3Error1: boolean = false;
    public retirementGoalStp4Error1: boolean = false;
    public planToUseError: boolean = false;
    public retireAgeError: boolean = false;
    public showGraph: boolean = false;

    data: any;

    //set flag if user already exist
    public isUserRegistered: boolean = false;

    /** control for the MatSelect filter keyword */
    public countryFilterCtrl: FormControl = new FormControl();

    /** list of banks filtered by search keyword */
    public filteredCountry: ReplaySubject<CountryDto[]> = new ReplaySubject<CountryDto[]>(1);

    @ViewChild('singleSelect') singleSelect: MatSelect;

    _lineType = { solid: 'solid', dashed: 'dashed' };

    /** Subject that emits when the component has been destroyed. */
    protected _onDestroy = new Subject<void>();

    constructor(private CountryDataService: CountryDataServiceProxy, private fb: FormBuilder,
        private _autumnUserService: AutumnUserServiceProxy) {
    }

    public noWhitespaceValidator(control: FormControl) {
        const isWhitespace = (control.value || '').trim().length === 0;
        if (control && control.value) {
            let removedSpaces = control.value.trimLeft();
            control.value !== removedSpaces && control.setValue(removedSpaces);
        }
        const isValid = !isWhitespace;
        return isValid ? null : true;
    }

    check() {
        this.showSpecificCountryError = (this.aboutYouForm.controls['countryCtrl'].value == 'Crimea') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Sevastopol') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Cuba') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Iran') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'North Korea') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Russia') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Syria') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Venezuela') ? true : false;
        return this.showSpecificCountryError;
    }

    SelectRetirementGoal(no) {
        switch (no) {
            case 1:
                this.retirementGoal.controls['desiredRetirementIncome'].setValue('');
                this.retirementGoal.controls['desiredLegacyAmount'].setValue('');
                this.retirementGoal.controls['desiredRetirementAge'].setValue('');
                this.selectedRetirementGoal = RetirementGoalsEnumDto.DesiredRetirementSum;
                this.saveAside = true;
                this.canLive = false;
                this.leaveBehind = false;
                this.year = false;
                this.retirementGoalStp1Error1 = false;
                this.retirementGoalStp1Error2 = false;
                this.retirementGoalStp2Error1 = false;
                this.retirementGoalStp3Error1 = false;
                this.retirementGoalStp4Error1 = false;
                this.retirementGoal.controls["desiredRetirementSum"].enable();
                this.retirementGoal.controls["desiredRetirementIncome"].disable();
                this.retirementGoal.controls["desiredLegacyAmount"].disable();
                this.retirementGoal.controls["desiredRetirementAge"].disable();
                break;

            case 2:
                this.retirementGoal.controls['desiredRetirementSum'].setValue('');
                this.retirementGoal.controls['desiredLegacyAmount'].setValue('');
                this.retirementGoal.controls['desiredRetirementAge'].setValue('');
                this.selectedRetirementGoal = RetirementGoalsEnumDto.DesiredRetirementIncome;
                this.saveAside = false;
                this.canLive = true;
                this.leaveBehind = false;
                this.year = false;
                this.retirementGoalStp1Error1 = false;
                this.retirementGoalStp1Error2 = false;
                this.retirementGoalStp2Error1 = false;
                this.retirementGoalStp3Error1 = false;
                this.retirementGoalStp4Error1 = false;
                this.retirementGoal.controls["desiredRetirementSum"].disable();
                this.retirementGoal.controls["desiredRetirementIncome"].enable();
                this.retirementGoal.controls["desiredLegacyAmount"].disable();
                this.retirementGoal.controls["desiredRetirementAge"].disable();
                break;

            case 4:
                this.retirementGoal.controls['desiredRetirementSum'].setValue('');
                this.retirementGoal.controls['desiredRetirementIncome'].setValue('');
                this.retirementGoal.controls['desiredRetirementAge'].setValue('');
                this.selectedRetirementGoal = RetirementGoalsEnumDto.DesiredLegacyAmount;
                this.saveAside = false;
                this.canLive = false;
                this.leaveBehind = true;
                this.year = false;
                this.retirementGoalStp1Error1 = false;
                this.retirementGoalStp1Error2 = false;
                this.retirementGoalStp2Error1 = false;
                this.retirementGoalStp3Error1 = false;
                this.retirementGoalStp4Error1 = false;
                this.retirementGoal.controls["desiredRetirementSum"].disable();
                this.retirementGoal.controls["desiredRetirementIncome"].disable();
                this.retirementGoal.controls["desiredLegacyAmount"].enable();
                this.retirementGoal.controls["desiredRetirementAge"].disable();
                break;

            case 5:
                this.retirementGoal.controls['desiredRetirementSum'].setValue('');
                this.retirementGoal.controls['desiredRetirementIncome'].setValue('');
                this.retirementGoal.controls['desiredLegacyAmount'].setValue('');
                this.selectedRetirementGoal = RetirementGoalsEnumDto.DesiredRetirementAge;
                this.saveAside = false;
                this.canLive = false;
                this.leaveBehind = false;
                this.year = true;
                this.retirementGoalStp1Error1 = false;
                this.retirementGoalStp1Error2 = false;
                this.retirementGoalStp2Error1 = false;
                this.retirementGoalStp3Error1 = false;
                this.retirementGoalStp4Error1 = false;
                this.retirementGoal.controls["desiredRetirementSum"].disable();
                this.retirementGoal.controls["desiredRetirementIncome"].disable();
                this.retirementGoal.controls["desiredLegacyAmount"].disable();
                this.retirementGoal.controls["desiredRetirementAge"].enable();
                break;
        }
    }

    ngOnInit() {
        this.user = new AutumnUserDto();
        this.changeTitle();
        this.CountryDataService.getAllCountries()
            .subscribe(data => {
                this.countries = data;

                this._autumnUserService.checkUserProgress().subscribe(data => {
                    if (data.registrationStep != undefined) {
                        this.step = data.registrationStep;
                        this.isUserRegistered = true;
                        this.aboutYouForm.controls['name'].setValue(data.name);
                        this.aboutYouForm.controls['age'].setValue(data.age);
                        var g = (data.gender == 1) ? "Male" : (data.gender == 2) ? "Female" : (data.gender == 3) ? "Neither" : (data.gender == 4) ? "Other" : (data.gender == 5) ? "NotDisclosed" : null;
                        this.aboutYouForm.controls['gender'].setValue(g);
                        this.aboutYouForm.controls['contactEmail'].setValue(data.emailAddress);
                        this.aboutYouForm.controls['countryCtrl'].setValue(this.countries.find(c => c.id == data.countryId).name);

                        this.income_Expenses.controls['totalMonthlyIncome'].setValue(data.totalMonthlyIncome);
                        this.income_Expenses.controls['totalMonthlyExpenses'].setValue(data.totalMonthlyExpences);


                        this.saving_liability.controls['initialSaved'].setValue(data.initialSaved);
                        this.saving_liability.controls['initialOwed'].setValue(data.initialOwed);

                        switch (data.retirementGoalOptions) {
                            case 1:
                                this.saveAside = true;
                                this.retirementGoal.controls['desiredRetirementSum'].setValue(data.desiredRetirementSum);
                                break;
                            case 2:
                                this.canLive = true;
                                this.retirementGoal.controls['desiredRetirementIncome'].setValue(data.desiredRetirementIncome);
                                break;
                            case 3:
                                this.leaveBehind = true;
                                this.retirementGoal.controls['desiredLegacyAmount'].setValue(data.desiredLegacyAmount);
                                break;
                            case 4:
                                this.year = true;
                                this.retirementGoal.controls['desiredRetirementAge'].setValue(data.desiredRetirementAge);
                                break;
                        }

                        this.planGoal.controls['retireAge'].setValue(data.desiredRetirementAge);
                        this.planGoal.controls['planToUse'].setValue(data.desiredRetirementIncome);
                    }
                });

                // set initial selection 
                this.aboutYouForm.controls['countryCtrl'].setValue(this.countries);

                // load the initial country list
                this.filteredCountry.next(this.countries.slice());


                // listen for search field value changes
                this.countryFilterCtrl.valueChanges
                    .pipe(takeUntil(this._onDestroy))
                    .subscribe(() => {
                        this.filterCountry();
                        this.showSpecificCountryError = (this.aboutYouForm.controls['countryCtrl'].value == 'Crimea') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Sevastopol') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Cuba') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Iran') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'North Korea') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Russia') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Syria') ? true : (this.aboutYouForm.controls['countryCtrl'].value == 'Venezuela') ? true : false;

                        if (this.showSpecificCountryError)
                            this.aboutYouForm.controls['countryCtrl'].setErrors({ 'invalid': this.showSpecificCountryError });
                        else
                            this.aboutYouForm.controls['countryCtrl'].setErrors(null);

                    });
            });

    }

    changeTitle() {
        this.stepTitle = (this.step == 1) ? "ABOUTYOU" : (this.step == 2) ? "INCOMEEXPENSES" : (this.step == 3) ? "SAVINGSLIABILITIES" : (this.step == 4) ? "RETIREMENTGOAL" : "RESULTS";
    }

    skipStep(step) {
        if (step == 4) {
            if (this.saving_liability.controls["initialSaved"].value == null)
                this.saving_liability.controls["initialSaved"].setValue(0);

            if (this.saving_liability.controls["initialOwed"].value == null)
                this.saving_liability.controls["initialOwed"].setValue(0);
        }
        this.step = step;
        this.changeTitle();
    }

    stepClick(step) {
        switch (step) {
            case 2:
                if (this.aboutYouForm.invalid || typeof (this.aboutYouForm.controls['countryCtrl'].value) == "object" || isNaN(+this.aboutYouForm.controls['age'].value)) {
                    if (typeof (this.aboutYouForm.controls['countryCtrl'].value) == "object") {
                        this.aboutYouForm.controls['countryCtrl'].setErrors({ 'required': true });
                    }
                    if (isNaN(+this.aboutYouForm.controls['age'].value) || this.aboutYouForm.controls['age'].value.trim() == "")
                        this.aboutYouForm.controls['age'].setErrors({ 'required': true });
                    if (this.aboutYouForm.controls['name'].value.trim() == "")
                        this.aboutYouForm.controls['name'].setErrors({ 'required': true });

                    return;
                }
                else {
                    this.continueBtnStp1 = true;
                    abp.ui.setBusy();
                    this._autumnUserService.checkEmailExists(this.aboutYouForm.controls['contactEmail'].value).subscribe(res => {
                        if (!res) {
                            abp.message.error("A user with this email ID is already taken.")
                            this.step = 1;
                            abp.ui.clearBusy();
                            return;
                        }
                        else {
                            this.user.name = this.aboutYouForm.controls['name'].value;
                            this.user.emailAddress = this.aboutYouForm.controls['contactEmail'].value;
                            this.user.userName = this.aboutYouForm.controls['contactEmail'].value;
                            this.user.password = this.aboutYouForm.controls['gender'].value;
                            this.user.age = +this.aboutYouForm.controls['age'].value;
                            this.user.gender = this.aboutYouForm.controls['gender'].value;
                            this.user.countryId = this.countries.find(x => x.name == this.aboutYouForm.controls['countryCtrl'].value).id;
                        }
                        abp.ui.clearBusy();
                        this.step = step;
                    });
                }
                this.changeTitle();
                break;

            case 3:
                if (this.income_Expenses.invalid || isNaN(+this.income_Expenses.controls['totalMonthlyIncome'].value) || isNaN(+this.income_Expenses.controls['totalMonthlyExpenses'].value)) {
                    if (isNaN(+this.income_Expenses.controls['totalMonthlyIncome'].value) || this.income_Expenses.controls['totalMonthlyIncome'].value.trim() == "")
                        this.income_Expenses.controls['totalMonthlyIncome'].setErrors({ 'required': true });
                    if (isNaN(+this.income_Expenses.controls['totalMonthlyExpenses'].value) || this.income_Expenses.controls['totalMonthlyIncome'].value.trim() == "")
                        this.income_Expenses.controls['totalMonthlyExpenses'].setErrors({ 'required': true });
                    return;
                }
                else {

                }
                this.step = step;
                this.changeTitle();
                break;
            case 4:
                if (isNaN(+this.saving_liability.controls['initialSaved'].value)) {
                    this.saving_liability.controls['initialSaved'].setErrors({ 'required': true });
                    return;
                }
                if (isNaN(+this.saving_liability.controls['initialOwed'].value)) {
                    this.saving_liability.controls['initialOwed'].setErrors({ 'required': true });
                    return;
                }
                this.retirementGoal.controls["desiredRetirementSum"].disable();
                this.retirementGoal.controls["desiredRetirementIncome"].disable();
                this.retirementGoal.controls["desiredLegacyAmount"].disable();
                this.retirementGoal.controls["desiredRetirementAge"].disable();

                if (this.saving_liability.controls["initialSaved"].value == null)
                    this.saving_liability.controls["initialSaved"].setValue("0");

                if (this.saving_liability.controls["initialOwed"].value == null)
                    this.saving_liability.controls["initialOwed"].setValue("0");
                this.step = step;
                this.changeTitle();
                break;
            case 5:
                if (this.innerStep == 1 || this.innerStep == 2) {
                    if (this.planGoal.invalid || isNaN(+this.planGoal.controls['retireAge'].value) || this.planGoal.controls['retireAge'].value.trim() == "") {
                        this.retireAgeError = true;
                        return;
                    }
                }
                else if (this.innerStep == 3 || this.innerStep == 4) {
                    if (this.planGoal.invalid || isNaN(+this.planGoal.controls['planToUse'].value) || this.planGoal.controls['planToUse'].value.trim() == "") {
                        this.planToUseError = true;
                        return;
                    }
                }
                this.step = step;
                this.changeTitle();
                this.setAllValues();
                abp.ui.setBusy();
                this._autumnUserService.genGraphData(this.user).subscribe(data => {
                    this.data = data;
                    this.showGraph = true;
                    abp.ui.clearBusy();
                })
                break;
        }

    }

    backStep(step: number) {
        switch (step) {
            //case 1:
            //    this.income_Expenses.controls['totalMonthlyIncome'].setValue('');
            //    this.income_Expenses.controls['totalMonthlyExpenses'].setValue('');
            //    break;
            //case 2:
            //    this.saving_liability.controls['initialSaved'].setValue(0);
            //    this.saving_liability.controls['initialOwed'].setValue(0);
            //    break;
            case 3:
                this.retirementGoal.controls['desiredRetirementSum'].setValue('');
                this.retirementGoal.controls['desiredRetirementIncome'].setValue('');
                this.retirementGoal.controls['desiredLegacyAmount'].setValue('');
                this.retirementGoal.controls['desiredRetirementAge'].setValue('');
                break;
            case 4:
                this.planGoal.controls['retireAge'].setValue('');
                this.planGoal.controls['planToUse'].setValue('');
                this.retireAgeError = false;
                this.planToUseError = false;
                this.innerStep = 0;
                break;
        }
        this.step = step;
        this.changeTitle();
    }

    goToLaststep() {
        this.innerStep = 0;
        this.step = 4;
    }

    innerStepClick(innerStep) {
        if (this.retirementGoal.invalid || isNaN(+this.retirementGoal.controls['desiredRetirementSum'].value) || isNaN(+this.retirementGoal.controls['desiredRetirementIncome'].value) || isNaN(+this.retirementGoal.controls['desiredLegacyAmount'].value) || isNaN(+this.retirementGoal.controls['desiredRetirementAge'].value)) {
            if (isNaN(+this.retirementGoal.controls['desiredRetirementSum'].value))
                this.retirementGoalStp1Error1 = true;
            if (isNaN(+this.retirementGoal.controls['desiredRetirementIncome'].value))
                this.retirementGoalStp2Error1 = true;
            if (isNaN(+this.retirementGoal.controls['desiredLegacyAmount'].value))
                this.retirementGoalStp3Error1 = true;
            if (isNaN(+this.retirementGoal.controls['desiredRetirementAge'].value))
                this.retirementGoalStp4Error1 = true;
            return;
        }
        else {
            switch (innerStep) {
                case 1:
                    if (this.retirementGoal.controls["desiredRetirementSum"].value == null || this.retirementGoal.controls["desiredRetirementSum"].value.trim() == "") {
                        this.retirementGoalStp1Error1 = true;
                        return
                    }
                    if (this.retirementGoal.controls["desiredRetirementSum"].value == "999999999999") {
                        this.retirementGoalStp1Error2 = true;
                        this.retirementGoalStp1Error1 = false;
                        return
                    }
                    break;

                case 2:
                    if (this.retirementGoal.controls["desiredRetirementIncome"].value == null || this.retirementGoal.controls["desiredRetirementIncome"].value.trim() == "") {
                        this.retirementGoalStp2Error1 = true;
                        return
                    }
                    break

                case 3:
                    if (this.retirementGoal.controls["desiredLegacyAmount"].value == null || this.retirementGoal.controls["desiredLegacyAmount"].value.trim() == "") {
                        this.retirementGoalStp3Error1 = true;
                        return
                    }
                    break;

                case 4:
                    if (this.retirementGoal.controls["desiredRetirementAge"].value == null || this.retirementGoal.controls["desiredRetirementAge"].value.trim() == "") {
                        this.retirementGoalStp4Error1 = true;
                        return
                    }
                    break;
            }
            this.innerStep = innerStep;
        }
    }

    protected filterCountry() {

        if (!this.countries) {
            return;
        }
        // get the search keyword
        let search = this.countryFilterCtrl.value;
        if (!search) {
            this.filteredCountry.next(this.countries.slice());
            return;
        } else {
            search = search.toLowerCase();
        }
        // filter the countries
        this.filteredCountry.next(
            this.countries.filter(country => country.name.toLowerCase().indexOf(search) > -1)
        );

    }

    redirectToDashboard() {
        if (this.innerStep == 1 || this.innerStep == 2) {
            if (this.planGoal.controls["retireAge"].value == null)
                this.planGoal.controls["retireAge"].setValue(65);
        }
        if (this.innerStep == 3 || this.innerStep == 4) {
            if (this.planGoal.controls["planToUse"].value == null)
                this.planGoal.controls["planToUse"].setValue(99999);
        }
        this.setAllValues();
        this.confirmSaveProgressModal.show(this.user);
    }

    setAllValues() {
        this.user.registrationStep = this.step;
        this.user.name = this.aboutYouForm.controls['name'].value;
        this.user.emailAddress = this.aboutYouForm.controls['contactEmail'].value;
        this.user.userName = this.aboutYouForm.controls['contactEmail'].value;
        this.user.password = this.aboutYouForm.controls['gender'].value;
        this.user.age = +this.aboutYouForm.controls['age'].value;
        this.user.gender = this.aboutYouForm.controls['gender'].value;
        this.user.countryId = this.countries.find(x => x.name == this.aboutYouForm.controls['countryCtrl'].value).id;

        this.user.retirementGoalOptions = this.selectedRetirementGoal;

        this.user.desiredRetirementSum = this.retirementGoal.controls['desiredRetirementSum'].value;

        if (this.planGoal.controls['planToUse'].value == "" || this.planGoal.controls['planToUse'].value == null)
            this.user.desiredRetirementIncome = this.retirementGoal.controls['desiredRetirementIncome'].value;
        else
            this.user.desiredRetirementIncome = this.planGoal.controls['planToUse'].value;

        this.user.desiredLegacyAmount = this.retirementGoal.controls['desiredLegacyAmount'].value;
        if (this.retirementGoal.controls['desiredRetirementAge'].value == "" || this.retirementGoal.controls['desiredRetirementAge'].value == null)
            this.user.desiredRetirementAge = this.planGoal.controls['retireAge'].value;
        else
            this.user.desiredRetirementAge = this.retirementGoal.controls['desiredRetirementAge'].value;
        this.user.initialSaved = this.saving_liability.controls['initialSaved'].value;
        this.user.initialOwed = this.saving_liability.controls['initialOwed'].value;

        this.user.totalMonthlyIncome = this.income_Expenses.controls['totalMonthlyIncome'].value;
        this.user.totalMonthlyExpences = this.income_Expenses.controls['totalMonthlyExpenses'].value;
    }

    saveProgress() {
        this.setAllValues();
        if (this.isUserRegistered)
            this._autumnUserService.updateUser(this.user).subscribe(data => {
                abp.notify.success("User update successfull");
                setTimeout(() => {
                    location.href = "/";
                }, 2000);
            });
        else
            this.saveProgressModal.show(this.user);
    }
}
