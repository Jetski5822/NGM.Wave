/// <reference path="ngm.wave.ui.js" />

(function ($, connection, window, ui) {
    "use strict";

    var wave = connection.waveHub,
        $ui = $(ui),
        contentitemid = null;

    wave.client.addUser = function (user, groupName, isOwner) {
        console.log("User Joined");
        ui.notifiedUserJoined(user);
    };

    wave.client.notifyThreadCreated = function (forumId, threadId, postId, location) {
        console.log("Thread Created with id: " + threadId + " at location: " + location);
        ui.notifiedThreadCreated(threadId, location);
    };

    wave.client.notifyThreadRemoved = function (forumId, threadId) {
        console.log("Thread Removed with id: " + threadId);
        ui.notifiedThreadRemoved(threadId);
    };

    wave.client.notifyPostCreated = function (forumId, threadId, postId, repliedOnId, location, text) {
        console.log("Post Created with id: " + postId + " at location: " + location);
        ui.notifiedPostCreated(threadId, postId, location, text);
    };

    wave.client.notifyPostRemoved = function (forumId, threadId, postId) {
        console.log("Post Removed with id: " + postId);
        ui.notifiedPostRemoved(postId, location);
    };

    $(function () {
        var initial = true;
        
        console.log("Wave UI Initializing...");
        ui.initialize();
        console.log("Wave UI Initialized!");

        $.wavehub = {};

        $.wavehub.initConnection = function (contentItemId) {
            //var transport = 'longPolling', // Due to Orchard not being 4.5 :(
            //    options = {};

            //if (transport) {
            //    options.transport = transport;
            //}
            
            console.log("Wave Connection Initializing...");
            contentitemid = contentItemId;
            
            connection.hub.logging = false;
            connection.hub.start(function() {
                console.log("Hub Started! Connecting to the wave...");
                wave.server.join(contentItemId)
                    .fail(function (e) {
                        console.log(e);
                        console.log("Wave Connection failed to Initialize!");
                    })
                    .done(function() {
                        console.log("Wave Connection Initialized!");
                    });
            });
            
            connection.hub.stateChanged(function (change) {
                if (change.newState === $.connection.connectionState.reconnecting) {
                    console.log("State Reconnecting");
                }
                else if (change.newState === $.connection.connectionState.connected) {
                    console.log("State Connected");
                    if (!initial) {
                        
                        
                    } else {
                        
                    }

                    initial = false;
                }
            });

            connection.hub.disconnected(function () {
                connection.hub.log('Dropped the connection from the server. Restarting in 5 seconds.');

                // Restart the connection
                setTimeout(function () {
                    connection.hub.start()
                        .done(function () {
                            console.log("Hub Re-Started!");
                            wave.server.join(contentItemId, true)
                                .fail(function (e) {
                                    console.log("Wave Connection failed to Re-Initialize!");
                                })
                                .done(function () {
                                    console.log("Wave Connection ReInitialized!");
                                });
                        });
                }, 5000);
            });

            connection.hub.error(function (err) {
                // Make all pending messages failed if there's an error
                console.log(err);
            });
        };
    });
})(jQuery, $.connection, window, window.wave.ui);
