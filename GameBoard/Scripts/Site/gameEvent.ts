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