var portfolioManager = function () {
    Date.prototype.yyyymmdd = function () {
        var mm = this.getMonth() + 1; // getMonth() is zero-based
        var dd = this.getDate();

        return [this.getFullYear(),
        (mm > 9 ? '' : '0') + mm,
        (dd > 9 ? '' : '0') + dd
        ].join('-');
    };
    // appends a row to the portfolio items table.
    // @parentSelector: selector to append a row to.
    // @obj: portfolio item object to append.
    var appendRow = function (parentSelector, obj) {
        var tr = $("<tr data-id='" + obj.ItemId + "'></tr>");
        tr.append("<td class='name' >" + obj.Symbol + "</td>");
        tr.append("<td class='number' >" + obj.SharesNumber + "</td>");
        tr.append("<td><button class='update-button btn btn-warning btn-sm'>Update</button><td><span class='delete-button glyphicon glyphicon-trash btn'></button>");
        tr.append("<td>" + Math.round10(obj.TodayPrice, -2) + "$</td>");
        tr.append("<td>" + Math.round10(obj.TodayPrice*obj.SharesNumber, -2) + "$</td>");

        $(parentSelector).append(tr);
    }

    // adds all portfolio items as rows (deletes all rows before).
    // @parentSelector: selector to append a row to.
    // @tasks: array of portfolio items to append.
    var displayPortfolioItems = function (parentSelector, portfolioItems) {
        $(parentSelector).empty();
        $.each(portfolioItems, function (i, item) {
            var date = new Date();
            date.setDate(date.getDate() - 1);
            $.getJSON('https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=' + item.Symbol + '&apikey=Q0KDAMVZNFQJGNFY', function (data) {
                try {
                    item.TodayPrice = data['Time Series (Daily)'][date.yyyymmdd()]['1. open'];
                }
                catch (e) {
                    console.log("error");
                    console.log(data);
                    item.TodayPrice = NaN;
                }
                appendRow(parentSelector, item);
            });
        });
    };

    // starts loading portfolio items from server.
    // @returns a promise.
    var loadPortfolioItems = function () {
        return $.getJSON("/api/portfolioitems");
    };

    // starts loading actual portfolio items from server.
    // @returns a promise.
    var loadSynchronizedPortfolioItems = function () {
        return $.getJSON("/api/portfolioitems/GetSynchronizedData");
    };

    // starts creating a portfolio item on the server.
    // @symbol: symbol name.
    // @sharesNumber: number of shares.
    // @return a promise.
    var createPortfolio = function (symbol, sharesNumber) {
        return $.post("/api/portfolioitems",
            {
                Symbol: symbol,
                SharesNumber: sharesNumber
            });
    };

    // starts updating a portfolio item on the server.
    // @id: id of the portfolio item to update.
    // @symbol: symbol name.
    // @sharesNumber: number of shares.
    // @return a promise.
    var updatePortfolioItem = function (id, symbol, sharesNumber) {
        return $.ajax(
            {
                url: "/api/portfolioitems",
                type: "PUT",
                contentType: 'application/json',
                data: JSON.stringify({
                    ItemId: id,
                    Symbol: symbol,
                    SharesNumber: sharesNumber
                })
            });
    };

    // starts deleting a portfolio item on the server.
    // @itemId: id of the item to delete.
    // @return a promise.
    var deletePortfolioItem = function (itemId) {
        return $.ajax({
            url: "/api/portfolioitems/" + itemId,
            type: 'DELETE'
        });
    };

    // returns public interface of portfolio manager.
    return {
        loadItems: loadPortfolioItems,
        loadSyncItems: loadSynchronizedPortfolioItems,
        displayItems: displayPortfolioItems,
        createItem: createPortfolio,
        deleteItem: deletePortfolioItem,
        updateItem: updatePortfolioItem
    };
}();


$(function () {
    // add new portfolio item button click handler
    $("#newCreate").click(function () {
        var symbol = $('#symbol')[0].value;
        var sharesNumber = $('#sharesNumber')[0].value;

        portfolioManager.createItem(symbol, sharesNumber)
            .then(portfolioManager.loadItems)
            .done(function (items) {
                console.log(items);
                portfolioManager.displayItems("#items > tbody", items);
                renewUpdateTimer();
            });
    });

    // bind update portfolio item checkbox click handler
    $("#items > tbody").on('click', '.update-button', function () {
        var tr = $(this).parent().parent();
        var itemId = tr.attr("data-id");
        var symbol = $('#symbol')[0].value;
        var sharesNumber = $('#sharesNumber')[0].value;
        //var symbol = tr.find('.symbol').text();
        //var sharesNumber = tr.find('.sharesNumber').text();

        portfolioManager.updateItem(itemId, symbol, sharesNumber)
            .then(portfolioManager.loadItems)
            .done(function (items) {
                portfolioManager.displayItems("#items > tbody", items);
                renewUpdateTimer();
            });
    });

    // bind delete button click for future rows
    $('#items > tbody').on('click', '.delete-button', function () {
        var itemId = $(this).parent().parent().attr("data-id");
        portfolioManager.deleteItem(itemId)
            .then(portfolioManager.loadItems)
            .done(function (items) {
                portfolioManager.displayItems("#items > tbody", items);
                renewUpdateTimer();
            });
    });

    // load all items on startup
    portfolioManager.loadItems()
        .done(function (items) {
            portfolioManager.displayItems("#items > tbody", items);
            refreshData();
        });

    var timer;
    var userMakeChanges = false;

    function renewUpdateTimer() {
        userMakeChanges = true;
        clearTimeout(timer);
        timer = setTimeout(refreshData, 30000);
    }

    function refreshData() {
        clearInterval(timer);
        userMakeChanges = false;
        console.log('data start refreshed');
        portfolioManager.loadSyncItems().done(function (items) {
            if (!userMakeChanges) {
                portfolioManager.displayItems("#items > tbody", items);
                console.log('done');
            }
            renewUpdateTimer();

        });
    }

    //TODO: finish and refactor
    $("#tableBody").click(function (event) {
        if (event.target.className == 'name') {
            showChart(event.target.innerHTML);
        }
        else { hideChart(); }
    });

    function showChart(symbol) {
        $("#chart").show();
        $.plot($("#chart"), [[[0, 0], [1, 1]]], { yaxis: { max: 1 } });
    }

    function hideChart() {
        $("#chart").hide();
    }
});


(function () {
    function decimalAdjust(type, value, exp) {
        if (typeof exp === 'undefined' || +exp === 0) {
            return Math[type](value);
        }
        value = +value;
        exp = +exp;
        // Если значение не является числом, либо степень не является целым числом...
        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
            return "--";
        }
        // Сдвиг разрядов
        value = value.toString().split('e');
        value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
        // Обратный сдвиг
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
    }

    // Десятичное округление к ближайшему
    if (!Math.round10) {
        Math.round10 = function (value, exp) {
            return decimalAdjust('round', value, exp);
        };
    }
})();

