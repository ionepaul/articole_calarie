﻿$(document).ready(function () {
    $('#back').on('click', function () {
        window.history.back();
    });

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
    });

    if ($(window).width() <= 639) {
        $('#appLogo').attr("src", '/images/mobile_logo.png');
    }

    $("#log-out-btn").on("click", function () {
        $("#logoutForm").submit();
    });

    if (window.location.hash && window.location.hash == '#_=_') {
        window.location.hash = '';
    }

    initCookieBar();
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