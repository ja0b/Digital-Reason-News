(function () {
    'use strict';

    var publishingComponent = {
        templateUrl: Umbraco.Sys.ServerVariables.application.applicationPath + 'App_Plugins/uSyncPublisher/Components/uSyncPublishingAction.html',
        bindings: {
            mode: '<',
            items: '=',
            options: '=',
            state: '=',
            actionButton: '=',
            headings: '=',
            isModal: '<',
            hideWhenPicked: '<'
        },
        controllerAs: 'vm',
        controller: publishingController
    };

    function publishingController($scope, $q, mediaResource, contentResource, dictionaryResource, localizationService,
        uSyncHub, uSyncPublishService, uSyncPublishingService, uSyncActionManager) {

        var vm = this;
        vm.process = {};
   
        vm.error = {};
        vm.report = [];

        vm.actionButton = { state: 'init', name: 'Send' };
      
        vm.showPickServer = true; // show the server select dialog.
        vm.servers = [];

        vm.selectedServer = null;
        vm.contentType = 'content'; // could be media, settings or dictionary

        vm.singleItem = false; // single item passed - changes ui, some actions.

        vm.onSelected = onSelected;

        var events = [];
        events.push($scope.$on('usync-publish-performAction', function () {
            onPerformAction();
        }));

        events.push($scope.$on('usync-publish-close', function () {
            onClose();
        }))

        /// /// ///
        function onSelected(server) {
            vm.flags = uSyncActionManager.prepToggles(server, vm.flags, vm.contentType);

            vm.headings.description =
                uSyncActionManager.getDescription(vm.mode, vm.contentType, server.Name);

            vm.server = server;

            startProcess();

            $scope.$broadcast('sync-server-selected', {
                server: server, flags: vm.flags
            });
        }

        function onPerformAction() {
            if (vm.process.view.show) {
                vm.state.actionLoaded = false;
            }
            performAction(vm.process);
        }

        function onClose() {
            clean(vm.process);
        }

        /// setup
        vm.$onInit = function () {

            vm.contentType = vm.options.contentType ?? 'content';

            vm.headings = {
                title: 'Select a server',
                description: vm.mode + ' ' + vm.contentType
            };

            var promises = [];
            if (vm.options.serverAlias !== undefined) {
                vm.showPickServer = false;
                promises.push(uSyncPublishService.getServer(vm.options.serverAlias)
                    .then(function (result) {
                        vm.selectedServer = result.data;
                    }));
            }

            $q.all(promises).then(function () {
                vm.flags = uSyncActionManager.initFlags();
                initComponent();
            });



            localizationService.localize('usyncpublish_' + vm.mode + "Button")
                .then(function (data) {
                    vm.actionButton.name = data;
                });


            initSignalRHub();
        }

        // clean up
        $scope.$on('destroy', function () {
            for (var e in events) { events[e](); }
        });

        function initComponent() {

            var promises = [];

            vm.itemMode = getItemMode(vm.items);
            if (vm.itemMode === 'single') {
                if (isRootItem(vm.items[0])) {
                    var root = makeRoot(vm.items[0], vm.contentType);
                    if (root !== null) {
                        vm.items[0] = root;
                    }
                }
                else if (vm.items[0].udi === undefined) {
                    promises.push(getItemById(vm.items[0], vm.contentType)
                        .then(function (item) {
                            vm.items[0] = item;
                        }));
                }
            }

            if (vm.showPickServer) {
                promises.push(uSyncPublishService.getServers(vm.mode)
                    .then(function (result) {
                        vm.servers = result.data;
                        checkServers(vm.servers);
                    }));
            }

            // initSignalR

            $q.all(promises).then(function () {
                if (!vm.showPickServer) {
                    setupServer();
                }

                vm.state.loading = false;
            });
        }

        function getItemMode(items) {
            return items.length === 1 ? 'single' : 'multi';
        }

        function isRootItem(item) {
            return item.id * 1 === -1;
        }

        function makeRoot(item, contentType) {

            // if (contentType === 'media' || contentType === 'content') {
                return {
                    id: -1,
                    udi: uSyncActionManager.makeUdi(contentType),
                    name: contentType,
                    variants: [{ name: contentType }]
                };
            // }
            return null;
        }

        function getItemById(item, contentType) {

            switch (contentType) {
                case 'media':
                    return mediaResource.getById(item.id);
                    break;
                case 'content':
                    return contentResource.getById(item.id);
                    break;
                case 'setting':
                    break;
                case 'dictionary-item':
                    return dictionaryResource.getById(item.id);
                    break;
            }

            return item;
        }

        function setupServer() {
            onSelected(vm.selectedServer);
        }

        //// utils.
        function checkServers(servers) {

            var checks = [];

            servers.forEach(function (server) {
                checks.push(
                uSyncPublishService.checkServer(server.Alias)
                    .then(function (result) {
                        server.status = result.data;
                    }));
            });

            $q.all(checks).then(function () {
                $scope.$broadcast('usync-servers-checked', servers);
            });
        }

        function getSyncItems(items) {

            return _.map(items, function (item) {

                var name = item.name;

                if (_.isArray(item.variants) && item.variants.length > 0) {
                // if (item.variants !== undefined && item.variants !== null && item.variants.length > 0) {
                    name = item.variants[0].name;
                }

                return {
                    id: item.id,
                    udi: item.udi,
                    name: name
                }
            });

        }

        // processing 
        // loop : getAction -> prepAction - > processAction 

        function startProcess() {

            vm.process = {
                id: uSyncActionManager.emptyGuid,
                actionAlias: '',
                server: vm.server.Alias,
                mode: vm.mode,
                items: getSyncItems(vm.items),
                steps: {
                    stepIndex: 0,
                    pageNumber: 0,
                    handlerFolder: '',
                },
                view: {
                    show: false,
                    path: ''
                },
                options: {
                    primaryType: vm.contentType,
                    removeOrphans: false,
                    includeFileHash: false,
                    includeSystemFileHash: false,
                    attributes: [],
                    Cultures: []
                }
            };

            getAction(vm.process);
        }

        function getAction(process) {

            vm.state.actionLoaded = false;

            var request = makePublishRequest(process)

            uSyncPublishingService.getAction(request)
                .then(function (result) {
                    vm.state.actionLoaded = true;
                    process.action = result.data;
                    prepAction(process);
                });
        }

        function prepAction(process) {

            if (process.action === null) {
                // end ? 
                return;
            }

            process.actionAlias = process.action.alias;

            if (process.action.view !== null && process.action.view.length > 0) {
                process.view = { show: true, path: process.action.view };

                vm.state.valid = true;
                vm.state.working = false;
                vm.state.hideClose = false;


                if (vm.showPickServer && vm.hideWhenPicked) {
                    vm.showPickServer = false;
                    vm.headings = {
                        title: 'Publish to ' + vm.server.Name,
                        description: vm.server.Url
                    };
                }

                return;
            }
            else {

                vm.state.working = true;
                vm.state.hideClose = true;

                process.view = { show: false, path: '' };
                // no view do the action
                performAction(process);
            }
        }

        function performAction(process) {


            vm.showPickServer = false; 

            updateState(process);

            var request = makePublishRequest(process);

            uSyncPublishingService.performAction(request)
                .then(function (result) {

                    var response = result.data;

                    if (response.success) {

                        process = updateProcess(process, response);

                        if (response.processComplete) {
                            // everything is done.
                            // this is the end.
                            console.log('end ?');
                        }
                        else if (response.actionComplete) {
                            // action complete - move to next.
                            getAction(process);
                        }
                        else {
                            performAction(process);
                        }
                    }
                    else {
                        // fail.
                        showError(response.error, request);
                    }
                });
        }

        // updates state - so footer UI can show progress.
        function updateState(process) {
            vm.state.actionName = process.actionAlias;
            vm.state.stepIndex = process.steps.stepIndex;
            vm.state.pageNumber = process.steps.pageNumber;
            vm.state.folder = process.steps.handlerFolder;
        }

        function makePublishRequest(process) {
            return {
                id: process.id,
                server: process.server,
                mode: process.mode,
                items: process.items,
                actionAlias: process.actionAlias,

                stepIndex: process.steps.stepIndex,
                handlerFolder: process.steps.handlerFolder,

                pageNumber: process.steps.pageNumber,
                clientId: getClientId(),
                options: process.options
            }
        }

        function updateProcess(process, response) {
            process.id = response.id;
            process.actionAlias = response.nextAction;
            process.items = response.items;
            process.steps = {
                stepIndex: response.stepIndex,
                pageNumber: response.nextPage,
                handlerFolder: response.nextFolder
            }

            if (response.actions !== undefined && response.actions !== null && response.actions.length > 0) {
                vm.report = response.actions;
            }

            if (response.progress != null && response.progress != undefined) {
                vm.progress.currentStepIndex = response.progress.currentStepIndex;
                vm.progress.currentStepName = response.progress.currentStepName;
                updateProgressSteps(response.progress.steps);
            }

            return process;
        }

        function updateProgressSteps(steps) {

            if (_.isArray(steps)) {
                if (vm.progress.steps.length != steps.length) {
                    vm.progress.steps = steps;
                }

                for (let n = 0; n < steps.length; n++) {
                    if (steps[n].icon != vm.progress.steps[n].icon ||
                        steps[n].name != vm.progress.steps[n].name ||
                        steps[n].status != vm.progress.steps[n].status
                    ) {
                        vm.progress.steps[n] = steps[n];
                    }
                }
            }
        }

        function clean(process) {
            if (process.id !== undefined && process.id !== null && process.id != uSyncActionManager.emptyGuid) {
                uSyncPublishingService.clean(process.id, process.server)
                    .then(function () { });
            }
        }

        // progress ui 
        // mainly driven by signalR
        vm.progress = {
            steps: [
                { icon: 'icon-settings', name: 'first-step', status: 0 },
                { icon: 'icon-settings', name: 'second-step', status: 0 },
                { icon: 'icon-settings', name: 'third-step', status: 0 }
            ]
        }

        vm.calcPercentage = calcPercentage;

        function calcPercentage(update) {
            if (update != null && update != undefined) {
                return (update.Count / update.Total) * 100;
            }
            return 0;
        }

        function showError(error, request) {

            vm.state.hideClose = false;
            vm.state.valid = false;
            vm.state.working = false; 

            vm.state.hasError = true;
            vm.error = error;
            vm.errorTitle = 'Error during ' + request.actionAlias;
        }

        //////// SignalR

        function initSignalRHub() {
            uSyncHub.initHub(function (hub) {
                vm.hub = hub;

                vm.hub.on('update', function (update) {
                    vm.update = update;
                    vm.update.blocks = update.Message.split('||');
                });

                vm.hub.on('add', function (status) {
                    vm.status = status;
                });

                vm.hub.on('publisher', function (message) {
                    vm.message = message;
                    calcStep(vm.message);
                });

                vm.hub.start();
            });
        }

        function getClientId() {
            if ($.connection !== undefined && $.connection.hub !== undefined) {
                return $.connection.hub.id;
            }
            return "";
        }

    }


    angular.module('umbraco')
        .component('usyncPublishingAction', publishingComponent);
})();