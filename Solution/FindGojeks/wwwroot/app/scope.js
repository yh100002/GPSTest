
//module
var app = angular.module("angularApp", [])

//each controller
app.controller("initDataContoller",  function ($scope, $http) {
    $scope.upload = function() {       
            $http.post("/api/drivers/init","").
            success(function (data, status, headers, config) {
                $scope.myTextArea1 = data;
            }).
            error(function (data, status, headers, config) {
                $scope.myTextArea1 = data;
            });
    }; 
});

app.controller("dataVerifyContoller",  function ($scope, $http) {
    var data = {
    url:"test"
    };

    var config = {
    params: data,    
    headers : {'Accept' : 'text/html'}
    };

    $scope.checking = function() {       
        $http.get("/api/drivers/getalllocation","").
            success(function (data, status, headers, config) {
                $scope.checkArea = JSON.stringify(data);
            }).
            error(function (data, status, headers, config) {
                $scope.checkArea = JSON.stringify(data);
            });
    }; 
});

app.controller("searchContoller", function($scope, $http) {
    $scope.search = function() {
        var mypos = {}; 
        mypos["Id"] = 0;
        mypos["Latitude"] = parseFloat($scope.lat);
        mypos["Longitude"] = parseFloat($scope.lon);
        mypos["Radius"] = parseFloat($scope.rad);
        mypos["limit"] = parseFloat($scope.lim);
        
        var config = {
        params: mypos,    
        headers : {'Accept' : 'application/json'}
        };

        //$scope.searchResponse = JSON.stringify(mypos);
        $http.get("/api/drivers",config).
        success(function (data, status, headers, config) {
            $scope.searchResponse = JSON.stringify(data);
        }).
        error(function (data, status, headers, config) {
            $scope.searchResponse = JSON.stringify(data);
        });
    };      
});

