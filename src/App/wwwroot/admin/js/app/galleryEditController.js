var galleryEditController = function (dataService) {
    function load() {
        var galleryId = window.location.search.replace('?id=', '');
        dataService.get("api/galleryedit?id=" + galleryId, loadCallback, fail);
    }

    function loadCallback(data) {
        $('#galleryImageList').empty();
        $.each(data, function (index) {            
            var galleryImage = data[index];
            var img = galleryImage.path;

            var tag = '<div class="post-grid-col">' +
                '	<div class="post-grid-item">' +
                '		<a href="javascript: void(0);" class="item-link" style="background-image:url(' + img + ');"><div class="item-title mt-auto">&nbsp;</div></a>' +
                '		<div class="item-info d-flex align-items-center">' +       
                '           <span class="item-date mr-auto"></span>' +
                '			<a href="javascript: void(0);" class="btn-unstyled item-favorite ml-3" data-tooltip="Delete" title="delete" data-original-title="profile">' +
                '				<i class="fas fa-trash" style="color: #ff6666;"></i>' +
                '			</a>' +
                '		</div>' +
                '	</div>' +
                '</div>';

            $('#galleryImageList').append(tag);

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