(function () {
    "use strict";

    angular
        .module("routing", ["services"])
        .config(
            ["$routeProvider", function ($routeProvider) {
                //$routeProvider.when("/", {
                //    controller: "tripsController as vm",
                //    templateUrl: "/views/tripsView.html"
                //});

                $routeProvider.when("/hello", {
                    controller: "",
                    templateUrl: "/views/hello.html"
                });

                $routeProvider.otherwise({
                    redirectTo: "/"
                });
            }]);
}());