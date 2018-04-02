
$(document).click(function () {


    var row = $("li.trips");
    var highlightedRow = $("li.trips.highlight");
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
            var locationSelection = $("h3.small_selection");
            var newSelection = $(this).data('name');
            locationSelection.html("Best option: " + newSelection);
        }
        else {
            var locationSelection = $("h3.small_selection");
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

    $("div.navbar.navbar-fixed.small_standards_and_map").hide();
    $(".breaks").hide();

    var fade_out = function () {
        $("#temp").fadeOut().empty();
    }

    setTimeout(fade_out, 7000);


    $(".dropdownTag").hide();
    $(".pullupMap").hide();


    $(".pullup").on("click", function () {
        if ($(window).scrollTop() >= 300) {

            $("div.navbar.navbar-fixed.small_standards_and_map").removeClass("navbar-fixed");
            $("div.navbar.small_standards_and_map").css("visibility", "hidden");
            $(".dropdownTag").css("margin-top", "-134px");
            $(".dropdownTag").css("position", "fixed");
            $(".pullup").hide();
            $(".dropdownTag").show();

        }

    });

    $(".dropdownTag").on("click", function () {


        $("div.navbar.small_standards_and_map").addClass("navbar-fixed");
        $("div.navbar.navbar-fixed.small_standards_and_map").css("visibility", "visible");
        $(".pullup").show();
        $(".dropdownTag").hide();




    });

    var map = $("div.form_map");
    $(".dropdownTagMap").on("click", function () {
        if ($("#standards").is(":visible")) {
            map.hide("slow");
            $("#standards").css("width", "100%");
            $(".pullupMap").show();
            $(".dropdownTagMap").hide();
        }
        else {
            map.hide("slow");
            $("#standards").css("width", "100%");
            $(".dropdownTag").css("margin-top", "20px");
            $(".pullupMap").show();
            $(".dropdownTagMap").hide();
        }

    });

    $(".pullupMap").on("click", function () {

        if ($(top).is(":visible")) {
            map.show("slow");
            $("#standards").css("width", "45vw");
            $(".pullupMap").hide();
            $(".dropdownTagMap").show();
        }
        else {
            map.show("slow");
            $("#standards").css("width", "45vw");
            $(".dropdownTag").css("margin-top", "0px");
            $(".pullupMap").hide();
            $(".dropdownTagMap").show();
        }


    });




    /* different standards bar based on where you are on the page*/




    var interval = setInterval(function () {
        if ($(window).width() >= 480) {
            if ($(window).scrollTop() >= 300 && !$("dropdownTag").is(":visible")) {

                $(".standards_and_map").css("visibility", "hidden");
                $(".standards_and_map").hide();
                $(".breaks").show();
                $("div.navbar.navbar-fixed.small_standards_and_map").css("visibility", "visible");
                $("div.navbar.navbar-fixed.small_standards_and_map").show();
                $(".small_standards_and_map").css("position", "fixed");
                $("div.navbar-fixed-top").css("margin-bottom", "100px");


            }
        }
    }, 250);

    var interval = setInterval(function () {
        if ($(window).scrollTop() <= 250) {

            $(".standards_and_map").css("visibility", "visible");
            $(".standards_and_map").show();
            $(".breaks").hide();
            $("div.navbar.navbar-fixed.small_standards_and_map").css("visibility", "hidden");
            $("div.navbar.navbar-fixed.small_standards_and_map").hide();
            $(".small_standards_and_map").css("position", "fixed");
            $("div.navbar-fixed-top").css("margin-bottom", "100px");


        }
    }, 250);


});






