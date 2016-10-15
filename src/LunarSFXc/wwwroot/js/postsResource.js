(function () {
    "use strict";

    angular
        .module("services")
        .factory("postsResource", ["$resource", postsResource]);

    function postsResource($resource) {
        return $resource("api/posts/:id");
    };
}());