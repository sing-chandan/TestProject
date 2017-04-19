debugger;
(function () {
    debugger;
    var app = angular.module('Blurtest', ['BlurAdmin','ngRoute']);


    //Array Prototype

    Array.prototype.sum = function (prop) {
        var total = 0
        for (var i = 0, _len = this.length; i < _len; i++) {
            total += this[i][prop]
        }
        return total
    }

    debugger;
    app.config(
    ['$routeProvider', '$locationProvider',

        function ($routeProvider, $locationProvider) {

            debugger;

            
            var baseSiteUrlPath = $("base").first().attr("href");
            debugger;
            
            $routeProvider.when('/',
            {

                templateUrl: baseSiteUrlPath + 'dev-release/index.html'
                //templateUrl: baseSiteUrlPath + 'dev-release/index.html'
                

            }).otherwise({
                templateUrl: baseSiteUrlPath + 'dev-release/index.html'
               // templateUrl: baseSiteUrlPath + 'dev-release/index.html'
            });

            $locationProvider.html5Mode(true);

        }]);

})();

