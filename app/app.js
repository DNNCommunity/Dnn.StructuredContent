var app = angular.module('StructuredContent', ['ngMessages', 'ngAnimate', 'ui.bootstrap', 'toastr', 'dndLists', 'ui.ace'], function ($locationProvider) {
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
});

app.config(function ($httpProvider) {
    $httpProvider.defaults.withCredentials = true;
});

app.config(function (toastrConfig) {
    angular.extend(toastrConfig, {
        positionClass: 'toast-top-right',
        timeOut: 3000,
        maxOpened: 1,
        progressBar: true,
        tapToDismiss: true,
        autoDismiss: true,
        toastClass: 'toastr'
    });
});

//app.config(function (ngIntlTelInputProvider) {
//    ngIntlTelInputProvider.set({ initialCountry: 'us' });
//});

app.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                if (val !== undefined) {
                    return parseInt(val, 10);
                }
                else {
                    return null;
                }
            });
            ngModel.$formatters.push(function (val) {
                if (val !== undefined) {
                    return '' + val;
                }
                else {
                    return null;
                }
            });
        }
    };
});

app.filter('search', function () {
    return function (list, field, partialString) {
        // if the search string is invalid or empty - return the entire list
        if (!angular.isString(partialString) || partialString.length === 0) return list;

        var results = [];

        angular.forEach(list, function (item) {
            if (angular.isString(item[field])) {
                if (item[field].search(new RegExp(partialString, "i")) > -1) {
                    results.push(item);
                }
            }
        });

        return results;
    };
});

var slugify = function (string) {
    const a = 'àáäâãåăæçèéëêǵḧìíïîḿńǹñòóöôœṕŕßśșțùúüûǘẃẍÿź·/_,:;';
    const b = 'aaaaaaaaceeeeghiiiimnnnoooooprssstuuuuuwxyz------';
    const p = new RegExp(a.split('').join('|'), 'g');
    return string.toString().toLowerCase()
        .replace(/\s+/g, '-') // Replace spaces with -
        .replace(p, c => b.charAt(a.indexOf(c))) // Replace special characters
        .replace(/&/g, '-and-') // Replace & with ‘and’
        .replace(/[^\w\-]+/g, '') // Remove all non-word characters
        .replace(/\-\-+/g, '-') // Replace multiple - with single -
        .replace(/^-+/, '') // Trim - from start of text
        .replace(/-+$/, ''); // Trim - from end of text
};

var safeColumnName = function (string) {
    const a = 'àáäâãåăæçèéëêǵḧìíïîḿńǹñòóöôœṕŕßśșțùúüûǘẃẍÿź·/-,:;';
    const b = 'aaaaaaaaceeeeghiiiimnnnoooooprssstuuuuuwxyz______';
    const p = new RegExp(a.split('').join('|'), 'g');
    return string.toString().toLowerCase()
        .replace(/\s+/g, '_') // Replace spaces with _
        .replace(p, c => b.charAt(a.indexOf(c))) // Replace special characters
        .replace(/&/g, '-and-') // Replace & with ‘and’
        .replace(/[^\w\-]+/g, '') // Remove all non-word characters
        .replace(/\_\_+/g, '_') // Replace multiple _ with single _
        .replace(/^_+/, '') // Trim _ from start of text
        .replace(/_+$/, ''); // Trim _ from end of text
};