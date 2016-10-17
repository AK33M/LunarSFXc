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
                { field: 'Title', width: 150 },
                { field: 'ShortDescription', enableSorting: false, width: 200 },
                { field: 'Description', enableSorting: false},
                { name: 'Category', field: 'Category.Name', width: 100 },
                { name: 'Tags', field: 'TagString', enableSorting: false, width: 100 },
                { field: 'Published', width: 50, type:'boolean' },
                { field: 'PostedOn', cellFilter: 'date', width: 100},
                { field: 'Modified', cellFilter: 'date', width: 100 },
                { name: 'Posted By', field: 'PostedBy.UserName', width: 100 },
                { field: 'Meta', width: 100 },
                { field: 'UrlSlug', width: 100 },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        postsResource.get(function (data) {
            $scope.gridOptions1.data = data.rows;

        });
    }

}());