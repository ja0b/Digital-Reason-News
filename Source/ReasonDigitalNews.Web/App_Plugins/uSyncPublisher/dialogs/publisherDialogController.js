(function () {
    'use strict';

    function publisherDialogController($scope) {

        var vm = this;

        vm.close = close;
        vm.performAction = performAction;

        vm.mode = $scope.model.mode;
        vm.options = $scope.model.options;
        vm.items = vm.options.items;
        vm.state = {
            complete: false,
            loading: true,
            hideClose: false,
            valid: false,
            working: false,
            hasError: false,
            error: ''
        };

        vm.ui = {
            button: {
                state: 'init',
                name: 'Send',
            },
            headings: {
                title: 'uSyncPublisher',
                description: 'push and pull things',
                boxTitle: 'uSyncPublisher_box',
                boxDescription: 'uSyncPublisher_box_desc'
            }
        }

        vm.actionButton = {
            state: 'init',
            name: 'Send',
            valid: false
        };


        function performAction() {
            $scope.$broadcast('usync-publish-performAction')
        }


        function close() {
            $scope.$broadcast('usync-publish-close');

            if (vm.state.complete && $scope.model.submit) {
                $scope.model.submit();
            }
            else if ($scope.model.close) {
                $scope.model.close();
            }
        }

    }

    angular.module('umbraco')
        .controller('uSyncPublisherDialogController', publisherDialogController);

})();