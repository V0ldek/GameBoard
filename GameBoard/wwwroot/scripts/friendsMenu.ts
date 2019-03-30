$("#friends-menu-collapse").click(() => {
    $("#friends-menu").toggleClass("active");
});

$().ready(() => {
    if (window.screen.availHeight > 768) {
        $("#friends-menu").toggleClass("active");
    }
});