$(document).ready(function () {
    $("#confirmed-orders-btn").on("click", function () {
        searchOrders("CONFIRMED");
    });

    $("#shipped-orders-btn").on("click", function () {
        searchOrders("SHIPPED");
    });

    $("#registred-orders-btn").on("click", function () {
        searchOrders("REGISTRED");
    });

    $("#completed-orders-btn").on("click", function () {
        searchOrders("COMPLETE");
    });

    $("#all-orders-btn").on("click", function () {
        searchOrders("ALL");
    });

    function searchOrders(status) {
        showLoader();

        $.ajax({
            type: "GET",
            cache: false,
            url: "OrderListPartial?pageNumber=1&status=" + status,
            success: function (result) {
                hideLoader();
                $('#orders-content').html(result);
                initDetailsPanel();
            }
        });
    }

    $(document).on("click", "#content-pager a[href]", function () {
        showLoader();

        $.ajax({
            url: $(this).attr("href"),
            type: 'GET',
            cache: false,
            success: function (result) {
                hideLoader();
                $('#orders-content').html(result);
                initDetailsPanel();
            }
        });

        return false;
    });
});

$(function () {
    initDetailsPanel();
})

function initDetailsPanel() {
    $("td[colspan=5]").find(".order-details").hide();

    $(".expand-btn").on("click", function (event) {
        event.stopPropagation();
        var $target = $(event.target);

        if ($target.closest("td").attr("colspan") > 1) {
            if ($(this).hasClass("fa-chevron-up")) {
                $(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
            }
            else {
                $(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
            }

            $target.slideUp();
        } else {
            if ($(this).hasClass("fa-chevron-up")) {
                $(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
            }
            else {
                $(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
            }

            $target.closest("tr").next().find(".order-details").slideToggle();
        }
    });
}

function openConfirmOrderModal(orderNumber) {
    $('#confirm-order-model-' + orderNumber).modal('toggle');
}

function openShipOrderModel(orderNumber) {
    $('#ship-order-modal-' + orderNumber).modal('toggle');
}

function openCompleteOrderModal(orderNumber) {
    $('#complete-order-modal-' + orderNumber).modal('toggle');
}

function changeOrderStatus(orderNumber, newStatus) {
    let url = window.location.origin + '/Order/ChangeOrderStatus';

    showLoader();

    $.ajax({
        url: url,
        type: 'POST',
        data: {
            orderNumber: orderNumber,
            newOrderStatus: newStatus
        },
        cache: false,
        success: function () {
            hideLoader();
            window.location.reload();
        }
    });
}