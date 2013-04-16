(function($, window) {
    "use strict";

    function removeContentItem(contentItemId, callback) {
        var article = $("article[data-contentitem-id='" + contentItemId + "']");
        if (article.length == 0)
            return;

        var li = article.parent("li");
        $(li).slideUp('slow', function () {
            // need to do last/first etc
            //li.next("li").addClass(li.attr('class'));
            li.remove();
            callback();
        });
    }

    function markContentItemRemoved(contentItemId) {
        var article = $("article[data-contentitem-id='" + contentItemId + "']");
        if (article.length == 0)
            return;

        var removedActionMessage = document.createElement('a');
        removedActionMessage.setAttribute('href', "#");
        removedActionMessage.setAttribute("data-contentitem-id", contentItemId);
        removedActionMessage.innerHTML = "This content item has been removed. Click here to temporarily view it.";
        removedActionMessage.className = "showhide";

        article.slideUp('fast', function () {
            article.find('.actions').remove();
            article.after($(removedActionMessage));
        });
    }

    $('#content').on('click', 'a.showhide', function () {
        var self = $(this);
        var contentItemId = self.data('contentitem-id');

        var article = $("article[data-contentitem-id='" + contentItemId + "']");
        if (article.length == 0)
            return;

        article.toggle('fast', function () {
            
        });
    });

    var ui = {
        events: {
            threadAdd: 'wave.ui.threadAdd',
            threadRemove: 'wave.ui.threadRemove',
            
            postAdd: 'wave.ui.postAdd',
            postRemove: 'wave.ui.postRemove',
        },
        
        notifiedThreadCreated: function (threadId, location) {
            // Do I create a content controller that this should go back to to get the content? Maybe accept a list of Ids..?
            var summaryLocation = '/Contents/Item/DisplaySummary/' + threadId;

            // Go an get the rendered shape...
            $.ajax({ url: summaryLocation })
            .done(function (data) {
                // No content shown, and a new content item comes in, "no content item p and create UL for forum-threads and append new li."
                var li = document.createElement('li');
                li.innerHTML = $(data.trim()).find(".content-item").parent().html();

                if ($("p.nocontentitems").is(":visible") == true) {
                    li.className = "first last";

                    $("p.nocontentitems").slideUp('slow');
                }
                else {
                    li.className = "first";
                }

                var threadList = $("ul.forum-threads");
                if (threadList.length == 0) {
                    var ul = document.createElement('ul');
                    ul.className = "forum-threads content-items";
                    $("p.nocontentitems").after(ul);
                }

                $(li)
                    .hide()
                    .prependTo("ul.forum-threads")
                    .slideDown("slow", function () {
                        $("ul.forum-threads li.first").removeClass("first");
                        $("ul.forum-threads li:first").addClass("first");

                        if ($("ul.forum-threads").is(":hidden") == true) {
                            $("ul.forum-threads").show();
                        }

                    });
            })
            .fail(function () { alert("Error updating threads feed."); });
        },
        
        notifiedThreadRemoved: function (threadId) {
            markContentItemRemoved(threadId);
            //removeContentItem(threadId, function () {
            //    // Forum Thread List
            //    if ($("ul.forum-threads li").length == 0) {
            //        $("ul.forum-threads").slideUp('slow', function () {
            //            $(".nocontentitems").show();
            //        });
            //    }
            //});
        },

        notifiedPostCreated: function (threadId, postId, location, text) {
            var article = $("article[data-contentitem-id='" + threadId + "']");
            if (article.length == 0)
                return;

            article.find(".post-content").html(text);
            //// Do I create a content controller that this should go back to to get the content? Maybe accept a list of Ids..?
            //var currentLocation = window.location;

            //// Go an get the rendered shape...
            //$.ajax({ url: currentLocation })
            //.done(function (data) {
            //})
            //.fail(function () { alert("Error updating threads feed."); });
        },

        notifiedPostRemoved: function (postId) {
            markContentItemRemoved(postId);
        },

        notifiedUserJoined: function (user) {
            if ($('ul.current-users li:contains("' + user.UserName + '")').length != 0)
                return;

            var li = document.createElement('li');
            li.innerHTML = user.UserName;
            $("ul.current-users").append(li);
        },
        
        initialize: function () {
            var $ui = $(this);
            
            //$("#content").on("click", ".thread a.remove", function (event) {
            //    event.preventDefault();
                
            //    $ui.trigger(ui.events.threadRemove, this);
                
            //    return false;
            //});

            //$("#content").on("click", ".user-wave .create", function (event) {
            //    event.preventDefault();

            //    var href = $(this).attr('href');
            //    $.ajax({ url: href })
            //    .done(function (data) {
            //        ui.updateSecondarySection(data);
            //    })
            //    .fail(function () { alert("error"); });

            //    return false;
            //});

            //$("#layout-content").on("submit", "form#thread-create", function (event) {
            //    event.preventDefault();

            //    var $form = $(this),
            //        dataToSend = $(this).serialize(),
            //        url = $form.attr('action');

            //    $.post(url, dataToSend)
            //    .done(function (data) {
            //        ui.updateSecondarySection(data);
            //    })
            //    .fail(function () { alert("error"); });

            //    return false;
            //});
        }
    };

    if (!window.wave) {
        window.wave = {};
    }

    window.wave.ui = ui;
})(jQuery, window);