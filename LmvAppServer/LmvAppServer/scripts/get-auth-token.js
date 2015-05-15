var getAuthToken = angular.module("getAuthToken", []);

// Sync call to LmvAuthTokenServer 
getAuthToken.factory('authTokenFactory', function () {

    var getAToken = getAccessToken; 
    return getAToken;
});
