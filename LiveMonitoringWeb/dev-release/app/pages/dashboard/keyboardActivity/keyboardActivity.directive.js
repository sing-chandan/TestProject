/**
 * @author v.lugovksy
 * created on 16.12.2015
 */

(function () {
  'use strict';

  angular.module('BlurAdmin.pages.dashboard')
      .directive('keyboardActivity', keyboardActivity);

  /** @ngInject */
  function keyboardActivity() {
    return {
      restrict: 'E',
      controller: 'KeyboardActivityCtrl',
      templateUrl: 'dev-release/app/pages/dashboard/keyboardActivity/keyboardActivity.html'
    };
  }
})();