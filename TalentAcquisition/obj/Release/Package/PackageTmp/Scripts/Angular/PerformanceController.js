(function (app) {
    'use strict';

    app.controller('AppraisalGradeController', ['$scope', 'appSetting', '$http', AppraisalGradeController]);
    function AppraisalGradeController($scope, appSetting, $http) {
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
            $http.get(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Grades", { params: {} })
                .then(function (result) {
                    console.log(result);
                    if (result.data.grades == null) {
                        $scope.grievanceTypes = [];
                    }
                    else
                    {
                        $scope.grievanceTypes = result.data.grades;
                    }
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
                Name:"",Value:"",disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.submitGrievanceTypes = function () {
            console.log($scope.grievanceTypes);
            $http.post(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Grades", { grades: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.grades;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
    };
    app.controller('AppraisalTypeController', ['$scope','appSetting', '$http', AppraisalTypeController]);
    function AppraisalTypeController($scope,appSetting, $http) {
        console.log("This works fine in this ctrl");
        //console.log(appSetting.apiBaseUrl + "Yo");
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
            console.log(appSetting.apiBaseUrl);
            $http.get(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Types", { params: {} })
                .then(function (result) {
                    console.log(result);
                    if (result.data.types == null) {
                        $scope.grievanceTypes = [];
                    }
                    else {
                        $scope.grievanceTypes = result.data.types;
                    }
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
            $http.post(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Types", { types: $scope.grievanceTypes })
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
    app.controller('AppraisalPeriodController', ['$scope', 'appSetting', '$http', AppraisalPeriodController]);
    function AppraisalPeriodController($scope, appSetting, $http) {
        console.log("This works fine in this ctrl");
        $scope.data = [{
            Code: "GA001", Description: "An Employee Who Fought Someone"
        },
        {
            Code: "GA002", Description: "An Employee Who Abused Someone"
        }];

        $scope.empoyees = angular.copy($scope.data);
        $scope.enabledEdit = [];
        $scope.nextIndex = "";
        $scope.nextcode = "";
        $scope.skillName = "";
        $scope.grievanceTypes = [];
        $scope.getGrievanceTypes = function () {
            // EmployeeService.GetSkills().then(function (result) {
            $http.get(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Periods", { params: {} })
                .then(function (result) {
                    console.log(result);
                    if (result.data.periods == null) {
                        $scope.grievanceTypes = [];
                    }
                    else {
                        $scope.grievanceTypes = result.data.periods;
                    }
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
        $scope.parseDate = function (date) {
           // var date1 = date.replaceAll("\\\\/Date\\((\\d+\\+\\d+)\\)\\\\/", "$1+$2");
            // console.log(date1);
            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(date);
            var dt = new Date(parseFloat(results[1]));
            console.log(dt);
            return dt.getDate() + "-" +(dt.getMonth() + 1)+ "-" + dt.getFullYear();
            //d = FOO.STRFTIME('%Y-%M-%D %H:%M:%S %Z');
            //var date1 = date.replaceAll("\\\\/Date\\((\\d+\\+\\d+)\\)\\\\/", "$1+$2");
            //var date2 = date1.split("\\+");
            //console.log(date);
            //return new Date(Date.parse(date));
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
            $http.post(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Periods", { periods: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.periods;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
    };
    app.controller('KPITemplateController', ['$scope', 'appSetting', '$http', KPITemplateController]);
    function KPITemplateController($scope,appSetting, $http) {
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
        $scope.HeaderCode = "";
        $scope.AppraisalClass = "";
        $scope.getGrievanceTypes = function () {
            $scope.grievanceTypes = linesbs;
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
                AppraisalNonJobKPICode: $scope.HeaderCode, AppraisalClass: $scope.AppraisalClass, disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.submitGrievanceTypes = function () {
            console.log($scope.grievanceTypes);
            $http.post(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Types", { types: $scope.grievanceTypes })
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
    app.controller('EmployeeAppraisalController', ['$scope', 'appSetting', '$http', EmployeeAppraisalController]);
    function EmployeeAppraisalController($scope, appSetting, $http, $filter) {
        console.log("This works fine in this ctrl");
        $scope.employees = employeesbs;
        $scope.KPICodes = kpicodesbs;
        $scope.enabledEdit = [];
        $scope.skillName = "";
        $scope.grievanceTypes = [];
        $scope.HeaderCode = "";
        $scope.AppraisalClass = "";
        $scope.getGrievanceTypes = function (code) {
            console.log($scope.AppraisalNo);
            $http.get(appSetting.apiBaseUrl + "/api/GetAppraisalKPIFromTemplate/", { params: { id: code, appraisalno: $scope.AppraisalNo } })
               .then(function (result) {
                   console.log(result);
                   if (result.data.lines == null) {
                       $scope.grievanceTypes = [];
                   }
                   else {
                       $scope.grievanceTypes = result.data.lines;
                   }
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
                AppraisalNonJobKPICode: $scope.HeaderCode, AppraisalClass: $scope.AppraisalClass, disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.submitGrievanceTypes = function () {
            console.log($scope.grievanceTypes);
            $http.post(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Types", { types: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.types;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
        $scope.selectedemployeechanged = function () {
            console.log($scope.SelectedEmployee);
            console.log($scope.SelectedEmployee.Name);
            $scope.selectedEmployeeName = $scope.SelectedEmployee.Name;
            $scope.selectedEmployeeJobCode = $scope.SelectedEmployee.JobCode;
            $scope.selectedEmployeeJobTitle = $scope.SelectedEmployee.JobTitle;
           // console.log($scope.SelectedEmployee.Supervisor);
            var luckyGrab = $filter('filter')($scope.employees, { Id: $scope.SelectedEmployee.Supervisor });
            var allGrab = $filter('filter')($scope.employees, function (o) {
                return o.Id == $scope.SelectedEmployee.Supervisor
            });
            if (allGrab[0] != null) {
                console.log("Not Broken");
                $scope.selectedEmployeeSupervisor = allGrab[0].Number;
                $scope.selectedEmployeeSupervisorName = allGrab[0].Name;
            }
            else {
                $scope.selectedEmployeeSupervisor = "";
                $scope.selectedEmployeeSupervisorName = "";
            }
            //console.log(allGrab);
        };
        $scope.selectedKPICodeChanged = function () {
            $scope.selectedKPICodeClass = $scope.selectedKPICode.Description;
            $scope.getGrievanceTypes($scope.selectedKPICode.Code);
           
            var itemgs = $scope.grievanceTypes;
            for (var index = 0; index < 5; index++)
            {
                $scope.enabledEdit[index] = true;
                //index = index + 1;
            };
            //angular.foreach($scope.grievanceTypes, function (value, key) {
            //    $scope.enabledEdit[index] = true;
            //    index = index + 1;
            //});
            
        };
    };
    app.controller('SupervisorAppraisalController', ['$scope','appSetting', '$http','$filter', SupervisorAppraisalController]);
    function SupervisorAppraisalController($scope, appSetting, $http, $filter) {
        console.log("This works fine in this ctrl");
        $scope.employees = employeesbs;
        $scope.KPICodes = kpicodesbs;
        $scope.Lines = linesbs;
        console.log($scope.Lines);
        $scope.enabledEdit = [];
        $scope.skillName = "";
        $scope.grievanceTypes = linesbs;
        $scope.HeaderCode = "";
        $scope.AppraisalClass = "";
        $scope.getGrievanceTypes = function (code) {
            console.log($scope.AppraisalNo);
            $http.get(appSetting.apiBaseUrl + "/api/GetAppraisalKPIFromTemplate/", { params: { id: code, appraisalno: $scope.AppraisalNo } })
               .then(function (result) {
                   console.log(result);
                   if (result.data.lines == null) {
                       $scope.grievanceTypes = [];
                   }
                   else {
                       $scope.grievanceTypes = result.data.lines;
                   }
                   console.log($scope.grievanceTypes);
               }, function (error) {
                   console.log(error);
                   console.log("Bad Request Process");
               });
        };
        $scope.validatesuccessful = false;
        $scope.init = function () {
            //console.log("employeeid" + $scope.employeeid);
            $scope.validatesuccessful = true;
            for (var index = 0; index < 5; index++) {
                $scope.enabledEdit[index] = true;
                //index = index + 1;
            };
           // $scope.getGrievanceTypes();
            //$scope.grievanceTypes = angular.copy($scope.data);
        };
        $scope.performance = function (a,b) {
            if (a > 0 && b > 0) {
                return a / b;
            }
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
                AppraisalNonJobKPICode: $scope.HeaderCode, AppraisalClass: $scope.AppraisalClass, disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.submitGrievanceTypes = function () {
            console.log($scope.grievanceTypes);
            $http.post(appSetting.apiBaseUrl + "/api/Performance/Appraisal/Types", { types: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.types;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
        $scope.selectedemployeechanged = function () {
            console.log($scope.SelectedEmployee);
            console.log($scope.SelectedEmployee.Name);
            $scope.selectedEmployeeName = $scope.SelectedEmployee.Name;
            $scope.selectedEmployeeJobCode = $scope.SelectedEmployee.JobCode;
            $scope.selectedEmployeeJobTitle = $scope.SelectedEmployee.JobTitle;
            // console.log($scope.SelectedEmployee.Supervisor);
            var luckyGrab = $filter('filter')($scope.employees, { Id: $scope.SelectedEmployee.Supervisor });
            var allGrab = $filter('filter')($scope.employees, function (o) {
                return o.Id == $scope.SelectedEmployee.Supervisor
            });
            if (allGrab[0] != null) {
                console.log("Not Broken");
                $scope.selectedEmployeeSupervisor = allGrab[0].Number;
                $scope.selectedEmployeeSupervisorName = allGrab[0].Name;
            }
            else {
                $scope.selectedEmployeeSupervisor = "";
                $scope.selectedEmployeeSupervisorName = "";
            }
            //console.log(allGrab);
        };
        $scope.selectedKPICodeChanged = function () {
            $scope.selectedKPICodeClass = $scope.selectedKPICode.Description;
            $scope.getGrievanceTypes($scope.selectedKPICode.Code);

            var itemgs = $scope.grievanceTypes;
            for (var index = 0; index < 5; index++) {
                $scope.enabledEdit[index] = true;
                //index = index + 1;
            };
            //angular.foreach($scope.grievanceTypes, function (value, key) {
            //    $scope.enabledEdit[index] = true;
            //    index = index + 1;
            //});

        };
    };
})(angular.module("NormalApp"));

