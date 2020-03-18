import { AbpSessionService } from '@abp/session/abp-session.service';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SessionServiceProxy, UpdateUserSignInTokenOutput, AutumnUserServiceProxy } from '@shared/service-proxies/service-proxies';
import { UrlHelper } from 'shared/helpers/UrlHelper';
import { OnboardService, ExternalOnBoardProvider } from './on-board.service';

declare var $: any;

@Component({
    templateUrl: './on-board.component.html',
    styleUrls: ['./on-board.component.scss'],
    animations: [accountModuleAnimation()]
})

export class OnboardComponent extends AppComponentBase implements OnInit {
    submitting = false;
    isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;
    displayIndex: number = 0;
    infoKeys = [];
    info: any = {};
    features: any = {};

    items = [{
        id: 0,
        text: "Consolidated view of your finances and insurances",
        image: "assets/common/images/investments.svg"
    }, {
        id: 1,
        text: "Health and wealth gap analysis",
        image: "assets/common/images/health.svg"
    }, {
        id: 2,
        text: "Share access with family and people you trust",
        image: "assets/common/images/heart.svg"
    }];

    carouselOptions = {
        loop: true,
        center: true,
        items: 3,
        dotsEach: true
    }

    owl: any;

    constructor(
        injector: Injector,
        public _onboardService: OnboardService,
        private _router: Router,
        private _sessionService: AbpSessionService,
        private _sessionAppService: SessionServiceProxy
    ) {
        super(injector);
    }

    get multiTenancySideIsTeanant(): boolean {
        return this._sessionService.tenantId > 0;
    }

    get isTenantSelfRegistrationAllowed(): boolean {
        return this.setting.getBoolean('App.TenantManagement.AllowSelfRegistration');
    }

    get isSelfRegistrationAllowed(): boolean {
        if (!this._sessionService.tenantId) {
            return false;
        }

        return this.setting.getBoolean('App.UserManagement.AllowSelfRegistration');
    }

    ngOnInit(): void {
        if (this._sessionService.userId > 0 && UrlHelper.getReturnUrl() && UrlHelper.getSingleSignIn()) {
            this._sessionAppService.updateUserSignInToken()
                .subscribe((result: UpdateUserSignInTokenOutput) => {
                    const initialReturnUrl = UrlHelper.getReturnUrl();
                    const returnUrl = initialReturnUrl + (initialReturnUrl.indexOf('?') >= 0 ? '&' : '?') +
                        'accessToken=' + result.signInToken +
                        '&userId=' + result.encodedUserId +
                        '&tenantId=' + result.encodedTenantId;

                    location.href = returnUrl;
                });
        }

        let state = UrlHelper.getQueryParametersUsingHash().state;
        if (state && state.indexOf('openIdConnect') >= 0) {
            this._onboardService.openIdConnectLoginCallback({});
        }

        setTimeout(() => {
            var owl = $('.owl-carousel');
            owl.owlCarousel();

            let idx = 0;
            var self = this;
            owl.find('.owl-stage').children().each(function (index) {
                $(this).attr('data-position', index); // NB: .attr() instead of .data()
                $(this).attr('data-index', self.items[idx].id);
                if (idx == self.items.length - 1) {
                    idx = 0;
                } else {
                    idx++;
                }
            });

            owl.on('changed.owl.carousel', function () {
                setTimeout(() => {
                    self.displayIndex = $(".owl-item.center").attr('data-index');
                }, 10);
            })

            $('.customNextBtn').click(function () {
                owl.trigger('prev.owl.carousel');
                let displayIndex = self.displayIndex + 1;
                if (displayIndex > 2) {
                    displayIndex = 0;
                }
                self.displayIndex = displayIndex;
            });

            $('.customPrevBtn').click(function () {
                owl.trigger('next.owl.carousel', [300]);
                let displayIndex = self.displayIndex - 1;
                if (displayIndex < 0) {
                    displayIndex = 2;
                }
                self.displayIndex = displayIndex;
            });

            $(document).on('click', '.owl-stage>div', function () {
                owl.trigger('to.owl.carousel', $(this).data('position'));
                self.displayIndex = $(this).data('index');
            });
        }, 150);

        this.info = {
            0: [{
                mainText: "All of your finances under one roof",
                image: "assets/common/images/bank.svg",
                headText: "Keep track of what you own and owe",
                text1: "Link your accounts through us and we'll be able to bring everything to you in one place -",
                text2: "bank balances, investments, CPF, and even your properties."
            }, {
                image: "assets/common/images/insurance.svg",
                headText: "Consolidate your insurance coverage",
                text1: "Add your health insurance policies for easy management, and to get a single view of all",
                text2: "your possible payouts."
            }, {
                image: "assets/common/images/child.svg",
                headText: "Benchmark with your peers",
                text1: "Find out what people in a similar situation may do, and see where you stand relative to",
                text2: "your peers."
            }],
            1: [{
                mainText: "Get personalised advice",
                image: "assets/common/images/investments.svg",
                headText: "Goals and gaps",
                text1: "We'll show you how each of your decisions impact your achievable future lifestyle by",
                text2: "actively comparing between your current trending and your ideal state."
            }, {
                image: "assets/common/images/goal.svg",
                headText: "Simulate life goals",
                text1: "Simulate different life goals, retirement scenarios & test your own assumptions, Identify",
                text2: "gaps in your plan and choose best solutions based on  your needs and financial goals."
            }, {
                image: "assets/common/images/insurance.svg",
                headText: "Start your legacy planning",
                text1: "Protect and provide for your next generation. You'll be able to easily write a legally",
                text2: "binding will on our platform. binding will on our platform."
            }],
            2: [{
                mainText: "Feel like at home",
                image: "assets/common/images/heart.svg",
                headText: "Plan with your family in mind",
                text1: "Find peace knowing your family will be ready for you to enjoy your retirement by adding",
                text2: "them and their needs to your plans."
            }, {
                image: "assets/common/images/talk.svg",
                headText: "Give access to your trusted network",
                text1: "Share your plan with family or your financial advisor. Control access levels in case of",
                text2: "emergency."
            }, {
                image: "assets/common/images/world.svg",
                headText: "Be a part of the community",
                text1: "Get tips from fellow like-minded users, and share your own ideas",
            }]
        }

        this.features = {
            0: [{
                headText: "Properties",
                text1: "Get Valuation",
                text2: "Track Yield",
            }, {
                headText: "Investments",
                text1: "Returns",
                text2: "Dividends",
                text3: "Maturity value"
            }, {
                headText: "Insurance",
                text1: "Your Coverage",
                text2: "View Cash value",
                text3: "See Your Premiums"
            }, {
                headText: "Bank accouts & cash",
                text1: "Add Multiple Bank Accounts",
                text2: "Add Multiple currencies"
            }],
            1: [{
                headText: "Insurance monitoring",
                text1: "Insurance coverage gap calculator",
                text2: "Cash value",
                text3: "Premiums"
            }, {
                headText: "Protection",
                text1: "Needs-based recommendations",
                text2: "Will planning/custody",
                text3: "Legacy planning",
                text4: "Health tips"
            }, {
                headText: "Investments",
                text1: "Portfolio diversification",
                text2: "Product ideas",
                text3: "Market outlook",
                text4: "Roboadvisory"
            }],
            2: [{
                headText: "Family",
                text1: "Spouse",
                text2: "Parents",
                text3: "Children"
            }, {
                headText: "Experts",
                text1: "Experienced retirees",
                text2: "Star users"
            }, {
                headText: "Peers",
                text1: "Retirement buddies",
                text2: "Users with common interests",
                text3: "People like you"
            }]
        }
    }

    goToLogin() {
        this._router.navigate(['account/login']);
    }

    goToRegisteration() {
        this._router.navigate(['account/registration']);
    }

    login(): void {
        abp.ui.setBusy(undefined, '', 1);
        this.submitting = true;
        this._onboardService.authenticate(
            () => {
                this.submitting = false;
                abp.ui.clearBusy();
            }
        );
    }

    externalLogin(provider: ExternalOnBoardProvider) {
        this._onboardService.externalAuthenticate(provider);
    }
}
