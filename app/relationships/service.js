app.factory('relationshipService', ['$http', function relationshipService($http) {

    var base_path = "/api/relationship";

    var service = {
        search: search,
        get: get,
        insert: insert,
        update: update,
        remove: remove,
        save: save,
        save_relationship: save_relationship
    };
    return service;

    // implementation    

    // search
    function search(content_type_id = null) {
        var request = $http({
            method: "get",
            url: base_path + '?content_type_id=' + content_type_id
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

    // save relationship
    function save_relationship(item, content_item_id, content_type_id) {        
        var request = $http({
            method: "post",
            url: base_path + '?content_item_id=' + content_item_id + '&content_type_id=' + content_type_id,
            data: item
        });
        return request;
    }

}]);
