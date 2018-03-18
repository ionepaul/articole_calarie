$(function () {
    initViewDetailsEvent();
});

$(document).on("click", "#content-pager-orders a[href]", function () {
    showLoader();

    $.ajax({
        url: $(this).attr("href"),
        type: 'GET',
        cache: false,
        success: function (result) {
            hideLoader();
            $('#user-orders').html(result);
            initViewDetailsEvent();
        }
    });
    return false;
});

function initViewDetailsEvent() {
    $(".user-order-details").hide();

    $("button.expand-order-details").on("click", function (event) {
        event.stopPropagation();
        var $target = $(event.target);

        if ($target.closest("td").attr("colspan") > 1) {
            if ($(this).children("i").hasClass("fa-angle-up")) {
                $(this).children("i").removeClass("fa-angle-up").addClass("fa-angle-down");
            }
            else {
                $(this).children("i").removeClass("fa-angle-down").addClass("fa-angle-up");
            }

            $target.slideUp();
        } else {
            if ($(this).children("i").hasClass("fa-angle-up")) {
                $(this).children("i").removeClass("fa-angle-up").addClass("fa-angle-down");
            }
            else {
                $(this).children("i").removeClass("fa-angle-down").addClass("fa-angle-up");
            }

            $target.next(".user-order-details").slideToggle();
        }
    });
}