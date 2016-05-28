'use strict';

(function (angular) {

    angular.module('ng-ajax', []).factory('$ajax', ['$http', '$q', '$window', function ($http, $q, $window) {
        var serviceBase = '/tophandler.ashx?method=';
        var loginUrl = '/Login.aspx'
        var factory = {};

        function ErrBreak(rsp) {
            if (rsp.code == 101) {
                $window.location.href = loginUrl + '?ReturnUrl=' + encodeURIComponent($window.location.pathname);
                return true
            }
            return false
        }

        function deferAjax(config) {
            var deferred = $q.defer()

            angular.extend(config, {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;' },
                transformRequest: transform,
            })

            $http(config).success(function (data, status, headers, config) {
                if (ErrBreak(data)) return
                deferred.resolve(data)
            }).error(function (data, status, headers, config) {
                deferred.resolve({ msg: "failed", code: -1 })
            })

            return deferred.promise
        }

        factory.get = function (method, data, callback) {
            var promise = deferAjax({
                method: 'GET',
                url: serviceBase + method,
                params: data,
            })

            if (angular.isFunction(callback)) {
                promise.then(callback)
            }
            else {
                return promise
            }
        }

        factory.deferGet = function (method, data) {
            return factory.get(method, data)
        }

        factory.post = function (method, data, callback) {
            var promise = deferAjax({
                method: 'POST',
                url: serviceBase + method,
                transformRequest: transform,
                data: data,
            })

            if (angular.isFunction(callback)) {
                promise.then(callback)
            }
            else {
                return promise
            }
        }

        function transform(obj) {
            if (angular.isObject(obj)) {
                var str = [];
                for (var p in obj) {
                    var value = obj[p]
                    if (angular.isDefined(value) && value != null) {
                        if (angular.isObject(value)) {
                            value = JSON.stringify(value)
                        }
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(value));
                    }
                }
                return str.join("&");
            }
            else {
                return obj;
            }
        }

        factory.GetQueryString = function (name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec($window.location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        return factory;
    }])

})(window.angular)