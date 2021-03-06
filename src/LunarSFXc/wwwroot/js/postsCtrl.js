﻿(function () {
    "use strict";

    angular
        .module("app-admin")
        .controller("PostsCtrl", ["$scope", "$location", "$log", "blogPostService", "dataResource", "uiGridConstants", PostsCtrl]);

    function PostsCtrl($scope, $location, $log, blogPostService, dataResource, uiGridConstants) {
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
                    if (sortColumns.length === 0) {
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
                        //var msg = 'row selected ' + row.entity;
                        blogPostToEdit(row.entity);
                        $scope.rowSelected = blogPostService.getBlogPostId();
                        //$log.log(gridApi.selection.getSelectedRows());
                    } else {
                        blogPostToEdit({});
                        $scope.rowSelected = null;
                    }
                });
            }
        };

        $scope.gridOptions2 = {
            enableColumnResizing: true,
            enableRowSelection: true,
            enableRowHeaderSelection: true,
            enableFullRowSelection: true,
            noUnselect: false,
            multiSelect: false,
            selectionRowHeaderWidth: 35,
            rowHeight: 35,
            showGridFooter: false,

            columnDefs: [
                { name: 'Id', visible: false },
                { name: 'Name', field: 'name', cellTooltip: true },
                { name: 'Description', field: 'description', cellTooltip: true, enableSorting: false },
                { name: 'UrlSlug', field: 'urlSlug', cellTooltip: true },
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.isSelected) {
                        $log.log(row);
                        //var msg = 'row selected ' + row.entity;
                        //blogPostToEdit(row.entity);
                        //$scope.rowSelected = blogPostService.getBlogPostId();
                        //$log.log(gridApi.selection.getSelectedRows());
                    } else {
                        //blogPostToEdit({});
                        //$scope.rowSelected = null;
                    }
                });
            }
        };

        $scope.gridOptions3 = {
            enableColumnResizing: true,
            enableRowSelection: true,
            enableRowHeaderSelection: true,
            enableFullRowSelection: true,
            noUnselect: false,
            multiSelect: false,
            selectionRowHeaderWidth: 35,
            rowHeight: 35,
            showGridFooter: false,

            columnDefs: [
                { name: 'Id', visible: false },
                { name: 'Name', field: 'name', cellTooltip: true },
                { name: 'Description', field: 'description', cellTooltip: true, enableSorting: false },
                { name: 'UrlSlug', field: 'urlSlug', cellTooltip: true },
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.isSelected) {
                        $log.log(row);
                        //var msg = 'row selected ' + row.entity;
                        //blogPostToEdit(row.entity);
                        //$scope.rowSelected = blogPostService.getBlogPostId();
                        //$log.log(gridApi.selection.getSelectedRows());
                    } else {
                        //blogPostToEdit({});
                        //$scope.rowSelected = null;
                    }
                });
            }
        };

        $scope.edit = function () {
            $location.path('add-post');
        };

        $scope.add = function () {
            $location.path('add-post/1');
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

        var getCategories = function () {
            dataResource.posts.getCategories(function (data) {
                $scope.gridOptions2.totalItems = data.categories.length;
                $scope.gridOptions2.data = data.categories;
            }, function (error) {
                //Error
            });
        };

        var getTags = function () {
            dataResource.posts.getTags(function (data) {
                $scope.gridOptions3.totalItems = data.tags.length;
                $scope.gridOptions3.data = data.tags;
            }, function (error) {
                //Error
            });
        };

        var blogPostToEdit = function (value) {
            blogPostService.setBlogPostId(value);
        };

        getPage();
        getCategories();
        getTags();
        blogPostToEdit({});
    }

}());