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

        this.autocomplete = new Autocomplete(this.userSearchInput, this.userSearchBox, "/User/Search");

        $(this.sidebar).mCustomScrollbar({
            theme: "minimal"
        });
        console.log("dupa2");
        this.sidebarCollapse.addEventListener("click", () => {
            console.log("dupa3");
            this.sidebarContainer.classList.toggle("toggled");
            this.toggleSidebarCollapseIcon();
        });
    }

    toggleSidebarCollapseIcon() {
        const icon = this.sidebarCollapse.querySelector("i");
        console.log("dupa");
        if (!icon) {
            console.error("Cannot find icon for toggleSidebarCollapseIcon");
            return;
        }

        if (icon.classList.contains("fa-angle-left")) {
            icon.classList.remove("fa-angle-left");
            icon.classList.add("fa-angle-right");
        } else {
            icon.classList.remove("fa-angle-right");
            icon.classList.add("fa-angle-left");
        }
    }
}

let friendsSidebar : FriendsSidebar;

document.addEventListener("DOMContentLoaded", () => {
    const friendsSidebarContainerElement = document.querySelector("#friends-sidebar-container") as HTMLElement;
    const friendsSidebarElement = document.querySelector("#friends-sidebar") as HTMLElement;
    const friendsSidebarCollapseElement = document.querySelector("#friends-sidebar-collapse") as HTMLButtonElement;
    const friendsSidebarSearchBoxElement = document.querySelector("#friends-sidebar-search-box") as HTMLElement;
    const friendsSidebarSearchBoxInputElement = document.querySelector("#friends-sidebar-search-box-input") as HTMLInputElement;

    if (!friendsSidebarContainerElement) {
        console.error("Cannot create the sidebar - container is null.");
        return;
    }
    if (!friendsSidebarElement) {
        console.error("Cannot create the sidebar - sidebar is null.");
        return;
    }
    if (!friendsSidebarCollapseElement) {
        console.error("Cannot create the sidebar - collapse is null.");
        return;
    }
    if (!friendsSidebarSearchBoxElement) {
        console.error("Cannot create the sidebar - search box is null.");
        return;
    }
    if (!friendsSidebarSearchBoxInputElement) {
        console.error("Cannot create the sidebar - search box input is null.");
        return;
    }

    friendsSidebar = new FriendsSidebar(friendsSidebarSearchBoxInputElement,
        friendsSidebarSearchBoxElement,
        friendsSidebarContainerElement,
        friendsSidebarElement,
        friendsSidebarCollapseElement);
});