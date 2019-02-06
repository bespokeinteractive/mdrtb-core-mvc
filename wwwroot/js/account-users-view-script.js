var fac = [];

jq(function() {
    jq('a.reset-password').click(function(){
        ResetPassword()
    });

    jq('a.disable-account').click(function(){
        var txt = jq(this).text();
        var opt = 0;
        var msg = "";

        if (txt == 'Disable Account') {
            opt = 0;
            msg = "disabled";
            jq(this).text('Enable Account');
        }
        else{
            opt = 1;
            msg = "enabled";
            jq(this).text('Disable Account')
        } 

        EnableAccount(opt, msg);
    });
});

function ResetPassword(){
    jq.ajax({
        dataType: "text",
        url: '/Account/ResetPassword',
        data: {
            "usr_idnt": idnt
        },
        success: function(results) {
            Materialize.toast('<span>Successfully reset password for ' + name + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
        },
        error: function(xhr, ajaxOptions, thrownError) {
            Materialize.toast('<span>' + xhr.status + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            Materialize.toast('<span>' + thrownError + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
        }
    });
}

function EnableAccount(opts, msgs){
    jq.ajax({
        dataType: "text",
        url: '/Account/EnableAccount',
        data: {
            "usr_idnt": idnt,
            "usr_opts": opts,
        },
        success: function(results) {
            Materialize.toast('<span>Successfully '+ msgs + ' account for ' + name + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
        },
        error: function(xhr, ajaxOptions, thrownError) {
            Materialize.toast('<span>' + xhr.status + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            Materialize.toast('<span>' + thrownError + '</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
        }
    });
}