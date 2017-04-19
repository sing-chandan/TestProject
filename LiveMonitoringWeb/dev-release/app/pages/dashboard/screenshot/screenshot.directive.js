/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
debugger;
(function () {
  'use strict';

  angular.module('BlurAdmin.pages.dashboard')
      .directive('screenshot', screenshot);

  /** @ngInject */
  function screenshot() {
    return {
      restrict: 'E',
      controller: 'ScreenshotCtrl',
      templateUrl: 'dev-release/app/pages/dashboard/screenshot/screenshot.html'
    };
  }
})();