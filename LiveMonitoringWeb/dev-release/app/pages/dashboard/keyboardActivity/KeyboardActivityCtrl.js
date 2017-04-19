/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
debugger;
(function () {
  'use strict';
  
  angular.module('BlurAdmin.pages.dashboard')
      .controller('KeyboardActivityCtrl', KeyboardActivityCtrl);


  /** @ngInject */
  function KeyboardActivityCtrl($scope,$http, baConfig, colorHelper) {
      debugger;
      
      var baseSiteUrlPath = $("base").first().attr("href");

      $http.get(baseSiteUrlPath + "Home/JsonRecentKeyActivity",{data:{}})
      .success(function (result,status,headers,config) {
          debugger;
          $scope.keyActivityData = result
      }) .error(function (data, status, headers, config) {

      });

    
  }
})();