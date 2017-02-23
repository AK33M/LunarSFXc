(function () {
    "use strict";


    angular
        .module("app-admin")
        .controller("AddPostCtrl", ["$log", "$location", "$stateParams", "blogPostService", "FileUploader", "post", "categories", "tags", AddPostCtrl]);

    function AddPostCtrl($log, $location, $stateParams, blogPostService, FileUploader, post, categories, tags) {
        var vm = this;
        vm.blogPost = post;
        vm.categories = categories.categories;
        vm.tags = tags.tags;

        if ($stateParams.isNew) {
            blogPostService.setBlogPostId({});
            vm.readonlyTitle = false;
        } else {
            vm.readonlyTitle = true;
        }

        vm.writeUrlSlug = function (input) {
            vm.blogPost.urlSlug = input.replace(/[\. ,:-]+/g, '_').toLowerCase()
        };

        vm.go = function (path) {
            $location.path(path);
        };

        vm.tagTransform = function (newTag) {
            var tag = {
                name: newTag,
                description: "About " + newTag,
                urlSlug: newTag.replace(/[\. ,:-]+/g, '-').toLowerCase()//OR this /[\. ,:-]+/
            };
            return tag;
        };

        vm.onSelectCallBack = function (item, model) {
            //$log.log(item);
            $log.log(vm.blogPost.tags);
        };

        vm.saveBlogPost = function () {
            //$log.log($scope.blogPost);
            vm.blogPost.$save(function (response) {
                vm.go('posts');
            }, function (error) {

            });
        };

        vm.removeImage = function ($index) {
            //$log.log($index);
            vm.blogPost.images.splice($index, 1);
        };

        //Tiny Mce
        vm.tinymceOptions = {
            plugins: 'link image code',
            toolbar: 'undo redo | bold italic | alignleft aligncenter alignright | code'
        };


        //AngularFileUpload http://nervgh.github.io/pages/angular-file-upload/examples/simple/
        // Creates a uploader
        var uploader = vm.uploader = new FileUploader({
            scope: vm,
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
            vm.blogPost.images.push(response.image);
        };

        uploader.onErrorItem = function (fileItem, response, status, headers) {
            //console.info('onErrorItem', fileItem, response, status, headers);
        };
    };
}());