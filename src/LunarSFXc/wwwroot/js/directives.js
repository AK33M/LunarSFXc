(function () {
    "use strict";

    angular
        .module('app-admin')
        .directive('ngThumb', ['$window', function ($window) {
            var helper = {
                support: !!($window.FileReader && $window.CanvasRenderingContext2D),
                isFile: function (item) {
                    return angular.isObject(item) && item instanceof $window.File;
                },
                isImage: function (file) {
                    var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            };

            return {
                restrict: 'A',
                template: '<canvas/>',
                link: function (scope, element, attributes) {
                    if (!helper.support) return;

                    var params = scope.$eval(attributes.ngThumb);

                    if (!helper.isFile(params.file)) return;
                    if (!helper.isImage(params.file)) return;

                    var canvas = element.find('canvas');
                    var reader = new FileReader();

                    reader.onload = onLoadFile;
                    reader.readAsDataURL(params.file);

                    function onLoadFile(event) {
                        var img = new Image();
                        img.onload = onLoadImage;
                        img.src = event.target.result;
                    }

                    function onLoadImage() {
                        var width = params.width || this.width / this.height * params.height;
                        var height = params.height || this.height / this.width * params.width;
                        canvas.attr({ width: width, height: height });
                        canvas[0].getContext('2d').drawImage(this, 0, 0, width, height);
                    }
                }
            };
        }])
        .config(function (LightboxProvider) {
            // set a custom template
            LightboxProvider.templateUrl = '/views/lightbox-modal.html';

          //  LightboxProvider.fullScreenMode = true;

            //// set the caption of each image as its text color
            //LightboxProvider.getImageCaption = function (imageUrl) {
            //    //return '#' + imageUrl.match(/00\/(\w+)/)[1];
            //    return "Caption";
            //};

            //// increase the maximum display height of the image
            //LightboxProvider.calculateImageDimensionLimits = function (dimensions) {
            //    return {
            //        'maxWidth': dimensions.windowWidth >= 768 ? // default
            //          dimensions.windowWidth - 92 :
            //          dimensions.windowWidth - 52,
            //        'maxHeight': 1600                           // custom
            //    };
            //};

            // the modal height calculation has to be changed since our custom template is
            // taller than the default template
            LightboxProvider.calculateModalDimensions = function (dimensions) {
                var width = Math.max(400, dimensions.imageDisplayWidth + 32);

                if (width >= dimensions.windowWidth - 20 || dimensions.windowWidth < 768) {
                    width = 'auto';
                }

                return {
                    'width': width,                             // default
                    'height': 'auto'                            // custom
                };
            };
        });
}());