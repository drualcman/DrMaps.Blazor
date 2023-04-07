const getFromAddress = (street, city, state, postalcode, country) => {
    //https://geocode.maps.co/
    let result = [];
    if (street !== null && street !== undefined) street = street.replace(' ', '+');
    if (city !== null && city !== undefined) city = city.replace(' ', '+');
    if (state !== null && state !== undefined) state = state.replace(' ', '+');
    if (postalcode !== null && postalcode !== undefined) postalcode = postalcode.replace(' ', '+');
    if (country !== null && country !== undefined) country = country.replace(' ', '+');
    let url = `https://geocode.maps.co/search?street=${street}&city=${city}&state=${state}&postalcode=${postalcode}&country=${country}`;
    return fetch(url, {
        method: 'get',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then((response) => {
        return response.json().then(data =>
        {

            data.forEach(place => result.push({
                Point: {
                    latitude: parseFloat(place.lat),
                    longitude: parseFloat(place.lon)
                },
                DisplayName: place.display_name,
                Id: place.place_id
            }));
            //let count = data.length;
            //if (count > 0) {
            //    for (var i = 0; i < count; i++) {
            //        let place = data[i];
            //        result.push({
            //                Point: {
            //                latitude: parseFloat(place.lat),
            //                longitude: parseFloat(place.lon)
            //                },
            //                DisplayName: place.display_name,
            //                Id: place.place_id
            //            });
            //    }
            //}
            console.info('getLatLongFromAddress result:', result);
            return result;
        });
    }).catch((error) => {
        console.info(`getLatLongFromAddress ${error}.`);
        console.info(error);
        return result;
    })
}


export { getFromAddress }
