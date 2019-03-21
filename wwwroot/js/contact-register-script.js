jq(function() {
    jq('a.export').on('click', function(event){
        Materialize.toast('<span>Your download will start shortly</span><a class="btn-flat yellow-text" href="#!">Close<a>', 3000);

        var table = jq('#contact-register');
        var args = [table, title];
        ExportTableToCSV.apply(this, args);
    });

    function ExportTableToCSV($table, filename) {
        var $rows = $table.find('tr.data'),
          tmpColDelim = String.fromCharCode(11), // vertical tab character
          tmpRowDelim = String.fromCharCode(0), // null character

          // actual delimiter characters for CSV format
          colDelim = '","',
          rowDelim = '"\r\n"',

          // Grab text from table into CSV formatted string
          csv = '"' + $rows.map(function(i, row) {
            var $row = $(row),
              $cols = $row.find('td');

            return $cols.map(function(j, col) {
              var $col = $(col),
                text = $col.text();

              return text.replace(/"/g, '""'); // escape double quotes

            }).get().join(tmpColDelim);

          }).get().join(tmpRowDelim)
          .split(tmpRowDelim).join(rowDelim)
          .split(tmpColDelim).join(colDelim) + '"';

        if (false && window.navigator.msSaveBlob) {
          var blob = new Blob([decodeURIComponent(csv)], {
            type: 'text/csv;charset=utf8'
          });

          window.navigator.msSaveBlob(blob, filename);

        } else if (window.Blob && window.URL) {
          var blob = new Blob([csv], {
            type: 'text/csv;charset=utf-8'
          });
          var csvUrl = URL.createObjectURL(blob);

          $(this)
            .attr({
              'download': filename,
              'href': csvUrl
            });
        } else {
          var csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(csv);

          $(this)
            .attr({
              'download': filename,
              'href': csvData,
              'target': '_blank'
            });
        }
    }
});