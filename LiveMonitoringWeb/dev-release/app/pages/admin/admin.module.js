/**
 * @author v.lugovsky
 * created on 16.12.2015
 */
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin', ['ui.select', 'ngSanitize'])
        .config(routeConfig);

    /** @ngInject */
    function routeConfig($stateProvider) {
        $stateProvider
            .state('admin', {
                url: '/admin',
                template: '<ui-view autoscroll="true" autoscroll-body-top></ui-view>',
                abstract: true,
                title: 'Admin',
                sidebarMeta: {
                    icon: 'ion-compose',
                    order: 250,
                },
            })
            .state('admin.category', {
                url: '/Category',
                templateUrl: 'dev-release/app/pages/admin/category/category.html',
                controller: 'CategoryCtrl',
                title: 'Category',
                sidebarMeta: {
                    order: 0,
                },
            })
            .state('admin.configuration', {
                url: '/Configuration',
                templateUrl: 'dev-release/app/pages/admin/configuration/configuration.html',
                controller: 'ConfigurationCtrl',
                title: 'Configuration',
                sidebarMeta: {
                    order: 100,
                },
            })
            .state('admin.createGroup',
            {
                url: '/Groups',
                templateUrl: 'dev-release/app/pages/admin/createGroup/createGroup.html',
                controller: 'CreateGroupCtrl',
                title: 'Groups',
                sidebarMeta: {
                    order: 200,
                },
            })
            .state('admin.createUser',
            {
                url: '/Users',
                templateUrl: 'dev-release/app/pages/admin/createUser/createUser.html',
                controller: 'CreateUserCtrl',
                title: 'Users',
                sidebarMeta: {
                    order: 200,
                },
            })
            .state('admin.myProfile',
            {
                url: '/Customer',
                templateUrl: 'dev-release/app/pages/admin/myProfile/myProfile.html',
                controller: 'MyProfileCtrl',
                title: 'My Profile',
                sidebarMeta: {
                    order: 200,
                },
            })
        .state('admin.scheduleReport',
            {
                url: '/ScheduleReports',
                templateUrl: 'dev-release/app/pages/admin/scheduleReport/scheduleReport.html',
                controller: 'ScheduleReportCtrl',
                title: 'Schedule Report Permission',
                sidebarMeta: {
                    order: 200,
                },
            })
        .state('admin.subCategory',
            {
                url: '/SubCategory',
                templateUrl: 'dev-release/app/pages/admin/subCategory/subCategory.html',
                controller: 'SubCategoryCtrl',
                title: 'Sub Category',
                sidebarMeta: {
                    order: 200,
                },
            })
        .state('admin.userPermission',
            {
                url: '/UserPermission',
                templateUrl: 'dev-release/app/pages/admin/userPermission/userPermission.html',
                controller: 'UserPermissionCtrl',
                title: 'User Permission',
                sidebarMeta: {
                    order: 200,
                },
            });
    }
})();
