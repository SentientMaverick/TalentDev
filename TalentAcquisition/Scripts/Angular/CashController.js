(function (app) {
    'use strict';

    app.controller('TransactionTypeController', ['$scope', 'appSetting', '$http', TransactionTypeController]);
    function TransactionTypeController($scope, appSetting, $http) {
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
            $http.get(appSetting.apiBaseUrl + "/api/Transaction/Types", { params: {} })
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
            $http.post(appSetting.apiBaseUrl + "/api/Transaction/Types", { types: $scope.grievanceTypes })
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
    app.controller('ExpenseRecordController', ['$scope', 'appSetting', '$http', ExpenseRecordController]);
    function ExpenseRecordController($scope, appSetting, $http) {
        console.log("This works fine in this ctrl");
        $scope.data = [{
            Name:"GA001",Description:"An Employee Who Fought Someone",Amount:"100,000.00"
        },
        {
            Name: "GA002", Description: "An Employee Who Abused Someone",Amount:"100,000.00"
        }];
        $scope.empoyees = angular.copy($scope.data);
        $scope.enabledEdit = [];
        $scope.skillName = "";
        $scope.TotalAmount = 0.00;
        $scope.grievanceTypes = [];
        $scope.getGrievanceTypes = function () {
            if (itemsbs == null) {
                $scope.grievanceTypes = [];
            }
            else {
                $scope.grievanceTypes = $scope.items;
                console.log($scope.grievanceTypes);
            }
            
            console.log($scope.grievanceTypes);
           // EmployeeService.GetSkills().then(function (result) {
            //$http.get("http://localhost:54105" + "/api/Transaction/Types", { params: { } })
            //    .then(function (result) {
            //    console.log(result);
            //    $scope.grievanceTypes = result.data.types;
            //    console.log($scope.grievanceTypes);
            //    }, function (error) {
            //        console.log(error);
            //    console.log("Bad Request Process");
            //});
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
                Id: "", Name: "", Description: "", Amount: 0.00, CashRequisitionNo:"", disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
        };
        $scope.submitGrievanceTypes = function () {
            console.log($scope.grievanceTypes);
            $http.post(appSetting.apiBaseUrl + "/api/Transaction/Types", { types: $scope.grievanceTypes })
                .then(function (result) {
                    console.log(result);
                    $scope.grievanceTypes = result.data.types;
                    console.log($scope.grievanceTypes);
                }, function (error) {
                    console.log(error);
                    console.log("Bad Request Process");
                });
        };
        $scope.employees = employeesbs;
        $scope.items = itemsbs;
        console.log($scope.employees);
        $scope.selectedEmployeeName = "";
        $scope.selectedEmployeeDepartment = "";
        $scope.selecteOffenderName = "";
        $scope.selectedEmployeeJobCode = "";
        $scope.selectedTrainingCourse = "";
        $scope.selectedTrainingCourse = "";
        $scope.selectedemployeechanged = function () {
            console.log($scope.SelectedEmployee);
            console.log($scope.SelectedEmployee.Name);
            $scope.selectedEmployeeName = $scope.SelectedEmployee.Name;
            $scope.selectedEmployeeJobCode = $scope.SelectedEmployee.JobCode;
            $scope.selectedEmployeeJobTitle = $scope.SelectedEmployee.JobTitle;
            $scope.selectedEmployeeDepartment = $scope.SelectedEmployee.Department;
        };
    };
    //app.controller('TrainingProviderController', ['$scope', '$http', TrainingProviderController]);
    //function TrainingProviderController($scope, $http) {
    //    console.log("This works fine in this ctrl");
    //    $scope.data = [{
    //        Code: "GA001", Description: "An Employee Who Fought Someone"
    //    },
    //    {
    //        Code: "GA002", Description: "An Employee Who Abused Someone"
    //    }];

    //    $scope.empoyees = angular.copy($scope.data);
    //    $scope.enabledEdit = [];
    //    $scope.skillName = "";
    //    $scope.grievanceTypes = [];
    //    $scope.getGrievanceTypes = function () {
    //        // EmployeeService.GetSkills().then(function (result) {
    //        $http.get("http://localhost:54105" + "/api/Complaint/IndisciplineTypes", { params: {} })
    //            .then(function (result) {
    //                console.log(result);
    //                $scope.grievanceTypes = result.data.types;
    //                console.log($scope.grievanceTypes);
    //            }, function (error) {
    //                console.log(error);
    //                console.log("Bad Request Process");
    //            });
    //    };
    //    $scope.init = function () {
    //        //console.log("employeeid" + $scope.employeeid);
    //        $scope.getGrievanceTypes();
    //        //$scope.grievanceTypes = angular.copy($scope.data);
    //    };
    //    $scope.edit = function (index) {
    //        console.log("edit index" + index);
    //        $scope.enabledEdit[index] = true;
    //    };
    //    $scope.delete = function (index) {
    //        $scope.grievanceTypes.splice(index, 1);
    //    };
    //    $scope.addGrievanceType = function () {
    //        var gtype = {
    //            Code: "", Description: "", disableEdit: false
    //        };
    //        $scope.grievanceTypes.push(gtype);
    //        $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
    //    };
    //    $scope.submitGrievanceTypes = function () {
    //        console.log($scope.grievanceTypes);
    //        $http.post("http://localhost:54105" + "/api/Complaint/IndisciplineTypes", { types: $scope.grievanceTypes })
    //            .then(function (result) {
    //                console.log(result);
    //                $scope.grievanceTypes = result.data.types;
    //                console.log($scope.grievanceTypes);
    //            }, function (error) {
    //                console.log(error);
    //                console.log("Bad Request Process");
    //            });
    //    };
    //};

    //app.controller('GrievanceActionController', ['$scope', '$http', GrievanceActionController]);
    //function GrievanceActionController($scope, $http) {
    //    console.log("This works fine in this gievance acttion controller");
    //    $scope.data = [{
    //        Code: "GA001", Description: "An Employee Who Fought Someone"
    //    },
    //    {
    //        Code: "GA002", Description: "An Employee Who Abused Someone"
    //    }];
    //    $scope.enabledEdit = [];
    //    $scope.skillName = "";
    //    $scope.grievanceTypes = [];
    //    $scope.getGrievanceTypes = function () {
    //        // EmployeeService.GetSkills().then(function (result) {
    //        console.log("gettting grevance actions");

    //        $http.get("http://localhost:54105" + "/api/Complaint/GrievanceActions", { params: {} })
    //            .then(function (result) {
    //                console.log(result);
    //                $scope.grievanceTypes = result.data.actions;
    //                console.log($scope.grievanceTypes);
    //            }, function (error) {
    //                console.log(error);
    //                console.log("Bad Request Process");
    //            });
    //    };
    //    $scope.init = function () {
    //        console.log("employeeid" + $scope.employeeid);
    //        $scope.getGrievanceTypes();
    //       // $scope.grievanceTypes = angular.copy($scope.data);
    //    };
    //    $scope.edit = function (index) {
    //        console.log("edit index" + index);
    //        $scope.enabledEdit[index] = true;
    //    };
    //    $scope.delete = function (index) {
    //        $scope.grievanceTypes.splice(index, 1);
    //    };
    //    $scope.setEmployeeId = function () {
    //        //$scope.employeeid = employeeid;
    //        // console.log("employeeid" + $scope.employeeid);
    //        $scope.init();
    //    };
    //    $scope.addGrievanceType = function () {
    //        var gtype = {
    //            Code: "", Description: "", disableEdit: false
    //        };
    //        $scope.grievanceTypes.push(gtype);
    //        $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
    //    };
    //    $scope.submitGrievanceTypes = function () {
    //        console.log($scope.grievanceTypes);
    //        $http.post("http://localhost:54105" + "/api/Complaint/GrievanceActions", { actions: $scope.grievanceTypes })
    //            .then(function (result) {
    //                console.log(result);
    //                $scope.grievanceTypes = result.data.actions;
    //                console.log($scope.grievanceTypes);
    //            }, function (error) {
    //                console.log(error);
    //                console.log("Bad Request Process");
    //            });
    //    };
    //};

    //app.controller('DisciplinaryActionController', ['$scope', '$http', DisciplinaryActionController]);
    //function DisciplinaryActionController($scope, $http) {
    //    console.log("This works fine in this gievance acttion controller");
    //    $scope.data = [{
    //        Code: "GA001", Description: "An Employee Who Fought Someone"
    //    },
    //    {
    //        Code: "GA002", Description: "An Employee Who Abused Someone"
    //    }];
    //    $scope.enabledEdit = [];
    //    $scope.skillName = "";
    //    $scope.grievanceTypes = [];
    //    $scope.getGrievanceTypes = function () {
    //        // EmployeeService.GetSkills().then(function (result) {
    //        console.log("gettting grevance actions");

    //        $http.get("http://localhost:54105" + "/api/Complaint/DisciplinaryActions", { params: {} })
    //            .then(function (result) {
    //                console.log(result);
    //                $scope.grievanceTypes = result.data.actions;
    //                console.log($scope.grievanceTypes);
    //            }, function (error) {
    //                console.log(error);
    //                console.log("Bad Request Process");
    //            });
    //    };
    //    $scope.init = function () {
    //        console.log("employeeid" + $scope.employeeid);
    //        $scope.getGrievanceTypes();
    //        // $scope.grievanceTypes = angular.copy($scope.data);
    //    };
    //    $scope.edit = function (index) {
    //        console.log("edit index" + index);
    //        $scope.enabledEdit[index] = true;
    //    };
    //    $scope.delete = function (index) {
    //        $scope.grievanceTypes.splice(index, 1);
    //    };
    //    $scope.setEmployeeId = function () {
    //        //$scope.employeeid = employeeid;
    //        // console.log("employeeid" + $scope.employeeid);
    //        $scope.init();
    //    };
    //    $scope.addGrievanceType = function () {
    //        var gtype = {
    //            Code: "", Description: "", disableEdit: false
    //        };
    //        $scope.grievanceTypes.push(gtype);
    //        $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
    //    };
    //    $scope.submitGrievanceTypes = function () {
    //        console.log($scope.grievanceTypes);
    //        $http.post("http://localhost:54105" + "/api/Complaint/DisciplinaryActions", { actions: $scope.grievanceTypes })
    //            .then(function (result) {
    //                console.log(result);
    //                $scope.grievanceTypes = result.data.actions;
    //                console.log($scope.grievanceTypes);
    //            }, function (error) {
    //                console.log(error);
    //                console.log("Bad Request Process");
    //            });
    //    };
    //};

    //app.controller('GrievanceRecordController', ['$scope', '$http', GrievanceRecordController]);
    //function GrievanceRecordController($scope, $http) {
    //    $scope.employees = employeesbs;
    //    $scope.grievances = grievancesbs;
    //    $scope.selectedEmployeeName = "";
    //    $scope.selecteOffenderName = "";
    //    $scope.selectedemployeechanged = function () {
    //        console.log($scope.SelectedEmployee);
    //        console.log($scope.SelectedEmployee.Name);
    //        $scope.selectedEmployeeName = $scope.SelectedEmployee.Name;
    //    };
    //    $scope.selectedOffenderChanged = function () {
    //        console.log($scope.SelectedOffender);
    //        console.log($scope.SelectedOffender.Name);
    //        $scope.selectedOffenderName = $scope.SelectedOffender.Name;
    //    };
    //    $scope.selectedGrievanceDescription = "";
    //    $scope.selectedGrievanceChanged = function () {
    //        console.log($scope.SelectedGrievance);
    //        console.log($scope.SelectedGrievance.Code);
    //        $scope.selectedGrievanceDescription = $scope.SelectedGrievance.Description;
    //    };
    //};

    //app.controller('DisciplinaryRecordController', ['$scope', '$http', DisciplinaryRecordController]);
    //function DisciplinaryRecordController($scope, $http) {
    //    $scope.employees = employeesbs;
    //    $scope.actions = actionsbs;
    //    $scope.selectedEmployeeName = "";
    //    $scope.selecteOffenderName = "";
    //    $scope.selectedEmployeeJobCode = "";
    //    $scope.selectedEmployeeJobTitle = "";
    //    $scope.selectedemployeechanged = function () {
    //        console.log($scope.SelectedEmployee);
    //        console.log($scope.SelectedEmployee.Name);
    //        $scope.selectedEmployeeName = $scope.SelectedEmployee.Name;
    //        $scope.selectedEmployeeJobCode = $scope.SelectedEmployee.JobCode;
    //        $scope.selectedEmployeeJobTitle = $scope.SelectedEmployee.JobTitle;
    //    };
    //    $scope.selectedOffenderChanged = function () {
    //        console.log($scope.SelectedOffender);
    //        console.log($scope.SelectedOffender.Name);
    //        $scope.selectedOffenderName = $scope.SelectedOffender.Name;
    //    };
    //    $scope.selectedGrievanceDescription = "";
    //    $scope.selectedGrievanceChanged = function () {
    //        console.log($scope.SelectedGrievance);
    //        console.log($scope.SelectedGrievance.Code);
    //        $scope.selectedGrievanceDescription = $scope.SelectedGrievance.Description;
    //    };
    //};

})(angular.module("NormalApp"));

