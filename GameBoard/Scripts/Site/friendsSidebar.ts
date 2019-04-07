let friendsSidebar: Collapsible;

document.addEventListener("DOMContentLoaded",
    () => {
        const friendsSidebarContainerElement = document.querySelector("#friends-sidebar-container") as HTMLElement;
        const friendsSidebarElement = document.querySelector("#friends-sidebar") as HTMLElement;
        const friendsSidebarCollapseElement = document.querySelector("#friends-sidebar-collapse") as HTMLButtonElement;

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

        friendsSidebar =
            new Collapsible(friendsSidebarContainerElement, friendsSidebarElement, friendsSidebarCollapseElement);
    });