$('#btnGetStarships').click(function () {
    GetStarships($('#hdnCurrentPageNumber').val(), $(this).attr("data-url"));
});

$('#btnPreviousPage').click(function () {
    GetStarships($('#hdnPreviousPageNumber').val(), $(this).attr("data-url"));
});

$('#btnNextPage').click(function () {
    GetStarships($('#hdnNextPageNumber').val(), $(this).attr("data-url"));
});

function GetStarships(pageNumber, url) {
    var distance = $('#txtDistance').val();

    //console.log('Given distance is ' + distance);

    if (distance) {
        GetStarshipsWithResupplies(distance, pageNumber, url);
    }
    else {
        $('#statusMessage').text('Provide a valid number please!');
        $('#starshipOutputGrid').text('');
        $('input[id="txtDistance"]').addClass("border border-danger");
    }

    $('#starshipPagination').show();
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
    var el = document.getElementById("starshipOutputGrid");
    el.innerHTML = "";
    el.appendChild(table);
}

function GetStarshipsWithResupplies(distance, pageNumber, url) {
    $.ajax({
        cache: false,
        type: "POST",
        data: { distance, pageNumber },
        url: url
    })
        .done(function (responseData) {
            if (responseData.transResult.apiStatusCode == 200) {
                if (responseData.dataList[0].starships.Count != 0) {
                    GenerateTable(responseData.dataList[0].starships);
                    RefreshPaging(
                        responseData.dataList[0].previousPageNumber,
                        responseData.dataList[0].currentPageNumber,
                        responseData.dataList[0].nextPageNumber)
                }
                else {
                    $('#statusMessage').text('Starships not found!');
                    $('#starshipOutputGrid').text('');
                }
            }
            else {
                $('#statusMessage').text('Response invalid!');
                $('#starshipOutputGrid').text('');
            }
        })
        .fail(function () {
            $('#statusMessage').text('Retrieval failed!');
            $('#starshipOutputGrid').text('');
        });
}

function RefreshPaging(
    previousPageNumber,
    currentPageNumber,
    nextPageNumber) {
    $('#hdnPreviousPageNumber').val(previousPageNumber);
    $('#hdnCurrentPageNumber').val(currentPageNumber);
    $('#lblCurrentPageNumber').text(currentPageNumber);
    $('#hdnNextPageNumber').val(nextPageNumber);

    if (previousPageNumber == 0) {
        $('#btnPreviousPage').hide();
    }
    else {
        $('#btnPreviousPage').show();
    }

    if (nextPageNumber == 0) {
        $('#btnNextPage').hide();
    }
    else {
        $('#btnNextPage').show();
    }
}