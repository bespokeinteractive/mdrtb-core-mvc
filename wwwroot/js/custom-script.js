/*================================================================================
	Item Name: Materialize - Material Design Admin Template
	Version: 4.0
	Author: PIXINVENT
	Author URL: https://themeforest.net/user/pixinvent/portfolio
================================================================================

NOTE:
------
PLACE HERE YOUR OWN JS CODES AND IF NEEDED.
WE WILL RELEASE FUTURE UPDATES SO IN ORDER TO NOT OVERWRITE YOUR CUSTOM SCRIPT IT'S BETTER LIKE THIS. */

var jq = jQuery;

jq(function() {
    jq('input.date-of-birth').on('blur', function(){
        jq.ajax({
            dataType: "text",
            url: '/Patient/GetBirthdateFromString',
            data: {
                "value": jq(this).val()
            },
            success: function(results) {
                jq('input.date-of-birth').val(results);
            },
            error: function(xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    });
});