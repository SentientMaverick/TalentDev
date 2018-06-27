(function (app) {

    'use strict';

    app.factory("EmployeeService", ["$http", "appSetting", "$q", EmployeeService]);

    function EmployeeService($http, appSetting, $q) {

        this.GetAll = function () {
            var deffered = $q.defer();

            var query = $http.get(appSetting.apiBaseUrl + "api/employee/GetEmployees");

            return query.then(function (response) {
                deffered.resolve(response.data);
                return deffered.promise;
            }, function (response) {
                deffered.reject(response);
                return deffered.promise;
            });
        };
        this.GetSkills = function () {
            var deffered = $q.defer();

            var query = $http.get(appSetting.apiBaseUrl + "api/employee/GetSkills", { params: {id:1} });

            return query.then(function (response) {
                deffered.resolve(response.data);
                return deffered.promise;
            }, function (response) {
                deffered.reject(response);
                return deffered.promise;
            });
        };
        return {
            GetAll: this.GetAll ,
            GetSkills:this.GetSkills
        }
    };

})(angular.module("NormalApp"));