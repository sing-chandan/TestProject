
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('UserIdleCtrl', UserIdleCtrl);

    /** @ngInject */
    function UserIdleCtrl($scope, $http, baConfig, $element, layoutPaths) {
        
        var layoutColors = baConfig.colors;
        var id = $element[0].getAttribute('id');

        var baseSiteUrlPath = $("base").first().attr("href");

        $http.get(baseSiteUrlPath + "Report/JsonIdleReport", { data: {} }).
  success(function (result, status, headers, config) {

      

      var barChart = AmCharts.makeChart(id, {
          type: 'serial',
          theme: 'blur',
          color: layoutColors.defaultText,
          dataProvider: result,
          valueAxes: [
            {
                axisAlpha: 0,
                position: 'left',
                title: 'Idel Time (Min.)',
                gridAlpha: 0.5,
                gridColor: layoutColors.border,
            }
          ],
          startDuration: 1,
          graphs: [
            {
                balloonText: '<b>[[category]]: [[value]]</b>',
                fillColorsField: 'color',
                fillAlphas: 0.7,
                lineAlpha: 0.2,
                type: 'column',
                valueField: 'IdleMinute'
            }
          ],
          chartCursor: {
              categoryBalloonEnabled: false,
              cursorAlpha: 0,
              zoomable: false
          },
          categoryField: 'User',
          categoryAxis: {
              gridPosition: 'start',
              labelRotation: 45,
              gridAlpha: 0.5,
              gridColor: layoutColors.border,
          },
          export: {
              enabled: true
          },
          creditsPosition: 'top-right',
          pathToImages: layoutPaths.images.amChart
      });


  }).
       error(function (data, status, headers, config) {

       });

    }
})();