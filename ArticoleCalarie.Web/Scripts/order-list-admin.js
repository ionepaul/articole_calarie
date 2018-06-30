$(document).ready(function () {
    $("#confirmed-orders-btn").on("click", function () {
        searchOrders("2");
    });

    $("#shipped-orders-btn").on("click", function () {
        searchOrders("3");
    });

    $("#registred-orders-btn").on("click", function () {
        searchOrders("1");
    });

    $("#completed-orders-btn").on("click", function () {
        searchOrders("4");
    });

    function searchOrders(status) {
        showLoader();
        window.location = "/Order/OrderList?pageNumber=1&status=" + status
    }

    $(document).on("click", "#content-pager a[href]", function () {
        showLoader();
        window.location = $(this).attr("href");
       
        return false;
    });

    $('.special-btns-container button').click(function () {
        $(this).siblings().removeClass('active')
        $(this).addClass('active');
    });
});

$(function () {
    initDetailsPanel();
})

function initDetailsPanel() {
    $("td[colspan=5]").find(".order-details").hide();

    $(".expand-btn").on("click", function (event) {
        var chevron = $(this);
        expandOrderDetails(event, chevron);
    });

    $(".order-row").on("click", function (event) {
        var chevron = $(this).last("td").find(".expand-btn");
        expandOrderDetails(event, chevron);
    });
}

function openConfirmOrderModal(orderNumber) {
    $('#confirm-order-model-' + orderNumber).modal('toggle');
}

function expandOrderDetails(event, chevron) {
    event.stopPropagation();
    var $target = $(event.target);

    if ($target.closest("td").attr("colspan") > 1) {
        if ($(chevron).hasClass("fa-chevron-up")) {
            $(chevron).removeClass("fa-chevron-up").addClass("fa-chevron-down");
        }
        else {
            $(chevron).removeClass("fa-chevron-down").addClass("fa-chevron-up");
        }

        $target.slideUp();
    } else {
        if ($(chevron).hasClass("fa-chevron-up")) {
            $(chevron).removeClass("fa-chevron-up").addClass("fa-chevron-down");
        }
        else {
            $(chevron).removeClass("fa-chevron-down").addClass("fa-chevron-up");
        }

        $target.closest("tr").next().find(".order-details").slideToggle();
    }
}

function openShipOrderModel(orderNumber) {
    $('#ship-order-modal-' + orderNumber).modal('toggle');
}

function openCompleteOrderModal(orderNumber) {
    $('#complete-order-modal-' + orderNumber).modal('toggle');
}

function changeOrderStatus(orderNumber, newStatus, id) {
    $('#' + id).modal('hide');
    let url = window.location.origin + '/Order/ChangeOrderStatus';

    showLoader();
    var deliveryTime = "";

    if (newStatus == "SHIPPED") {
        deliveryTime = $("#order-time").val();
    }

    $.ajax({
        url: url,
        type: 'POST',
        data: {
            orderNumber: orderNumber,
            newOrderStatus: newStatus,
            deliveryTime: deliveryTime
        },
        cache: false,
        success: function () {
            window.scroll(0, 0);
            hideLoader();
            window.location.reload();
        },
        error: function () {
            window.location = "/error";
        }
    });
}