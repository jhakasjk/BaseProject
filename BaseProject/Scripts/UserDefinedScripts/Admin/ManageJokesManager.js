var isSaved = false;
var sortOrder = 'ASC', sortColumn = 'DatePosted';
window.onbeforeunload = function () {
    if (!isSaved) {
        return "You have not saved your data yet.  If you continue, your work will not be saved.";
    }
}

$(document).ready(function () {

    $("table[name=Listing] thead tr th i.fa-sort").on("click", function () {
        return Joke.Sort($(this));
    });
    $("table[name=Listing] tbody tr td a#DeleteJoke").on("click", function () {
        return Joke.DeleteJoke($(this));
    });

    $("#AddEditJoke").on("click", function () {
        debugger;
        return Joke.AddUpdateJoke($(this));
    });

   
});
var Joke = {
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
    DeleteJoke: function (sender) {
        if (confirm("Are you sure do you delete this Joke. After deleting Joke you have no any option to receive this Joke.")) {
            var JokeId = $(sender).parents('tr:first').attr('rowid');
            $("body").removeClass("loaded");
            $.ajaxExt({
                type: "POST",
                validate: false,
                parentControl: $(sender).parents("form:first"),
                data: { JokeId: JokeId },
                messageControl: null,
                showThrobber: false,
                throbberPosition: { my: "left center", at: "right center", of: sender, offset: "5 0" },
                url: BaseUrl + '/ManageJokes/DeleteJoke',
                success: function (results, message) {
                    Paging(sender);
                }
            });
        }

    },   
  
    AddUpdateJoke: function (sender) {
        $.ajaxExt({
            url: BaseUrl + '/ManageJokes/AddUpdateJoke',
            type: 'POST',
            validate: true,
            showErrorMessage: true,
            showthrobber: true,
            containFiles: true,
            button: $(sender),
            messageControl: $('#ErrorMessage'),
            formToValidate: $(sender).parents("form:first"),
            formToPost: $(".AddUpdateJokeForm"),
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
    ManageJoke: function (totalCount) {
        var totalRecords = 0;
        totalRecords = parseInt(totalCount);
        PageNumbering(totalRecords);
    },
    SearchJoke: function (sender) {
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
        url: BaseUrl + '/ManageJokes/GetManageJokePagingList',
        success: function (results, message) {
            $('#Result').html(results[0]);
            PageNumbering(results[1]);

            $("table[name=Listing] thead tr th i.fa-sort").on("click", function () {
                return Joke.Sort($(this));
            });

            $("table[name=Listing] tbody tr td a#DeleteJoke").on("click", function () {
                return Joke.DeleteJoke($(this));
            });
            $("body").addClass("loaded");
        }
    });
}