(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin')
        .controller('ConfigurationCtrl', ConfigurationCtrl);

    /** @ngInject */
    function ConfigurationCtrl($scope, $filter, $http, editableOptions, editableThemes, SweetAlert) {

        $scope.rowCollection = [];

        var baseSiteUrlPath = $("base").first().attr("href");

        $scope.fillGrid = function () {

            $http.get(baseSiteUrlPath + "Configuration/JsonConfigurations", { data: {} }).
      success(function (data, status, headers, config) {
          debugger;
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
        }
        $scope.displayedCollection = [].concat($scope.rowCollection);

        $scope.fillGrid();

        $scope.update = function (data) {

            debugger;

            $http.post(baseSiteUrlPath + "Configuration/Save", JSON.stringify(data)).
        success(function (data, status, headers, config) {


            $scope.fillGrid();
        }).
       error(function (data, status, headers, config) {

       });
        }

        $scope.delete = function (item) {

           
            debugger;
            SweetAlert.swal({
                title: "Are you sure want to delete?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "No, cancel plx!",
                closeOnConfirm: true,
                closeOnCancel: true
            },
function (isConfirm) {
    if (isConfirm) {
        var ID = item.ConfigurationId;

        $http.post(baseSiteUrlPath + "Configuration/Delete", { id: ID }).
success(function (data, status, headers, config) {
    debugger;

    $scope.fillGrid();
}).
    error(function (data, status, headers, config) {

    });
    } else {

    }
});
        }
    }
            

})();
