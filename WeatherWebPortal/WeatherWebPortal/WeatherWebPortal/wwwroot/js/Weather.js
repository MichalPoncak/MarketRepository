$('#btnGetWeather').click(function () {
    GetWeather($(this).attr("data-url"));
});

function GetWeather(url) {
    var WOEIDLocation = $('#txtLocation').val();

    console.log('location = ' + WOEIDLocation);

    if (WOEIDLocation) {
        $.ajax({
            cache: false,
            type: "POST",
            data: { WOEIDLocation },
            url: url
        })
            .done(function (responseData) {
                if (responseData.transResult.apiStatusCode == 200) {
                    if (responseData.dataList.Count != 0) {
                        GenerateTable(responseData.dataList);
                    }
                    else {
                        $('#statusMessage').text('Weather forecast not found!');
                        $('#weatherOutputGrid').text('');
                    }
                }
                else {
                    $('#statusMessage').text('Response invalid!');
                    $('#weatherOutputGrid').text('');
                }
            })
            .fail(function () {
                $('#statusMessage').text('Retrieval failed!');
                $('#weatherOutputGrid').text('');
            });
    }
    else {
        $('#statusMessage').text('Provide a valid location woeid please!');
        $('#weatherOutputGrid').text('');
        $('input[id="txtLocation"]').addClass("border border-danger");
    }
}

function GenerateTable(list) {
    // source: https://www.geeksforgeeks.org/how-to-convert-json-data-to-a-html-table-using-javascript-jquery/
    var cols = [];

    for (var i = 0; i < list.length; i++) {
        for (var k in list[i]) {
            if (cols.indexOf(k) === -1) {

                // Push all keys to the array
                cols.push(k);
            }
        }
    }

    // Create a table element
    var table = document.createElement("table");
    table.className = "table";

    // Create table row tr element of a table
    var tr = table.insertRow(-1);

    for (var i = 0; i < cols.length; i++) {

        // Create the table header th element
        var theader = document.createElement("th");
        theader.innerHTML = cols[i];

        // Append columnName to the table row
        tr.appendChild(theader);
    }

    // Adding the data to the table
    for (var i = 0; i < list.length; i++) {

        // Create a new row
        trow = table.insertRow(-1);
        for (var j = 0; j < cols.length; j++) {
            var cell = trow.insertCell(-1);

            // Inserting the cell at particular place
            cell.innerHTML = list[i][cols[j]];
        }
    }

    // Add the newely created table containing json data
    var el = document.getElementById("weatherOutputGrid");
    el.innerHTML = "";
    el.appendChild(table);
}