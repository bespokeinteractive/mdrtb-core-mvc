var fac = [];
var idx = 0;

jq(function() {
    jq('.modal').modal();
    jq('select').material_select();

    jq('#queue-table').DataTable({
        "displayLength": 25,
    });

    jq('#queue-table_filter').on('click', 'i', function() {
        jq('form')[0].reset();
        jq('#Facility_Id').val('0');
        jq('#add-edit-modal').modal('open');
    });

    jq('#queue-table td').on('click', 'i.blue-text', function() {
        idx = jq(this).data('idnt');
        jq('form')[0].reset();
        
        jq.ajax({
            dataType: "json",
            url: '/Account/GetFacility',
            data: {
                "idnt": idx
            },
            success: function(data) {
                jq('#Facility_Id').val(data.id);
                jq('#Facility_Prefix').val(data.prefix);
                jq('#Facility_Name').val(data.name);
                jq('#Facility_Description').val(data.description);

                SelectHandler(jq('#Facility_Region_Id'), data.region.id);
                SelectHandler(jq('#Facility_Agency_Id'), data.agency.id);

                Materialize.updateTextFields();

                jq('#add-edit-modal').modal('open');
            },
            error: function(xhr, ajaxOptions, thrownError) {
                Materialize.toast('<span>' + xhr.status + ', ' + thrownError + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            }
        });
    });

    jq('#queue-table td').on('click', 'i.red-text', function() {
        idx = jq(this).data('idnt');
        var dlgs = jq('#confirm-delete-modal');
        
        jq.ajax({
            dataType: "json",
            url: '/Account/GetFacility',
            data: {
                "idnt": idx
            },
            success: function(data) {
                jq('#Facility_Id').val(data.id);
                dlgs.find('p.records').html('Confirm deleting facility ' + data.prefix + ', titled ' + data.name + '?' );

                if (data.count > 0){
                    Materialize.toast('<span>Facility has patient data and can not be removed</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
                    return;
                }

                dlgs.modal('open');
            },
            error: function(xhr, ajaxOptions, thrownError) {
                Materialize.toast('<span>' + xhr.status + ', ' + thrownError + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            }
        });
    });

    jq('<i class="material-icons medium blue-text right">person_add</i>').insertBefore(jq("#queue-table_filter label"));

    jq('a.btn-post').click(function(){
        if (jq('#Facility_Prefix').val().trim().length < 5){
            Materialize.toast('<span>Invalid Format for Facility Prefix</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        if(jq('#Facility_Name').val().trim().length < 3){
            Materialize.toast('<span>Facility Name must atleast be 3 characters long</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        jq.ajax({
            dataType: "text",
            url: '/Account/CheckIfFacilityExists',
            data: {
                "fac_idnt": jq('#Facility_Id').val(),
                "fac_name": jq('#Facility_Name').val(),
                "fac_prefix": jq('#Facility_Prefix').val(),
            },
            success: function(results) {
                if (results != 0){
                    Materialize.toast('<span>Facility with the same Name or Prefix already exists</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
                    return;
                }
                else {
                    jq('form').submit();
                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                Materialize.toast('<span>' + xhr.status + ', ' + thrownError + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            }
        });
    });

    jq('a.btn-delete').click(function(){
        jq.ajax({
            dataType: "text",
            url: '/Account/DeleteFacility',
            data: {
                "idnt": idx
            },
            success: function(results) {
                Materialize.toast('<span>Successfully removed Facility</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
                window.location.href = "/administrator/facilities";
            },
            error: function(xhr, ajaxOptions, thrownError) {
                Materialize.toast('<span>' + xhr.status + ', ' + thrownError + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            }
        });
    });

    function SelectHandler(obj, values) {
        var selectClass = 'selected active';
        var text = obj.val(values).find('option:selected').text();
        var $input = obj.prevAll('input.select-dropdown');
        var $ul = obj.prevAll('ul.select-dropdown');
        var $li = $ul.find('li:contains(' + text + ')');
        $ul.children().removeClass(selectClass);
        $li.siblings().removeClass(selectClass).end().addClass(selectClass);
        $input.val(text);
    }
});