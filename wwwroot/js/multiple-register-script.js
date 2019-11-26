$(function() {
    $('.modal').modal();

    jq('a.btn-patient-bulk-row').click(function(){
        jq("#patients-table tbody tr.rows").each(function() {
            if (jq(this).hasClass('hide')){
                jq(this).removeClass('hide');
                return false;
            }
        });
    });

    jq('a.remove-patient-row').click(function(){
        jq(this).closest('tr').remove();

        jq("#patients-table tbody tr.rows").each(function(i, row) {
            jq(this).find('td:eq(0)').text(eval(i+1) + '.');
        });
    });

    jq('a.btn-save').click(function(){
        jq("#patients-table tbody tr.rows").each(function(i, row) {
            if (jq(this).hasClass('hide')){
                return;
            }

            if (jq(this).find('td:eq(2) input').val().trim() == '') {
                Materialize.toast('<span>Patient Name in row ' + eval(i+1) + ' cannot be blank</span><a class="btn-flat yellow-text" href="#!">Correct that</a>', 3000)
                err_count++;
                return false;
            }

            var count = jq(this).find('td:eq(2) input').val().trim().split(' ').length;
            if (count < 2){
                Materialize.toast('<span>Patient Name in row ' + eval(i+1) + ' must have atleast two names</span><a class="btn-flat yellow-text" href="#!">Correct that</a>', 3000)
                err_count++;
                return false;
            }

            if (jq(this).find('td:eq(3) input').val().trim() == '') {
                Materialize.toast('<span>Patient Age in row ' + eval(i+1) + ' cannot be blank</span><a class="btn-flat yellow-text" href="#!">Correct that</a>', 3000)
                err_count++;
                return false;
            }
        });

        jq('#patient-registration-form').submit();
    });

    jq('i.change-facility').click(function(){
        jq('#change-facility-modal').modal('open');
    });

    jq('#change-facility-modal .modal-footer .modal-post').click(function() {
        jq('input.facility-idnt').val(jq('#change-facility-modal select').val());
        jq('span.facility-name').text(jq('#change-facility-modal input').val());
    });
});