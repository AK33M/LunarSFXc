(function () {
    "use strict";

    angular
        .module("services")
        .factory("dataResource", ["$resource", dataResource]);

    function dataResource($resource) {
        return {
            posts: $resource("api/posts/:action/:id/:year/:month/:title", {}, {
                'getAll': { method: 'GET', params: { id: '@paginationOptions' }, isArray: false },
                'getPost': { method: 'GET', params: { action: 'post', year: '@blogPostId.year', month: '@blogPostId.month', title: '@blogPostId.title' }, isArray: false },
                'getCategories': { method: 'GET', params: { action: 'categories' }, isArray: false },
                'getTags': { method: 'GET', params: { action: 'tags' }, isArray: false },
                'save': { method: 'POST', params: { action: 'save' } },
                'getCategory': { method: 'GET', params: { action: 'category', id: '@categoryId' }, isArray: false },
                'saveCategory': { method: 'POST', params: { action: 'saveCategory' } },
            }),
            images: $resource("api/images/:action", {}, {
                'getAll': { method: 'GET', params: { action: 'list', containerName: '@containerName' }, isArray: false },
                'upload': { method: 'POST', url: "/upload" }
            }),
            aboutme: $resource("api/aboutme/:action/:id", {}, {
                'getAll': { method: 'GET', params: { action: 'events' }, isArray: false },
                'get': { method: 'GET', params: { action: 'event', id: '@id' }, isArray: false },
                'save': { method: 'POST', params: { action: 'save' } },
                'delete': { method: 'DELETE', params: { action: 'delete', id: '@id' } }
            })
        };
    }
}());