jq(function() {
    jq('a.btn-search').click(function(){
        var searchString = jq('#search_string').val().trim();

        if (searchString == '') {
            window.location.href = "/patients/search";
        }
        else {
            window.location.href = "/patients/search?q=" + searchString;
        }
    }); 

    jq('li.collection-item.dismissable p.contacts').click(function(){
        window.location.href = jq(this).data('url');
    });

    jq('#search_string').keypress(function (e) {
        var key = e.which;
        if(key == 13) {
            jq('a.btn-search').click();;
        }
   });
});