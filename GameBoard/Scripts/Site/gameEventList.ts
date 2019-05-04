document.addEventListener("DOMContentLoaded",
    () => {
        const gameEventsContainer = document.querySelector("#game-events-container") as HTMLElement;

        $(gameEventsContainer)
            .mCustomScrollbar({
                theme: "minimal"
            });
    });