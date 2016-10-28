﻿(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("AddAboutMeCtrl", ["$scope", "$location", "dataResource", "FileUploader", AddAboutMeCtrl]);

    function AddAboutMeCtrl($scope, $location, dataResource, FileUploader) {
        var vm = this;
        var TimelineEvent = dataResource.aboutme.get({ id: 0 });
        $scope.aboutMeEvent = TimelineEvent;

        $scope.saveEvent = function () {
            $scope.aboutMeEvent.$post(function (response) {
                //success callback
                $location.path('aboutme');
            }, function (error) {
                //error callback 
                $scope.alerts = [{ type: 'danger', msg: 'Oops!: ' + error.data.message }];
            });
        };

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };


        //AngularFileUpload http://nervgh.github.io/pages/angular-file-upload/examples/simple/
        // Creates a uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            url: 'api/images/upload',
            queueLimit: 1,
            removeAfterUpload: true,
            autoUpload: true,
            formData: [{ containerName: 'aboutmetimeline' }]
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
    }
}());
