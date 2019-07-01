var galleryEditController = function (dataService) {
    function load() {

    }

    function loadCallback() {

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
    debugger;
    var output = data + '](' + webRoot + data + ')';

}