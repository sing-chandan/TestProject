/**
 * @author v.lugovsky
 * created on 16.12.2015
 */
(function () {
  'use strict';

  angular.module('BlurAdmin.pages', [
    'ui.router',
    'ui.select', 
    'ngSanitize',

    'BlurAdmin.pages.dashboard',
    //'BlurAdmin.pages.ui',
    //'BlurAdmin.pages.components',
    //'BlurAdmin.pages.form',
    //'BlurAdmin.pages.tables',
    'BlurAdmin.pages.reports',
    //'BlurAdmin.pages.charts',
    //'BlurAdmin.pages.maps',
    //'BlurAdmin.pages.profile',
    'BlurAdmin.pages.admin'
    
  ])
      .config(routeConfig)
  .config(function(){
      $.jstree.defaults.core.themes.url = true;
      $.jstree.defaults.core.themes.dir = "dev-release/assets/img/theme/vendor/jstree/dist/themes";
  });

  /** @ngInject */
  function routeConfig($urlRouterProvider, baSidebarServiceProvider) {
      $urlRouterProvider.otherwise('/dashboard');

    //baSidebarServiceProvider.addStaticItem({
    //  title: 'Pages',
    //  icon: 'ion-document',
    //  subMenu: [{
    //    title: 'Sign In',
    //    fixedHref: 'auth.html',
    //    blank: true
    //  }, {
    //    title: 'Sign Up',
    //    fixedHref: 'reg.html',
    //    blank: true
    //  }, {
    //    title: 'User Profile',
    //    stateRef: 'profile'
    //  }, {
    //    title: '404 Page',
    //    fixedHref: '404.html',
    //    blank: true
    //  }]
    //});
    //baSidebarServiceProvider.addStaticItem({
    //  title: 'Menu Level 1',
    //  icon: 'ion-ios-more',
    //  subMenu: [{
    //    title: 'Menu Level 1.1',
    //    disabled: true
    //  }, {
    //    title: 'Menu Level 1.2',
    //    subMenu: [{
    //      title: 'Menu Level 1.2.1',
    //      disabled: true
    //    }]
    //  }]
    //});
  }

})();