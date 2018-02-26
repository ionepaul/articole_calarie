$(function () {
    $(".user-order-details").hide();

    $("p.expand-order-details").on("click", function (event) {
        event.stopPropagation();
        var $target = $(event.target);

        if ($target.closest("td").attr("colspan") > 1) {
            if ($(this).children("i").hasClass("fa-chevron-up")) {
                $(this).children("i").removeClass("fa-chevron-up").addClass("fa-chevron-down");
            }
            else {
                $(this).children("i").removeClass("fa-chevron-down").addClass("fa-chevron-up");
            }

            $target.slideUp();
        } else {
            if ($(this).children("i").hasClass("fa-chevron-up")) {
                $(this).children("i").removeClass("fa-chevron-up").addClass("fa-chevron-down");
            }
            else {
                $(this).children("i").removeClass("fa-chevron-down").addClass("fa-chevron-up");
            }

            $target.next(".user-order-details").slideToggle();
        }
    });
})