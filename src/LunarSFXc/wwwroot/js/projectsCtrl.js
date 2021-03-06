﻿(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("ProjectsCtrl", ["$scope", "$location", "$log", "dataResource", "uiGridConstants", ProjectsCtrl]);

    function ProjectsCtrl($scope, $location, $log, dataResource, uiGridConstants) {
        var vm = this;

        $scope.go = function (path) {
            $location.path(path);
        };

        $scope.edit = function (path) {
            //$log.log(path);
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
                { name: 'Title', cellTooltip: true },
                { name: 'SubTitle', cellTooltip: true },
                { name: 'Description', cellTooltip: true, enableSorting: false },
                { name: 'Category', field: 'Category.Name', cellTooltip: true },
                { name: 'Date', enableSorting: false },
                { name: 'Image', field: 'Image.FileName' },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.isSelected) {
                        var msg = 'row selected ' + row.entity.Id;
                        $scope.rowSelected = row.entity.Id;
                        //$log.log(msg);
                        $scope.projectToEdit = gridApi.selection.getSelectedRows();
                    } else {
                        $scope.rowSelected = null;
                        $scope.projectToEdit = null;
                    }
                });
            }
        };

        $scope.gridOptions.rowIdentity = function (row) {
            return row.id;
        };

        var getPage = function () {
            dataResource.projects.getAll(function (data) {
                $scope.gridOptions.totalItems = data.records;
                $scope.gridOptions.data = data.rows;
            });
        };

        getPage();
    }

}());