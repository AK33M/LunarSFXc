(function () {
    "use strict";

    angular
        .module("services",
            [
                "ngResource",
                "ngMessages",
                "ngRoute",
                "ngAnimate",
                "ngSanitize",
                "ngTouch",
                "ui.grid",
                "ui.grid.pagination",
                "ui.grid.selection",
                "angularFileUpload",
                "ui.bootstrap",
                "bootstrapLightbox",
                "ui.select"
            ])
        .service("blogPostService",
            function ($filter) {
                var blogPostId = {};

                //functions
                var getBlogPostId = function () {
                    return blogPostId;
                }

                var setBlogPostId = function (value) {
                    blogPostId.year = $filter('date')(value.PostedOn, 'yyyy');
                    blogPostId.month = $filter('date')(value.PostedOn, 'MM');
                    blogPostId.title = value.UrlSlug;
                }

                return {
                    getBlogPostId: getBlogPostId,
                    setBlogPostId: setBlogPostId
                };
        });
    //.constant("appSettings",
    //{
    //    serverPath: ""
    //})
    //.service("", function () { });
}());