//====== Enums & Constants =========

var AjaxActionToBePerformed = '';

var ajaxRequestQueue = [];

var MessageType = {
    Success: 1,
    Error: 2,
    Warning: 3,
    None: 4
};

var ActionStatus = {
    Success: 200,
    Error: 400
}

var UserRoles = {
    User: 1,
    CompanyAdmin: 2,
    SuperAdmin: 3
}

var EditActionType = {
    Edit: 1,
    EditAndNew: 2,
    Cancel: 3
}



//====== End Enums & Constants =====

$(document).ready(function () {

});



$.ScrollToTop = function () {
    var offset = 220;
    var duration = 500;
    $(window).scroll(function () {
        if ($(this).scrollTop() > offset) $('.back-to-top').fadeIn(duration);
        else $('.back-to-top').fadeOut(duration);
    });


    $('.back-to-top').click(function (event) {
        event.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, duration);
        return false;
    });
}

$.CreateWaterMark = function () {

    /*==========Sample Usage & Datatype Examples =============    
    <input name="" type="text" watermark="XXXXXXXX">    
    ==========================================================*/

    $("input[type=text][watermark], textarea[watermark]").each(function () {
        $(this).bind("focus", function () {
            if ($(this).val() == $(this).attr("watermark")) $(this).val("");
        });

        $(this).bind("blur", function () {
            if ($(this).val() == "") $(this).val($(this).attr("watermark"));
        });
    });
}

$.ShowThrobber = function (throbberPosition) {
    $("#MainThrobberImage").show().position(throbberPosition);
}

$.RemoveThrobber = function () {
    $("#MainThrobberImage").hide();
}


$.ShowMessage = function (messageSpan, message, messageType) {
    /*================= Sample Usage =========================
    $.ShowMessage($(selector), "This is a dummy message", MessageType.Success)
    ==========================================================*/
    if (messageType == MessageType.Success) {
        $(messageSpan).html(message).removeClass('error').removeClass("info").addClass("success").fadeIn();
        //$("html,body").animate({ scrollTop: "0" }, "slow");
    }
    else if (messageType == MessageType.Error) {
        $(messageSpan).html(message).addClass('error').removeClass("success").removeClass("info").fadeIn();
        $("html,body").animate({ scrollTop: "0" }, "slow");
    }
    else if (messageType == MessageType.Warning) {
        $(messageSpan).html(message).removeClass("success").addClass("info").fadeIn();
        $("html,body").animate({ scrollTop: "0" }, "slow");
    }
    else $(messageSpan).html('').hide();
}

$.IsNumericCustom = function (input) {
    return (input - 0) == input && input.length > 0;
}

$.IsEmailCustom = function (email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

$.ValidateFiles = function (form) {

    var isValid = true;

    $(form).find("input[type=file][required-field]").each(function () {
        if (!$.trim($(this).val())) {
            isValid = false;
            $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-error').removeClass('field-validation-valid').html($(this).attr('required-field'));
        }
        else $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-valid').removeClass('field-validation-error').html('');
    });

    $(form).find("input[type=file][allowed-formats]").each(function () {
        if ($(this).val()) {
            var filetype = $(this).val().split(".");
            filetype = filetype[filetype.length - 1].toLowerCase();

            if ($(this).attr("allowed-formats").indexOf(filetype) == -1) {
                isValid = false;
                $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-error').removeClass('field-validation-valid').html($(this).attr('error-message'));
            }
            else $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-valid').removeClass('field-validation-error').html('');
        }
    });

    return isValid;
}
$.IsAlphaNumericCustom = function (val) {
    if (/[^a-zA-Z0-9 ]/.test(val)) return false;
    return true;
}

$.ValidateFiles = function (form) {

    var isValid = true;

    $(form).find("input[type=file][required-field]").each(function () {
        if (!$.trim($(this).val())) {
            isValid = false;
            $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-error').removeClass('field-validation-valid').html($(this).attr('required-field'));
        }
        else $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-valid').removeClass('field-validation-error').html('');
    });

    $(form).find("input[type=file][allowed-formats]").each(function () {
        if ($(this).val()) {
            var filetype = $(this).val().split(".");
            filetype = filetype[filetype.length - 1].toLowerCase();

            if ($(this).attr("allowed-formats").indexOf(filetype) == -1) {
                isValid = false;
                $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-error').removeClass('field-validation-valid').html($(this).attr('error-message'));
            }
            else $(form).find('span[for=' + $(this).attr('name') + ']').addClass('field-validation-valid').removeClass('field-validation-error').html('');
        }
    });

    return isValid;
}


$.ajaxExt = function (parameters) {
    /*=====================================Sample Usage======================================================
    $.ajaxExt({
    type: "POST",                                                                                       //default is "POST"
    error: function () { },                                                                             //called when an unexpected error occurs
    data: {name: "value"}                                                                               //overwrites the form parameter
    messageControl:  $(selector),                                                                       //the control where the status message needs to be displayed
    throbberPosition: { my: "left center", at: "right center", of: sender, offset: "5 0" },             //the position at which the throbber needs to be displayed 
    url: url,                                                                                           //the url that needs to be hit
    success: function (data) {},                                                                        //called after the request has been executed without any unhandeled exception
    showThrobber: false                                                                                 //If the throbber need to be displayed
    showErrorMessage : true                                                                             //If the error message needs to be displayed
    containFiles: false                                                                                 //If the form contains files            
    formToPost: $('form'),                                                                               //The reference to the form to be posted
    abort: true,                                                                                        // Pass this parameter if you want to abort the ajax request on focus lost
    forPopup: true,                                                                                      // Pass this as true if want to open the result in a popup
    title: 'popupTitle',
    width:300,
    html:"htmlcontent" //The html content that need to be shown in the popup
    });
    ===============================================================================================*/
    function onError(a, b, c, parameters) {
        if (parameters.showErrorMessage != false) $.ShowMessage($(parameters.messageControl), "Unexpected Error", MessageType.Error);
        else if (parameters.error != undefined) parameters.error("Unexpected Error");

        if (parameters.showThrobber == true) $.RemoveThrobber();
    }

    function onSuccess(data, parameters) {
        // console.clear();        
        if (parameters.showThrobber == true) $.RemoveThrobber();

        try {
            if (data.Status == undefined) {
                if (parameters.showErrorMessage != false) $.ShowMessage($(parameters.messageControl), "Invalid data returned in the response", MessageType.Error);
                else if (parameters.error != undefined) parameters.error("Invalid data returned in the response");

                return false;
            }
        }
        catch (ex) {
            if (parameters.showErrorMessage != false) $.ShowMessage($(parameters.messageControl), "Invalid data returned in the response", MessageType.Error);
            else if (parameters.error != undefined) parameters.error("Invalid data returned in the response");
        }

        if (data.Status == ActionStatus.Error) {
            if (parameters.showErrorMessage != false)
                $.ShowMessage($(parameters.messageControl), data.Message, MessageType.Error);
            if (parameters.error != undefined) {
                parameters.error(data.Message);
            }
            if (data.Results != undefined && data.Results[0] == 'LoginExipred')
                window.location.href = data.Results[1];
                // else if (data.Results != undefined && data.Results[0] == "HttpRequestValidationException")
                //alert(data.Message);
                //else
                //alert(data.Message)
            else if (parameters.error != undefined)
                parameters.error(data.Results, data.Message)
        }
        else if (parameters.success) {
            if (parameters.forPopup == true) {
                $.OpenPopup(parameters, data.Results)
                $('.backgroundPopup').show();
            } else {
                $.ShowMessage($(parameters.messageControl), data.Message, MessageType.Success);
                parameters.success(data.Results, data.Message);
            }
            setTimeout(function () {
                var h = $('.inner-popup').outerHeight();
                if (h > 0)
                    $('.popupbox').css({ 'height': h + 'px' });
            }, 100);
        }
    }

    parameters.type = parameters.type == undefined ? "POST" : parameters.type;
    parameters.showErrorMessage = parameters.showErrorMessage == undefined ? false : parameters.showErrorMessage;
    parameters.showThrobber = parameters.showThrobber == undefined ? true : parameters.showThrobber;
    parameters.validate = parameters.validate == undefined ? false : parameters.validate;
    parameters.containFiles = parameters.containFiles == undefined ? false : parameters.containFiles;

    if (parameters.validate == true) {
        var isValidForm = $(parameters.formToValidate).valid();
        var isValidFiles = $.ValidateFiles(parameters.formToValidate);
        //if (!!isValidForm) return false;
        if (!isValidForm || !isValidFiles) return false;
    }


    if (parameters.showErrorMessage != false) $.ShowMessage($(parameters.messageControl), "", MessageType.None);
    if (parameters.showThrobber == true) {
        if (parameters.throbberPosition == undefined)
            parameters.throbberPosition = { my: "center center", at: "center center", of: $(window), offset: "5 0" };
        $.ShowThrobber(parameters.throbberPosition);
    }
    if (parameters.forPopup == true && parameters.html != "" && parameters.html != undefined) {
        $(parameters.html).show();
    }
    else {
        if (parameters.containFiles == true) {
            // inside event callbacks 'this' is the DOM element so we first 
            // wrap it in a jQuery object and then invoke ajaxSubmit 
            $(parameters.formToPost).ajaxSubmit({
                target: parameters.messageControl,   // target element(s) to be updated with server response 
                beforeSubmit: function () { $.ShowThrobber(parameters.throbberPosition); },  // pre-submit callback
                success: function (data) { onSuccess(data, parameters); },  // post-submit callback 
                // other available options: 
                url: parameters.url,         // override for form's 'action' attribute 
                type: parameters.type,        // 'get' or 'post', override for form's 'method' attribute 
                dataType: 'json',        // 'xml', 'script', or 'json' (expected server response type) 
                clearForm: false,        // clear all form fields after successful submit 
                resetForm: false        // reset the form after successful submit 
            });

            // !!! Important !!! 
            // always return false to prevent standard browser submit and page navigation 
            return false;
        }
        else {
            // Enable Div Overlay to prevent user clicks to interrupt ajax request.
            $('#lightbox_overlay').show();
            var request = $.ajax({
                url: parameters.url,
                type: parameters.type,
                dataType: "json",
                data: parameters.data,
                error: function (a, b, c) { $('#lightbox_overlay').hide(); onError(a, b, c, parameters); },
                success: function (data) { $('#lightbox_overlay').hide(); onSuccess(data, parameters); }
            });
            if (parameters.abort == true)
                ajaxRequestQueue.push(request);
        }
    }
}

$(document).on('click', '.backgroundPopup', function () {
    //$.ClosePopupWindow();
});

$.OpenPopup = function (parameters, data) {
    var offsetX = parameters.offsetX == undefined ? "0" : parameters.offsetX;
    var offsetY = parameters.offsetY == undefined ? "0" : parameters.offsetY;
    $("#lightBox div.popUpContent h4[name=Title]").html(parameters.title)
    $("#lightBox").show().position({ my: "center center", at: "center center", of: $(window), offset: offsetX + " " + offsetY });
    $("#lightBox div.popUpContent div[name=ActualContent]").html(data[0])
                                                           .css({ 'overflow-y': 'auto', 'overflow-x': 'hidden', 'width': parameters.width });
    $("#lightBox").show()
                  //.css({ 'min-height': '400px' })
                  .position({ my: "center center", at: "center center", of: $(window), offset: offsetX + " " + offsetY });
    $('.popUp .popUpContent .selectbox-section ul').css({ 'width': (parameters.width - 4) + 'px' });
    var form = $("#lightBox form:first");
    $.ResetUnobtrusiveValidation(form);
}
$.OpenPopupWindow = function (parameters) {
    /*=====================================Sample Usage======================================================
    $.OpenPopupWindow({
    url: url,           //the url that needs to be hit
    width: xxx,         //The width of the popup window 
    offsetX: xxx,       //No of pixels to be added horizontally from the center of the screen 
    offsetY: xxx,       //No of pixels to be added vertically from the center of the screen 
    title: "xxxxxx"     //The text to be displayed as the title of the popup windiw 
    html:"htmlcontent" //The html content that need to be shown in the popup
    type: "POST",
    height:xxx
    });
    ===============================================================================================*/

    var offsetX = parameters.offsetX == undefined ? "0" : parameters.offsetX;
    var offsetY = parameters.offsetY == undefined ? "0" : parameters.offsetY;
    var type = parameters.type == undefined ? "POST" : parameters.type;
    var scroll = parameters.scroll == undefined ? false : true;

    $("#lightbox_overlay1").show();
    $("#lightBox div.popUpContent h4[name=Title]").html(parameters.title)
    //var popupdiv = $("#lightBox div.popUpContent div[name=ActualContent]");
    //popupdiv.css("min-width", parameters.width == undefined ? 400 : parameters.width);
    $("#lightBox div.popUpContent div[name=ActualContent]").css("width", parameters.width == undefined ? 400 : parameters.width).css("overflow", "hidden");
    //height and scroll is uncommented By Nishant mahajan changes 1/11/2014 for EAlert Popup.  
    $("#lightBox div.popUpContent div[name=ActualContent]").css("height", parameters.height == undefined ? 400 : parseInt(parameters.height) + 50)
    if (scroll == true) {
        $("#lightBox div.popUpContent div[name=ActualContent]").css("overflow-x", "hidden").css("overflow-y", "auto");
    }
    $("#lightBox").show().position({ my: "center center", at: "center center", of: $(window), offset: offsetX + " " + offsetY });

    if (parameters.form != undefined) parameters.data = "null";
    else if (parameters.data == undefined) parameters.data = "null";

    if (!parameters.html) {
        $.ajaxExt({
            type: type,
            validate: false,
            showThrobber: true,
            throbberPosition: { my: "center center", at: "center center", of: $("#lightBox div.popUpContent div[name=ActualContent]"), offset: "0 0" },
            messageControl: $("div[name=StatusMessagePopup]"),
            url: parameters.url,
            data: parameters.data,
            success: function (results) {
                $("#lightBox div.popUpContent div[name=ActualContent]").html(results[0]);
                var maskHeight = $(window).height();
                var maskWidth = $(window).width();

                // calculate the values for center alignment
                var dialogTop = (maskHeight - $('#lightBox').height()) / 2;
                var dialogLeft = (maskWidth - $('#lightBox').width()) / 2;
                if (dialogTop > 300) dialogTop = dialogTop - 150;
                $("#lightBox").css({ top: dialogTop, left: dialogLeft }).css("max-width", maskWidth - 50).show();
                //.position({ my: "center center", at: "center center", of: $(window), offset: offsetX + " " + offsetY });

                //$.CreateWaterMark();
                //$.ApplyMaxLength();
                //$.InitializeDatatypeControls();

                var form = $("#lightBox div.popUpContent div[name=ActualContent] form:first");
                $.ResetUnobtrusiveValidation(form);
            }
        });
    } else {
        $("#lightBox div.popUpContent div[name=ActualContent]").html(parameters.html);
        $("#lightBox").show().position({ my: "center center", at: "center center", of: $(window), offset: offsetX + " " + offsetY });
    }
}

$.CenterPopupWindow = function (parameters) {
    var offsetX = parameters.offsetX == undefined ? "0" : parameters.offsetX;
    var offsetY = parameters.offsetY == undefined ? "0" : parameters.offsetY;

    $("#lightBox").show().position({ my: "center center", at: "center center", of: $(window), offset: offsetX + " " + offsetY });
}

$(document).on('click', '.popupbox .inner-popup .close-btn', function () {
    $.ClosePopupWindow();
});

$.ClosePopupWindow = function () {
    $('.backgroundPopup').fadeOut(300);
    $("#lightBox").fadeOut(300);
    $("#lightbox_overlay1").fadeOut(300);
    setTimeout(function () {
        $("#lightBox div.popUpContent div[name=ActualContent]").html('<div class="autofeedback"><span name="StatusMessagePopup" class="errormessage" style="display: none"></span></div>');
    }, 300);
    $.RemoveThrobber();
}

$.postifyData = function (value) {
    var result = {};

    var buildResult = function (object, prefix) {
        for (var key in object) {
            var postKey = isFinite(key)
                ? (prefix != "" ? prefix : "") + "[" + key + "]"
                : (prefix != "" ? prefix + "." : "") + key;

            switch (typeof (object[key])) {
                case "number": case "string": case "boolean":
                    result[postKey] = object[key];
                    break;

                case "object":
                    if (object[key] != null) {
                        if (object[key].toUTCString) result[postKey] = object[key].toUTCString().replace("UTC", "GMT");
                        else buildResult(object[key], postKey != "" ? postKey : key);
                    }
            }
        }
    };
    buildResult(value, "");

    return result;
}

$.CompareDateRange = function (url, startDate, endDate, callback) {
    $.ajaxExt({
        type: 'POST',
        validate: false,
        showThrobber: false,
        showErrorMessage: false,
        url: url,
        data: { startDate: startDate, endDate: endDate },
        error: function (message) { alert(message); },
        success: function (results) { callback(results[0]); }
    });
}

$.ResetUnobtrusiveValidation = function (form) {
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
}

//=============================================
//EXTENSION METHODS============================
//=============================================

if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}

if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return $.trim(this);
    };
}

if (!String.prototype.isNullOrEmpty) {
    String.prototype.isNullOrEmpty = function () {
        return (this.trim() == '' || this == null || this == undefined)
    };
}

if (!String.prototype.fetchNumber) {
    String.prototype.fetchNumber = function () {
        return parseInt(this.trim().replace(/[^\d.]/g, ''));
    };
}

function setDefaultText(editorid, msg) {
    if (CKEDITOR.instances[editorid].getData() == '') {
        CKEDITOR.instances[editorid].setData(msg);
    }
}

function RemoveDefaultText(editorid, msg) {
    if (CKEDITOR.instances[editorid].getData().indexOf(msg) >= 0) {
        CKEDITOR.instances[editorid].setData('');
        CKEDITOR.instances[editorid].focus();
    }
}

function matchYoutubeUrl(url) {
    var p = /^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/;
    return (url.match(p)) ? RegExp.$1 : false;
}

function YoutubeVideoID(url) {
    var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
    var match = url.match(regExp);

    if (match && match[2].length == 11) {
        return match[2];
    } else {
        return 'error';
    }
}

Array.prototype.clean = function (deleteValue) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == deleteValue) {
            this.splice(i, 1);
            i--;
        }
    }
    return this;
};

function pad(number, length) {
    var str = "" + number;
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}

function TimeZoneOffset() {
    var offset = new Date().getTimezoneOffset();
    offset = ((offset < 0 ? '+' : '-') + pad(parseInt(Math.abs(offset / 60)), 2) + ":" + pad(Math.abs(offset % 60), 2));
    return offset;
}





$.FillSelectList = function (selectControl, data, clearData, defaultText) {

    data = $.parseJSON(data);
    if (clearData != false) $(selectControl).html("");
    if (defaultText != undefined && defaultText != "") {
        $(selectControl).append('<option value="">' + defaultText + '</option>');
    }
    if (data != null) {
        for (var i = 0; i < data.length; i++) {
            $(selectControl).append('<option value="' + data[i].Value + '">' + data[i].Text + '</option>');
        }
    }
}


//function ResetUnobtrusiveValidation(form) {
//    form.removeData('validator');
//    form.removeData('unobtrusiveValidation');
//    $.validator.unobtrusive.parse(form);
//}

/// <summary>
/// <para>Author: Sudeep Sehgal</para>
/// <para>Created: 23 May 2012</para>
/// <para>It appends the data of a json list to the specified dropdown</para>
/// </summary>
function FillSelectList(selectControl, data, clearData, selectedItem) {

    data = $.parseJSON(data);
    if (clearData != false) $(selectControl).html("");

    for (var i = 0; i < data.length; i++) {
        if (selected != '' && data[i].Value == selectedItem)
            $(selectControl).append('<option selected="selected" value="' + data[i].Value + '">' + data[i].Text + '</option>');
        else
            $(selectControl).append('<option value="' + data[i].Value + '">' + data[i].Text + '</option>');
    }
}

function ResizeIFrame(iframeClass, height) {
    $('iframe').css('height', height);
}