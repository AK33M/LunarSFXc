(function () {
    "use strict";

    angular
        .module("services",
            [
                "ngResource",
                "ngMessages",
                "ngRoute",
                "ngAnimate",
                "ngTouch",
                "ui.grid",
                "ui.grid.pagination",
                "ui.grid.selection",
                "angularFileUpload",
                "ui.bootstrap",
                "bootstrapLightbox"
            ]);
    //.constant("appSettings",
    //{
    //    serverPath: ""
    //})
    //.service("", function () { });
}());