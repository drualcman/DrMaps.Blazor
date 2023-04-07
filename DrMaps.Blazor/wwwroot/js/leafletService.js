import * as L from "./leaflet.esm.js"
import * as geoCoder from "./geoCoder.js"

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

const addMarker = (mapId, point, title, description) => {
    let map = maps.get(mapId);
    let marker = L.marker([point.latitude, point.longitude], {
        title: title
    })
        .bindPopup(description)
        .addTo(map);
    return map.addedMarkers.push(marker) - 1;       // Devuelve el indice del elemento insertado
}

const removeMarkers = (mapId) => {
    let map = maps.get(mapId);
    maps.addedMarkers.forEch(marker => marker.removeFrom(map));
    maps.addedMarkers = [];
}

const drawCircle = (mapId, center, color, fillColor, fillOpacity, radious) => {
    let map = maps.get(mapId);
    L.circle([center.latitude, center.longitude], {
        color: color,
        fillColor: fillColor,
        fillOpacity: fillOpacity,
        radious: radious
    }).addTo(map); 
    //optionalmente, guardar el circulo
}

export { createMap, deleteMap, setView, addMarker, removeMarkers, drawCircle, geoCoder }
