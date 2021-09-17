app.factory('visualizerService', ['$http', function visualizerService($http) {

    var base_path = "/api/visualizer";
        
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
    function search(name = '', verbose = null, skip = null, take = null) {
        var request = $http({
            method: "get",
            url: base_path + '?name=' + name + '&verbose=' + verbose + '&skip=' + skip + '&take=' + take
        });
        return request;
    }

    // get 
    function get(module_id) {
        var request = $http({
            method: "get",
            url: base_path + '?module_id=' + module_id
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
