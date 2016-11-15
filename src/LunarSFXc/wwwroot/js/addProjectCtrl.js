(function () {
    "use strict";
    
    angular
       .module("app-admin")
       .controller("AddProjectCtrl", [
                                        "$scope",
                                        "$location",
                                        "$routeParams",
                                        "$uibModal",
                                        "$log",
                                        "dataResource",
                                        "FileUploader",
                                        AddProjectCtrl]);

    function AddProjectCtrl($scope, $location, $routeParams, $uibModal, $log, dataResource, FileUploader) {
        var vm = this;

        $scope.go = function (path) {
            $location.path(path);
        };

        $scope.alerts = [];

        var PortfolioProject = dataResource.projects.get({ id: $routeParams.Id == null ? 0 : $routeParams.Id });
        $scope.portfolioProject = PortfolioProject;

        dataResource.posts.getCategories(function (data) {
            //success
            $scope.categories = data.categories
        }, function (error) {
            //error
            $log.log(error);
        });

        $scope.saveProject = function () {
            $scope.portfolioProject.$save(function (response) {
                //success callback
                $location.path('/projects');
            }, function (error) {
                //error callback 
                $scope.alerts.push({
                    type: 'danger', msg: error.data.message
                });
                var errorMessage = '';
                angular.forEach(error.data.modelState, function (value, key) {
                    errorMessage = value.errors[0].errorMessage;
                    $scope.alerts.push({
                        type: 'danger', msg: errorMessage
                    });
                });
            });
        };

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

        $scope.removeImage = function () {
            $scope.portfolioProject.image = null;
        };

        $scope.deleteProject = function (projectId) {
            dataResource.projects.delete({ id: $routeParams.Id }, function (response) {
                //success
                $scope.go('/projects');
            }, function (error) {
                //error
                $scope.alerts.push({
                    type: 'danger', msg: error.data.message
                });
            });
        };

        $scope.open = function (projectId, size, parentSelector) {
            var parentElem = parentSelector ?
              angular.element($document[0].querySelector('#portfolioProjectForm' + parentSelector)) : undefined;
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/confirmDelete.html',
                controller: 'ModalInstanceCtrl',
                controllerAs: '$ctrl',
                size: 'sm',
                appendTo: parentElem,
                resolve: {
                    item: function () {
                        return projectId;
                    }
                }
            });

            modalInstance.result.then(function (projectId) {
                //if OK/Agreed
                $scope.deleteProject(projectId);
            }, function () {
                //if Cancel/Not Agreed/Dismissed Modal
                $log.info('modal-component dismissed at: ' + new Date());
            });
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
                containerName: 'portfolio'
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
            //console.info('onSuccessItem', fileItem, response, status, headers);
            $scope.portfolioProject.image = response.image;
        };

        uploader.onErrorItem = function (fileItem, response, status, headers) {
            //console.info('onErrorItem', fileItem, response, status, headers);
        };

    }

}());