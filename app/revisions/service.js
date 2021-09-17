app.factory('revisionService', ['$http', function contentFieldTypeService($http) {

    var base_path = "/api/revision";
        
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
    function search(content_type_id = null, item_id = null, verbose = null, skip = null, take = null) {
        var request = $http({
            method: "get",
            url: base_path + '?content_type_id=' + content_type_id + '&item_id=' + item_id + '&verbose=' + verbose + '& skip=' + skip + '& take=' + take
        });
        return request;
    }

    // get 
    function get(id) {
        var request = $http({
            method: "get",
            url: base_path + '/' + id
        });
        return request;
    }

    // insert
    function insert(item) {
        var request = $http({
            method: "post",
            url: base_path,
            data: item
        });
        return request;
    }

    // update 
    function update(item) {
        var request = $http({
            method: "put",
            url: base_path,
            data: item
        });
        return request;
    }

    // delete
    function remove(id) {
        var request = $http({
            method: "delete",
            url: base_path + '/' + id
        });
        return request;
    }

    // save
    function save(item) {
        if (item.id > 0) {
            return update(item);
        }
        else {
            return insert(item);
        }
    }

}]);
