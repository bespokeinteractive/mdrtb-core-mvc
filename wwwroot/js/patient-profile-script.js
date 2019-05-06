jq(function() {
    jq('.modal').modal();
    jq('select').material_select();

    jq('#edit-patient-modal a.btn-post').click(function(){
        var count = jq('#Patient_Person_Name').val().split(' ').length;
        if (count < 2){
            Materialize.toast('<span>Specify atleast two Names</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        if(jq('input.date-of-birth').val() == ""){
            Materialize.toast('<span>Specify Date of Birth</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        if(jq('#Patient_Person_Address_Address').val() == ""){
            Materialize.toast('<span>Specify Physical Address/Landmark</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        jq('#edit-patient-form').submit();
    });

    jq('#update-outcome-modal a.btn-post').click(function(){
        jq('#update-outcome-form').submit();
    });
    
    jq('a.btn-add-contacts').click(function(){
        window.location.href = "/contacts/add?p=" + xProg;
    });

    jq('a.view-contacts').click(function(){
        jq('ul.tabs').tabs('select_tab', 'contacts');
    });

    jq('a.modal-alerts').click(function(){
        var dead = jq(this).data('dead');
        if (dead == 1){
            Materialize.toast('<span>Action cannot be performed on a deceased Patient</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }
    });
});