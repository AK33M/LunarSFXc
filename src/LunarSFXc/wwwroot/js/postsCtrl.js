(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("PostsCtrl", ["$scope", "postsResource", PostsCtrl]);

    function PostsCtrl($scope, postsResource) {
        var vm = this;

        $scope.gridOptions1 = {
            enableSorting: true,
            columnDefs: [
                { field: 'Title' },
                { field: 'ShortDescription' },
                { field: 'Description' },
                { name: 'Category', field: 'Category.Name', enableSorting: false },
                { name: 'Tags', field: 'TagString', enableSorting: false },
                { field: 'Published', enableSorting: false },
                { field: 'PostedOn' },
                { field: 'PostedBy.UserName' },
                { field: 'Meta' },
                { field: 'UrlSlug' },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        postsResource.get(function (data) {
            console.log(data.rows);

            $scope.gridOptions1.data = data.rows;

        });
    }

}());