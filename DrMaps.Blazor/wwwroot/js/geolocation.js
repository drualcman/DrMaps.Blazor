function getPositionAsync() {
    return new Promise((resolve, reject) => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(returnPosition, returnError, { enableHighAccuracy: true, maximumAge: 0 });
        }

        function returnPosition(position) {
            resolve({
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            });
        }

        function returnError(error) {
            let errorMessage;
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    errorMessage = 'User denied the request for Geolocation';
                    break;
                case error.POSITION_UNAVAILABLE:
                    errorMessage = "Location information is unavailable";
                    break;
                case error.TIMEOUT:
                    errorMessage = 'The request to get user location timed out!';
                    break;
                case error.UNKNOW:
                    errorMessage = 'An unknow error ocurred';
                    break;
            }
            reject(errorMessage);
        }
    });
}

function checkGeolocationPermission() {
    return navigator.permissions.query({ name: 'geolocation' })
        .then((permissionStatus) => {
            return permissionStatus.state === 'granted';
        })
        .catch((error) => {
            // Maneja cualquier error que ocurra durante la verificación del permiso
            console.error('Error checking geolocation permission:', error);
            return false;
        });
}

export { getPositionAsync, checkGeolocationPermission }