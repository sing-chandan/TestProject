
(function () {
    'use strict';

    angular.module('BlurAdmin.pages.reports')
        .controller('ScreenShotCtrl', ScreenShotCtrl);

    /** @ngInject */
    function ScreenShotCtrl($scope, $http, baConfig, $element, layoutPaths) {
        debugger;
      
   
        var baseSiteUrlPath = $("base").first().attr("href");

        $http.get(baseSiteUrlPath + "ScreenShot/Index", { data: {} }).
  success(function (result, status, headers, config) {

      debugger;

  }).
       error(function (data, status, headers, config) {

       });

    }
})();