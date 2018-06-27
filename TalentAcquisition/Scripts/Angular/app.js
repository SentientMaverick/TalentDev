(function () {
    'use strict';

    var app = angular.module("NormalApp", ['ui.router']);

    app.constant("appSetting", {
        apiBaseUrl: "http://localhost:54105"
    });

})();