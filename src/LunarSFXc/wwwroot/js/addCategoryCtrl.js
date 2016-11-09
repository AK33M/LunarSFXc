(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("AddCategoryCtrl", [
            "$scope",
            "$log",
            "$location",
            "$routeParams",
            "blogPostService",
            "dataResource",
            AddCategoryCtrl]);

    function AddCategoryCtrl($scope, $log, $location, $routeParams, blogPostService, dataResource) {
        var vm = this;

        dataResource.posts.getCategory(function (data) {
            //Success
            $scope.category = data
        }, function (error) {
            //Error
        });

        //These methods are pretty universal.. maybe look into directives?
        $scope.writeUrlSlug = function (input) {
            $scope.category.urlSlug = input.replace(/[\. ,:-]+/g, '_').toLowerCase()
        };

        $scope.go = function (path) {
            $location.path(path);
        };

        $scope.saveCategory = function () {
            $scope.category.$saveCategory(function (response) {
                $scope.go('posts');

            }, function (error) {

            });
        };
    };

}());