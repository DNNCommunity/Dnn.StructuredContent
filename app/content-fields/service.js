app.factory('contentFieldService', ['$http', function contentFieldService($http) {

    var base_path = siteRoot + "api/contentField";

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
    function search(contentTypeUrlSlug = '', verbose = null, skip = null, take = null) {
        var request = $http({
            method: "get",
            url: base_path + '/' + contentTypeUrlSlug + '/' + '?verbose=' + verbose + '&skip=' + skip + '&take=' + take
        });
        return request;
    }

    // get 
    function get(contentTypeUrlSlug, id) {
        var request = $http({
            method: "get",
            url: base_path + '/' + contentTypeUrlSlug + '/' + id
        });
        return request;
    }

    // insert
    function insert(contentTypeUrlSlug, item) {
        var request = $http({
            method: "post",
            url: base_path + '/' + contentTypeUrlSlug,
            data: item
        });
        return request;
    }

    // update 
    function update(contentTypeUrlSlug, item) {
        var request = $http({
            method: "put",
            url: base_path + '/' + contentTypeUrlSlug,
            data: item
        });
        return request;
    }

    // delete
    function remove(contentTypeUrlSlug, id) {
        var request = $http({
            method: "delete",
            url: base_path + '/' + contentTypeUrlSlug + '/' + id
        });
        return request;
    }

    // save
    function save(contentTypeUrlSlug, item) {
        if (item.id > 0) {
            return update(contentTypeUrlSlug, item);
        }
        else {
            return insert(contentTypeUrlSlug, item);
        }
    }

}]);
