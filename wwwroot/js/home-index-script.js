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

});