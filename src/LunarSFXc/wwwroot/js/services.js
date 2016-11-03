(function () {
    "use strict";

    angular
        .module("services",
            [
                "ngResource",
                "ngMessages",
                "ngRoute",
                "ngAnimate",
                "ngSanitize",
                "ngTouch",
                "ui.grid",
                "ui.grid.pagination",
                "ui.grid.selection",
                "angularFileUpload",
                "ui.bootstrap",
                "bootstrapLightbox",
                "ui.select"
            ]);
    //.constant("appSettings",
    //{
    //    serverPath: ""
    //})
    //.service("", function () { });
}());