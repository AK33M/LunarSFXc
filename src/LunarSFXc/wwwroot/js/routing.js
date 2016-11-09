(function () {
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

                            $routeProvider.when("/images", {
                                controller: "ImagesCtrl as vm",
                                templateUrl: "/views/images.html"
                            });

                            $routeProvider.when("/aboutme", {
                                controller: "AboutMeCtrl as vm",
                                templateUrl: "/views/about-me-events.html"
                            });

                            $routeProvider.when("/add-post/:isNew?", {
                                controller: "AddPostCtrl as vm",
                                templateUrl: "/views/add-post.html"
                            });

                            $routeProvider.when("/add-images", {
                                controller: "AddImagesCtrl as vm",
                                templateUrl: "/views/add-images.html"
                            });

                            $routeProvider.when("/add-aboutme/:Id?", {
                                controller: "AddAboutMeCtrl as vm",
                                templateUrl: "/views/add-about-me-event.html"
                            });

                            $routeProvider.when("/add-category", {
                                controller: "AddCategoryCtrl as vm",
                                templateUrl: "/views/add-category.html"
                            });

                            $routeProvider.otherwise({
                                redirectTo: "/posts"
                            });
                        }]);
}());