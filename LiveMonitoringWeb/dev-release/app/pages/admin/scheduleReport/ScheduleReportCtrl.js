﻿(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin')
        .controller('ScheduleReportCtrl', ScheduleReportCtrl);

    /** @ngInject */
    function ScheduleReportCtrl($scope, $filter, $http, editableOptions, editableThemes) {

        
        $scope.rowCollection = [];
        $scope.ScheduleType = [];

        var baseSiteUrlPath = $("base").first().attr("href");
        $http.get(baseSiteUrlPath + "ScheduleReports/JsonScheduleReport", { data: {} }).
      success(function (data, status, headers, config) {
      
      $scope.rowCollection = data[0].Screenlist;

      $scope.ScheduleType = data[0].ScheduleType;

     
    


  }).
       error(function (data, status, headers, config) {

       });
        $scope.displayedCollection = [].concat($scope.rowCollection);

        $scope.btnSave = function () {
            $http.post(baseSiteUrlPath + "ScheduleReports/SubmitSehedule", { data: {} }).
     success(function (data, status, headers, config) {

         $scope.rowCollection = data[0].Screenlist;

         $scope.ScheduleType = data[0].ScheduleType;





     }).
      error(function (data, status, headers, config) {

      });
        }
     



    }

})();
