
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('SitesDetailsCtrl', SitesDetailsCtrl);

    /** @ngInject */
    function SitesDetailsCtrl($scope,$http, baConfig, $element, layoutPaths) {
        debugger;
        var layoutColors = baConfig.colors;
        var id = $element[0].getAttribute('id');

        var baseSiteUrlPath = $("base").first().attr("href");

        $http.get(baseSiteUrlPath + "Report/JsonSitesReport", { data: {} }).
  success(function (result, status, headers, config) {

      debugger;

        var barChart = AmCharts.makeChart(id, {
            type: 'serial',
            theme: 'blur',
            color: layoutColors.defaultText,
            dataProvider: result,
            valueAxes: [
              {
                  axisAlpha: 0,
                  position: 'left',
                  title: 'Sites Frequency',
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
                  valueField: 'Count'
              }
            ],
            chartCursor: {
                categoryBalloonEnabled: false,
                cursorAlpha: 0,
                zoomable: false
            },
            categoryField: 'Domain',
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