(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("ImagesCtrl", ["$scope", "dataResource", ImagesCtrl]);

    function ImagesCtrl($scope, dataResource) {
        dataResource.images.getAll({ containerName: 'imagesupload' },
            function (data) {
                $scope.imagesSrcs = data;
            });
    }
}());