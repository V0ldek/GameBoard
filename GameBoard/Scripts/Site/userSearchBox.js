let friendRequestPopupGenerator = new PopupGenerator();
let userSearchBox;
document.addEventListener("DOMContentLoaded", () => {
    const searchBoxInputElement = document.querySelector("#user-search-box-input");
    const searchBoxElement = document.querySelector("#user-search-box");
    if (!searchBoxElement) {
        console.error("Cannot create the user search box - search box is null.");
        return;
    }
    if (!searchBoxInputElement) {
        console.error("Cannot create the user search box - search box input is null.");
        return;
    }
    userSearchBox = new Autocomplete(searchBoxInputElement, searchBoxElement, "/User/Search");
});
//# sourceMappingURL=userSearchBox.js.map