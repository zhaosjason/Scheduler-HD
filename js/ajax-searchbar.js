/*jslint  browser: true, white: true, plusplus: true */
/*global $: true */

$(function () {
    'use strict';

    // Load names & usernames, then initialize plugin:
    $.ajax({
        url: 'services/ajax-searchbar-data.txt',
        dataType: 'json'
    }).done(function (source) {

        var usersArray = $.map(source, function (value, key) { return { value: value, data: key }; }),
            users = $.map(source, function (value) { return value; });

        // Initialize autocomplete with local lookup:
        $('#tbSearch').autocomplete({
            lookup: usersArray,
            onSelect: function (suggestion) {
                //$('#tbSearch-data').html(suggestion.data);
            }
        });
        
    });

});