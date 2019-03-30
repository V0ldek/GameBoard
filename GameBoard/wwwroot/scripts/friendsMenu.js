$("#friends-menu-collapse").click(function () {
    $("#friends-menu").toggleClass("active");
});
$().ready(function () {
    if (window.screen.availHeight > 768) {
        $("#friends-menu").toggleClass("active");
    }
});
//# sourceMappingURL=friendsMenu.js.map