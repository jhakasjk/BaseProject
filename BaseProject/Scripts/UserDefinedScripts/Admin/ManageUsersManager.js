var isSaved = false;
var sortOrder = 'ASC', sortColumn = 'Name';
window.onbeforeunload = function () {
    if (!isSaved) {
        return "You have not saved your data yet.  If you continue, your work will not be saved.";
    }
}

$(document).ready(function () {

    $("table[name=Listing] thead tr th i.fa-sort").on("click", function () {
        return Home.Sort($(this));
    });
    $("table[name=Listing] tbody tr td a#DeleteUser").on("click", function () {
        return Home.DeleteUser($(this));
    });

    $("#AddEditUser").on("click", function () {
        debugger;
        return Home.AddUpdateUser($(this));
    });

    $("select#Country").on("change", function () {
        return Home.BindStates($(this));
    });
    $("select#State").on("change", function () {
        return Home.BindCitys($(this));
    });
});
var Home = {
    Sort: function (sender) {

        sortColumn = $(sender).find('span').attr('class');
        sortOrder = sortOrder == 'Desc' ? 'Asc' : 'Desc';
        $(sender).parents("tr:first").find("i.fa-sort").each(function () {
            $(this).removeClass("fa-sort-asc");
            $(this).removeClass("fa-sort-desc");
        });
        if (sortOrder == 'Desc') {
            $(sender).removeClass('fa-sort-asc').addClass('fa-sort-desc');
        }
        else {
            $(sender).removeClass('fa-sort-desc').addClass('fa-sort-asc');
        }
        Paging(sender);
    },
    DeleteUser: function (sender) {

        if (confirm("Are you sure do you delete this User. After deleting user you have no any option to receive this user.")) {
            var UserId = $(sender).parents('tr:first').attr('rowid');
            $("body").removeClass("loaded");
            $.ajaxExt({
                type: "POST",
                validate: false,
                parentControl: $(sender).parents("form:first"),
                data: { UserId: UserId },
                messageControl: null,
                showThrobber: false,
                throbberPosition: { my: "left center", at: "right center", of: sender, offset: "5 0" },
                url: BaseUrl + '/ManageUsers/DeleteUser',
                success: function (results, message) {
                    Paging(sender);
                }
            });
        }

    },
    BindStates: function (sender) {
        //debugger;
        if ($(sender).val() == "") {
            $.FillSelectList($("select#State"), null, true, "Select State");
        } else {
            $.ajaxExt({
                url: BaseUrl + '/ManageUsers/GetStateList',
                type: 'POST',
                validate: false,
                showErrorMessage: true,
                showPopupThrobber: true,
                button: $(sender),
                Async: false,
                messageControl: $('#status-division'),
                formToValidate: null,
                throbberPosition: { my: "left+100 center", at: "right center", of: $(sender) },
                data: { country: $(sender).val() },
                success: function (results, message) {
                    $.FillSelectList($("select#State"), results[0], true, "Select State");
                }
            });
        }
    },
    BindCitys: function (sender) {
        //debugger;
        if ($(sender).val() == "") {
            $.FillSelectList($("select#City"), null, true, "Select City");
        } else {
            $.ajaxExt({
                url: BaseUrl + '/ManageUsers/GetCitysList',
                type: 'POST',
                validate: false,
                showErrorMessage: true,
                showPopupThrobber: true,
                button: $(sender),
                Async: false,
                messageControl: $('#status-division'),
                formToValidate: null,
                throbberPosition: { my: "left+100 center", at: "right center", of: $(sender) },
                data: { country: $(sender).val() },
                success: function (results, message) {
                    $.FillSelectList($("select#City"), results[0], true, "Select State");

                }
            });
        }
    },
    AddUpdateUser: function (sender) {
        $.ajaxExt({
            url: BaseUrl + '/ManageUsers/AddUpdateUser',
            type: 'POST',
            validate: true,
            showErrorMessage: true,
            showthrobber: true,
            containFiles: true,
            button: $(sender),
            messageControl: $('#ErrorMessage'),
            formToValidate: $(sender).parents("form:first"),
            formToPost: $(".AddUpdateUserForm"),
            containFiles: true,
            throbberPosition: { my: "left+77 center", at: "right center", of: $(sender) },
            data: $(sender).parents("form:first").serializeArray(),
            success: function (results, message) {
                $.ShowMessage($('#ErrorMessage'), message, MessageType.Success);
                isSaved = true;
                setTimeout(function () {
                    location.href = results[0];
                }, 2000);
            }
        });

    },
    ManageUsers: function (totalCount) {
        var totalRecords = 0;
        totalRecords = parseInt(totalCount);
        PageNumbering(totalRecords);
    },
    SearchUser: function (sender) {
        paging.startIndex = 1;
        Paging(sender);
    },
    ShowRecords: function (sender) {
        paging.startIndex = 1;
        paging.pageSize = 10;
        Paging(sender);
    }
};
function Paging(sender) {
    var obj = new Object();
    obj.PageNo = paging.startIndex;
    obj.RecordsPerPage = paging.pageSize;
    obj.SortOrder = sortOrder;
    obj.SortColumn = sortColumn;
    $("body").removeClass("loaded");
    $.ajaxExt({
        type: "POST",
        validate: false,
        parentControl: $(sender).parents("form:first"),
        data: $.postifyData(obj),
        messageControl: null,
        showThrobber: false,
        throbberPosition: { my: "left center", at: "right center", of: sender, offset: "5 0" },
        url: BaseUrl + '/ManageUsers/GetManageUsersPagingList',
        success: function (results, message) {
            $('#Result').html(results[0]);
            PageNumbering(results[1]);

            $("table[name=Listing] thead tr th i.fa-sort").on("click", function () {
                return Home.Sort($(this));
            });

            $("table[name=Listing] tbody tr td a#DeleteUser").on("click", function () {
                return Home.DeleteUser($(this));
            });
            $("body").addClass("loaded");
        }
    });
}