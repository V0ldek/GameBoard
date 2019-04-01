$("#friends-sidebar").mCustomScrollbar({
    theme: "minimal"
});

$("#friends-sidebar-collapse").click(() => {
    $("#friends-sidebar-container").toggleClass("toggled");
    toggleSidebarCollapseIcon();
});

$().ready(() => {
    setupAutocomplete($("#friends-sidebar-searchbox-input"));
});

function toggleSidebarCollapseIcon() {
    if ($("#friends-sidebar-collapse-icon").hasClass("fa-angle-left")) {
        $("#friends-sidebar-collapse-icon").removeClass("fa-angle-left");
        $("#friends-sidebar-collapse-icon").addClass("fa-angle-right");
    } else {
        $("#friends-sidebar-collapse-icon").removeClass("fa-angle-right");
        $("#friends-sidebar-collapse-icon").addClass("fa-angle-left");
    }
}

