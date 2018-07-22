(function () {
    'use strict';

    var app = angular.module("NormalApp", ['ui.router']);

    app.constant("appSetting", {
        //apiBaseUrl: "http://localhost:54105"
        apiBaseUrl: "https://talenthrm.azurewebsites.net"
    });

})();