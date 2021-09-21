app.factory('contentFieldService', ['$http', function contentFieldService($http) {

    var base_path = "/api/contentField";

    var service = {
        search: search,
        get: get,
        insert: insert,
        update: update,
        remove: remove,
        save: save
    };
    return service;

    // implementation    

    // search
    function search(contentType = '', verbose = null, skip = null, take = null) {
        var request = $http({
            method: "get",
            url: base_path + '/' + contentType + '/' + '?verbose=' + verbose + '&skip=' + skip + '&take=' + take
        });
        return request;
    }

    // get 
    function get(contentType, id) {
        var request = $http({
            method: "get",
            url: base_path + '/' + contentType + '/' + id
        });
        return request;
    }

    // insert
    function insert(contentType, item) {
        var request = $http({
            method: "post",
            url: base_path + '/' + contentType,
            data: item
        });
        return request;
    }

    // update 
    function update(contentType, item) {
        var request = $http({
            method: "put",
            url: base_path + '/' + contentType,
            data: item
        });
        return request;
    }

    // delete
    function remove(contecontentTypent_type, id) {
        var request = $http({
            method: "delete",
            url: base_path + '/' + contentType + '/' + id
        });
        return request;
    }

    // save
    function save(contentType, item) {
        if (item.id > 0) {
            return update(contentType, item);
        }
        else {
            return insert(contentType, item);
        }
    }

}]);
