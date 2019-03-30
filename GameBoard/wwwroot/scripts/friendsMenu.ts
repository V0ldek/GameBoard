$("#friends-sidebar").mCustomScrollbar({
    theme: "minimal"
});

$("#friends-sidebar-collapse").click(() => {
    $("#friends-sidebar-container").toggleClass("toggled");
});
