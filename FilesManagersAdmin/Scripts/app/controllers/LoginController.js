
//Создаем модуль
var app = angular.module("MyApp",[]);
//Создаем контроллер
app.controller("LoginController", function ($scope, loginService) { // Сдесь $scope используется как посредник данных между видом и контроллером
    $scope.IsLoginIn = false;
    $scope.message = "";
    $scope.submitted = false;
    $scope.IsFormValid = false;

    $scope.LoginData = {
        UserName: "",
        Password: ""
    };

    $scope.$watch("loginForm.$valid", function (newData) {  
        $scope.IsFormValid = newData;
    });

    $scope.Login = function () {            //Метод объекта $scope Login
        $scope.submitted = true;
        if ($scope.IsFormValid) {
            loginService.UserLogin($scope.LoginData).then(function (d) {
                if (d.data.login != null) {
                    $scope.IsLoginIn = true;
                    $scope.message = "Вход успешно выполнен. Добро пожаловать " + d.data.name;
                } else {
                    alert("Неверные данные");
                }
            });
        }

    };

}).factory("loginService", function ($http) {
    var fac = {};
    fac.UserLogin = function (d) {
        return $http({
            url: "/Home/UserLogin",
            method: "POST",
            data: JSON.stringify(d),
            headers: { "content-type": "application/json" }

        });
    };
    return fac;
});

