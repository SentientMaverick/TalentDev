(function (app) {
    'use strict';

    app.controller('EmployeeController', ['$scope', 'appSetting', '$http', EmployeeController]);
    function EmployeeController($scope, appSetting, $http) {
        console.log("This works fine in this ctrl");
        //$scope.getAllEmployees = function () {
        //    EmployeeService.GetAll().then(function (result) {
        //        $scope.employees = result;
        //    }, function (error) {
        //        console.log("Bad Request Process");
        //    });
        //};
        $scope.employeeid = null;
        $scope.skillName = "";
        $scope.employeeskills = [];
        $scope.getEmployeeSkills = function () {
           // EmployeeService.GetSkills().then(function (result) {
            $http.get(appSetting.apiBaseUrl + "/api/employee/GetSkills", { params: { id: $scope.employeeid } })
                .then(function (result) {
                console.log(result);
                $scope.employeeskills = result.data.skills;
                console.log($scope.employeeskills);
                }, function (error) {
                    console.log(error);
                console.log("Bad Request Process");
            });
        };
        $scope.init = function () {
            console.log("employeeid" + $scope.employeeid);
            $scope.getEmployeeSkills();
        };
        $scope.setEmployeeId = function (employeeid) {
            $scope.employeeid = employeeid;
           // console.log("employeeid" + $scope.employeeid);
            $scope.init();
        };
        $scope.addSkill = function () {
            console.log($scope.skillName);
            var id = $scope.employeeid;
            $http.post(appSetting.apiBaseUrl + "/api/employee/addskill", { id: $scope.employeeid, Title: $scope.skillName })
               .then(function (result) {
                   console.log(result);
                   $scope.skillName = '';
                   $scope.init();
               }, function (error) {
                   console.log("Bad Request Process");
               });
        };
    };
})(angular.module("NormalApp"));
