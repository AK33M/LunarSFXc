(function(){
    "use strict";


    angular
        .module("app-admin")
        .controller("AddPostCtrl", ["$scope", "postsResource", AddPostCtrl]);

    function AddPostCtrl($scope, postsResource) {
        var vm = this;
        $scope.greeting = "Hello Form";
    };
}());