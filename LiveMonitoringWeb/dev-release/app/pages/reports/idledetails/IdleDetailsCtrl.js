﻿/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
debugger;
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('IdleDetailsCtrl', IdleDetailsCtrl);


 

    /** @ngInject */
    function IdleDetailsCtrl($scope, $filter,$http, editableOptions, editableThemes) {
        
        debugger;
        $scope.rowCollection = [];

        var baseSiteUrlPath = $("base").first().attr("href");

        $http.get(baseSiteUrlPath + "IdleDetails/JsonIdleDetail", { data: {} }).
success(function (data, status, headers, config) {

        debugger;
        $scope.rowCollection = data;

}).
       error(function (data, status, headers, config) {

       });
        $scope.displayedCollection = [].concat($scope.rowCollection);


    }
})();