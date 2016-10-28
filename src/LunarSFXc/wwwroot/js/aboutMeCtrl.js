﻿(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("AboutMeCtrl", ["$scope", "$location", "dataResource", AboutMeCtrl]);

    function AboutMeCtrl($scope, $location, dataResource) {
        var vm = this;

        $scope.go = function (path) {
            $location.path(path);
        };

        var paginationOptions = {
            pageNumber: 1,
            pageSize: 25,
            columnName: null,
            sort: null
        };

        $scope.gridOptions = {
            enableSorting: true,
            enableColumnResizing: true,
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            enableFullRowSelection: true,
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: false,

            columnDefs: [
                { name: 'Title', cellTooltip: true },
                { name: 'Description', cellTooltip: true, enableSorting: false },
                { name: 'StartDate', cellTooltip: false },
                { name: 'EndDate', enableSorting: false },
                { name: 'Image', field: 'Image.FileName' },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                //$scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                //    if (sortColumns.length == 0) {
                //        paginationOptions.sort = null;
                //    } else {
                //        paginationOptions.sort = sortColumns[0].sort.direction;
                //        paginationOptions.columnName = sortColumns[0].name;
                //    }
                //    getPage();
                //});
                //gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                //    paginationOptions.pageNumber = newPage;
                //    paginationOptions.pageSize = pageSize;

                //    getPage();
                //});
            }
        };

        var getPage = function () {
            dataResource.aboutme.getAll(function (data) {
                $scope.gridOptions.totalItems = data.records;
                // var firstRow = (paginationOptions.pageNumber - 1) * paginationOptions.pageSize;
                $scope.gridOptions.data = data.rows;//.slice(firstRow, firstRow + paginationOptions.pageSize);

            });
        };

        getPage();
    }
}());