(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("PostsCtrl", ["$scope", "$interval", "uiGridConstants", "postsResource", PostsCtrl]);

    function PostsCtrl($scope, $interval, uiGridConstants, postsResource) {
        var vm = this;

        $scope.gridOptions = {
            useExternalSorting: true,
            columnDefs: [
                { name: 'Title', width: 150, cellToolTip: true },
                { name: 'ShortDescription', width: 200, cellToolTip: true, enableSorting: false },
                { name: 'Description', cellToolTip: true, enableSorting: false },
                { name: 'Category', field: 'Category.Name', width: 100 },
                { name: 'Tags', field: 'TagString', width: 100, enableSorting: false },
                { name: 'Published', width: 50, type: 'boolean' },
                { name: 'PostedOn', cellFilter: 'date', width: 100 },
                { name: 'Modified', cellFilter: 'date', width: 100 },
                { name: 'Posted By', field: 'PostedBy.UserName', width: 100, enableSorting: false },
                { name: 'Meta', width: 100, enableSorting: false },
                { name: 'UrlSlug', width: 100, enableSorting: false },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, $scope.sortChanged);
                $scope.sortChanged($scope.gridApi.grid, [$scope.gridOptions.columnDefs[1]]);
            }
        };

        var jqViewModel = {
            rows: 2,
            page: 1,
            sidx: "",
            sord: ""
        };

        $scope.sortChanged = function (grid, sortColumns) {


            if (sortColumns.length === 0 || sortColumns[0].name !== $scope.gridOptions.columnDefs[0].name) {
                console.log("default");

                jqViewModel.sord = "asc";
                jqViewModel.sidx = $scope.gridOptions.columnDefs[0].name;
                //$http.get('/data/100.json')
                //.success(function (data) {
                //    $scope.gridOptions.data = data;
                //});
            } else {
                switch (sortColumns[0].sort.direction) {
                    case uiGridConstants.ASC:
                        console.log("ASC");
                        jqViewModel.sord = "asc";
                        jqViewModel.sidx = $scope.gridOptions.columnDefs[0].name;
                        //$http.get('/data/100_ASC.json')
                        //.success(function (data) {
                        //    $scope.gridOptions.data = data;
                        //});
                        break;
                    case uiGridConstants.DESC:
                        console.log("DESC");
                        jqViewModel.sord = "desc";
                        jqViewModel.sidx = $scope.gridOptions.columnDefs[0].name;
                        //$http.get('/data/100_DESC.json')
                        //.success(function (data) {
                        //    $scope.gridOptions.data = data;
                        //});
                        //break;
                    case undefined:
                        console.log("undefined");

                        jqViewModel.sidx = $scope.gridOptions.columnDefs[0].name;
                        //$http.get('/data/100.json')
                        //.success(function (data) {
                        //    $scope.gridOptions.data = data;
                        //});
                        break;
                }
            }

            postsResource.get(jqViewModel, function (data) {
                //console.log(data.rows);
                $scope.gridOptions.data = data.rows;

            });
        }


        postsResource.get(jqViewModel, function (data) {
            //console.log(data.rows);
            $scope.gridOptions.data = data.rows;

        });
    }

}());