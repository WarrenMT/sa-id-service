var app = angular.module('app', ['ngRoute']);

app.config(function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: '../templates/home.html',
            controller: 'HomeController as home'
        }).
        when('/view/:id_number', {
            templateUrl: '../templates/view.html',
            controller: 'ViewController as view'
        }).
        otherwise({
            redirectTo: '/home'
        });
});

app.controller('HomeController', function ($http, $location) {
    var vm = this;

    vm.id_number = "";

    vm.generateId = function () {
        $http.get('/identification/generate').then(
            function (response) {
                var identification = response.data.Data;

                if (response.data.Error == null)
                {
                    $location.path('/view/' + identification.IdNumber);
                }
                else
                {
                    $.growl.error({ title: "Something went wrong", message: response.data.Error })
                }
            },
            function (response) {
                console.log(response);
            }
        );
    }
});

app.controller('ViewController', function ($http, $routeParams) {

    var vm = this;
    vm.identification = null;

    vm.verifyId = function () {
        $http.post('/identification/verify', { id: $routeParams.id_number }).then(
            function (response) {
                vm.identification = response.data.Data;
            },
            function (response) {
                console.log(response);
            }
        );
    }

});