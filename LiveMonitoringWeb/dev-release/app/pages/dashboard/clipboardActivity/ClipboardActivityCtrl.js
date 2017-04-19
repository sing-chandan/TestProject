/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
debugger;
(function () {
  'use strict';
  
  angular.module('BlurAdmin.pages.dashboard')
      .controller('ClipboardActivityCtrl', ClipboardActivityCtrl);


  /** @ngInject */
  function ClipboardActivityCtrl($scope,$http, baConfig, colorHelper) {
      debugger;
      
      var baseSiteUrlPath = $("base").first().attr("href");

      $http.get(baseSiteUrlPath + "Home/JsonClipboardActivity", { data: {} })
       .success(function (result, status, headers, config) {
           $scope.ClipboardData = result;
         
       }).error(function (data, status, headers, config) {

       });

    
  }
})();