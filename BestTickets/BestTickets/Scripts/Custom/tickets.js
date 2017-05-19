

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
        }).success(function (data) {
            $('#tickets').empty();
            var generatedHtml = "";
            if (data.length != 0) {
                generatedHtml = '<div class="text-center"><h4 class="text-center headerText marginSub">Доступные билеты</h4><br /></div><div class="schedule col-sm-offset-1 col-sm-10 col-md-offset-1 col-md-10 col-lg-offset-2 col-lg-8" id="ticketsSchedule">';
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
            }
            else {
                $('#tickets').html('<p><h4 class="text-center ticketNotFoundColor">Билетов не найдено</h4></p>');
            }

        })
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

var routesUri = '../api/routes';

$(document).ready(function () {
    $.getJSON(routesUri)
        .done(function (data) {
            $.each(data, function (key, item) {
                var spanElement = document.createElement("span");
                spanElement.className = "mostFrequentRoute";
                var routeLink = document.createElement("button");
                routeLink.addEventListener("click", function () { fillRouteFields(item.DeparturePlace, item.ArrivalPlace) });
                routeLink.innerText = showRoute(item);
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

function showRoute(item) {
    return item.DeparturePlace + " - " + item.ArrivalPlace;
}
