$(document).ready(function() {

    /* First, Display a map on the canvas */

    var map_options =
        {
            zoom: 2,
            center: new google.maps.LatLng(0,0),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

    map = new google.maps.Map(document.getElementById('map_canvas'), map_options);

    var marker = new google.maps.Marker({map:map,
                    animation: google.maps.Animation.DROP});
    var infoWindow = new google.maps.InfoWindow();
    var info;

    /* Second, mark the location */

    getNextEvent();

    /* Third, On next event button click, display next or first event */

    $("#nextEvent").on("click", getNextEvent);


    /* Functions */

    function getNextEvent(){

        var eventId=$("#eventId").val();

        $.ajax({
            url: "./home/getnextevent",
            data: {currentEventId:eventId},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (event) {
                $("#eventId").val(event.Id);


                var address = event.Address1 + " " +
                              event.Address2 + ", " +
                              event.Suburb + ", " +
                              event.State + ", " +
                              event.Country;

                getGeolocation(event,address);
            }
        });
    }

    /* Get coordinates of address*/

    function getGeolocation(event,address){

        var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address;
        var coordinates;

        $.getJSON( url, function( data ) {

            coordinates = data.results[0].geometry.location;

            plotEvent(event,address,coordinates);
        });

    }

    /* Place marker in map */

    function plotEvent(event,address,coordinates){

            info = "<h3>" + event.Name + "</h3>" + 
                        event.Date + 
                        "<br><br>Address:<br>" + address;

            marker.setPosition(coordinates);

            infoWindow.setContent(info);
            infoWindow.open(map, marker);

            /* Zoom in */

            map.setZoom(15);
            map.setCenter(marker.getPosition());

    }

});
