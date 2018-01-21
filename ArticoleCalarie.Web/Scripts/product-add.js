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
            $.getJSON('/Product/GetBrands?searchTerm=' + request.term, function (data) {
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
            var formattedFileName = file.name.replace(' ', '').replace('-', '').replace('_', '');
            var containerId = formattedFileName.replace('.', '');

            var upload = new Upload(file, isSizeChart);
            upload.doUpload();

            var ImageDir = new FileReader();
            ImageDir.onload = function (e) {
                if (!isSizeChart) {
                    $('#imgDirectory').append('<div id="' + containerId + '"></div>');
                    $('#' + containerId).append('<div id="progress-wrp"><div class="progress-bar"></div><div class="status">0%</div></div>');
                    $('#' + containerId).append('<img src="' + e.target.result + '" height="100" />');
                    $('#' + containerId).append('<a onclick="deleteImage(\'' + formattedFileName + '\')">delete image</a>');
                } else {
                    $('#newChartSizeImgContainer').append('<div id="' + containerId + '"></div>');
                    $('#' + containerId).append('<div id="progress-wrp"><div class="progress-bar"></div><div class="status">0%</div></div>');
                    $('#' + containerId).append('<img src="' + e.target.result + '" height="100" />');
                    $('#' + containerId).append('<a onclick="deleteImage(\'' + formattedFileName + '\')">delete image</a>');
                }
            }

            ImageDir.readAsDataURL(file);
        });
    }
}

function deleteImage(fileName) {
    $.ajax({
        type: "POST",
        url: "DeleteImage?filename=" + fileName,
        success: function () {
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
        url: "GetSizeCharts",
        success: function (data) {
            if (data && data.length > 0) {
                $.each(data, function (i, sizeChart) {
                    $('#sizeChartImgContainer').append('<img id="' + i + '" src="' + window.location.origin + '/images/products/' + sizeChart.FileName + '" height="200" onclick="selectSizeChart(' + sizeChart.Id + ')"/>');
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
        url: "GetColors",
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
        },
        error: function (error) {
            // handle error
        },
        async: true,
        timeout: 60000
    });

    selectedCategoryId = $('#categories-select').find(":selected").value;
    $('#CategoryId').val(selectedCategoryId);

    $('#categories-select').on('change', function () {
        selectedCategoryId = this.value;
        $('CategoryId').val(selectedCategoryId);
    })
}

function selectSizeChart(sizeChartId) {
    $('#sizeChartImgContainer img').removeClass('selected');
    $('#sizeChartImgContainer img#' + sizeChartId).addClass('selected');
    $('#SizeChartImage').val(sizeChartId);
}

function selectColor(colorName, colorId) {
    let colorIndexInArray = savedColors.indexOf(colorId);
    if (colorIndexInArray >= 0) {
        $(colorName).removeClass('selected');
        savedColors.slice(colorIndexInArray, 1);
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
        url: "UploadImage",
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
    var percent = 0;
    var position = event.loaded || event.position;
    var total = event.total;
    var progress_bar_id = "#progress-wrp";
    if (event.lengthComputable) {
        percent = Math.ceil(position / total * 100);
    }
    // update progressbars classes so it fits your code
    $(progress_bar_id + " .progress-bar").css("width", +percent + "%");
    $(progress_bar_id + " .status").text(percent + "%");
};