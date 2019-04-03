class FriendsSidebar {
    private readonly autocomplete: Autocomplete;
    private readonly userSearchInput: HTMLInputElement;
    private readonly userSearchBox: HTMLElement;
    private readonly sidebarContainer: HTMLElement;
    private readonly sidebar: HTMLElement;
    private readonly sidebarCollapse: HTMLButtonElement;

    constructor(userSearchInput: HTMLInputElement,
        userSearchBox: HTMLElement,
        sidebarContainer: HTMLElement,
        sidebar: HTMLElement,
        sidebarCollapse: HTMLButtonElement) {
        this.userSearchInput = userSearchInput;
        this.userSearchBox = userSearchBox;
        this.sidebarContainer = sidebarContainer;
        this.sidebar = sidebar;
        this.sidebarCollapse = sidebarCollapse;

        this.autocomplete = new Autocomplete(userSearchInput, userSearchBox, "/User/Search");

        $(sidebar).mCustomScrollbar({
            theme: "minimal"
        });
        $(sidebarCollapse).click(() => {
            $(sidebarContainer).toggleClass("toggled");
            this.toggleSidebarCollapseIcon();
        });
    }

    toggleSidebarCollapseIcon() {
        const icon = $(this.sidebarCollapse).children("i");

        if (icon.hasClass("fa-angle-left")) {
            icon.removeClass("fa-angle-left");
            icon.addClass("fa-angle-right");
        } else {
            icon.removeClass("fa-angle-right");
            icon.addClass("fa-angle-left");
        }
    }
}

let friendsSidebar : FriendsSidebar;

$().ready(() => {
    friendsSidebar = new FriendsSidebar($("#friends-sidebar-searchbox-input")[0] as HTMLInputElement,
        $("#friends-sidebar-search-box")[0],
        $("#friends-sidebar-container")[0],
        $("#friends-sidebar")[0],
        $("#friends-sidebar-collapse")[0] as HTMLButtonElement);
});