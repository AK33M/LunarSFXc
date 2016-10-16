﻿(function () {
    "use strict";

    angular
        .module("routing", ["services"])
        .config([
                    "$routeProvider",
                        function ($routeProvider) {
                            //$routeProvider.when("/", {
                            //    controller: "tripsController as vm",
                            //    templateUrl: "/views/tripsView.html"
                            //});

                            $routeProvider.when("/posts", {
                                controller: "PostsCtrl as vm",
                                templateUrl: "/views/posts.html"
                            });

                            $routeProvider.otherwise({
                                redirectTo: "/"
                            });
                        }]);
}());