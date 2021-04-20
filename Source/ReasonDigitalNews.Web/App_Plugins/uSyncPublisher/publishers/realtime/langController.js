(function () {
    'use strict';

    function langController($scope, uSyncActionManager) {
        var pvm = this;

        pvm.loading = true;
        pvm.all = true;

        pvm.process = $scope.vm.process;

        pvm.mode = $scope.vm.mode;

        pvm.flags = $scope.vm.flags;
        pvm.server = pvm.process.server;

        if ($scope.model != null) {
            $scope.model.title = 'Select languages';
            $scope.model.subtitle = "Select Languages to publish to " + pvm.server.Name;
        }

        pvm.variants = [];
        $scope.vm.items[0].variants.forEach(function (variant) {
            variant._checked = true;
            pvm.variants.push(variant);
        });

        pvm.loading = false;


        var watchEvent = $scope.$watch('pvm.variants', function (newValue) {
            if (newValue !== undefined) {
                pvm.process.options.Cultures = [];
                $scope.vm.valid = false;

                for (let i = 0; i < newValue.length; i++) {
                    if (newValue[i]._checked === true) {
                        pvm.process.options.Cultures.push(newValue[i].language.culture);
                        $scope.vm.valid = true;
                    }
                }

                if (pvm.process.options.Cultures.length == newValue.length) {
                    // all selected
                    pvm.process.options.Cultures = []
                    pvm.all = true;
                }
                else {
                    pvm.all = false;
                }

            }
        }, true);

        $scope.$on('$destroy', function () {
            watchEvent();
        })
    }

    angular.module('umbraco')
        .controller('uSyncPublisherLanguageController', langController);

})();