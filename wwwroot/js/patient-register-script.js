$(function() {
    jq('#left-menu li.menu-item').click(function(){
        if (jq(this).is(':last-child') || jq(this).hasClass('selected')){
            return
        }

        jq(this).addClass('selected');

        if(jq(this).is(':first-child')){
            jq('.item-2').removeClass('selected');
            jq('.tab-1').show();
            jq('.tab-2').hide();
        } 
        else{
            jq('.item-1').removeClass('selected');
            jq('.tab-1').hide();
            jq('.tab-2').show();
        }
    });

    jq('.input-field.next a').click(function(){
        jq('.item-1').removeClass('selected');
        jq('.item-2').addClass('selected');
        jq('.tab-1').hide();
        jq('.tab-2').show();
    });

    jq('.input-field.prev a').click(function(){
        jq('.item-1').addClass('selected');
        jq('.item-2').removeClass('selected');
        jq('.tab-1').show();
        jq('.tab-2').hide();
    });

    jq('.input-field.post a').click(function(){
        var count = jq('#Patient_Person_Name').val().split(' ').length;
        if (count < 2){
            Materialize.toast('<span>Specify atleast two Names</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        if(jq('#DateOfBirth').val() == ""){
            Materialize.toast('<span>Specify Date of Birth</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        if(jq('#Address_Address').val() == ""){
            Materialize.toast('<span>Specify Physical Address/Landmark</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);
            return;
        }

        jq('form').submit();

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