let groupAddPopoverGenerator = new PopoverGenerator();
let friendSearchBox: Autocomplete;

document.addEventListener("DOMContentLoaded",
    () => {
        const searchBoxInputElement = document.querySelector("#friend-search-box-input") as HTMLInputElement;
        const searchBoxElement = document.querySelector("#friend-search-box") as HTMLElement;

        if (!searchBoxElement) {
            console.error("Cannot create the friend search box - search box is null.");
            return;
        }
        if (!searchBoxInputElement) {
            console.error("Cannot create the friend search box - search box input is null.");
            return;
        }

        friendSearchBox = new Autocomplete(searchBoxInputElement, searchBoxElement, "/User/SearchFriend");
    });