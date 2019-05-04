let friendsSidebar: Collapsible;

document.addEventListener("DOMContentLoaded",
    () => {
        const friendsSidebarContainerElement = document.querySelector("#friends-sidebar-container") as HTMLElement;
        const friendsSidebarElement = document.querySelector("#friends-sidebar") as HTMLElement;
        const friendsSidebarCollapseElement = document.querySelector("#friends-sidebar-collapse") as HTMLButtonElement;

        if (!friendsSidebarContainerElement) {
            console.warn("Cannot create the sidebar - container is null.");
            return;
        }
        if (!friendsSidebarElement) {
            console.warn("Cannot create the sidebar - sidebar is null.");
            return;
        }
        if (!friendsSidebarCollapseElement) {
            console.warn("Cannot create the sidebar - collapse is null.");
            return;
        }

        $(friendsSidebarElement)
            .mCustomScrollbar({
                theme: "minimal"
            });

        friendsSidebar =
            new Collapsible(friendsSidebarContainerElement, friendsSidebarElement, friendsSidebarCollapseElement);
    });