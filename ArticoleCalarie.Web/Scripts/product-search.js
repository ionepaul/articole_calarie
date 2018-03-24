$(document).ready(function () {
    var colorIds = [];
    var sizes = [];

    $("#product-list-page").on("click", "#searchColors li", function () {
        let colorId = $(this).attr("id");
        var colorIndex = colorIds.indexOf(colorId);

        if (colorIndex > -1) {
            $(this).removeClass("current");
            colorIds.splice(colorIndex, 1);
        }
        else {
            $(this).addClass("current");
            colorIds.push(colorId);
        }

        console.log(colorIds);
    });

    $("#product-list-page").on("click", "#searchSizes li", function () {
        let size = $(this).attr("id");
        var sizeIndex = sizes.indexOf(size);

        if (sizeIndex > -1) {
            $(this).removeClass("active");
            sizes.splice(sizeIndex, 1);
        }
        else {
            $(this).addClass("active");
            sizes.push(size);
        }
    });

    $.each($("#searchColors li"), function (i, el) {
        if ($(this).hasClass("current")) {
            let colorId = $(this).attr("id");
            colorIds.push(colorId);
        }
    });

    $.each($("#searchSizes li"), function (i, el) {
        if ($(this).hasClass("active")) {
            let size = $(this).attr("id");
            sizes.push(size);
        }
    });

    $("#product-list-page").on('click', '#search', function () {
        let minPrice = $(".amount-range-price .from").text().replace(/\s/g, '').replace(/-/g, '').replace(',', '.');
        let maxPrice = $(".amount-range-price .to").text().replace(/\s/g, '').replace(/-/g, '').replace(',', '.');

        showLoader();

        window.location.href = '/produse/' + categoryName + '/' + subcategoryId + '/' + subcategoryName + '?page=1&minp=' + minPrice + '&maxp=' + maxPrice + '&cl=' + colorIds + '&sz=' + sizes;
    });
});