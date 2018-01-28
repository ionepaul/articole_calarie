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