$(document).ready(function () {
    $('#editDeliveryAddressBtn').on('click', function () {
        $('#deliveryAddress').addClass('hidden');
        $('#deliveryAddressForm').removeClass('hidden');
    });

    $('#editBillingAddressBtn').on('click', function () {
        $('#billingAddress').addClass('hidden');
        $('#billingAddressForm').removeClass('hidden');
    });
});

function acccounEditAddressOnComplete() {
    window.location.href = '/account/administrare';
}
