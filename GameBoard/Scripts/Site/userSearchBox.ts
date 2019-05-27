let friendRequestPopoverGenerator = new PopoverGenerator();
let userSearchBox: Autocomplete;

document.addEventListener("DOMContentLoaded",
    () => {
        const searchBoxInputElement = document.querySelector("#user-search-box-input") as HTMLInputElement;
        const searchBoxElement = document.querySelector("#user-search-box") as HTMLElement;

        if (!searchBoxElement) {
            console.warn("Cannot create the user search box - search box is null.");
            return;
        }
        if (!searchBoxInputElement) {
            console.warn("Cannot create the user search box - search box input is null.");
            return;
        }

        userSearchBox = new Autocomplete(searchBoxInputElement, searchBoxElement, "/User/SearchUser?");
    });