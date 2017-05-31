/**
 * @author v.lugovksy
 * created on 16.12.2015
 */

(function () {
  'use strict';

  angular.module('BlurAdmin.pages.dashboard')
      .directive('clipboardActivity', clipboardActivity);

  /** @ngInject */
  function clipboardActivity() {
    return {
      restrict: 'E',
      controller: 'ClipboardActivityCtrl',
      templateUrl: 'dev-release/app/pages/dashboard/clipboardActivity/clipboardActivity.html'
    };
  }
})();