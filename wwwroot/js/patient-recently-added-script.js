var fac = [];

jq(function() {
    jq('a.get-patients').click(function(){
        GetRecentlyAdded();
    }).click();

    jq('a.get-contacts').click(function(){
        GetRecentContacts();
    });

    jq('li.contacts').click(function(){
        if(jq(this).data('loaded') == 0){
            GetRecentContacts();
            jq(this).data('loaded', 1);
        }
    });
});

function GetRecentlyAdded(){
    jq.ajax({
        dataType: "json",
        url: '/Patient/GetRecentlyAdded',
        data: {
            "start": jq('#patients-start-date').val(),
            "stops": jq('#patients-stops-date').val(),
            "filter": jq('#patients-filter').val()
        },
        beforeSend: function() {
            jq('body').removeClass('loaded');
        },
        success: function(results) {
            jq('#patients-table tbody').empty();
            jq('#patients-table tfoot').empty();

            jq.each(results, function(i, ps) {
                var row = "<tr>";
                row += "<td>" + (i+1) + "</td>";
                row += "<td>" + ps.program.tbmuNumber + "</td>";
                row += "<td><a class='blue-text' href='/patients/profile/" + ps.patient.uuid + "'>" + ps.patient.person.name + "</a></td>";
                row += "<td>" + ps.age + "</td>";
                row += "<td>" + ps.patient.person.gender + "</td>";
                row += "<td>" + ps.facility + "</td>";
                row += "<td>" + ps.addedOn + "</td>";
                row += "<td>" + ps.status + "</td>";
                row += "<td>&nbsp;</td>";
                row += "</tr>";

                jq('#patients-table tbody').append(row);
            })

            if(results.length == 0){
                jq('#patients-table tbody').append('<tr><td colspan="9" style="padding-left: 20px">NO RECENT PATIENTS FOUND</td></tr>');
            }
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

function GetRecentContacts(){
    jq.ajax({
        dataType: "json",
        url: '/Patient/GetRecentContacts',
        data: {
            "start": jq('#contacts-start-date').val(),
            "stops": jq('#contacts-stops-date').val(),
            "filter": jq('#contacts-filter').val()
        },
        beforeSend: function() {
            jq('body').removeClass('loaded');
        },
        success: function(results) {
            jq('#contacts-table tbody').empty();
            jq('#contacts-table tfoot').empty();

            jq.each(results, function(i, ct) {
                var row = "<tr>";
                row += "<td>" + (i+1) + "</td>";
                row += "<td>" + ct.identifier + "</td>";
                row += "<td><a class='blue-text' href='/contacts/" + ct.uuid + "'>" + ct.person.name + "</a></td>";
                row += "<td>" + ct.age + "</td>";
                row += "<td>" + ct.person.gender + "</td>";
                row += "<td>" + ct.index.patient.person.name + "</td>";
                row += "<td>" + ct.addedOn.substring(0,10) + "</td>";
                row += "<td>" + ct.status.name + "</td>";
                row += "<td>&nbsp;</td>";
                row += "</tr>";

                jq('#contacts-table tbody').append(row);
            })

            if(results.length == 0){
                jq('#contacts-table tbody').append('<tr><td colspan="9" style="padding-left: 20px">NO RECENT CONTACTS FOUND</td></tr>');
            }
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