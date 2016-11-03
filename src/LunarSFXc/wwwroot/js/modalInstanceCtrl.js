(function () {
    "use strict";

    angular.module('app-admin')
        .controller('ModalInstanceCtrl',
                    [
                        "$uibModalInstance",
                        "item",
                        ModalInstanceCtrl
                    ]);

    function ModalInstanceCtrl($uibModalInstance, item) {
        var $ctrl = this;
        $ctrl.eventId = item;
        $ctrl.ok = function () {
            $uibModalInstance.close($ctrl.eventId);
        };

        $ctrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
}());