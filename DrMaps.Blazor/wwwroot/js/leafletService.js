import * as L from "./leaflet.esm.js"

const maps = new Map();     // new Dictionary<string, object> in c#

const createMap = (mapId, point, zoomLevel) => {
    let map = L.map(mapId)
        .setView([point.latitude, point.longitude], zoomLevel);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: zoomLevel,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright" target="_blank">OpenStreetMap</a>'
    }).addTo(map);
    map.addedMarkers = [];      // Attibuto nuevo personalizado
    maps.set(mapId, map);

    /*
    Otra forma de guardar el objecto map:
    var element = document.getElementById(mapId);
    element.Map = map;
    */

    console.info(`map ${mapId} created.`);
}

const deleteMap = (mapId) => {
    let map = maps.get(mapId);
    maps.delete(mapId);
    map.remove();
    console.info(`map ${mapId} removed.`);
}

const setView = (mapId, point, zoomLevel) => {
    let map = maps.get(mapId);
    map.setView([point.latitude, point.longitude], zoomLevel);
}

const addMarker = (mapId, point, title, description, iconUrl, dragable, dotNet) => {
    let map = maps.get(mapId);
    let options = {
        title: title,
        draggable: 'true' 
    }
    if (iconUrl) {
        options.icon = L.icon({ iconUrl: iconUrl, iconSize: [32, 32], iconAnchor: [16, 16] });
    }
    let marker = L.marker([point.latitude, point.longitude], options)
        .bindPopup(description)
        .addTo(map);
    if (dragable) {
        marker.on('dragend', function (event) {
            let marker = event.target;
            let position = marker.getLatLng();
            let point = {
                Latitude: position.lat,
                Longitude: position.lng
            }
            dotNet.invokeMethodAsync("OnDragend", point);
        });
    }
    return map.addedMarkers.push(marker) - 1;       // Devuelve el indice del elemento insertado
}

const removeMarkers = (mapId) => {
    let map = maps.get(mapId);
    map.addedMarkers.forEach(marker => marker.removeFrom(map));
    map.addedMarkers = [];
}

const drawCircle = (mapId, center, color, fillColor, fillOpacity, radius) => {
    let map = maps.get(mapId);
    L.circle([center.latitude, center.longitude], {
        color: color,
        fillColor: fillColor,
        fillOpacity: fillOpacity,
        radius: radius
    }).addTo(map); 
    //optionalmente, guardar el circulo
}

const moveMarker = (mapId, markerId, newPoint) => {
    let map = maps.get(mapId);
    let marker = map.addedMarkers[markerId];
    marker.setLatLng([newPoint.latitude, newPoint.longitude]);
}

export { createMap, deleteMap, setView, addMarker, removeMarkers, drawCircle, moveMarker }
