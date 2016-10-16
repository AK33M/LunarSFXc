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
                { field: 'Published', enableSorting: false },
                { field: 'PostedOn' },
                { field: 'Category.Name', enableSorting: false },
                { field: 'TagString', enableSorting: false }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        postsResource.get(function (data) {
            console.log(data);
            $scope.gridOptions1.data = data.rows;

            
        });
    }

}());