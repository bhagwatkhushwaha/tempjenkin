import { AbpMultiTenancyService } from '@abp/multi-tenancy/abp-multi-tenancy.service';
import { Injectable } from '@angular/core';
import { ApplicationInfoDto, GetCurrentLoginInformationsOutput, SessionServiceProxy, TenantLoginInfoDto, UserLoginInfoDto, UiCustomizationSettingsDto } from '@shared/service-proxies/service-proxies';

@Injectable()
export class AppSessionService {

    private _user: UserLoginInfoDto;
    private _tenant: TenantLoginInfoDto;
    private _application: ApplicationInfoDto;
    private _theme: UiCustomizationSettingsDto;

    constructor(
        private _sessionService: SessionServiceProxy,
        private _abpMultiTenancyService: AbpMultiTenancyService) {
    }

    get application(): ApplicationInfoDto {
        return this._application;
    }

    set application(val: ApplicationInfoDto) {
        this._application = val;
    }

    get user(): UserLoginInfoDto {
        return this._user;
    }

    get userId(): number {
        return this.user ? this.user.id : null;
    }

    get tenant(): TenantLoginInfoDto {
        return this._tenant;
    }

    get tenancyName(): string {
        return this._tenant ? this.tenant.tenancyName : '';
    }

    get tenantId(): number {
        return this.tenant ? this.tenant.id : null;
    }

    getShownLoginName(): string {
        const userName = this._user.userName;
        if (!this._abpMultiTenancyService.isEnabled) {
            return userName;
        }

        return (this._tenant ? this._tenant.tenancyName : '.') + '\\' + userName;
    }

    get theme(): UiCustomizationSettingsDto {
        return this._theme;
    }

    set theme(val: UiCustomizationSettingsDto) {
        this._theme = val;
    }

    init(): Promise<UiCustomizationSettingsDto> {
        return new Promise<UiCustomizationSettingsDto>((resolve, reject) => {
            this._sessionService.getCurrentLoginInformations().toPromise().then((result: GetCurrentLoginInformationsOutput) => {
                this._application = result.application;
                this._user = result.user;
                this._tenant = result.tenant;    
                this._theme = result.theme;
                this.doTheme9Configuration(result.theme);
                resolve(result.theme);
            }, (err) => {
                reject(err);
            });
        });
    }


    doTheme9Configuration(item: UiCustomizationSettingsDto) {
        this._theme.allowMenuScroll = true;
        this._theme.isLeftMenuUsed = false;
        this.theme.isTabMenuUsed = false;
        this._theme.isTopMenuUsed = true;

        this._theme.baseSettings.footer.fixedFooter = false;
        this._theme.baseSettings.header.desktopFixedHeader = true;
        this._theme.baseSettings.header.headerSkin = "light";
        this._theme.baseSettings.header.mobileFixedHeader = false;

        this._theme.baseSettings.layout.contentSkin = "light2";
        this._theme.baseSettings.layout.fixedBody = false;
        this._theme.baseSettings.layout.layoutType = null;
        this._theme.baseSettings.layout.mobileFixedBody = false;
        this._theme.baseSettings.layout.themeColor = "theme9";

        this._theme.baseSettings.menu.allowAsideHiding = false;
        this._theme.baseSettings.menu.allowAsideMinimizing = false;
        this._theme.baseSettings.menu.asideSkin = "light";
        this._theme.baseSettings.menu.defaultHiddenAside = false;
        this._theme.baseSettings.menu.defaultMinimizedAside = false;
        this._theme.baseSettings.menu.fixedAside = false;
        this._theme.baseSettings.menu.position = "top";
        this._theme.baseSettings.menu.submenuToggle = null;

        this._theme.baseSettings.theme = "theme9";
    }

    changeTenantIfNeeded(tenantId?: number): boolean {
        if (this.isCurrentTenant(tenantId)) {
            return false;
        }

        abp.multiTenancy.setTenantIdCookie(tenantId);
        location.reload();
        return true;
    }

    private isCurrentTenant(tenantId?: number) {
        let isTenant = tenantId > 0;

        if (!isTenant && !this.tenant) { // this is host
            return true;
        }

        if (!tenantId && this.tenant) {
            return false;
        } else if (tenantId && (!this.tenant || this.tenant.id !== tenantId)) {
            return false;
        }

        return true;
    }
}
