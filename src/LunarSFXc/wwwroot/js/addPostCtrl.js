(function(){
    "use strict";


    angular
        .module("app-admin")
        .controller("AddPostCtrl", ["$scope", "dataResource", AddPostCtrl]);

    function AddPostCtrl($scope, dataResource) {
        var vm = this;
        $scope.greeting = "Hello Form";
    };
}());