(function ($, window) {
    "use strict";

    $("#content").on("click", 'a:not(".close,.remove")', function (e) {
        e.preventDefault();

        var href = ($(this).attr('href'));
        $.ajax({
            url: href
        })
        .done(function (data) {
            ui.populateSecondarySection(data);
        })
        .fail(function () { alert("error"); });

        return false;
    });

    $("#content").on("submit", ".secondary-section form", function(event) {
 
        /* stop form from submitting normally */
        event.preventDefault();
 
        /* get some values from elements on the page: */
        var $form = $( this ),
            dataToSend = $( this ).serialize(),
            url = $form.attr( 'action' );
 
        $.post(url, dataToSend)
        .done(function (data) {
            ui.populateSecondarySection(data);
        })
        .fail(function () { alert("error"); });
 
    });

    $("#content").on("click", ".secondary-section a.close", function (e) {
        e.preventDefault();
        
        var href = ($(this).attr('href'));

        if (href !== undefined) {
            $.ajax({
                url: href
            })
            .done(function (data) {
                ui.populateSecondarySection(data);
            })
            .fail(function () { alert("error"); });
        } else {
            $(".secondary-section").animate({ width: 'toggle' }, 1000, function () {
                $("#layout-main").removeClass("area-large");
                $(".zone.zone-content").removeClass("split-container");
            });
        }
        
        return false;
    });

    $("#content").on("click", ".secondary-section a.remove", function (e) {
        e.preventDefault();

        var href = ($(this).attr('href'));
        $.ajax({
            url: href
        })
        .done(function () {
            $(".secondary-section").animate({ width: 'toggle' }, 1000, function () {
                $("#layout-main").removeClass("area-large");
                $(".zone.zone-content").removeClass("split-container");
            });
        })
        .fail(function () { alert("error"); });

        return false;
    });

    var ui = {
        populateSecondarySection: function (data) {
            if (!$("#layout-main").hasClass("area-large")) {
                $("#layout-main").addClass("area-large");
            }

            var threadCreateMarkup = $(data).find("#content");
            $(threadCreateMarkup).find(".zone.zone-content").addClass("secondary-section");

            var secondarySection = $(".secondary-section");
            if (secondarySection.val() != undefined) {
                $(secondarySection).remove();
            }

            $("#content").append(threadCreateMarkup.html());

            $(".zone.zone-content").addClass("split-container");
        }
    };

    $(function() {
        $(".content-items .content-item a").first().click();
    });
})(jQuery, window);
