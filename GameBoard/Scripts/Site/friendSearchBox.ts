let groupAddPopoverGenerator = new PopoverGenerator();
let friendSearchBoxes: Autocomplete[] = [];

document.addEventListener("DOMContentLoaded",
    () => {
        const friendSearchBoxElements = document.querySelectorAll(".friend-search-box");

        for (let i = 0; i < friendSearchBoxElements.length; i++) {
            const friendSearchBoxElement = friendSearchBoxElements[i] as HTMLElement;
            const friendSearchBoxInputElement =
                friendSearchBoxElement.querySelector(".friend-search-box-input") as HTMLInputElement;
            const groupId = friendSearchBoxElement.querySelector(".group-id-input") as HTMLInputElement;
            const username = friendSearchBoxElement.querySelector(".user-name-input") as HTMLInputElement;

            if (!friendSearchBoxElement) {
                console.warn("Cannot create the friend search box - search box is null.");
                return;
            }
            if (!friendSearchBoxInputElement) {
                console.warn("Cannot create the friend search box - search box input is null.");
                return;
            }

            if (!groupId) {
                console.warn("Cannot create the friend search box - groupId is null");
                return;
            }

            if (!username) {
                console.warn("Cannot create the friend search box - username is null");
                return;
            }

            const friendSearchBox = new Autocomplete(friendSearchBoxInputElement,
                friendSearchBoxElement,
                `/Friends/SearchFriendsForGroup?userName=${username.value}&groupId=${groupId.value}&`);
            friendSearchBoxes.push(friendSearchBox);
        }
    });