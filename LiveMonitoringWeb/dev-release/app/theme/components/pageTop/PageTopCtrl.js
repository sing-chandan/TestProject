/**
 * @author v.lugovksy
 * created on 16.12.2015
 */

(function () {
    'use strict';

    angular.module('BlurAdmin.theme.components')
        .controller('PageTopCtrl', PageTopCtrl);


    /** @ngInject */
    function PageTopCtrl($scope, $http) {
        

        var baseSiteUrlPath = $("base").first().attr("href");

        $scope.logoff = function () {

            $http.post(baseSiteUrlPath + "Account/LogOff", { data: {} }).
      success(function (data, status, headers, config) {
          location.reload();
      }).
           error(function (data, status, headers, config) {

           });
        }
        

    }
})();