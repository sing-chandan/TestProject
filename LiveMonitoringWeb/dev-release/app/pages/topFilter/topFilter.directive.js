/**
 * @author v.lugovksy
 * created on 16.12.2015
 */
(function () {
    'use strict';

    angular.module('BlurAdmin.pages')
        .directive('topFilter', topFilter);

    /** @ngInject */
    function topFilter() {
        return {
            restrict: 'E',
            controller: 'TopFilterCtrl',
            templateUrl: 'dev-release/app/pages/topFilter/topFilter.html'
        };
    }
})();