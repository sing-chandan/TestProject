(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin')
        .controller('CreateGroupCtrl', CreateGroupCtrl);

    /** @ngInject */
    function CreateGroupCtrl($scope, $filter, $http, editableOptions, editableThemes) {

        $scope.rowCollection = [];

        var baseSiteUrlPath = $("base").first().attr("href");
        $http.get(baseSiteUrlPath + "Groups/GroupDetails", { data: {} }).
  success(function (data, status, headers, config) {
      
      $scope.rowCollection = data;

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

      editableOptions.theme = 'bs3';
      editableThemes['bs3'].submitTpl = '<button type="submit" class="btn btn-primary btn-with-icon"><i class="ion-checkmark-round"></i></button>';
      editableThemes['bs3'].cancelTpl = '<button type="button" ng-click="$form.$cancel()" class="btn btn-default btn-with-icon"><i class="ion-close-round"></i></button>';


  }).
       error(function (data, status, headers, config) {

       });
        $scope.displayedCollection = [].concat($scope.rowCollection);


        $scope.statuses = [
          { value: 1, text: 'Good' },
          { value: 2, text: 'Awesome' },
          { value: 3, text: 'Excellent' },
        ];

        $scope.groups = [
          { id: 1, text: 'user' },
          { id: 2, text: 'customer' },
          { id: 3, text: 'vip' },
          { id: 4, text: 'admin' }
        ];

        $scope.showGroup = function (user) {
            if (user.group && $scope.groups.length) {
                var selected = $filter('filter')($scope.groups, { id: user.group });
                return selected.length ? selected[0].text : 'Not set';
            } else return 'Not set'
        };

        $scope.showStatus = function (user) {
            var selected = [];
            if (user.status) {
                selected = $filter('filter')($scope.statuses, { value: user.status });
            }
            return selected.length ? selected[0].text : 'Not set';
        };




    }

})();
