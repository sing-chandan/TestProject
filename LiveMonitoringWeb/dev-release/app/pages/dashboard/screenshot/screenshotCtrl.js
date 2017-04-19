/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
debugger;
(function () {
  'use strict';
  
  angular.module('BlurAdmin.pages.dashboard')
      .controller('ScreenshotCtrl', ScreenshotCtrl);


  /** @ngInject */
  function ScreenshotCtrl($scope,$http, baConfig, colorHelper) {
      debugger;
      
      $scope.baseSiteUrlPath = $("base").first().attr("href");

      $http.get($scope.baseSiteUrlPath + "Home/Jsonscreenshot", { data: {} })
      .success(function (result, status, headers, config) {
          $scope.ScreenShotData = result
      }).error(function (data, status, headers, config) {

      });

    
  }
})();