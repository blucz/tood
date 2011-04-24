function app_signin() {
    $('#welcome').hide();
    $('#app').show();
}

function app_signout() {
    $('#welcome').show();
    $('#app').hide();
    login_init();
}

$(function() {
    app_signout();
});
