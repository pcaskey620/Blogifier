var galleryController = function (dataService) {

    function load(page) {
        dataService.get("api/gallery", loadCallback, fail);
    }

    function loadCallback(data) {
        $('#galleryList').empty();
        $.each(data, function (index) {
            var gallery = data[index];
            var img = gallery.directory + "/cover.jpg";
            var galleryLink = webRoot + 'admin/gallery/edit?id=' + gallery.id;

            var tag = '<div class="post-grid-col">' +
                '	<div class="post-grid-item">' +
                '		<a href="' + galleryLink + '" class="item-link" style="background-image:url(' + img + ');"><div class="item-title mt-auto">&nbsp;</div></a>' +
                '		<div class="item-info d-flex align-items-center">' +
                '			<span class="item-date mr-auto">' + gallery.title + '</span>' +
                '			<a href="' + galleryLink + '" class="btn-unstyled item-favorite ml-3" data-tooltip="" title="" data-original-title="profile">' +
                '				<i class="fas fa-external-link-square-alt"></i>' +
                '			</a>' +
                '		</div>' +
                '	</div>' +
                '</div>';

            $('#galleryList').append(tag);
        });
    }

    function add() {
        var frm = $('#frmCollection');
        frm.validate();
        if (frm.valid() === false) {
            return false;
        } 
        var obj = {
            title: $('#txtTitle').val()
        };

        dataService.post("api/gallery", obj, addCallback, fail);
    }

    function addCallback(data) {
        toastr.success('Completed');
        $('#dlgCollection').modal('hide');
        load(1);
    }

    return {
        load: load,
        add: add
    };

}(DataService);