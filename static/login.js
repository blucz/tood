function login_init() {
    var FADE_DURATION = 0;
    var is_hidden = false;

    function hide_login() {
        is_hidden = true;
        $('#login_email').attr('disabled', true);
        $('#login_password').attr('disabled', true);
        disable_buttons();
        $('#login').hide();
    }

    function show_login(msg) {
        is_hidden = false;
        $('#login_email').attr('disabled', false);
        $('#login_password').attr('disabled', false);
        ev_validate();
        $('.login_pane').hide();
        $('#login').fadeIn(FADE_DURATION);
        $('#login_email').focus().select();
        if (msg) {
            $('#login_failed').html(msg);
            $('#login_failed').fadeIn(FADE_DURATION).delay(2000).fadeOut(FADE_DURATION);
        }
    }

    function enable_buttons() {
        $('#login_signup').attr('disabled', false);
        $('#login_signin').attr('disabled', false);
        $('#login_signup').addClass('loginbutton_enabled');
        $('#login_signin').addClass('loginbutton_enabled');
    }

    function disable_buttons() {
        $('#login_signup').attr('disabled', true);
        $('#login_signin').attr('disabled', true);
        $('#login_signup').removeClass('loginbutton_enabled');
        $('#login_signin').removeClass('loginbutton_enabled');
    }

    function ev_signup() {
        hide_login();
        show_signup_in_progress();
        console.log('sign up')

        service.signup($('#login_email').val(), $('#login_password').val(), function(ok,msg) {
            if (ok) {
                $('#welcome').hide();
                app_init();
            } else {
                show_login(msg);
            }
        });

        return false;
    }

    function ev_signin() {
        hide_login();
        show_signin_in_progress();
        console.log('sign in')

        service.signin($('#login_email').val(), $('#login_password').val(), function(ok,msg) {
            if (ok) {
                app_signin();
            } else {
                show_login(msg);
                show_login_failure();
            }
        });

        return false;
    }

    function ev_validate() {
        if (is_hidden) return;
        if ($('#login_email').val() && $('#login_password').val())
            enable_buttons();
        else
            disable_buttons();
    }
    
    function hide_in_progress() {
        $('#login_in_progress').hide();
    }
    
    function show_signin_in_progress() {
        $('.login_pane').hide();
        $('#login_signup_text').hide();
        $('#login_signin_text').show();
        $('#login_in_progress').fadeIn(FADE_DURATION);
    }

    function show_signup_in_progress() {
        $('.login_pane').hide();
        $('#login_signin_text').hide();
        $('#login_signup_text').show();
        $('#login_in_progress').fadeIn(FADE_DURATION);
    }

    $('#login_email').change(ev_validate);
    $('#login_password').change(ev_validate);
    $('#login_email').keyup(ev_validate);
    $('#login_password').keyup(ev_validate);
    $('#login_signin').click(ev_signin);
    $('#login_signup').click(ev_signup);

    show_login(false);

    FADE_DURATION = 500;
}

