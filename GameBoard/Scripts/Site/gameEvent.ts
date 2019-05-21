document.addEventListener("DOMContentLoaded",
    () => {
        const gamesList = document.querySelector(".game-list") as HTMLElement;
        const inviteeList = document.querySelector(".invitee-list") as HTMLElement;
        const participantList = document.querySelector(".participant-list") as HTMLElement;

        $(gamesList)
            .mCustomScrollbar({
                theme: "minimal"
            });

        $(inviteeList)
            .mCustomScrollbar({
                theme: "minimal"
            });

        $(participantList)
            .mCustomScrollbar({
                theme: "minimal"
            });

        var defaultOpenTabButton = document.getElementById("defaultOpenTabButton");
        var openDescriptionTabButton = document.getElementById("openDescriptionTabButton");

        if (defaultOpenTabButton) {
            defaultOpenTabButton.addEventListener("click", (evt) => {
                openTab(evt, "info");
            });
        }
        if (openDescriptionTabButton) {
            openDescriptionTabButton.addEventListener("click", (evt) => {
                openTab(evt, "description");
            });
        }
    });

var defaultOpenTabButton = document.getElementById("defaultOpenTabButton");

if (defaultOpenTabButton != null) {
    defaultOpenTabButton.click();
}

function openTab(evt: Event, tabName: string) {
    var target = (evt.target as Element);
    Array.from(document.getElementsByClassName("tabcontent")).forEach(v => {
        v.setAttribute("hidden", "true");
    });
    Array.from(document.getElementsByClassName("tablinks")).forEach(v => {
         v.classList.remove("active");
    });

    var tab = document.getElementById(tabName);

    if (tab != null) {
        tab.removeAttribute("hidden");
    }

    target.classList.add("active");
}