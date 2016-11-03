(function () {
    "use strict";

    angular
        .module("services")
        .factory("dataResource", ["$resource", dataResource]);

    function dataResource($resource) {
        return {
            posts: $resource("api/posts/:action/:id", {}, {
                'getCategories': { method: 'GET', params: { action: 'categories' }, isArray: false },
                'getTags': { method: 'GET', params: { action: 'tags' }, isArray: false }
            }),
            images: $resource("api/images/:action", {}, {
                'getAll': { method: 'GET', params: { action: 'list', containerName: '@containerName' }, isArray: false },
                'upload': { method: 'POST', url: "/upload" }
            }),
            aboutme: $resource("api/aboutme/:action/:id", {}, {
                'getAll': { method: 'GET', params: { action: 'events' }, isArray: false },
                'get': { method: 'GET', params: { action: 'event', id: '@id' }, isArray: false },
                'post': { method: 'POST', params: { action: 'post' } },
                'delete': { method: 'DELETE', params: { action: 'delete', id: '@id' } }
            })
        };
    }
}());