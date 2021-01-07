var galleryController = function (dataService) {

    function load(page) {
        $('#btnAddGallery').click(add);
        $('#btnEditGallery').click(edit);

        $('#frmCollection').validate({
            rules: {
                txtTitle: {
                    required: true
                }
            }
        });

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
                '		<a href="' + galleryLink + '" class="item-link" style="background-image:url(https://aajoaihlwo.cloudimg.io/v7/https://www.parksandwils.com' + img + '?h=360&w=640&gravity=auto);"><div class="item-title mt-auto">&nbsp;</div></a>' +
                '		<div class="item-info d-flex align-items-center">' +
                '			<span class="item-date mr-auto">' + gallery.title + '</span>' +
                '			<a id="gallery-' + gallery.id + '" href="javascript: galleryController.openEditGallery(&quot;' + gallery.id + '&quot;);" class="btn-unstyled item-favorite ml-3" data-tooltip="" title="" data-original-title="profile">' +
                '				<i class="fas fa-pencil-alt" style="color: grey;"></i>' +
                '			</a>' +
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

    function edit() {
        var frm = $('#frmCollection');
        frm.validate();
        if (frm.valid() === false) {
            return false;
        }
        var obj = {
            title: $('#txtTitle').val(),
            season: $('#drpCategory option:selected').text(),
            id: $('#hiddenGalleryId').val() 
        };

        dataService.post("api/gallery", obj, editCallback, fail);
    }

    function editCallback(data) {
        toastr.success('Completed');
        $('#dlgCollection').modal('hide');
        load(1);
    }

    function openAddGallery() {
        $('#frmCollection').validate().resetForm();

        $('#hdrSettings').html('Add Collection');

        $('#hiddenGalleryId').val('');

        $('#btnAddGallery').show();
        $('#btnEditGallery').hide();

        $('#dlgCollection').modal();

        return false;
    };

    var openEditGallery = function (id) {
        $('#frmCollection').validate().resetForm();

        $('#hdrSettings').html('Edit Collection');

        $('#hiddenGalleryId').val(id);

        $('#btnEditGallery').show();
        $('#btnAddGallery').hide();        

        dataService.get("api/galleryedit/id/" + id, openEditGalleryCallback, fail);

        return false;
    };

    var openEditGalleryCallback = function (data) {
        $('#txtTitle').val(data.title);
        $('#drpCategory').val(data.season);

        $('#dlgCollection').modal();
    };

    return {
        load: load,
        add: add,
        edit: edit,
        openAddGallery: openAddGallery,
        openEditGallery: openEditGallery
    };

}(DataService);