var fac = [];

jq(function() {
    jq('#User_Role_Id').change(function(){
        var index = jq(this).val();

        if (index == 3){
            jq('#regionroles').show();
            jq('#agencyroles').hide();
            jq('#facilityroles').hide();
        }
        else if (index == 4){
            jq('#agencyroles').show();
            jq('#regionroles').hide();
            jq('#facilityroles').hide();
        }
        else if (index == 5 || index == 6){
            jq('#facilityroles').show();
            jq('#agencyroles').hide();
            jq('#regionroles').hide();
        }
        else {
            jq('#agencyroles').hide();
            jq('#regionroles').hide();
            jq('#facilityroles').hide();
        }
    }).change();

    jq('input.facilities').change(function(){
        var itm = jq(this).data('idnt')
        if (jq(this).is(':checked')){
            fac.push(itm);
        }
        else {
            fac.splice($.inArray(itm, fac), 1);
        }

        jq('#Facility').val(fac.toString())
    });

    jq('a.btn-post').click(function(){
        var count = jq('#User_Name').val().split(' ').length;
        if (count < 2){
            Materialize.toast('<span>Specify atleast two Names</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if(!regex.test(jq('#User_Email').val())) {
            Materialize.toast('<span>Specify atleast a Valid Email Address</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        if(jq('#User_Username').val().length < 5){
            Materialize.toast('<span>Username must atleast be 4 characters long</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        if ((jq('#User_Role_Id').val() == 5 || jq('#User_Role_Id').val() == 6) && jq('#Facility').val() == "") {
            Materialize.toast('<span>No Facilities Selected for the User</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        jq.ajax({
            dataType: "text",
            url: '/Account/CheckIfUserExists',
            data: {
                "usr_idnt": idnt,
                "usr_name": jq('#User_Username').val(),
            },
            success: function(results) {
                if (results != 0){
                    Materialize.toast('<span>User with the same username already exists</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
                    return;
                }
                else {
                    jq('form').submit();
                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                Materialize.toast('<span>' + xhr.status + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
                Materialize.toast('<span>' + thrownError + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            }
        });
    });

    jq('input[type=checkbox].facilities').each(function () {
        var itm = jq(this).data('idnt')
        if (jq(this).is(':checked')){
            fac.push(itm);
        }

        jq('#Facility').val(fac.toString())
    });
});