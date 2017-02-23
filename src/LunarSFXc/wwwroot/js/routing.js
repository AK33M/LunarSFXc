(function () {
    "use strict";

    angular
        .module("routing", ["services"])
        .config([
                    "$stateProvider",
                    "$urlRouterProvider",
                        function ($stateProvider, $urlRouterProvider) {
                            $urlRouterProvider.otherwise("/posts");

                            $stateProvider.state("posts", {
                                url: "/posts",
                                controller: "PostsCtrl as vm",
                                templateUrl: "/views/posts.html"
                            });

                            $stateProvider.state("images", {
                                url: "/images",
                                controller: "ImagesCtrl as vm",
                                templateUrl: "/views/images.html"
                            });

                            $stateProvider.state("aboutme", {
                                url: "/aboutme",
                                controller: "AboutMeCtrl as vm",
                                templateUrl: "/views/about-me-events.html"
                            });

                            $stateProvider.state("projects", {
                                url: "/projects",
                                controller: "ProjectsCtrl as vm",
                                templateUrl: "/views/projects.html"
                            });

                            $stateProvider.state("users", {
                                url: "/users",
                                controller: "UsersCtrl as vm",
                                templateUrl: "/views/users.html"
                            });

                            $stateProvider.state("add-post", {
                                url: "/add-post/:isNew?",
                                controller: "AddPostCtrl as vm",
                                templateUrl: "/views/add-post.html",
                                resolve: {
                                    dataResource: "dataResource",
                                    blogPostService: "blogPostService",
                                    post: function (dataResource, blogPostService) {
                                        return dataResource.posts.getPost(blogPostService.getBlogPostId(), function (data) {
                                            //Success
                                            return data.$promise;
                                        }, function (error) {
                                            //Error
                                            return error;
                                        });
                                    },
                                    categories: function (dataResource) {
                                        return dataResource.posts.getCategories(function (data) {
                                            //success
                                            return data.$promise;
                                        }, function (error) {
                                            //error
                                            return error;
                                        });
                                    },
                                    tags: function (dataResource) {
                                        return dataResource.posts.getTags(function (data) {
                                            //success
                                            return data.$promise;
                                        }, function (error) {
                                            //error
                                            return error;
                                        });
                                    }
                                }
                            });

                            $stateProvider.state("add-images", {
                                url: "/add-images",
                                controller: "AddImagesCtrl as vm",
                                templateUrl: "/views/add-images.html"
                            });

                            $stateProvider.state("add-about-me", {
                                url: "/add-aboutme/:Id?",
                                controller: "AddAboutMeCtrl as vm",
                                templateUrl: "/views/add-about-me-event.html"
                            });

                            $stateProvider.state("add-project", {
                                url: "/add-project/:Id?",
                                controller: "AddProjectCtrl as vm",
                                templateUrl: "/views/add-project.html"
                            });

                            $stateProvider.state("add-category", {
                                url: "/add-category",
                                controller: "AddCategoryCtrl as vm",
                                templateUrl: "/views/add-category.html"
                            });

                            $stateProvider.state("edit-user", {
                                url: "/edit-user/:UserName?",
                                controller: "EditUserCtrl as vm",
                                templateUrl: "/views/edit-user.html"
                            });

                        }]);
}());