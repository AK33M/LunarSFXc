(function () {
    "use strict";


    angular
        .module("app-admin")
        .controller("AddPostCtrl", ["$scope", "$log", "$location", "$routeParams", "blogPostService", "dataResource", "FileUploader", AddPostCtrl]);

    function AddPostCtrl($scope, $log, $location, $routeParams, blogPostService, dataResource, FileUploader) {
        var vm = this;

        if ($routeParams.isNew) {
            blogPostService.setBlogPostId({});
            $scope.readonlyTitle = false;
        } else {
            $scope.readonlyTitle = true;
        }

        $scope.writeUrlSlug = function (input) {
            $scope.blogPost.urlSlug = input.replace(/[\. ,:-]+/g, '_').toLowerCase()
        };

        dataResource.posts.getPost(blogPostService.getBlogPostId(), function (data) {
            //Success
            $scope.blogPost = data
        }, function (error) {
            //Error
        });

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

        $scope.tagTransform = function (newTag) {
            var tag = {
                name: newTag,
                description: "About " + newTag,
                urlSlug: newTag.replace(/[\. ,:-]+/g, '-').toLowerCase()//OR this /[\. ,:-]+/
            };
            return tag;
        };

        $scope.onSelectCallBack = function (item, model) {
            //$log.log(item);
            $log.log($scope.blogPost.tags);
        };

        $scope.saveBlogPost = function () {
            //$log.log($scope.blogPost);
            $scope.blogPost.$save(function (response) {
                $scope.go('posts');
            }, function (error) {

            });
        };

        $scope.removeImage = function ($index) {
            //$log.log($index);
            $scope.blogPost.images.splice($index, 1);
        };


        //AngularFileUpload http://nervgh.github.io/pages/angular-file-upload/examples/simple/
        // Creates a uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            url: 'api/images/upload',
            queueLimit: 3,
            removeAfterUpload: true,
            autoUpload: false,
            formData: [{
                containerName: 'blogphotos'
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
            $scope.blogPost.images.push(response.image);
        };

        uploader.onErrorItem = function (fileItem, response, status, headers) {
            //console.info('onErrorItem', fileItem, response, status, headers);
        };
    };
}());