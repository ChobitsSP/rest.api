'use strict';

Vue.http.interceptors.push(function () {

    var serviceBase = '/tophandler.ashx?method='

    function isObject(value) {
        // http://jsperf.com/isobject4
        return value !== null && typeof value === 'object';
    }

    function isDefined(value) { return typeof value !== 'undefined'; }

    function transform(obj) {
        if (isObject(obj)) {
            var str = [];
            for (var p in obj) {
                var value = obj[p]
                if (isDefined(value) && value != null) {
                    obj[p] = JSON.stringify(value)
                }
            }
        }
        return obj
    }

    return {
        request: function (request) {
            request.url = serviceBase + request.url
            if (request.method == 'post') {
                request.emulateJSON = true
                request.data = transform(request.data)
            }
            return request
        },
        response: function (response) {
            return response
        }
    }
})