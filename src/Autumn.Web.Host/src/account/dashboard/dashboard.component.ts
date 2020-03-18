import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';

declare var $: any;

@Component({
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    animations: [accountModuleAnimation()]
})

export class DashboardComponent implements OnInit {

    constructor(private _router: Router) {

    }

    ngOnInit() {

    }

    setTopbarStep(id: string) {
        $(".topBarValues").removeClass('active');
        $("#" + id).addClass('active');
    }

}