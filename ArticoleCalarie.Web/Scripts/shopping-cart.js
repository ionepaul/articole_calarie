﻿$(document).ready(function () {
    var color;
    if ($(".product-single-page .list-color li").length > 0) {
        $(".product-single-page .list-color li").first().addClass("current");
        color = $(".product-single-page .list-color li:first a").attr("id");
    }

    $(".product-single-page .list-color li a").on("click", function () {
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
            productCode: productCode,
            productCategoryName: productCategoryName,
            productSubcategoryName: productSubcategoryName,
            productSubcategoryId: productSubcategoryId
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
                window.scrollTo(0, 0);
                $("#added-to-order").fadeIn("slow");
                setTimeout(function () {
                    $("#added-to-order").fadeOut("slow");
                }, 3000);
            },
            error: function (error) {
                hideLoader();
                window.location = "/error";
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
            if (window.location.href.indexOf("comanda/produse") > -1) {
                window.location.reload();
            } else {
                hideLoader();
            }
        },
        error: function (error) {
            hideLoader();
            window.location = "/error";
        },
        async: true,
        timeout: 60000
    });
}

function viewProduct(categoryName, subcategoryId, subcategoryName, productCode, productNameUrl) {
    showLoader();

    window.location.href = window.location.origin + '/produse/' + categoryName + '/' + subcategoryId + '/' + subcategoryName + '/' + productCode + '/' + productNameUrl;
}