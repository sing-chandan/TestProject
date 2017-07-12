/**
 * @author p.maslava
 * created on 28.11.2016
 */
(function () {
    'use strict';

    angular.module('BlurAdmin.pages')
      .controller('TopFilterCtrl', TopFilterCtrl);

    /** @ngInject */
    function TopFilterCtrl($scope, $http, $route, $filter, $uibModal, baProgressModal) {
        

        $scope.CustomDate = false;
       
        $scope.standardItem = {};
       $scope.standardSelectItems = [
     { label: 'Today', value: 0 },
          { label: 'Yesterday', value: -1 },
          { label: 'Last 7 Day', value: -7},
          { label: 'Last 30 Day', value: -30},
       { label: 'Custom Date', value: "0"}
        ];
      

       $scope.standardItem.selected = { label: 'Today', value: 1 };

       $scope.customDate = function () {
           


       }

       $scope.open = function (page, size) {
           $uibModal.open({
               animation: true,
               templateUrl: page,
               size: size,
               resolve: {
                   items: function () {
                       return $scope.items;
                   }
               }
           });
       };
       $scope.openProgressDialog = baProgressModal.open;
    
      // $scope.selectedItem.selected = $scope.standardSelectItems[0];

       $scope.datechange = function (item) {
           
           $scope.ignoreChanges = false;
           

           var baseSiteUrlPath = $("base").first().attr("href");
           
           var SelectedDate = $scope.standardItem.selected.value;

           var DateTo = "";
           var DateFrom = "";

           if (SelectedDate != "0") {

               var Today = new Date();

               DateTo = $filter('date')(Today, 'MM/dd/yyyy')

               var From = new Date(Today.setDate(Today.getDate() + SelectedDate));

               DateFrom = $filter('date')(From, 'MM/dd/yyyy');


               DateTo = DateTo + " 23:59:59";
               DateFrom = DateFrom + " 00:00:00";

               $http.post(baseSiteUrlPath + 'Home/SetTimeInterval', { DateFrom: DateFrom, DateTo: DateTo })
               .success(function (data, status, heades, config) {
                   
                   $route.reload();
               })
               .error(function (data, status, headers, config) {

               });

           } else {
               $scope.CustomDate = true;
           }
           
           
           
       }

     


       $scope.roleList1 = [
      {
          "roleName": "User", "roleId": "role1", "children": [
          { "roleName": "subUser1", "roleId": "role11", "children": [] },
          { "roleName": "subUser1", "roleId": "role11", "children": [] }
          
          ]
      }
             ];


       $scope.getDefaultData =function(){
          
        
           var baseSiteUrlPath = $("base").first().attr("href");

           $http.get(baseSiteUrlPath + 'ScreenShot/GetTreelist', { data: {} })
                .success(function (data, status, heades, config) {
                    debugger;
                   // return data;
                    //   $scope.ignoreChanges = true;

                    $scope.roleList = $scope.treedata(data);
                    
                })
                .error(function (data, status, headers, config) {

                });
       }

       
       $scope.treedata = function (treedata) {
           var TreeArray = [];
           for (var _t = 0; (_t < treedata.length && treedata[_t].parent==0) ; _t++) {
               var treeobj = {};
               treeobj.roleName = treedata[_t].text;
               treeobj.roleId = treedata[_t].id;
               treeobj.children = [];
               for(var _i = 0; (_i < treedata.length) ; _i++){
                   if (treedata[_i].parent == treedata[_t].id) {
                       var treechildobj = {};
                       treechildobj.roleName = treedata[_i].text;
                       treechildobj.roleId = treedata[_i].id;
                       treechildobj.children = [];
                       treeobj.children.push(treechildobj);
                   }
               }
               TreeArray.push(treeobj);
           }

           return TreeArray;
       }
     
        // $scope.treeData = 
       $scope.getDefaultData();
     
       $scope.selectNodeLabel = function (a, b) {
           debugger;
           alert('test');
       }
      
    }
})();


