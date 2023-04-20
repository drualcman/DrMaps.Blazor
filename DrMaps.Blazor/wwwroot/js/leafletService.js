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

const addMarkerWithOptions = (mapId, point, description, options) => {
    let map = maps.get(mapId);
    let marker = L.marker([point.latitude, point.longitude], options)
        .bindPopup(description)
        .addTo(map);
    let marketId = map.addedMarkers.push(marker) - 1;
    return marketId;       // Devuelve el indice del elemento insertado
}

const buildMarkerOptions = (title, iconUrl, draggable) => {
    let options = {
        title: title
    }
    if (iconUrl) {
        options.icon = L.icon({ iconUrl: iconUrl, iconSize: [32, 32], iconAnchor: [16, 16] });
    }
    if (draggable) {
        options.draggable = draggable
    }
    return options;
}

const addMarker = (mapId, point, title, description, iconUrl) =>
    addMarkerWithOptions(mapId, point, description, buildMarkerOptions(title, iconUrl));

const addDraggableMarker = (mapId, point, title, description, iconUrl) => {
    let options = buildMarkerOptions(title, iconUrl, true);
    let markerId = addMarkerWithOptions(mapId, point, description, options);
    let map = maps.get(mapId);
    let marker = GetMarker(mapId, markerId);
    marker.on('dragend', function (event) {
        let marker = event.target;
        let position = marker.getLatLng();
        let point = {
            MarkerId: markerId,
            Point: {
                Latitude: position.lat,
                Longitude: position.lng
            }
        };
        let dotNet = map.markerHelper.dotNetObjectReference;
        let dragendHandler = map.markerHelper.dragendHandler;
        if (dotNet !== null && dotNet !== undefined)
            dotNet.invokeMethodAsync(dragendHandler, point);
        else
            console.warn("Can't connect with the app.");
    });    
    return markerId;       // Devuelve el indice del elemento insertado
}

const setMarkerHelper = (mapId, dotNet, dragendHandler) => {
    let map = maps.get(mapId);
    map.markerHelper = {
        dotNetObjectReference: dotNet,
        dragendHandler: dragendHandler
    };
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
    GetMarker(mapId, markerId)
        .setLatLng([newPoint.latitude, newPoint.longitude]);
}

const setPopupMarkerContent = (mapId, markerId, content) => {
    GetMarker(mapId, markerId)
        .setPopupContent(content);
}

const GetMarker = (mapId, markerId) =>
    maps.get(mapId).addedMarkers[markerId];

export {
    createMap, deleteMap, setView, addMarker, addMarkerWithOptions, addDraggableMarker,
    removeMarkers, drawCircle, moveMarker, buildMarkerOptions, setMarkerHelper,
    setPopupMarkerContent
}
