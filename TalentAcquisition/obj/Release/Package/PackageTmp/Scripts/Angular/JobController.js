(function (app) {
    'use strict';

    app.controller('JobController', ['$scope', 'appSetting', '$http','$filter', JobController]);
    function JobController($scope, appSetting, $http,$filter) {
        console.log("This works fine in this ctrl");
        $scope.codes = codesbs;
        $scope.types = typesbs;
        $scope.appevals = [];
        $scope.jobreqs = [];
        $scope.validtypes = [];
        $scope.skillName = "";
        $scope.enabledEdit = [];
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
            $scope.appevals = appevalsbs;
            $scope.jobreqs = jobreqsbs;
            console.log($scope.appevals);
            console.log($scope.jobreqs);
            //var codes = $scope.codes;
            //var types=$scope.types;
            for (var i = 0; i < $scope.jobreqs.length; i++) {
                $scope.SelectedCodes[i] = $scope.jobreqs[i].QualificationCode;
            }
            console.log($scope.SelectedCodes);
        };
        $scope.edit = function (index) {
            console.log("edit index" + index);
            $scope.enabledEdit[index] = true;
        };
        $scope.delete = function (index) {
            $scope.grievanceTypes.splice(index, 1);
        };
        $scope.grievanceTypes = [];
        $scope.SelectedCodes = [];
        $scope.addGrievanceType = function () {
            var gtype = {
                QualificationType: "", QualificationCode: "", Priority: "0.00", ScoreID: "0.00", NeedCode: "0.00", StageCode: "0.00", DesiredScore: "0.00", Mandatory: true, disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.addJobRequirement = function () {
            var gtype = {
                QualificationType: "", QualificationCode: "", Priority: "0.00", ScoreID: "0.00", NeedCode: "0.00", StageCode: "0.00", DesiredScore: "0.00", Mandatory: true, disableEdit: false
            };
            $scope.jobreqs.push(gtype);
            //$scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.addApplicantEvalMetric = function () {
            var gtype = {
                EvaluationDescription: "", EvaluationCode: "",MaximumScore:0, disableEdit: false
            };
            $scope.appevals.push(gtype);
           // $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.codeSelected = function (value) {
            console.log("Responding..."+ value);
            console.log($scope.SelectedCodes[value]);
            var allGrab = $filter('filter')($scope.types, function (o) {
                return o.QualificationCode == $scope.SelectedCodes[value].QualificationCode
            });
            console.log(allGrab);
            if (allGrab[0] != null) {
                console.log("Not Broken");
                $scope.validtypes = allGrab;
            }
            else {
                $scope.validtypes = [];
            }
        };
    };
})(angular.module("NormalApp"));
