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
    });

var defaultOpenTabButton = document.getElementById("defaultOpenTabButton");

if (defaultOpenTabButton != null) {
    defaultOpenTabButton.click();
}

function openTab(evt: { currentTarget: { className: string; }; }, tabName: string) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].setAttribute("hidden", "true");
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].classList.remove("active");
    }

    var tab = document.getElementById(tabName);

    if (tab != null) {
        tab.removeAttribute("hidden");
    }

    evt.currentTarget.className += " active";
}