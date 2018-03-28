
$(document).click(function () {


    var row = $("tr");
    var highlightedRow = $(".highlight");
    var lat = "";
    var long = "";


    row.click(function (e) {
        highlightedRow.removeClass("highlight");
        $(this).addClass("highlight");
        lat = $(this).data("lat");
        long = $(this).data("long");

        $(document).ready(function () {

            var center = new google.maps.LatLng(lat, long);
            map = new google.maps.Map(document.getElementById("map"), {
                zoom: 5,
                center: center,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });

            var flightPlanCoordinates = [

                { lat: 34.0522, lng: -118.243 },
                { lat: lat, lng: long }



            ];
            var flightPath = new google.maps.Polyline({
                path: flightPlanCoordinates,
                geodesic: true,
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 1
            });

            flightPath.setMap(map);
        });

        if ($(this).attr('id') == "0") {
            var locationSelection = $("h3.selection");
            var newSelection = $(this).data('name');
            locationSelection.html("Best option: " + newSelection);
        }
        else {
            var locationSelection = $("h3.selection");
            var newSelection = $(this).data('name');
            locationSelection.html("Current selection: " + newSelection);
        }

    });




});


$(document).keydown(function (e) {

    var direction = "";
    var firstRow = $("tbody tr:nth-child(2)");
    var lastRow = $("tbody tr:last-child");
    var highlightedRow = $("tr.highlight");
    var nextRowDown = highlightedRow.next();
    var nextRowUp = $("tr").filter('#' + rowUp(highlightedRow));
    var lat = "";
    var long = "";

    function rowUp(row) {
        return parseInt(row.attr('id')) - 1;
    }

    if (e.which === 40) {
        direction = "down";
    }
    else if (e.which === 38) {
        direction = "up";
    }
    console.log(direction);

    if (direction == "down") {
        if (highlightedRow.attr('id') != lastRow.attr('id')) {
            highlightedRow.removeClass("highlight");
            nextRowDown.addClass("highlight");
            lat = nextRowDown.data("lat");
            long = nextRowDown.data("long");


            var locationSelection = $("h3.selection");
            var newSelection = nextRowDown.data('name');
            locationSelection.html("Current selection: " + newSelection);


            $(document).ready(function () {


                var center = new google.maps.LatLng(lat, long);
                map = new google.maps.Map(document.getElementById("map"), {
                    zoom: 5,
                    center: center,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                });

                var flightPlanCoordinates = [

                    { lat: 34.0522, lng: -118.243 },
                    { lat: lat, lng: long }



                ];
                var flightPath = new google.maps.Polyline({
                    path: flightPlanCoordinates,
                    geodesic: true,
                    strokeColor: '#FF0000',
                    strokeOpacity: 1.0,
                    strokeWeight: 1
                });

                flightPath.setMap(map);
            });
        }

    }
    else if (direction == "up") {
        if (highlightedRow.attr('id') != firstRow.attr('id')) {
            highlightedRow.removeClass("highlight");
            nextRowUp.addClass("highlight");
            lat = nextRowUp.data("lat");
            long = nextRowUp.data("long");

            if (nextRowUp.attr('id') == "0") {
                var locationSelection = $("h3.selection");
                var newSelection = nextRowUp.data('name');
                locationSelection.html("Best option: " + newSelection);
            }
            else {
                var locationSelection = $("h3.selection");
                var newSelection = nextRowUp.data('name');
                locationSelection.html("Current selection: " + newSelection);
            }

            $(document).ready(function () {

                var center = new google.maps.LatLng(lat, long);
                map = new google.maps.Map(document.getElementById("map"), {
                    zoom: 5,
                    center: center,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                });

                var flightPlanCoordinates = [

                    { lat: 34.0522, lng: -118.243 },
                    { lat: lat, lng: long }



                ];
                var flightPath = new google.maps.Polyline({
                    path: flightPlanCoordinates,
                    geodesic: true,
                    strokeColor: '#FF0000',
                    strokeOpacity: 1.0,
                    strokeWeight: 1
                });

                flightPath.setMap(map);
            });
        }

    }




});

$(document).ready(function () {
    

    var fade_out = function () {
        $("#temp").fadeOut().empty();
    }

    setTimeout(fade_out, 7000);


});



