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
            }
        });

        return false;
    });
});

//function openDeleteModal(productCode) {
//    $('#' + productCode).modal('toggle');
//}

//function deleteProduct(productId) {
//    let url = window.location.origin + '/Product/Delete?productId=' + productId;

//    $.ajax({
//        url: url,
//        type: 'POST',
//        cache: false,
//        success: function () {
//            window.location.href = window.location.origin + '/Product/List?pageNumber=1';
//        }
//    });
//}

