let groupAddPopoverGenerator = new PopoverGenerator();
let friendSearchBoxes: AutocompleteFriends[] = [];

document.addEventListener("DOMContentLoaded",
    () => {
        const friendSearchBoxElements = document.querySelectorAll(".friend-search-box");

        for (let i = 0; i < friendSearchBoxElements.length; i++) {
            const friendSearchBoxElement = <HTMLElement> friendSearchBoxElements[i];
            const friendSearchBoxInputElement =
                <HTMLInputElement>friendSearchBoxElement.querySelector(".friend-search-box-input");
            const groupId = <HTMLInputElement>friendSearchBoxElement.querySelector(".group-id-input");
            const username = <HTMLInputElement>friendSearchBoxElement.querySelector(".user-name-input");

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

            const friendSearchBox = new AutocompleteFriends(friendSearchBoxInputElement, friendSearchBoxElement, "/Friends/SearchFriendsForGroup", username.value, groupId.value);
            friendSearchBoxes.push(friendSearchBox);
        }
    });