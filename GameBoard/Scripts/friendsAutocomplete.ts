const countries = ["Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua &amp; Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia &amp; Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central Arfrican Republic", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cuba", "Curacao", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauro", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea", "Norway", "Oman", "Pakistan", "Palau", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre &amp; Miquelon", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Korea", "South Sudan", "Spain", "Sri Lanka", "St Kitts &amp; Nevis", "St Lucia", "St Vincent", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad &amp; Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks &amp; Caicos", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States of America", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe"];


function setupAutocomplete(input: JQuery<HTMLInputElement>) {
    input.on("input", () => showAutocompleteResults(input));
    input.focusin(() => showAutocompleteResults(input));
    $("body").click((e) => {
        const target = $(e.target);
        if (target.closest("#friends-sidebar-search-form").length === 0) {
            closeAllAutocompleteResults();
        }
    });
}

interface IUser {
    id: string;
    username: string;
    email: string;
}

function showAutocompleteResults(input : JQuery<HTMLInputElement>) {
    const value = input.val() as string;

    if (!value || value.length < 3) {
        return;
    }

    closeAllAutocompleteResults();

    const requestData: JQuery.PlainObject = {
        input: value
    };

    $.ajax({
        type: "GET",
        url: "/User/Search",
        dataType: "html",
        data: requestData,
        success: (response : string) => {
            input.parent().prepend(response);
            const resultDiv = input.parent().children(".autocomplete-items");
            resultDiv.css("top", `-${resultDiv.height() + 50}px`);
            resultDiv.mCustomScrollbar();
            /*
            const resultDiv = $(document.createElement("div"));
            resultDiv.attr("id", input.attr("id") + "-autocomplete-results");
            resultDiv.attr("class", "autocomplete-items");
            
            for (let item of response) {
                resultDiv.append(createResultItem(item));
            }


            input.parent().prepend(resultDiv);
            resultDiv.css("top", `-${resultDiv.height() + 50}px`);
            console.log(resultDiv);*/
        },
        error: (xhr) => {
            console.log("xhr: ", xhr);
        }
    });
}

/*function createResultItem(item: IUser): JQuery<HTMLDivElement> {
    const itemDiv = $(document.createElement("div"));
    const itemText = $(document.createElement("p"));

    itemDiv.attr("class", "autocomplete-item");
    itemText.text(item.username);

    const sendButton = createFriendRequestButton(itemDiv);

    itemDiv.append(itemText);
    itemDiv.append(sendButton);

    return itemDiv;
}

function createFriendRequestForm(itemDiv: JQuery<HTMLDivElement>): JQuery<HTMLButtonElement> {
    const form = $(document.createElement("form"));
    form.attr("action", "/FriendRequest/Create");
    form.attr("method")

    const button = $(document.createElement("button"));
    button.attr("class", "btn btn-success friend-request-button");
    button.attr("type", "submit");

    const icon = $(document.createElement("i"));
    icon.attr("class", "fa fa-user-plus");

    button.append(icon);

    return button;
}*/

function closeAllAutocompleteResults() {
    $(".autocomplete-items").remove();
}