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
        debugger;

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
           debugger;


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
                   debugger;
                   $route.reload();
               })
               .error(function (data, status, headers, config) {

               });

           } else {
               $scope.CustomDate = true;
           }
           
           
           
       }

       function initLocationCodes(geography) {
           var root = {
               id:"ieki",
               text: "Tutti",
               children: geography
           };
          
           $scope.treeLocationCodes.config.version++;
       }

       function init(data) {
           initLocationCodes(data);
       };


       $scope.locationCodes = [];

       $scope.treeLocationCodes = {
           config: {
               core: {
                   animation: true,
                   themes: {
                       icons: false
                   },
                   worker: true
               },
               version: 0,
               
           },
           instance: null
           
          
       };

       $scope.getDefaultData =function(){
          
        
           var baseSiteUrlPath = $("base").first().attr("href");

           $http.get(baseSiteUrlPath + 'ScreenShot/GetTreelist', { data: {} })
                .success(function (data, status, heades, config) {
                    debugger;
                   // return data;
                 //   $scope.ignoreChanges = true;
                    init([
       {
           "id": "n1",
           "parent": "#",
           "type": "folder",
           "text": "Node 1",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n2",
           "parent": "#",
           "type": "folder",
           "text": "Node 2",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n3",
           "parent": "#",
           "type": "folder",
           "text": "Node 3",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n5",
           "parent": "n1",
           "text": "Node 1.1",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n6",
           "parent": "n1",
           "text": "Node 1.2",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n7",
           "parent": "n1",
           "text": "Node 1.3",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n8",
           "parent": "n1",
           "text": "Node 1.4",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n9",
           "parent": "n2",
           "text": "Node 2.1",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n10",
           "parent": "n2",
           "text": "Node 2.2 (Custom icon)",
           "icon": "ion-help-buoy",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n12",
           "parent": "n3",
           "text": "Node 3.1",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n13",
           "parent": "n3",
           "type": "folder",
           "text": "Node 3.2",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n14",
           "parent": "n13",
           "text": "Node 3.2.1",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n15",
           "parent": "n13",
           "text": "Node 3.2.2",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n16",
           "parent": "n3",
           "text": "Node 3.3",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n17",
           "parent": "n3",
           "text": "Node 3.4",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n18",
           "parent": "n3",
           "text": "Node 3.5",
           "state": {
               "opened": true
           }
       },
       {
           "id": "n19",
           "parent": "n3",
           "text": "Node 3.6",
           "state": {
               "opened": true
           }
       }
                    ]);
                })
                .error(function (data, status, headers, config) {

                });
       }

       debugger;
     
        // $scope.treeData = 
       $scope.getDefaultData();
       
    }
})();


