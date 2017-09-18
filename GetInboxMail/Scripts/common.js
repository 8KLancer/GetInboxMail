(function () {
    onLiPaginationClick = function () {
        var pageNumberTO = this.children[0].innerText;
        var activePage = $("#paging>li.active");
        var preloader = $("#preloader");

        activePage.removeClass("active");
        $(this).addClass("active");

        $.ajax({
            url: "/api/GmailAPI",
            type: "POST",
            data: JSON.stringify(pageNumberTO),
            contentType: "application/json",
            beforeSend: function() {
                preloader.removeAttr("style").css("display", "block");
            },
            success: function (data) {
                preloader.removeAttr("style").css("display", "none");
                var tbody = $("#tbodycontent").empty();
                for (var i = 0; i < data.length; i++) {
                    tbody.append("<tr><td>" + data[i].Id + "</td><td>" + data[i].From + "</td><td>" + data[i].Date + "</td><td>" + data[i].Subject + "</td></tr>");
                }
            }
        });
    },

    onSuccess = function (data, preloader) {
        preloader.removeAttr("style").css("display", "none");
        var tbody = $("#tbodycontent").empty();
        for (var i = 0; i < data.Messages.length; i++) {
            tbody.append("<tr><td>" + data.Messages[i].Id + "</td><td>" + data.Messages[i].From + "</td><td>" + data.Messages[i].Date + "</td><td>" + data.Messages[i].Subject + "</td></tr>");
        }

        var paging = $("#paging");
        i = 1;
        var newLi = $('<li class="li_page active"><a href="#">' + i + "</a></li>");
        newLi.bind("click", onLiPaginationClick);
        paging.append(newLi);

        for (i = 1; i < data.PageInfo.TotalPages; i++) {
            var newLi = $('<li class="li_page"><a href="#">' + (i + 1) + "</a></li>");
            newLi.bind("click", onLiPaginationClick);
            paging.append(newLi);
        }
    }
})();