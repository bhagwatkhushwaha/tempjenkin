import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem('Overview', '', '', '/app/main/overview'),
            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', '', '/app/admin/hostDashboard'),
            new AppMenuItem('Dashboard', '', '', '/app/main/dashboard'),
                new AppMenuItem('Users', '', '', '/app/admin/users'),
            new AppMenuItem('FamilyAndNetWork', '', '', '/app/main/familyAndNetwork'),

            //new AppMenuItem('Tenants', 'Pages.Tenants', '', '/app/admin/tenants'),
            //new AppMenuItem('Editions', 'Pages.Editions', '', '/app/admin/editions'),
            new AppMenuItem('Administration', '', '', '', [
                new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', '', '/app/admin/organization-units'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', '', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', '', '/app/admin/users'),
                new AppMenuItem('Languages', 'Pages.Administration.Languages', '', '/app/admin/languages'),
                new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', '', '/app/admin/auditLogs'),
                new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', '', '/app/admin/maintenance'),
                new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', '', '/app/admin/subscription-management'),
                new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', '', '/app/admin/ui-customization'),
                new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', '', '/app/admin/hostSettings'),
                new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', '', '/app/admin/tenantSettings')
            ])
            //new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', '', '/app/admin/demo-ui-components'),
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName && this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            } else if (subMenuItem.items && subMenuItem.items.length) {
                return this.checkChildMenuItemPermission(subMenuItem);
            }
        }

        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }
}
