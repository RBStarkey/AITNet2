/// <reference path="../types/MicrosoftMaps/CustomMapStyles.d.ts" />
/// <reference path="../types/MicrosoftMaps/Microsoft.Maps.d.ts" />
/// <reference path="../types/Microsoft.JSInterop.d.ts" />

function SetCheckboxesToFalse(): void {
	try {
		let inputs = document.getElementsByTagName('input');
		for (let i in inputs) {
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
let map: Microsoft.Maps.Map;
const BingMapsKey: string = "Ak0EDik0dzq2cwCb7KXlI_QTFw6kQpOR-RZ-hd62l2cCf2g-H5gGqXrRv8FTdm0c";

function GetMap(): void {
	try {

		let bounds = Microsoft.Maps.LocationRect.fromLocations(new Microsoft.Maps.Location(51.50220108, -0.15202590), new Microsoft.Maps.Location(51.509965, -0.120092));
		// Initialize the map
		let options = {
			// MapOptions
			credentials: "",
			// ViewOptions
			zoom: 4,
			enableSearchLogo: false,
			mapTypeId: Microsoft.Maps.MapTypeId.road,
			showBreadcrumb: false,// shows the whole state of CO
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
let pinCount: number = 1;
function onMapClick(e) {
	try {
		let address: string = "",
			confidence: string;

		const latlng = e.location.latitude.toString() + ", " + e.location.longitude.toString()
		const target: string = "https://dev.virtualearth.net/REST/v1/Locations/" + latlng + "?o=json&key=" + BingMapsKey;

		const Http = new XMLHttpRequest();
		const url = target;
		Http.open("GET", url, false); // false for synchronous request
		Http.send(null);

		if (Http.responseText == null) {
			address = "No data";
			confidence = "-"
		}
		else {
			const json: Object = JSON.parse(Http.responseText);
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

			let locPin: Microsoft.Maps.Pushpin = new Microsoft.Maps.Pushpin(e.location,
				{
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
function RemovePins(): void {
	try {
		const ol: HTMLOListElement = document.getElementById('hitListOL') as HTMLOListElement;
		for (let i = map.entities.getLength() - 1; i >= 0; i--) {
			const pushpin = map.entities.get(i);
			if (pushpin instanceof Microsoft.Maps.Pushpin) {
				map.entities.removeAt(i);
			}
		}
		const listLength: number = ol.children.length as number;
		for (let i = 0; i < listLength; i++) {
			ol.removeChild(ol.children[0]
			);
		}
		pinCount = 1;
	}
	catch (err) {
		alert("RemovePins - Error: " + err.message);
	}
}
//#endregion




