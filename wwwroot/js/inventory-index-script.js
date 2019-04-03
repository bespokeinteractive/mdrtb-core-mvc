jq(function() {
    jq('.modal').modal();

    jq('div.btn-change').click(function() {
        jq('#change-facility-modal').modal('open');
    });

    jq('a.btn-post').click(function(){
        window.location.href = "/inventory/?fac=" + jq('#Active_Id').val();
    });
});