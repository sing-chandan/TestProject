(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin')
        .controller('CreateUserCtrl', CreateUserCtrl)
    .filter('DateFormat', DateFormat);

    /** @ngInject */
    function CreateUserCtrl($scope, $filter, $http, editableOptions, editableThemes) {

        $scope.rowCollection = [];

        var baseSiteUrlPath = $("base").first().attr("href");
        $http.get(baseSiteUrlPath + "CreateUser/JsonUserData", { data: {} }).
  success(function (data, status, headers, config) {
      
      $scope.rowCollection = data.Userlist;
      $scope.UserRoles = data.UserRoles;

      $scope.removeUser = function (index) {
          $scope.rowCollection.splice(index, 1);
      };

      $scope.addUser = function () {
          $scope.inserted = {
              id: $scope.rowCollection.length + 1,
              name: '',
              status: null,
              group: null
          };
          $scope.rowCollection.unshift($scope.inserted);
      };

      $scope.showRoles = function (user) {
          var selected = [];
          if (user.RoleId) {
              selected = $filter('filter')($scope.UserRoles, { RoleId: user.RoleId });
          }
          return selected.length ? selected[0].RoleName : 'Not set';
      };

      editableOptions.theme = 'bs3';
      editableThemes['bs3'].submitTpl = '<button type="submit" class="btn btn-primary btn-with-icon"><i class="ion-checkmark-round"></i></button>';
      editableThemes['bs3'].cancelTpl = '<button type="button" ng-click="$form.$cancel()" class="btn btn-default btn-with-icon"><i class="ion-close-round"></i></button>';


  }).
       error(function (data, status, headers, config) {

       });
        $scope.displayedCollection = [].concat($scope.rowCollection);


        

        $scope.showGroup = function (user) {
            if (user.group && $scope.groups.length) {
                var selected = $filter('filter')($scope.groups, { id: user.group });
                return selected.length ? selected[0].text : 'Not set';
            } else return 'Not set'
        };

        $scope.opened = {};

        $scope.open = function ($event, elementOpened) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened[elementOpened] = !$scope.opened[elementOpened];
        };

        $scope.updateUser = function (data) {
            
        }



    }

    function DateFormat() {
        
        return function (value) {
            
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
        };
    }

})();
