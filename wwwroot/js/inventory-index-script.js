jq(function() {
    jq('.modal').modal();

    jq('div.btn-change').click(function() {
        jq('#change-facility-modal').modal('open');
    });

    jq('a.btn-post').click(function(){
        window.location.href = "/inventory/?fac=" + jq('#Active_Id').val();
    });

    jq('div.get-stock a').click(function(){
        GetInventoryDrugs();
    });
});

String.prototype.toAccounting = function() {
    var str =  parseFloat(this).toFixed(0).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    if (str.charAt(0) == '-'){
        return '(' + str.substring(1,40) + ')';
    }
    else {
        return str;
    }
};

function GetInventoryDrugs(){
    jq.ajax({
        dataType: "json",
        url: '/Inventory/GetInventoryDrugs',
        data: {
            "facl":     facl,
            "catg":     jq('#stock_catg').val(),
            "filter":   jq('#stock_filter').val()
        },
        beforeSend: function() {
            jq('body').removeClass('loaded');
        },
        success: function(results) {
            jq('#stock-table tbody').empty();
            jq('#stock-table tfoot').empty();

            var available = 0.0;

            jq.each(results, function(i, item) {
                available += item.available;

                var row = '<tr>';
                row += '<td>' + (i+1) + '</td>';
                row += '<td>' + item.drug.name.toUpperCase() + '</td>';
                row += '<td>' + item.drug.category.name.toUpperCase() + '</td>';
                row += '<td>' + item.drug.formulation.name + ' ' + item.drug.formulation.dosage + '</td>';
                row += '<td>' + item.available.toString().toAccounting() + '</td>';
                row += '<td>' + item.reorder.toString().toAccounting() + '</td>';
                row += '<td>N/A</td>';
                row += '<td><a class="material-icons tiny-box grey-text right">border_color</a></td>';
                row += '</tr>';

                jq('#stock-table tbody').append(row);
            })

            if(results.length == 0){
                jq('#stock-table tbody').append('<tr><td>&nbsp;</td><td colspan="3">NO DRUGS FOUND</td><td>0.00</td><td>0.00</td><td>N/A</td></tr>');
            }

            var footr = '<tr><td>&nbsp;</td><th colspan="3">SUMMARY</th>';
            footr += '<th>' + available.toString().toAccounting() + '.00</th>';
            footr += '<th>&mdash;</td><th>&mdash;</td></tr>';

            jq('#stock-table tfoot').append(footr);
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        },
        complete: function() {
            $('body').addClass('loaded');
        }
    });
}