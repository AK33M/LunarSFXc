(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("ImagesCtrl", ["$scope", "dataResource", "Lightbox", ImagesCtrl]);

    function ImagesCtrl($scope, dataResource, Lightbox) {

        var images = [];

        dataResource.images.getAll({ containerName: 'imagesupload' },
            function (data) {
                $scope.imagesSrcs = data.images;
                data.images.map(function (item) {
                    images.push(item.imageUri);
                });
            });


        $scope.openLightboxModal = function (index) {
            Lightbox.openModal(images, index);
        };
    }
}());