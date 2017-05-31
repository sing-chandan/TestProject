(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin')
        .controller('UserPermissionCtrl', UserPermissionCtrl);

    /** @ngInject */
    function UserPermissionCtrl($scope, $filter, $http, editableOptions, editableThemes) {

        
        $scope.rowCollection = [];
        $scope.ScheduleType = [];

        var baseSiteUrlPath = $("base").first().attr("href");
        $http.get(baseSiteUrlPath + "UserPermission/JsonUserPermission", { data: {} }).
  success(function (data, status, headers, config) {
      
      $scope.rowCollection = data.ScreenList;

      $scope.Userlist = data.Userlist;

      $scope.User = data.selectId.toString();



  }).
       error(function (data, status, headers, config) {

       });
        $scope.displayedCollection = [].concat($scope.rowCollection);






    }

})();
