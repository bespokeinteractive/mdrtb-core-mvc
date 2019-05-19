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
        if (validate(jq('#OutcomeDate').val()) == false){
            Materialize.toast('<span>Invalid date format for Outcome Date</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        jq('#update-outcome-form').submit();
    });

    jq('#update-regimen-modal a.btn-post').click(function(){
        if (validate(jq('#RegimenDate').val()) == false){
            Materialize.toast('<span>Invalid date format for Regimen Date</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        jq('#update-regimen-form').submit();
    });

    jq('#transfer-patient-modal a.btn-post').click(function(){
        if (validate(jq('#TransferDate').val()) == false){
            Materialize.toast('<span>Invalid date format for Regimen Date</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        jq('#transfer-patient-form').submit();
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

        var outcome = jq(this).data('outcome');
        if (outcome != 0){
            Materialize.toast('<span>Action applicable to only Active Patient</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }
    });

    jq('.datepicker').pickadate({
        container: 'body' //this will append to body
    });
});

function validate(date){
    if (date.length != 10){
        return false;
    }

    var dateRegex = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
    return dateRegex.test(date);
}