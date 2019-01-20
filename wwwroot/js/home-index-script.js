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

});