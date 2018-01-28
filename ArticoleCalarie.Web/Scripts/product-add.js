//globalVariables
var selectedCategoryId;
var savedImages = new Array();
var savedColors = new Array();

$(document).ready(function () {
    loadSizeCharts();
    loadColors();
    loadCategories();

    $("#subcategory-autocomplete").autocomplete({
        source: function (request, response) {
            $.getJSON('/Subcategory/GetSubcategories?categoryId=' + selectedCategoryId + '&searchTerm=' + request.term, function (data) {
                if (data.length == 0) {
                    $('#SubcategoryId').val(request.term);
                }
                response($.map(data, function (item) {
                    return {
                        label: item.Name,
                        value: item.Id
                    };
                }));
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            var categoryName = ui.item.label;
            $(this).val(categoryName);
            event.preventDefault();
        },
        select: function (event, ui) {
            event.preventDefault();
            var categoryName = ui.item.label;
            $(this).val(categoryName);
            $('#SubcategoryId').val(ui.item.value);
        }
    });

    $("#brand-autocomplete").autocomplete({
        source: function (request, response) {
            $.getJSON('/Brand/GetBrands?searchTerm=' + request.term, function (data) {
                if (data.length == 0) {
                    $('#Brand').val(request.term);
                }
                response($.map(data, function (item) {
                    return {
                        label: item.Name,
                        value: item.Id
                    };
                }));
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            var categoryName = ui.item.label;
            $(this).val(categoryName);
            event.preventDefault();
        },
        select: function (event, ui) {
            event.preventDefault();
            var categoryName = ui.item.label;
            $(this).val(categoryName);
            $('#Brand').val(ui.item.value);
        }
    });
});

function previewAndUpload(input, isSizeChart) {
    if (input.files) {
        $.each(input.files, function (i, file) {
            var formattedFileName = file.name.replace(/\s/g, '').replace(/-/g, '').replace(/_/g, '').replace(/\(/g, '').replace(/\)/g, '');
            var containerId = formattedFileName.replace('.', '');
            var upload = new Upload(file, isSizeChart);
            upload.doUpload();

            var ImageDir = new FileReader();
            ImageDir.onload = function (e) {
                if (!isSizeChart) {
                    $('#imgDirectory').append('<div class="image-wrapper" id="' + containerId + '"></div>');
                   // $('#' + containerId).append('<div id="progress-wrp"><div class="progress-bar"></div><div class="status">0%</div></div>');
                    $('#' + containerId).append('<img class="image" src="' + e.target.result + '" height="100" />');
                    $('#' + containerId).append('<a class="delete-btn" onclick="deleteImage(\'' + formattedFileName + '\')">delete image</a>');
                } else {
                    $('#newChartSizeImgContainer').empty();
                    $('#newChartSizeImgContainer').append('<div class="image-wrapper" id="' + containerId + '"></div>');
                    //$('#' + containerId).append('<div id="progress-wrp"><div class="progress-bar"></div><div class="status">0%</div></div>');
                    $('#' + containerId).append('<img class="image" src="' + e.target.result + '" height="100" />');
                    $('#' + containerId).append('<a class="delete-btn" onclick="deleteImage(\'' + formattedFileName + '\')">delete image</a>');
                }
            }

            ImageDir.readAsDataURL(file);
        });
    }
}

function clearFileInput(input) {
    input.value = null;
}

function deleteImage(fileName, isSizeChart) {
    $.ajax({
        type: "POST",
        url: "../Image/DeleteImage?filename=" + fileName,
        success: function () {
            if (!isSizeChart) {
                let arrayIndex = savedImages.indexOf(fileName);
                if (arrayIndex >= 0) {
                    savedImages.splice(arrayIndex, 1);
                }

                $('#Images').val(savedImages);
            }
            else {
                $('#SizeChartImage').val("");
            }

            $('#' + fileName.replace('.', '')).remove();
        },
        error: function (error) {
            // handle error
        },
        async: true,
        timeout: 60000
    });
}

function loadSizeCharts() {
    $.ajax({
        type: "GET",
        url: "../SizeChart/GetSizeCharts",
        success: function (data) {
            if (data && data.length > 0) {
                $.each(data, function (i, sizeChart) {
                    var formattedSizeChartName = sizeChart.FileName.replace('.', '');

                    $('#sizeChartImgContainer').append('<img id="' + formattedSizeChartName + '" src="' + window.location.origin + '/images/products/' + sizeChart.FileName + '" height="200" onclick="selectSizeChart(' + formattedSizeChartName + ',' + sizeChart.Id + ')"/>');
                });
            }
        },
        error: function (error) {
            // handle error
        },
        async: true,
        timeout: 60000
    });
}

function loadColors() {
    $.ajax({
        type: "GET",
        url: "../Color/GetColors",
        success: function (data) {
            if (data && data.length > 0) {
                $.each(data, function (i, color) {
                    $('#colors-list').append('<li id="' + color.Name + '" style="background-color:' + color.Hex + '" onclick="selectColor(' + color.Name + ',' + color.Id + ')"/>');
                });
            }
        },
        error: function (error) {
            // handle error
        },
        async: true,
        timeout: 60000
    });
}

function loadCategories() {
    $.ajax({
        type: "GET",
        url: "../Category/GetCategories",
        success: function (data) {
            if (data && data.length > 0) {
                $.each(data, function (i, category) {
                    $('#categories-select').append('<option value="' + category.Id + '">' + category.Name + '</option>');
                });
            }

            selectedCategoryId = $('#categories-select option:selected')[0].value;
            $('#CategoryId').val(selectedCategoryId);
        },
        error: function (error) {
            // handle error
        },
        async: true,
        timeout: 60000
    });

    $('#categories-select').on('change', function () {
        selectedCategoryId = this.value;
        $('#CategoryId').val(selectedCategoryId);
    })
}

function selectSizeChart(sizeChart, sizeChartId) {
    $('#sizeChartImgContainer img').removeClass('selected');
    $(sizeChart).addClass('selected');
    $('#SizeChartImage').val(sizeChartId);
}

function selectColor(colorName, colorId) {
    let colorIndexInArray = savedColors.indexOf(colorId);
    if (colorIndexInArray >= 0) {
        $(colorName).removeClass('selected');
        savedColors.splice(colorIndexInArray, 1);
    }
    else {
        $(colorName).addClass('selected');
        savedColors.push(colorId);
    }
    $('#Colors').val(savedColors);
}

//Uploading object 
var Upload = function (file, isSizeChart) {
    this.file = file;
    this.isSizeChart = isSizeChart;
};

Upload.prototype.getType = function () {
    return this.file.type;
};
Upload.prototype.getSize = function () {
    return this.file.size;
};
Upload.prototype.getName = function () {
    return this.file.name;
};
Upload.prototype.doUpload = function () {
    var that = this;
    var formData = new FormData();

    // add assoc key values, this will be posts values
    formData.append("file", this.file, this.getName());
    formData.append("upload_file", true);

    $.ajax({
        type: "POST",
        url: "../Image/UploadImage",
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                myXhr.upload.addEventListener('progress', that.progressHandling, false);
            }
            return myXhr;
        },
        success: function (data) {
            if (!that.isSizeChart) {
                savedImages.push(data);
                $('#Images').val(savedImages);
            }
            else {
                $('#SizeChartImage').val(data);
            }
        },
        error: function (error) {
            // handle error
        },
        async: true,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        timeout: 60000
    });
};

Upload.prototype.progressHandling = function (event) {
   /* var percent = 0;
    var position = event.loaded || event.position;
    var total = event.total;
    var progress_bar_id = "#progress-wrp";
    if (event.lengthComputable) {
        percent = Math.ceil(position / total * 100);
    }
    // update progressbars classes so it fits your code
    $(progress_bar_id + " .progress-bar").css("width", +percent + "%");
    $(progress_bar_id + " .status").text(percent + "%"); */
};