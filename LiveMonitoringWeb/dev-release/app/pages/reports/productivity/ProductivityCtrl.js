
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('ProductivityCtrl', ProductivityCtrl);

    /** @ngInject */
    function ProductivityCtrl($scope, $http, baConfig, $element, layoutPaths) {
        debugger;
        var layoutColors = baConfig.colors;
        var id = $element[0].getAttribute('id');

        var baseSiteUrlPath = $("base").first().attr("href");

        $http.get(baseSiteUrlPath + "Report/JsonProductiveReport", { data: {} }).
  success(function (result, status, headers, config) {

      debugger;

      var label = result.map(function (a) { return a.User; });
      var NonProdData = result.map(function (a) { return a.NonProductive; });
      var ProdData = result.map(function (a) { return a.Productive; });

      $scope.labels = label;
      $scope.type = 'StackedBar';
      $scope.series = ['NonProductive', 'Productive'];
      $scope.options = {
          scales: {
              xAxes: [{
                  stacked: true,
              }],
              yAxes: [{
                  stacked: true
              }]
          }
      };

      $scope.data = [
        NonProdData,
        ProdData
      ];


  }).
       error(function (data, status, headers, config) {

       });

    }
})();