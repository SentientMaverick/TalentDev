(function (app) {
    'use strict';

    app.controller('GrievanceTypeController', ['$scope', 'appSetting', '$http', GrievanceTypeController]);
    function GrievanceTypeController($scope, appSetting, $http) {
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
            $http.get(appSetting.apiBaseUrl + "/api/Complaint/GrievanceTypes", { params: {} })
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
            $scope.getGrievanceTypes();
            //$scope.grievanceTypes = angular.copy($scope.data);
        };
        $scope.edit = function (index) {
            console.log("edit index" + index);
            $scope.enabledEdit[index] = true;
        };
        $scope.delete = function (index) {
            $scope.grievanceTypes.splice(index, 1);
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
            $http.post(appSetting.apiBaseUrl + "/api/Complaint/GrievanceTypes", { types: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.types;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
    };

    app.controller('IndisciplineTypeController', ['$scope', 'appSetting', '$http', IndisciplineTypeController]);
    function IndisciplineTypeController($scope, appSetting, $http) {
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
            $http.get(appSetting.apiBaseUrl + "/api/Complaint/IndisciplineTypes", { params: {} })
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
            $scope.getGrievanceTypes();
            //$scope.grievanceTypes = angular.copy($scope.data);
        };
        $scope.edit = function (index) {
            console.log("edit index" + index);
            $scope.enabledEdit[index] = true;
        };
        $scope.delete = function (index) {
            $scope.grievanceTypes.splice(index, 1);
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
            $http.post(appSetting.apiBaseUrl + "/api/Complaint/IndisciplineTypes", { types: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.types;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
    };

    app.controller('GrievanceActionController', ['$scope', 'appSetting', '$http', GrievanceActionController]);
    function GrievanceActionController($scope, appSetting, $http) {
        console.log("This works fine in this gievance acttion controller");
        $scope.data = [{
            Code: "GA001", Description: "An Employee Who Fought Someone"
        },
        {
            Code: "GA002", Description: "An Employee Who Abused Someone"
        }];
        $scope.enabledEdit = [];
        $scope.skillName = "";
        $scope.grievanceTypes = [];
        $scope.getGrievanceTypes = function () {
            // EmployeeService.GetSkills().then(function (result) {
            console.log("gettting grevance actions");

            $http.get(appSetting.apiBaseUrl + "/api/Complaint/GrievanceActions", { params: {} })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.actions;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
        $scope.init = function () {
            console.log("employeeid" + $scope.employeeid);
            $scope.getGrievanceTypes();
           // $scope.grievanceTypes = angular.copy($scope.data);
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
            $http.post(appSetting.apiBaseUrl + "/api/Complaint/GrievanceActions", { actions: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.actions;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
    };

    app.controller('DisciplinaryActionController', ['$scope', 'appSetting', '$http', DisciplinaryActionController]);
    function DisciplinaryActionController($scope, appSetting, $http) {
        console.log("This works fine in this gievance acttion controller");
        $scope.data = [{
            Code: "GA001", Description: "An Employee Who Fought Someone"
        },
        {
            Code: "GA002", Description: "An Employee Who Abused Someone"
        }];
        $scope.enabledEdit = [];
        $scope.skillName = "";
        $scope.grievanceTypes = [];
        $scope.getGrievanceTypes = function () {
            // EmployeeService.GetSkills().then(function (result) {
            console.log("gettting grevance actions");

            $http.get(appSetting.apiBaseUrl + "/api/Complaint/DisciplinaryActions", { params: {} })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.actions;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
        $scope.init = function () {
            console.log("employeeid" + $scope.employeeid);
            $scope.getGrievanceTypes();
            // $scope.grievanceTypes = angular.copy($scope.data);
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
            $http.post(appSetting.apiBaseUrl + "/api/Complaint/DisciplinaryActions", { actions: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.actions;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
    };

    app.controller('GrievanceRecordController', ['$scope', 'appSetting', '$http', GrievanceRecordController]);
    function GrievanceRecordController($scope, appSetting, $http) {
        $scope.employees = employeesbs;
        $scope.grievances = grievancesbs;
        $scope.selectedEmployeeName = "";
        $scope.selecteOffenderName = "";
        $scope.selectedemployeechanged = function () {
            console.log($scope.SelectedEmployee);
            console.log($scope.SelectedEmployee.Name);
            $scope.selectedEmployeeName = $scope.SelectedEmployee.Name;
        };
        $scope.selectedOffenderChanged = function () {
            console.log($scope.SelectedOffender);
            console.log($scope.SelectedOffender.Name);
            $scope.selectedOffenderName = $scope.SelectedOffender.Name;
        };
        $scope.selectedGrievanceDescription = "";
        $scope.selectedGrievanceChanged = function () {
            console.log($scope.SelectedGrievance);
            console.log($scope.SelectedGrievance.Code);
            $scope.selectedGrievanceDescription = $scope.SelectedGrievance.Description;
        };
    };

    app.controller('DisciplinaryRecordController', ['$scope', 'appSetting', '$http', DisciplinaryRecordController]);
    function DisciplinaryRecordController($scope, appSetting, $http) {
        console.log($scope.actions);
        $scope.employees = employeesbs;
        $scope.actions = actionsbs;
        $scope.selectedEmployeeName = "";
        $scope.selecteOffenderName = "";
        $scope.selectedEmployeeJobCode = "";
        $scope.selectedEmployeeJobTitle = "";
        $scope.SelectedDisciplinaryAction = "";
        $scope.selectedDisciplinaryActionDetails = "";
        $scope.selectedemployeechanged = function () {
            console.log($scope.SelectedEmployee);
            console.log($scope.SelectedEmployee.Name);
            $scope.selectedEmployeeName = $scope.SelectedEmployee.Name;
            $scope.selectedEmployeeJobCode = $scope.SelectedEmployee.JobCode;
            $scope.selectedEmployeeJobTitle = $scope.SelectedEmployee.JobTitle;
        };
        $scope.selectedDisciplinaryActionChanged = function () {
            console.log($scope.SelectedDisciplinaryAction);
            console.log($scope.SelectedDisciplinaryAction.Code);
            $scope.selectedDisciplinaryActionDetails = $scope.SelectedDisciplinaryAction.Description;
        };
        $scope.selectedGrievanceDescription = "";
        $scope.selectedGrievanceChanged = function () {
            console.log($scope.SelectedGrievance);
            console.log($scope.SelectedGrievance.Code);
            $scope.selectedGrievanceDescription = $scope.SelectedGrievance.Description;
        };
    };

})(angular.module("NormalApp"));

