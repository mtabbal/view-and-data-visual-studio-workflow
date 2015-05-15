var app = angular.module("LmvAppSvr", ["ngRoute", "getAuthToken"])

app.config(function ($routeProvider) {
    $routeProvider
    .when ('/lmviewer', {
        controller: 'lmvCtrl',
        templateUrl: 'partials/lmviewer.html'
    })
    .otherwise({redirectTo: '/lmviewer'})
});

app.controller('lmvCtrl', ['$scope', 'authTokenFactory',
    function ($scope, authTokenFactory) {
        //$scope.checkPlumbing = authTokenFactory();
        //console.log("checkPlumbing . . .", $scope.checkPlumbing);
        $scope.authToken = authTokenFactory(); // Only updated on initial model load, not on refresh called by viewer.

       initialize();

       function initialize() {
           // Change docUrn value to your Encoded URN see Step 10 from http://fast-shelf-9177.herokuapp.com/
           var docUrn = 'replace with your document URN generated with same client credentials as above';
            var viewerElement = document.getElementById('viewer');
            ViewerEmbed.initialize(getToken, docUrn, viewerElement);
        }

        function getToken() {
            return authTokenFactory(); 
        }
    }
]);