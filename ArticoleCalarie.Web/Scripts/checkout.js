$(document).ready(function () {
    //$(".continue").on("click", function () {
    //    $(this).closest(".hidden-content").removeClass("show");
    //    $(this).closest(".parent-content").next(".parent-content").find(".show-content").click();
    //});

    $("#checkout-login-btn").on("click", function () {
        $("#login-form").submit();
    });

    $("#checkout-register-btn").on("click", function () {
        $("#register-form").submit();
    });

    $("#checkout-save-delivery-address-btn").on("click", function () {
        $("#delivery-address-form").submit();
    });

    $("#checkout-save-billing-address-btn").on("click", function () {
        $("#billing-address-form").submit();
    });

    $("#guest-email-form-btn").on("click", function () {
        $("#guest-email-form").submit();
    });

    $("#place-order-btn").on("click", function () {
        showLoader();
    });

    $("#same-as-delivery-btn").on("change", function () {
        var isChecked = $(this).prop("checked");
        $(this).val(isChecked);

        if (!isChecked) {
            $("#billing-form-container").removeClass("hidden");
        }
        else {
            $("#billing-form-container").addClass("hidden");
        }
    });

    $("#ramburs-btn").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#terms-policy-error-msg").addClass("hidden");

        if ($("#terms-accepted-checkout").length > 0 && $("#policy-accepted-checkout").length > 0) {

            let isTermsChecked = document.getElementById("terms-accepted-checkout").checked;
            let isPolicyChecked = document.getElementById("policy-accepted-checkout").checked;

            if (!isTermsChecked || !isPolicyChecked) {
                $("#terms-policy-error-msg").removeClass("hidden");
            }
            else {
                $("#terms-accepted-checkout").attr("disabled", true);
                $("#policy-accepted-checkout").attr("disabled", true);
                showNextContainer('#delivery-method');
            }  
        } else {
            showNextContainer('#delivery-method');
        }
    });

    noClick();
});

function formOnComplete() {
    hideLoader();
}

function deliveryAddressOnSuccess() {
    showNextContainer("#delivery-address-form");
}

function billingAddressOnSuccess() {
    showNextContainer("#billing-address-form");
}

function guestEmailFormOnSuccess() {
    showNextContainer("#guest-email-form");
}

var screenX = 120;

function showNextContainer(id) {
    $(id).closest(".hidden-content").removeClass("show");
    $(id).closest(".hidden-content").closest(".parent-content").removeClass("active");
    $(id).closest(".parent-content").next(".parent-content").find("a.show-content").removeClass("noClick");
    $(id).closest(".parent-content").next(".parent-content").find(".show-content").click();

    window.scroll(0, screenX);

    screenX += 50;
    if (screenX >= 270) {
        screenX = 120;
    }
}

function noClick() {

    var list = $('.list-choosen');
    var noClickLink = $('a.noClick');
    var links = list.find(noClickLink);

    links.each(function () {
        $(this).click(function (e) {
            if ($(this).hasClass('noClick')) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        });
    });

}

function failure() {
    window.location = "/error";
}