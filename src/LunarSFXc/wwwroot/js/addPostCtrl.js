(function () {
    "use strict";


    angular
        .module("app-admin")
        .controller("AddPostCtrl", ["$scope", "$log", "$location", "$routeParams", "dataResource", "FileUploader", AddPostCtrl]);

    function AddPostCtrl($scope, $log, $location, $routeParams, dataResource, FileUploader) {
        var vm = this;

        dataResource.posts.getCategories(function (data) {
            //success
            $scope.categories = data.categories
        }, function (error) {
            //error
            $log.log(error);
        });

        dataResource.posts.getTags(function (data) {
            //success
            $scope.tags = data.tags
        }, function (error) {
            //error
            $log.log(error);
        });

        $scope.go = function (path) {
            $location.path(path);
        };

        //AngularFileUpload http://nervgh.github.io/pages/angular-file-upload/examples/simple/
        // Creates a uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            url: 'api/images/upload',
            queueLimit: 1,
            removeAfterUpload: true,
            autoUpload: true,
            formData: [{
                containerName: 'aboutmetimeline'
            }]
        });

        // Registers a filter: images only
        uploader.filters.push({
            name: 'imageFilter',
            fn: function (item, options) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            }
        });

        //CALL BACKS
        uploader.onProgressItem = function (fileItem, progress) {
            //console.info('onProgressItem', fileItem, progress);
        };

        uploader.onSuccessItem = function (fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
            $scope.aboutMeEvent.image = response.image;
        };

        uploader.onErrorItem = function (fileItem, response, status, headers) {
            //console.info('onErrorItem', fileItem, response, status, headers);
        };
    };
}());