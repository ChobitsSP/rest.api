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

        factory.get = function (method, data, callback) {
            $http({
                method: 'GET',
                url: serviceBase + method,
                params: data,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;' }
            }).success(function (data, status, headers, config) {
                if (ErrBreak(data)) return
                if (angular.isFunction(callback)) {
                    callback(data);
                }
            }).error(function (data, status, headers, config) {
                if (angular.isFunction(callback)) {
                    callback({ msg: "请求失败", code: -1 });
                }
            });
        };

        factory.deferGet = function (method, data) {
            var deferred = $q.defer();

            $http({
                method: 'GET',
                cache: false,
                url: serviceBase + method,
                params: data,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;' }
            }).success(function (data, status, headers, config) {
                if (ErrBreak(data)) return
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.resolve({ msg: "请求失败", code: -1 });
            });

            return deferred.promise;
        }

        factory.post = function (method, data, callback) {
            this.AjaxPostWithNoAuthenication(method, data, function (rsp) {
                if (ErrBreak(data)) return
                if (angular.isFunction(callback)) {
                    callback(rsp);
                }
            });
        };

        factory.AjaxPostWithNoAuthenication = function (method, data, callback) {
            $http({
                method: 'POST',
                url: serviceBase + method,
                transformRequest: transform,
                data: data,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded;' }
            }).success(function (data, status, headers, config) {
                if (angular.isFunction(callback)) {
                    callback(data);
                }
            }).error(function (data, status, headers, config) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.

                if (angular.isFunction(callback)) {
                    callback({ msg: "请求失败", code: -1 });
                }
            });
        };

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