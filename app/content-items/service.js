app.factory('contentItemService', ['$http', function contentItemService($http) {

    var base_path = "/api/content";

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
    function search(content_type = '', name = '', verbose = null, skip = null, take = null) {
        var request = $http({
            method: "get",
            url: base_path + '/' + content_type + '/' + '?name=' + name + '&verbose=' + verbose + '&skip=' + skip + '&take=' + take
        });
        return request;
    }

    // get 
    function get(content_type, id) {
        var request = $http({
            method: "get",
            url: base_path + '/' + content_type + '/' + id
        });
        return request;
    }

    // insert
    function insert(content_type, item) {
        var request = $http({
            method: "post",
            headers: {
                'Content-Type': "application/json"
            },
            url: base_path + '/' + content_type,
            data: item
        });
        return request;
    }

    // update 
    function update(content_type, item) {
        var request = $http({
            method: "put",
            url: base_path + '/' + content_type,
            headers: {
                'Content-Type': "application/json"
            },
            data: item
        });
        return request;
    }

    // delete
    function remove(content_type, id) {
        var request = $http({
            method: "delete",
            url: base_path + '/' + content_type + '/' + id
        });
        return request;
    }

    // save
    function save(content_type, item) {
        if (item.id > 0) {
            return update(content_type, item);
        }
        else {
            return insert(content_type, item);
        }
    }

}]);
