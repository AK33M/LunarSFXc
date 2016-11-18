(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("UsersCtrl", [
                                    "$scope",
                                    "$location",
                                    "$log",
                                    "dataResource",
                                    "uiGridConstants",
                                    UsersCtrl
        ]);

    function UsersCtrl($scope, $location, $log, dataResource, uiGridConstants) {
        var vm = this;

        $scope.go = function (path) {
            $location.path(path);
        };

        $scope.edit = function (path) {
            $location.path(path);
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
                { name: 'UserName', cellTooltip: true },
                { name: 'FirstWords', cellTooltip: true },
                { name: 'Image', field: 'ProfileImage.FileName' },
                { name: 'Total Comments', field: 'CommentCount' },
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.isSelected) {
                        var msg = 'row selected ' + row.entity.UserName;
                        $scope.rowSelected = row.entity.UserName;
                        $scope.userToEdit = gridApi.selection.getSelectedRows();
                        $log.log($scope.userToEdit);

                    } else {
                        $scope.rowSelected = null;
                        $scope.userToEdit = null;
                    }
                });
            }
        };

        $scope.gridOptions.rowIdentity = function (row) {
            return row.UserName;
        };

        var getPage = function () {
            dataResource.users.getAll(function (data) {
                $scope.gridOptions.totalItems = data.records;
                $scope.gridOptions.data = data.rows;
            });
        };

        getPage();
    }
}());