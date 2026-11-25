// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// To display as DataTable lib
$(document).ready(function () {

    $('.data-table').DataTable({
        scrollX: true
    });

    $('.data-table-sort-by-col-3').DataTable({
        scrollX: true,
        order: [[2, 'desc']]//3rd column
    });

    $('.data-table-sort-by-col-1').DataTable({
        scrollX: true,
        order: [[0, 'desc']]//1st column
    });

    $('label > input').addClass("text-light bg-dark");
    $('label > select').addClass("text-light bg-dark");

    //$("select").addClass("text-light bg-dark");
    //$("input").addClass("bg-dark text-light");
    //$("textarea").addClass("bg-dark text-light");
    
});
