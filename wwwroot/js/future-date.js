// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    //document.getElementById('ActiveDepartureDate').valueAsDate = new Date();
jQuery.validator.addMethod("futuredate", function (value, element, param) {
    if (value === '') return false;

    var dateToCheck = new Date(value);
    if (isNaN(dateToCheck)) return false;

    var maxYears = Number(param);

    var today = new Date();
    today.setHours(0, 0, 0, 0);

    var maxDate = new Date();
    maxDate.setFullYear(maxDate.getFullYear() + maxYears);

    return (dateToCheck > today && dateToCheck <= maxDate);
});

jQuery.validator.unobtrusive.adapters.addSingleVal("futuredate", "years");