(function (app) {
    'use strict';
    app.controller('OnboardingTempController', ['$scope', 'appSetting', '$http', '$filter', OnboardingTempController]);
    function OnboardingTempController($scope, appSetting, $http, $filter) {
        console.log("This works fine in this ctrl");
        $scope.Ids = [];
        $scope.validtypes = [];
        $scope.enabledEdit = [];
        $scope.Body="";
        $scope.TemplateActivities = activitiesbs;
        $scope.parseDate = function (date) {
            // var date1 = date.replaceAll("\\\\/Date\\((\\d+\\+\\d+)\\)\\\\/", "$1+$2");
            // console.log(date1);
            console.log(date);
            if(new Date().getYear() == new Date(date).getYear())
            {
                return date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
            }
            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(date);
            var dt = new Date(parseFloat(results[1]));
            console.log(dt);
            return dt.getDate() + "-" + (dt.getMonth() + 1) + "-" + dt.getFullYear();
            //d = FOO.STRFTIME('%Y-%M-%D %H:%M:%S %Z');
            //var date1 = date.replaceAll("\\\\/Date\\((\\d+\\+\\d+)\\)\\\\/", "$1+$2");
            //var date2 = date1.split("\\+");
            //console.log(date);
            //return new Date(Date.parse(date));
        };
        $scope.init = function () {
            console.log('Here');
            for (var i = 0; i < 10;i++)
            {
                $scope.enabledEdit[i] = true;
            }
            console.log($scope.enabledEdit);
          //  console.log($scope.TemplateActivities);
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
        $scope.addActivity = function () {
           
            var gtype = {
                Title:'Enter Title Here',DueDate:new Date(),Body:'Enter Activity Details Here', disableEdit: false};
            $scope.TemplateActivities.push(gtype);
           // $scope.enabledEdit[$scope.TemplateActivities.length - 1] = true;
        };
        $scope.addGrievanceType = function () {
            var gtype = {
                QualificationType: "", QualificationCode: "", Priority: "0.00", ScoreID: "0.00", NeedCode: "0.00", StageCode: "0.00", DesiredScore: "0.00", Mandatory: true, disableEdit: false
            };
            $scope.grievanceTypes.push(gtype);
            $scope.enabledEdit[$scope.grievanceTypes.length - 1] = true;
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
        $scope.editorOptions = {

        };
    };
})(angular.module("NormalApp2"));
// @*<textarea ng-model="type.Body" ckeditor="editorOptions" class="form-control" name="Line[{{$index}}].Body" rows="8" cols="20" ng-disabled="!enabledEdit[{{$index}}]"></textarea>*@
                                                         //   @*<input class="hidden hghg" name="Line[{{$index+1}}].Priority" id="{{$index+1}}" />*@ 
                                                          //  <script>
                                                               // var y= $("#activitypanel").find('.hghg');
                                                                //var y= $(document.getElementById('activitypanel')).find('.panel-collapse');
                                                                //console.log(y[0].id);
                                                              //  console.log($("#activitypanel").closest("#hghg").val());
                                                               // var x=y.val();
                                                                //console.log($(this).find("#acts").length);
                                                       //         var myToolbar = [
                                                       //['Bold'],
                                                       //['Styles'],
                                                       //['Link', 'Anchor']
                                                       //         ];
                                                       //         var config ={
                                                       //             toolbar_mySimpleToolbar: myToolbar,
                                                       //             toolbar: 'mySimpleToolbar'
                                                       //         };
                                                       //         config.filebrowserUploadMethod = 'form';
                                                       //         config.filebrowserUploadMethod = 'form';
                                                       //         CKEDITOR.replace('myBody');
                                                          //  </script>