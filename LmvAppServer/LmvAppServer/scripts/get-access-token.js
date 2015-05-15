function getAccessToken() {
    var theUrl = 'http://localhost:52938/api/authToken';
    var xmlHttp = null;
    xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET", theUrl, false);
    xmlHttp.send(null);
    if (xmlHttp.status === 200 || xmlHttp.status === 304) {
        var rt = xmlHttp.responseText;
        if (rt == null) {
            alert("xmlHttp.responseText is null")
        } else {
            return JSON.parse(rt);
        }
    }
}