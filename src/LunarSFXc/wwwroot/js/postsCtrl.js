(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("PostsCtrl", ["postsResource", PostsCtrl]);

    function PostsCtrl(postsResource) {
        var vm = this;

        postsResource.query(function (data) {
            vm.posts = data;
        });
    }

}());