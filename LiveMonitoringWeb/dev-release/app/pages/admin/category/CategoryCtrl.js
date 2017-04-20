(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin')
        .controller('CategoryCtrl', CategoryCtrl);

    /** @ngInject */
    function CategoryCtrl($scope, $filter, $http, editableOptions, editableThemes, SweetAlert) {

        $scope.rowCollection = [];
        $scope.AddRow = false;

        $scope.Category = {};

    var baseSiteUrlPath = $("base").first().attr("href");

   
    $scope.fillGrid = function () {
        $http.get(baseSiteUrlPath + "Category/JsonCategory", { data: {} }).
  success(function (data, status, headers, config) {
      debugger;
      $scope.rowCollection = data;

      $scope.removeUser = function (index) {
          $scope.rowCollection.splice(index, 1);
      };

      $scope.CatAdd = function () {
          $scope.AddRow = true;
      }
      
      $scope.btnCancel = function () {
          $scope.Category = undefined;
          $scope.AddRow = false;
      }

      editableOptions.theme = 'bs3';
      editableThemes['bs3'].submitTpl = '<button type="submit" class="btn btn-primary btn-with-icon"><i class="ion-checkmark-round"></i></button>';
      editableThemes['bs3'].cancelTpl = '<button type="button" ng-click="$form.$cancel()" class="btn btn-default btn-with-icon"><i class="ion-close-round"></i></button>';


  }).
       error(function (data, status, headers, config) {

       });
    }

        $scope.displayedCollection = [].concat($scope.rowCollection);

        $scope.fillGrid();

        $scope.deactive = function (item) {
           

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
        var ID = item.CategoryId;

        $http.post(baseSiteUrlPath + "Category/Delete", { id: ID }).
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
        

        $scope.addUser = function () {

            
            $scope.inserted = {
                CategoryName: null,
                IsActive: false,
                IsBlocked: false,
                IsDeleted: false
            };
            $scope.rowCollection.unshift($scope.inserted);
        };

        $scope.AddCatgory = function (AddObj) {
            debugger;
            var Category = JSON.parse(angular.toJson(AddObj));

            $http.post(baseSiteUrlPath + "Category/Save", JSON.stringify(Category)).
    success(function (data, status, headers, config) {
        debugger;
        $scope.AddRow = false;
        $scope.fillGrid();

    }).
        error(function (data, status, headers, config) {

        });
        }

        $scope.updateUser = function (item) {
            debugger;
            //item.CreatedDate = null;
            //item.ModifiedDate = null;
            //item.DeletedDate = null;
            var Category = JSON.parse(angular.toJson(item));

            $http.post(baseSiteUrlPath + "Category/Save",JSON.stringify(Category) ).
 success(function (data, status, headers, config) {
     debugger;

     $scope.fillGrid();
 }).
        error(function (data, status, headers, config) {

        });

        }

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
