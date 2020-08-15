
$(function () {
    $(document).ready(function () {
        var separator = "/";
        var url = window.location.pathname.split(separator);
        var controllerName = url[1];

        if (controllerName !== "") {

            var actionMethodName = url[2];

            var selectedButtonLink = separator +
                controllerName +
                separator +
                actionMethodName;

            $('.nav li a[href="' + selectedButtonLink + '"]').first().addClass('active');
        }
        else {
            $('.nav li a').first().addClass('active');
        }
    });
});