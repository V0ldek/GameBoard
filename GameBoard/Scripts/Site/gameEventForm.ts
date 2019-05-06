let gamesListInput: ListInput;

document.addEventListener("DOMContentLoaded",
    () => {
        const textareaInput = document.querySelector("#games-text-area") as HTMLTextAreaElement;
        const previewList = document.querySelector("#games-preview-list") as HTMLUListElement;

        if (!textareaInput) {
            console.warn("Cannot create games ListInput - games textarea is null.");
            return;
        }
        if (!previewList) {
            console.warn("Cannot create games ListInput - games preview list in null.");
            return;
        }

        $(previewList)
            .mCustomScrollbar({
                theme: "minimal"
            });

        gamesListInput = new ListInput(textareaInput, previewList, true);
    });