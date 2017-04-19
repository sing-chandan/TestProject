/**
 * @author v.lugovsky
 * created on 16.12.2015
 */
(function () {
  'use strict';

  angular.module('BlurAdmin.pages.reports', ['ui.select', 'ngSanitize','smart-table'])
      .config(routeConfig);

  /** @ngInject */
  function routeConfig($stateProvider) {
    $stateProvider
        .state('reports', {
            url: '/reports',
          template : '<ui-view autoscroll="true" autoscroll-body-top></ui-view>',
          abstract: true,
          title: 'Reports',
          sidebarMeta: {
              icon: 'ion-stats-bars',
            order: 250,
          },
        })
        .state('reports.screenshot', {
            url: '/screenshot',
            templateUrl: 'dev-release/app/pages/reports/screenshot/screenshot.html',
            controller: 'ScreenShotCtrl',
          title: 'Screen Shot',
          sidebarMeta: {
            order: 0,
          },
        })
        .state('reports.machinedetail', {
          url: '/MachineDetail',
          templateUrl: 'dev-release/app/pages/reports/machinedetail/machinedetail.html',
          controller: 'MachineDetailCtrl',
          title: 'Machine Detail',
          sidebarMeta: {
            order: 100,
          },
        })
        .state('reports.keylogger',
        {
          url: '/KeyLogger',
          templateUrl: 'dev-release/app/pages/reports/keylogger/keylog.html',
          controller: 'KeyloggerCtrl',
          controllerAs: 'vm',
          title: 'Key Logger',
          sidebarMeta: {
            order: 200,
          },
        })
      .state('reports.browserdetails',
        {
            url: '/BrowserDetails',
            templateUrl: 'dev-release/app/pages/reports/browserdetails/browserdetails.html',
            controller: 'BrowserDetailsCtrl',
            controllerAs: 'main',
            title: 'Browser Details',
            sidebarMeta: {
                order: 200,
            },
        })
      .state('reports.idledetails',
        {
            url: '/IdleDetails',
            templateUrl: 'dev-release/app/pages/reports/idledetails/idledetails.html',
            controller: 'IdleDetailsCtrl',
            controllerAs: 'vm',
            title: 'Machine Idle Details',
            sidebarMeta: {
                order: 200,
            },
        })
      .state('reports.appdetails',
        {
            url: '/AppDetails',
            templateUrl: 'dev-release/app/pages/reports/appdetails/appdetails.html',
            controller: 'AppdetailsCtrl',
            controllerAs: 'vm',
            title: 'App Details',
            sidebarMeta: {
                order: 200,
            },
        })
      .state('reports.productivity',
        {
            url: '/productivity',
            templateUrl: 'dev-release/app/pages/reports/productivity/productivity.html',
            controller: 'ProductivityCtrl',
            controllerAs: 'vm',
            title: 'Productivity Report',
            sidebarMeta: {
                order: 200,
            },
        })
      .state('reports.useridle',
        {
            url: '/IdleReport',
            templateUrl: 'dev-release/app/pages/reports/useridle/useridle.html',
            controller: 'UserIdleCtrl',
            controllerAs: 'vm',
            title: 'User Idle Report',
            sidebarMeta: {
                order: 200,
            },
        })
      .state('reports.sitesdetails',
        {
            url: '/SiteReport',
            templateUrl: 'dev-release/app/pages/reports/sitesdetails/sitesdetails.html',
            controller: 'SitesDetailsCtrl',
            controllerAs: 'vm',
            title: 'Site Report',
            sidebarMeta: {
                order: 200,
            },
        })
      .state('reports.projectsummary',
        {
            url: '/ProjectSummary',
            templateUrl: 'dev-release/app/pages/reports/projectsummary/projectsummary.html',
            controller: 'ProjectSummaryCtrl',
            controllerAs: 'vm',
            title: 'Project Summary',
            sidebarMeta: {
                order: 200,
            },
        })
      .state('form.workperform',
        {
            url: '/WorkingPerformance',
            templateUrl: 'dev-release/app/pages/reports/workperform/workperform.html',
            controller: 'WorkPerformCtrl',
            controllerAs: 'vm',
            title: 'Working Performance',
            sidebarMeta: {
                order: 200,
            },
        });
  }
})();
