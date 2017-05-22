var tickets;
var sortParametr = "";
var descending = false;

$(function () {
    $("#getTicketsBtn").on("click", function () {
        var departurePlace = $("#DepPlace").val();
        var arrivalPlace = $("#ArrPlace").val();
        var date = $("#TickDate").val();
        if (validateRoute(departurePlace, arrivalPlace) == false)
            return false;
       
        $.ajax({
            type: "GET",
            url: "../api/tickets/?DeparturePlace=" + departurePlace + "&ArrivalPlace=" + arrivalPlace + "&Date=" + date,
            datatype: "JSON",
            cache: false,
        }).success(function (data) { generateTicketsScheduleHtml(data); })
    })
});

$(document).ajaxStart(function () {
    $(".pageFade").show();
    $(".loadingProgress").show();
});

$(document).ajaxComplete(function () {
    $(".pageFade").hide();
    $(".loadingProgress").hide();
});

function generateTicketsScheduleHtml(data) {
        $('#tickets').empty();
        var generatedHtml = "";
        if (data.length != 0) {
            tickets = data;
            generatedHtml = '<div class="text-center"><h4 class="text-center headerText marginSub">Доступные билеты</h4><br /><div id="ticketsFilter"></div><br/></div><div class="schedule col-sm-offset-1 col-sm-10 col-md-offset-1 col-md-10 col-lg-offset-2 col-lg-8" id="ticketsSchedule">';
            for (var i = 0; i < data.length; i++) {
                generatedHtml += '<div class="ticketCard-sm ticketCard"><div class="text-center routeLabel"><strong>' +
                                        data[i].Name + '    <span>Маршрут: </span>' + data[i].Route + '</strong></div>' +
                                        '<div><span>Тип:</span>' + data[i].Type + '</div><div><span>Отправление: </span>' +
                                        data[i].DepartureTime + '<span>Прибытие: </span>' + data[i].ArrivalTime +
                                        '</div><div><span>Свободные места: </span>';
                for (var j = 0; j < data[i].Places.length; j++) {
                    generatedHtml += '<p> <span class="placeType">' + data[i].Places[j].Type +
                    ' / </span><span class="placeAmount">' + data[i].Places[j].Amount + ' / </span><span class="placeCost">';
                    if (data[i].Places[j].isCostLessThanAverage) {
                        generatedHtml += '<span class="label-success"><strong>' + data[i].Places[j].Cost + '</strong><span>руб.</span></span>';
                    }
                    else {
                        generatedHtml += '<span class="label-danger"><strong>' + data[i].Places[j].Cost + '</strong><span>руб.</span></span>';
                    }
                    generatedHtml += '</span></p>';
                }
                generatedHtml += '</div></div>';
            }
            generatedHtml += '</div>';
            $("#tickets").html(generatedHtml);
            addTicketsFilterPanel()
        }
        else {
            $('#tickets').html('<p><h4 class="text-center ticketNotFoundColor">Билетов не найдено</h4></p>');
        }
}


var routesUri = '../api/routes';

$(document).ready(function () {
    $.getJSON(routesUri)
        .done(function (data) {
            $.each(data, function (key, item) {
                var spanElement = document.createElement("span");
                spanElement.className = "mostFrequentRoute";
                var routeLink = document.createElement("button");
                routeLink.addEventListener("click", function () { fillRouteFields(item.DeparturePlace, item.ArrivalPlace) });
                routeLink.innerText = item.DeparturePlace + " - " + item.ArrivalPlace;
                spanElement.appendChild(routeLink);
                $(spanElement).appendTo("#topRoutes");
            });
        });
});

function validateRoute(depPlace, arrPlace) {
    var validationErrors = "";
    $("#validationErrors").empty();
    if (depPlace === "" || arrPlace === "") {
        if (depPlace === "")
            validationErrors += "<p class='text-danger text-center'>Требуется место отправления<p>";
        if (arrPlace == "")
            validationErrors += "<p class='text-danger text-center'>Требуется место прибытия<p>";
        $("#validationErrors").html(validationErrors);
        return false;
    }
}

function fillRouteFields(depPlaceValue, arrPlaceValue) {
    var departurePlaceField = document.querySelectorAll(".departurePlace");
    var arrivalPlaceField = document.querySelectorAll(".arrivalPlace");
    for (i = 0; i < departurePlaceField.length; i++) {
        departurePlaceField[i].value = depPlaceValue;
        arrivalPlaceField[i].value = arrPlaceValue;
    }
}

function createElement(name, text) {
    var elem = document.createElement(name);
    elem.innerText = text;
    return elem;
}

function descendingSwitcher() {
    if (descending)
        descending = false;
    else
        descending = true;
}


function addTicketsFilterPanel() {
    var ticketsOrdering = $("#ticketsFilter");
    ticketsOrdering.append(createSortByDepartureTimeButton());
    ticketsOrdering.append(createSortByArrivalTimeButton());
    ticketsOrdering.append(createSortByPriceButton());
    ticketsOrdering.append(createVehicleKindSelectList());
}

function createSortByDepartureTimeButton() {
    var sortTicketsByDepartureTimeButton = createElement("button", "По времени отправления");
    sortTicketsByDepartureTimeButton.addEventListener("click", function () { sortTicketsByDepartureTime(descending); generateTicketsScheduleHtml(tickets); });
    sortTicketsByDepartureTimeButton.className = "btn btn-default sortButton";
    return sortTicketsByDepartureTimeButton;
}

function createSortByArrivalTimeButton() {
    var sortTicketsByArrivalTimeButton = createElement("button", "По времени прибытия");
    sortTicketsByArrivalTimeButton.addEventListener("click", function () { sortTicketsByArrivalTime(descending); generateTicketsScheduleHtml(tickets); });
    sortTicketsByArrivalTimeButton.className = "btn btn-default sortButton";
    return sortTicketsByArrivalTimeButton;
}

function createSortByPriceButton() {
    var sortByPriceButton = createElement("button", "По цене");
    sortByPriceButton.addEventListener("click", function () { sortTicketsByPrice(descending); generateTicketsScheduleHtml(tickets); });
    sortByPriceButton.className = "btn btn-default sortButton";
    return sortByPriceButton;
}

function createVehicleKindSelectList() {
    var vehicleKindSelectList = document.createElement("select");
    vehicleKindSelectList.className = "form-control";
    vehicleKindSelectList.setAttribute("id", "vehicleTypeSelectList");
    vehicleKindSelectList.value = "";
    vehicleKindSelectList.addEventListener("onchange", function () {
        var selectedKindIndex = vehicleKindSelectList.selectedIndex;
        var filteredTickets = filterTicketsByVehicleKind(vehicleKindSelectList.options(selectedKindIndex).innerHtml);
        generateTicketsScheduleHtml(filteredTickets);
    })
    $(vehicleKindSelectList).html("<option value='Маршрутка/Автобус'>Маршрутка/Автобус</option><option value='Поезд/Электричка'>Поезд/Электричка</option><option value=''>Не фильтровать</option>");
    return vehicleKindSelectList;
}

function sortTicketsByDepartureTime(isDescending){
    tickets.sort(function (a, b) {
        var firstTicketDate = new Date();
        var firstTicketTime = a.DepartureTime.split(":");
        firstTicketDate.setFullYear(2017, 02, 01);
        firstTicketDate.setHours(firstTicketTime[0], firstTicketTime[1]);
        var secondTicketDate = new Date();
        var secondTicketTime = b.DepartureTime.split(":");
        secondTicketDate.setFullYear(2017, 02, 01);
        secondTicketDate.setHours(secondTicketTime[0], secondTicketTime[1]);
        return firstTicketDate - secondTicketDate;
    });
    if (isDescending)
        tickets.reverse();
    descendingSwitcher();

}

function sortTicketsByArrivalTime(isDescending) {
    tickets.sort(function (a, b) {
        var firstTicketDate = new Date();
        var firstTicketTime = a.ArrivalTime.split(":");
        firstTicketDate.setFullYear(2017, 02, 01);
        firstTicketDate.setHours(firstTicketTime[0], firstTicketTime[1]);
        var secondTicketDate = new Date();
        var secondTicketTime = b.ArrivalTime.split(":");
        secondTicketDate.setFullYear(2017, 02, 01);
        secondTicketDate.setHours(secondTicketTime[0], secondTicketTime[1]);
        return firstTicketDate - secondTicketDate;
    });
    if (isDescending)
        tickets.reverse();
    descendingSwitcher();

}

function sortTicketsByPrice(isDescending) {
    tickets.sort(function (a, b) {
        var firstTicketPrice = a.Places[0];
        var secondTicketPrice = b.Places[0];
        return firstTicketPrice - secondTicketPrice;
    });
    if (isDescending)
        tickets.reverse();
    descendingSwitcher();
}

function filterTicketsByVehicleKind(kind) {
    if (kind == "") return tickets;
    var filteredTickets;
    for (i = 0; i < tickets.length; i++) {
        if (tickets[i].Kind == kind)
            filteredTickets.push(tickets[i]);
    }
    return filteredTickets;
}


