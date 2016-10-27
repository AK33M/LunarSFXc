(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("ImagesCtrl", ["$scope", "dataResource", "Lightbox", ImagesCtrl]);

    function ImagesCtrl($scope, dataResource, Lightbox) {
        dataResource.images.getAll({ containerName: 'imagesupload' },
            function (data) {
                $scope.imagesSrcs = data.images;
            });

        $scope.openLightboxModal = function (index) {
            Lightbox.openModal($scope.imagesSrcs, index);
        };
    }
}());