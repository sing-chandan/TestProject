(function () {
    'use strict';

    angular.module('BlurAdmin.pages.admin')
        .controller('SubCategoryCtrl', SubCategoryCtrl);

    /** @ngInject */
    function SubCategoryCtrl($scope, $filter, $http, editableOptions, editableThemes, SweetAlert) {

        
        $scope.rowCollection = [];
        $scope.Categorylist = [];
        $scope.SubCatType = [];

        $scope.smartTablePageSize = 20;

        var baseSiteUrlPath = $("base").first().attr("href");

        $scope.fillGrid=function(){
        $http.get(baseSiteUrlPath + "SubCategory/JsonSubCategoryData", { data: {} }).
  success(function (data, status, headers, config) {

      



      $scope.rowCollection = data.SubCategory;

      $scope.Categorylist = data.Categorylist;

      $scope.SubCatType = data.SubCategoryType;

      $scope.removeUser = function (index) {
          $scope.rowCollection.splice(index, 1);
      };

     


      $scope.showCategory = function (user) {

          
          var selected = [];
          if (user.CategoryId) {
              selected = $filter('filter')($scope.Categorylist, { CategoryId: user.CategoryId });
          }
          return selected.length ? selected[0].CategoryName : 'Not set';
      };


      $scope.showSubCatType = function (user) {
          var selected = [];
          if (user.SubCategoryTypeId) {
              selected = $filter('filter')($scope.SubCatType, { SubCategoryTypeId: user.SubCategoryTypeId });
          }
          return selected.length ? selected[0].SubCategoryTypeName : 'Not set';
      };


      editableOptions.theme = 'bs3';
      editableThemes['bs3'].submitTpl = '<button type="submit" class="btn btn-primary btn-with-icon"><i class="ion-checkmark-round"></i></button>';
      editableThemes['bs3'].cancelTpl = '<button type="button" ng-click="$form.$cancel()" class="btn btn-default btn-with-icon"><i class="ion-close-round"></i></button>';




  }).
       error(function (data, status, headers, config) {

       });
        }

        $scope.SubCatAdd = function () {
            $scope.AddRow = true;
        }

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
        var ID = item.SubCategoryId;

        $http.post(baseSiteUrlPath + "SubCategory/Delete", { id: ID }).
success(function (data, status, headers, config) {
    

    $scope.fillGrid();
}).
    error(function (data, status, headers, config) {

    });
    } else {

    }
});



        }

        $scope.displayedCollection = [].concat($scope.rowCollection);


        $scope.AddSubCatgory = function (AddObj) {
            var SubCat = JSON.parse(angular.toJson(AddObj));

            $http.post(baseSiteUrlPath + "SubCategory/Save", JSON.stringify(SubCat)).
    success(function (data, status, headers, config) {
        
        $scope.AddRow = false;
        $scope.fillGrid();

    }).
        error(function (data, status, headers, config) {

        });
        }

        $scope.SubmitSubCategory = function (data, item) {
            
            data.SubCategoryId= item.SubCategoryId ;
            
            $http.defaults.headers.common['X-XSRF-Token'] =angular.element('input[name="__RequestVerificationToken"]').attr('value');

            $http.post(baseSiteUrlPath + "SubCategory/Save", JSON.stringify(data)).
  success(function (data, status, headers, config) {

      
      return false;

  }).
       error(function (data, status, headers, config) {

       });
        }
       
        $scope.addUser = function () {
            
            $scope.inserted = {
                id: $scope.rowCollection.length + 1,
                CategoryName: null,
                SubCategoryTypeName: null,
                SubCategoryName: null
            };
            $scope.rowCollection.unshift($scope.inserted);
            $scope.displayedCollection.unshift($scope.inserted);
        };


    }

})();
