/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
(function () {
  'use strict';

  angular.module('BlurAdmin.pages.dashboard')
      .controller('TopappChartCtrl', TopappChartCtrl);


  /** @ngInject */
  function TopappChartCtrl($scope,$http, baConfig, colorHelper) {
      
      
      var baseSiteUrlPath = $("base").first().attr("href");

      $http.get(baseSiteUrlPath + "Home/JsonTopApps",{data:{}})
      .success(function (result,status,headers,config) {
          
          var lablel = [];
          var dta = [];
          var pre = [];

          var total = result.sum("Count");

          $scope.tot=total

          result.forEach(function (ele) {
              lablel.push(ele.AppName);
              dta.push(ele.Count);
              pre.push(Math.round(ele.Count / total * 100));
          });

          $scope.transparent = baConfig.theme.blur;
          var dashboardColors = baConfig.colors.dashboard;
          $scope.doughnutData = {
              labels: lablel,
              datasets: [
                  {
                      data: dta,
                      backgroundColor: [
                          dashboardColors.white,
                          dashboardColors.blueStone,
                          dashboardColors.surfieGreen,
                          dashboardColors.silverTree,


                      ],
                      hoverBackgroundColor: [
                          colorHelper.shade(dashboardColors.white, 15),
                          colorHelper.shade(dashboardColors.blueStone, 15),
                          colorHelper.shade(dashboardColors.surfieGreen, 15),
                          colorHelper.shade(dashboardColors.silverTree, 15),

                      ],
                      percentage: pre
                  }]
          };

          var ctx = document.getElementById('top-app-chart').getContext('2d');
          window.myDoughnut = new Chart(ctx, {
              type: 'doughnut',
              data: $scope.doughnutData,
              options: {
                  cutoutPercentage: 64,
                  responsive: true,
                  elements: {
                      arc: {
                          borderWidth: 0
                      }
                  }
              }
          });

      })
      .error(function (data, status, headers, config) {

      });

    
  }
})();