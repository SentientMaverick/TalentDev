(function (app) {
    'use strict';

    app.controller('GrievanceTypeController', ['$scope', '$http', GrievanceTypeController]);
    function GrievanceTypeController($scope,$http) {
        console.log("This works fine in this ctrl");
        $scope.data = [{
            Code:"GA001",Description:"An Employee Who Fought Someone"
        },
        {
            Code: "GA002", Description: "An Employee Who Abused Someone"
        }];

        $scope.empoyees = angular.copy($scope.data);
        $scope.enabledEdit = [];
        $scope.skillName = "";
        $scope.grievanceTypes = [];
        $scope.getGrievanceTypes = function () {
           // EmployeeService.GetSkills().then(function (result) {
            $http.get("http://localhost:54105" + "/api/Complaint/GetGrievanceTypes", { params: { } })
                .then(function (result) {
                console.log(result);
                $scope.grievanceTypes = result.data.types;
                console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                console.log("Bad Request Process");
            });
        };
        $scope.init = function () {
            //console.log("employeeid" + $scope.employeeid);
            // $scope.getGrievanceTypes();
            $scope.grievanceTypes = angular.copy($scope.data);
        };
        $scope.edit = function (index) {
            console.log("edit index" + index);
            $scope.enabledEdit[index] = true;
        };
        $scope.delete = function (index) {
            $scope.grievanceTypes.splice(index, 1);
        };
        $scope.setEmployeeId = function () {
            //$scope.employeeid = employeeid;
           // console.log("employeeid" + $scope.employeeid);
            $scope.init();
        };
        $scope.addGrievanceType = function () {
            var gtype ={
                Code:"",Description:"",disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.submitGrievanceTypes = function () {
            console.log($scope.grievanceTypes);
        };
        $scope.SaveNewGrievanceType = function () {
            //console.log($scope.skillName);
            //var id = $scope.employeeid;
            $http.post("http://localhost:54105" + "/api/complaint/addGrievanceType", { id: $scope.employeeid, Title: $scope.skillName })
               .then(function (result) {
                   console.log(result);
                   $scope.init();
               }, function (error) {
                   console.log("Bad Request Process");
               });
        };
    };
    app.controller('GrievanceActionController', ['$scope', '$http', GrievanceActionController]);
    function GrievanceActionController($scope, $http) {
        console.log("This works fine in this ctrl");
        $scope.data = [{
            Code: "GA001", Description: "An Employee Who Fought Someone"
        },
        {
            Code: "GA002", Description: "An Employee Who Abused Someone"
        }];

        $scope.empoyees = angular.copy($scope.data);
        $scope.enabledEdit = [];
        $scope.skillName = "";
        $scope.grievanceTypes = [];
        $scope.getGrievanceTypes = function () {
            // EmployeeService.GetSkills().then(function (result) {
            $http.get("http://localhost:54105" + "/api/Complaint/GetGrievanceActions", { params: {} })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.types;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
        $scope.init = function () {
            //console.log("employeeid" + $scope.employeeid);
            // $scope.getGrievanceTypes();
            $scope.grievanceTypes = angular.copy($scope.data);
        };
        $scope.edit = function (index) {
            console.log("edit index" + index);
            $scope.enabledEdit[index] = true;
        };
        $scope.delete = function (index) {
            $scope.grievanceTypes.splice(index, 1);
        };
        $scope.setEmployeeId = function () {
            //$scope.employeeid = employeeid;
            // console.log("employeeid" + $scope.employeeid);
            $scope.init();
        };
        $scope.addGrievanceType = function () {
            var gtype = {
                Code: "", Description: "", disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.submitGrievanceTypes = function () {
            console.log($scope.grievanceTypes);
        };
        $scope.SaveNewGrievanceType = function () {
            //console.log($scope.skillName);
            //var id = $scope.employeeid;
            $http.post("http://localhost:54105" + "/api/complaint/addGrievanceType", { id: $scope.employeeid, Title: $scope.skillName })
               .then(function (result) {
                   console.log(result);
                   $scope.init();
               }, function (error) {
                   console.log("Bad Request Process");
               });
        };
    };
})(angular.module("NormalApp"));

