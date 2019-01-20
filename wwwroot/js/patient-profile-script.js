jq(function() {
    jq('a.btn-add-contacts').click(function(){
        window.location.href = "/contacts/add?p=" + xProg;
    });
});