(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("PostsCtrl", ["$scope", "$log", "dataResource", "uiGridConstants", PostsCtrl]);

    function PostsCtrl($scope, $log, dataResource, uiGridConstants) {
        var vm = this;

        var paginationOptions = {
            pageNumber: 1,
            pageSize: 25,
            columnName: null,
            sort: null
        };

        $scope.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnResizing: true,
            enableRowSelection: true,
            enableRowHeaderSelection: true,
            enableFullRowSelection: true,
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: false,
            selectionRowHeaderWidth: 35,
            rowHeight: 35,
            showGridFooter: true,

            columnDefs: [
                { name: 'Title', width: 150, cellTooltip: true },
                { name: 'ShortDescription', width: 200, cellTooltip: true, enableSorting: false },
                { name: 'Description', enableSorting: false },
                { name: 'Category', field: 'Category.Name', cellTooltip: true, width: 100 },
                { name: 'Tags', field: 'TagString', cellTooltip: true, width: 100, enableSorting: false },
                { name: 'Published', width: 50, type: 'boolean' },
                { name: 'PostedOn', cellFilter: 'date', width: 100 },
                { name: 'Modified', cellFilter: 'date', width: 100 },
                { name: 'Posted By', field: 'PostedBy.UserName', width: 100, enableSorting: false },
                { name: 'Meta', width: 100, enableSorting: false },
                { name: 'UrlSlug', width: 100, enableSorting: false },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptions.sort = null;
                    } else {
                        paginationOptions.sort = sortColumns[0].sort.direction;
                        paginationOptions.columnName = sortColumns[0].name;
                    }
                    getPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptions.pageNumber = newPage;
                    paginationOptions.pageSize = pageSize;

                    getPage();
                });
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.isSelected) {
                        var msg = 'row selected ' + row.entity.Id;
                        $scope.rowSelected = row.entity.Id;                        
                        $scope.postToEdit = gridApi.selection.getSelectedRows();
                        $log.log($scope.postToEdit);
                    } else {
                        $scope.rowSelected = null;
                        $scope.postToEdit = null;
                    }
                });
            }
        };

        var getPage = function () {
            dataResource.posts.getAll(paginationOptions, function (data) {
                $scope.gridOptions.totalItems = data.records;
                // var firstRow = (paginationOptions.pageNumber - 1) * paginationOptions.pageSize;
                $scope.gridOptions.data = data.rows;//.slice(firstRow, firstRow + paginationOptions.pageSize);
            }, function (error) {
                //Error
            });
        };

        getPage();
    }

}());