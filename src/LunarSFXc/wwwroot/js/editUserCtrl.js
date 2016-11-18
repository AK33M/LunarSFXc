(function () {
    "use strict";

    angular
       .module("app-admin")
       .controller("EditUserCtrl", [
                                    "$scope",
                                    "$location",
                                    "$routeParams",
                                    "$uibModal",
                                    "$log",
                                    "dataResource",
                                    "FileUploader",
                                    EditUserCtrl
       ]);

    function EditUserCtrl($scope, $location, $routeParams, $uiModal, $log, dataResource, FileUploader) {
        var vm = this;

        $scope.go = function (path) {
            $location.path(path);
        };

        $scope.alerts = [];

        $scope.userToEdit = dataResource.users.get({ userName: $routeParams.UserName });

        $log.log($scope.userToEdit);

    }
}());