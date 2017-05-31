/**
 * @author v.lugovksy
 * created on 16.12.2015
 */

(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('ProjectSummaryCtrl', ProjectSummaryCtrl);


    /** @ngInject */
    function ProjectSummaryCtrl($scope, $filter,$http, editableOptions, editableThemes) {
        
        $scope.rowCollection = [];

        var baseSiteUrlPath = $("base").first().attr("href");

       

        $http.get(baseSiteUrlPath + "Report/JsonProjectSummary", { data: {} }).
   success(function (data, status, headers, config) {
       
     
       $scope.rowCollection = data;


   }).
       error(function (data, status, headers, config) {

       });


    $scope.displayedCollection = [].concat($scope.rowCollection);


    }
})();