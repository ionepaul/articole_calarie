$(document).ready(function () {
    var searchTimeout = null;

    $('#searchInput').on('keyup', function () {
        clearTimeout(searchTimeout);
        var $this = $(this);
        searchTimeout = setTimeout(function () { searchProducts($this.val()) }, 500);
    });

    function searchProducts(productCode) {
        $.ajax({
            type: "GET",
            cache: false,
            url: "ProductListForAdmin?pageNumber=1&productCode=" + productCode,
            success: function (result) {
                $('#products-content').html(result);
            }
        });
    }

    $(document).on("click", "#content-pager a[href]", function () {
        $.ajax({
            url: $(this).attr("href"),
            type: 'GET',
            cache: false,
            success: function (result) {
                $('#products-content').html(result);
            }
        });
        return false;
    });
});

function openDeleteModal(productCode) {
    $('#' + productCode).modal('toggle');
}

function deleteProduct(productId) {
    let url = window.location.origin + '/Product/Delete?productId=' + productId;

    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        success: function () {
            window.location.href = window.location.origin + '/Product/List?pageNumber=1';
        }
    });
}