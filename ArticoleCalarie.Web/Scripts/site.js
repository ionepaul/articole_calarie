﻿$(document).ready(function () {
    $(window).bind("pageshow", function (event) {
        if (event.originalEvent.persisted) {
            window.location.reload();
        }
    });

    $('#back').on('click', function () {
        window.history.back();
    });

    changeLogo();

    $("#content-pager a[href]").on("click", function () {
        showLoader();
    });

    $("#menu-main-menu a[href]").on("click", function () {
        showLoader();
    });

    $("form").not("#logoutForm").on("submit", function () {
        if ($(this).valid()) {
            showLoader();
        }
    });

    $("a").not(".no-loading").on("click", function () {
        showLoader();
    });

    $(window).load(function () {
        setTimeout(function () {
            hideLoader();
        }, 200);

        checkCarousel();
        checkPagination();
    });

    $(window).resize(function () {
        changeLogo();
        checkCarousel();
    });

    $("#log-out-btn").on("click", function () {
        $("#logoutForm").submit();
    });

    if (window.location.hash && window.location.hash === '#_=_') {
        window.location.hash = '';
    }

    initCookieBar();

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

    $('#user-newsletter-checkbox').on("click", function () {
        var isSubscribed = document.getElementById('user-newsletter-checkbox').checked;
        let url = window.location.origin + '/account/updatenewslettersubscription?issubscribed=' + isSubscribed;

        showLoader();

        $.ajax({
            type: "POST",
            url: url,
            success: function () {
                hideLoader();
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

function initCookieBar() {
    let url = window.location.origin + '/home/cookiesaccepted'

    $('#cookie-accept-btn').on("click", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: url,
            success: function (result) {
                $('.cookies-bar').css('display', 'none');
            }
        });
    });
}

function showLoader() {
    $("#spinner").removeClass("hidden");
}

function hideLoader() {
    $("#spinner").addClass("hidden");
}

function checkCarousel() {
    let isOnDetails = window.location.pathname.indexOf('produse') > -1;
    if (isOnDetails && $('.owl-thumbs').length > 0) {
        var obj = $('.owl-thumbs').first();
        var height = $(obj).height();
        if (height > $('.main-img').first().height()) {
            $('.owl-carousel').first().height(height);
        }
    }
}

function checkPagination() {
    if ($('.pagination').length > 0 && $('.pagination > li.disabled.PagedList-ellipses a').length > 0) {
        $('.pagination > li.disabled.PagedList-ellipses a').addClass('no-loading hidden');
    }
}

function getRelatedProducts(id) {
    let url = window.location.origin + '/product/getrelatedproducts'

    $.ajax({
        type: "GET",
        cache: false,
        url: url,
        success: function (result) {
            $(id).html(result);
        }
    });
}

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

function openDeleteAccountModal() {
    $('#delete-account-modal').modal('toggle');
}

function deleteAccount() {
    let url = window.location.origin + '/account/deleteuser';
    showLoader();
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        success: function () {
            window.location = "/";

        },
        error: function () {
            window.location = "/error";
        }
    });
}

function changeLogo() {
    if ($(window).width() < 639) {
        $('#appLogo').attr("src", '/images/mobile_logo.png');
    } else {
        $('#appLogo').attr("src", '/images/articole-calarie-logo.png');
    }
}

function viewProduct(categoryName, subcategoryId, subcategoryName, productCode, productNameUrl) {
    showLoader();

    window.location.href = window.location.origin + '/produse/' + categoryName + '/' + subcategoryId + '/' + subcategoryName + '/' + productCode + '/' + productNameUrl;
}