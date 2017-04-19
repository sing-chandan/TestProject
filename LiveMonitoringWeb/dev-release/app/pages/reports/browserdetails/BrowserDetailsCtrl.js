/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
debugger;
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('BrowserDetailsCtrl', BrowserDetailsCtrl);


    /** @ngInject */
    function BrowserDetailsCtrl($scope, $filter, $http, editableOptions, editableThemes) {
        
        var baseSiteUrlPath = $("base").first().attr("href");

       
     debugger;
        
    $scope.rowCollection = [];
            $http.get(baseSiteUrlPath + "BrowserDetails/JsonBrowserDetail", { data: {} }).
       success(function (data, status, headers, config) {
           debugger;
           $scope.itemsByPage = 5;
           $scope.rowCollection = data;
         
       }).
       error(function (data, status, headers, config) {
           
       });

            $scope.displayedCollection = [].concat($scope.rowCollection);


    }
})();