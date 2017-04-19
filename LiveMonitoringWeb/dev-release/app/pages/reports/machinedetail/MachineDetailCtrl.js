/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
debugger;
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('MachineDetailCtrl', MachineDetailCtrl);


    /** @ngInject */
    function MachineDetailCtrl($scope, $filter,$http, editableOptions, editableThemes) {
        
        debugger;
        $scope.rowCollection = [];

        var baseSiteUrlPath = $("base").first().attr("href");

        $http.get(baseSiteUrlPath + "MachineDetail/JsonMachineDetail", { data: {} }).
   success(function (data, status, headers, config) {
      

       $scope.rowCollection = data;


   }).
       error(function (data, status, headers, config) {

       });


        $scope.displayedCollection = [].concat($scope.rowCollection);

    }
})();