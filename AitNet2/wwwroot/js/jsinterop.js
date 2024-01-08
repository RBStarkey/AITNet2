/// <reference path="../types/MicrosoftMaps/CustomMapStyles.d.ts" />
/// <reference path="../types/MicrosoftMaps/Microsoft.Maps.d.ts" />
/// <reference path="../types/Microsoft.JSInterop.d.ts" />
function SetCheckboxesToFalse() {
    try {
        var inputs = document.getElementsByTagName('input');
        for (var i in inputs) {
            if (inputs[i].type === 'checkbox') {
                inputs[i].checked = false;
            }
        }
    }
    catch (ex) {
        alert("SetCheckboxesToFalse - Exception: " + ex.message);
    }
}
//#region  Bing Map
var map;
var BingMapsKey = "Ak0EDik0dzq2cwCb7KXlI_QTFw6kQpOR-RZ-hd62l2cCf2g-H5gGqXrRv8FTdm0c";
function GetMap() {
    try {
        var bounds = Microsoft.Maps.LocationRect.fromLocations(new Microsoft.Maps.Location(51.50220108, -0.15202590), new Microsoft.Maps.Location(51.509965, -0.120092));
        // Initialize the map
        var options = {
            // MapOptions
            credentials: "",
            // ViewOptions
            zoom: 4,
            enableSearchLogo: false,
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            showBreadcrumb: false, // shows the whole state of CO
            showCopyright: true,
            tileBuffer: 4,
            inertiaIntensity: 1,
            showScalebar: true,
            width: 410,
            height: 410,
            maxBounds: bounds
        };
        // Check for null or undefined
        if (map == null) {
            map = new Microsoft.Maps.Map('#myMap', options);
            Microsoft.Maps.Events.addHandler(map, 'click', onMapClick);
        }
    }
    catch (err) {
        alert("GetMap - Error: " + err.message);
    }
}
var pinCount = 1;
function onMapClick(e) {
    try {
        var address = "", confidence = void 0;
        var latlng = e.location.latitude.toString() + ", " + e.location.longitude.toString();
        var target = "https://dev.virtualearth.net/REST/v1/Locations/" + latlng + "?o=json&key=" + BingMapsKey;
        var Http = new XMLHttpRequest();
        var url = target;
        Http.open("GET", url, false); // false for synchronous request
        Http.send(null);
        if (Http.responseText == null) {
            address = "No data";
            confidence = "-";
        }
        else {
            var json = JSON.parse(Http.responseText);
            address = json["resourceSets"][0]["resources"][0].name;
            if (address === null) {
                address = "Address not returned";
            }
            confidence = json["resourceSets"][0]["resources"][0].confidence;
            if (confidence === null) {
                confidence = "Confidence not returned";
            }
            address = address + ".\nLatitude: " + e.location.latitude.toFixed(3).toString()
                + ". Longitude: " + e.location.longitude.toFixed(3).toString();
            var locPin = new Microsoft.Maps.Pushpin(e.location, {
                color: 'blue',
                text: pinCount.toString(),
                title: address,
                subTitle: confidence
            });
            map.entities.push(locPin);
            pinCount += 1;
            DotNet.invokeMethodAsync('AitNet2', 'InvokeFromJS', address);
        }
    }
    catch (err) {
        alert("onMapClick - Error: " + err.message);
    }
}
function RemovePins() {
    try {
        var ol = document.getElementById('hitListOL');
        for (var i = map.entities.getLength() - 1; i >= 0; i--) {
            var pushpin = map.entities.get(i);
            if (pushpin instanceof Microsoft.Maps.Pushpin) {
                map.entities.removeAt(i);
            }
        }
        var listLength = ol.children.length;
        for (var i = 0; i < listLength; i++) {
            ol.removeChild(ol.children[0]);
        }
        pinCount = 1;
    }
    catch (err) {
        alert("RemovePins - Error: " + err.message);
    }
}
//#endregion
//# sourceMappingURL=jsinterop.js.map