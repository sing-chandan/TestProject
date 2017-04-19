/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
(function () {
  'use strict';

  angular.module('BlurAdmin.pages.dashboard')
      .directive('topappChart', topappChart);

  /** @ngInject */
  function topappChart() {
    return {
      restrict: 'E',
      controller: 'TopappChartCtrl',
      templateUrl: 'dev-release/app/pages/dashboard/topappChart/topappChart.html'
    };
  }
})();