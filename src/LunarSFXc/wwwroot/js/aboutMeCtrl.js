(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("AboutMeCtrl", ["$scope", "$log", "$location", "dataResource", "uiGridConstants", AboutMeCtrl]);

    function AboutMeCtrl($scope, $log, $location, dataResource, uiGridConstants) {
        var vm = this;

        $scope.go = function (path) {
            $location.path(path);
        };

        $scope.edit = function (path) {
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
            enableRowHeaderSelection: true,
            enableFullRowSelection: true,

            multiSelect: false,
            modifierKeysToMultiSelect: false,
            noUnselect: false,

            enableSelectAll: true,
            selectionRowHeaderWidth: 35,
            rowHeight: 35,
            showGridFooter: true,

            columnDefs: [
                 { name: 'Id', visible: false },
                { name: 'Title', cellTooltip: true },
                { name: 'Description', cellTooltip: true, enableSorting: false },
                { name: 'StartDate', cellTooltip: false },
                { name: 'EndDate', enableSorting: false },
                { name: 'Image', field: 'Image.FileName' },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.isSelected) {
                        var msg = 'row selected ' + row.entity.Id;
                        $scope.rowSelected = row.entity.Id;
                       // $log.log(msg);
                        $scope.eventToEdit = gridApi.selection.getSelectedRows();
                    } else {
                        $scope.rowSelected = null;
                        $scope.eventToEdit = null;
                    }
                });

                //gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
                //    //var msg = 'rows changed ' + rows.length;
                //    //$log.log(msg);
                //});
            }
        };

        $scope.gridOptions.rowIdentity = function (row) {
            return row.id;
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