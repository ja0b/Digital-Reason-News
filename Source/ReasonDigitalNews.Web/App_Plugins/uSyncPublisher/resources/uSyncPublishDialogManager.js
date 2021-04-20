(function () {
    'use strict';

    function dialogManager($rootScope, $timeout, editorService, navigationService) {

        var emptyGuid = '00000000-0000-0000-0000-000000000000';

        return {
            openPublisherDialog: openPublisherDialog,
            openPublisherPullDialog: openPublisherPullDialog,
            openPublisherMediaPush: openPublisherMediaPush,
            openPublisherMediaPull: openPublisherMediaPull,

            openPublisherDictionaryPush: openPublisherDictionaryPush,
            openPublisherDictionaryPull: openPublisherDictionaryPull,

            openSettingsPush: openSettingsPush,

            openSyncDialog: openSyncDialog,

            openServerDialog: openServerDialog,
            openNewServerDialog: openNewServerDialog
        };

        function openPublisherDialog(options, cb) {
            options.contentType = 'content';
            openSyncDialog('Publish Content', 'publisherDialog', options, cb, 'push');
        }

        function openPublisherPullDialog(options, cb) {
            options.contentType = 'content';
            openSyncDialog('Pull Content', 'publisherDialog', options, cb, 'pull');
        }

        function openPublisherMediaPush(options, cb) {
            options.contentType = 'media';
            openSyncDialog('Publish Media', 'publisherDialog', options, cb, 'push');
        }

        function openPublisherDictionaryPush(options, cb) {

            var deployOptions = {
                entity: options.entity,
                contentType: 'dictionary-item'
            };


            openSyncDialog('Deploy Settings', 'publisherDialog', deployOptions, cb, "dictionaryPush");
        }

        function openPublisherDictionaryPull(options, cb) {

            var deployOptions = {
                entity: options.entity,
                contentType: 'dictionary-item'
            };


            openSyncDialog('Deploy Settings', 'publisherDialog', deployOptions, cb, "dictionaryPull");
        }

        function openPublisherMediaPull(options, cb) {
            options.contentType = 'media';
            openSyncDialog('Pull Media', 'publisherDialog', options, cb, 'pull');
        }

        function openSettingsPush(options, cb) {

            var items = [
                {
                    uid: 'umb://document-type/' + emptyGuid, name: 'ContentType'
                },
                {
                    udi: 'umb://data-type/' + emptyGuid, name: 'DataType'
                },
                {
                    udi: 'umb://media-type/' + emptyGuid, name: 'MediaType'
                }];

            var deployOptions = {
                entity: {
                    id: '-1',
                    items: items
                },
                serverAlias: options.entity.id,
                contentType: 'settings'
            };

            openSyncDialog('Deploy Settings', 'publisherDialog', deployOptions, cb, "SettingsPush", '');
        }

        function openSyncDialog(dialogTitle, dialogView, options, cb, mode, size = 'small') {

            if (options.entity !== undefined) {
                options.items = [options.entity];
            }

            editorService.open({
                options: options,
                mode: mode,
                title: dialogTitle,
                size: size,
                view: Umbraco.Sys.ServerVariables.uSyncPublisher.pluginPath + 'dialogs/' + dialogView + '.html',
                submit: function (done) {
                    editorService.close();
                    navigationService.hideNavigation();
                    if (cb !== undefined) {
                        cb(true);
                    }
                },
                close: function () {
                    editorService.close();
                    navigationService.hideNavigation();
                    if (cb !== undefined) {
                        cb(false);
                    }
                }
            });

            // wrap in a timeout, get rid of the 'bounce' 
            $timeout(function () {
                navigationService.hideDialog();
            });
        }


        function openNewServerDialog(entity, cb) {
            editorService.open({
                view: Umbraco.Sys.ServerVariables.uSyncPublisher.pluginPath + 'dialogs/addServer.html',
                title: 'Add Server',
                size: 'small',
                submit: function (model) {
                    editorService.close();
                    navigationService.hideNavigation();
                    openServerDialog(model.Alias, cb);
                },
                close: function () {
                    editorService.close();
                    navigationService.hideNavigation();
                    if (cb !== undefined) {
                        cb(false);
                    }
                }
            });
        }

        function openServerDialog(alias, cb) {
            editorService.open({
                serverAlias: alias,
                title: 'Server View',
                view: Umbraco.Sys.ServerVariables.uSyncPublisher.pluginPath + 'backoffice/uSyncPublisher/server.html',
                submit: function (done) {
                    $rootScope.$broadcast('usync-publisher-settings-reload');
                    navigationService.hideNavigation();
                    editorService.close();
                    if (cb !== undefined) {
                        cb(true);
                    }
                },
                close: function () {
                    $rootScope.$broadcast('usync-publisher-settings-reload');
                    navigationService.hideNavigation();
                    editorService.close();
                    if (cb !== undefined) {
                        cb(false);
                    }
                }
            });
        }

    }

    angular.module('umbraco')
        .factory('uSyncPublishDialogManager', dialogManager);
})();