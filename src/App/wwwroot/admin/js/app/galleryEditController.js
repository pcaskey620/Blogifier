var galleryEditController = function (dataService) {
    function load() {
        var galleryId = window.location.search.replace('?id=', '');
        dataService.get("api/galleryedit?id=" + galleryId, loadCallback, fail);
    }

    function loadCallback(data) {
        $.each(data, function () {
            var galleryImage = data[index];
        });
    }

    function add() {

    }

    function addCallback() {

    }

    return {
        load: load,
        add: add
    }
}(DataService);

function openFileMgr() {    
    fileManagerController.open(insertImageCallback);
}

function insertImageCallback(data) {
    var output = data + '](' + webRoot + data + ')';
}