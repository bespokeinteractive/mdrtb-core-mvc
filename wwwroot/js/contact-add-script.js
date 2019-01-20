$(function() {
    jq('#submit_form').click(function(){
        var count = jq('#Contact_Person_Name').val().split(' ').length;
        var error = 0;
        
        if (count < 2){
            setTimeout(function(){
                Materialize.toast('<span>Specify atleast two Names</span><a class="btn-flat yellow-text right" href="#!">Close<a>', 3000);
            }, 500);
            error++;
        }

        if(jq('#DateOfBirth').val() == ""){
            setTimeout(function(){
                Materialize.toast('<span>Specify Date of Birth</span><a class="btn-flat yellow-text right" href="#!">Close<a>', 3000);
            }, 1000);
            error++;
        }

        if(jq('#Contact_Person_Address_Address').val() == ""){
            setTimeout(function(){
                Materialize.toast('<span>Specify Physical Address/Landmark</span><a class="btn-flat yellow-text right" href="#!">Close<a>', 3000);
            }, 1500);
            error++;
        }

        if (error > 0) {
            return;
        }

        jq('form').submit();
    });

    jq('a.next-btn').click(function(){
        var index = jq(this).data('index');
        jq('.collapsible').collapsible('open', index);
    }); 

    jq('#DateOfBirth').on('blur', function(){
        jq.ajax({
            dataType: "text",
            url: '/Patient/GetBirthdateFromString',
            data: {
                "value": jq(this).val()
            },
            success: function(results) {
                jq('#DateOfBirth').val(results);
            },
            error: function(xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    });

});