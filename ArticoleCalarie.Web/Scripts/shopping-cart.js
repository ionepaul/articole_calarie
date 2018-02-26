$(document).ready(function () {
    var color;
    if ($(".list-color li").length > 0) {
        $(".list-color li").first().addClass("current");
        color = $(".list-color li:first a").attr("id");
    }

    $(".list-color li a").on("click", function () {
        $(".list-color li").removeClass("current");
        $(this).parent().addClass("current");
        color = $(this).attr("id");
    });

    $("#addToCardBtn").on("click", function () {
        let cartModel = {
            productName: productName,
            imageName: productImage,
            quantity: $("#quantity").val(),
            size: $("#size").val(),
            price: price,
            color: color,
            productCode: productCode
        };

        let url = window.location.origin + '/order/addtocart';
        showLoader();
        $.ajax({
            type: "POST",
            url: url,
            data: {
                shoppingCartItem: cartModel
            },
            success: function (data) {
                $("#shopping-cart").html(data);
                hideLoader();
            },
            error: function (error) {
                hideLoader();
                alert("Eroare la incarcarea tabelelor de marimi. Faceti un refresh la pagina.");
            },
            async: true,
            timeout: 60000
        });
    });
});

function deleteFromCart(productCode) {
    let url = window.location.origin + '/order/deletefromcart?productCode=' + productCode;
    showLoader();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            $("#shopping-cart").html(data);
            if (window.location.href.indexOf("ShoppingCartDetails") > -1) {
                window.location.reload();
            } else {
                hideLoader();
            }
        },
        error: function (error) {
            hideLoader();
            alert("Eroare la incarcarea tabelelor de marimi. Faceti un refresh la pagina.");
        },
        async: true,
        timeout: 60000
    });
}

function viewProduct(productCode) {
    showLoader();
    window.location.href = window.location.origin + '/product/details?productCode=' + productCode;
}