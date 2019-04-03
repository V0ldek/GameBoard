interface IUser {
    id: string;
    username: string;
    email: string;
}

class Autocomplete {
    private readonly source: HTMLInputElement;
    private readonly resultSource: HTMLElement;
    private readonly minimalCharactersThreshold: number;
    private readonly getUrl: string;
    private static readonly autocompleteResultsClass = "autocomplete-items";

    public constructor(source: HTMLInputElement, resultSource: HTMLElement, getUrl: string, minimalCharactersThreshold = 3) {
        this.source = source;
        this.minimalCharactersThreshold = minimalCharactersThreshold;
        this.resultSource = resultSource;
        this.getUrl = getUrl;

        if (!this.source) {
            throw new Error("Cannot setup autocomplete on a null source.");
        }
        if (!this.resultSource) {
            throw new Error("Cannot setup autocomplete on with a null result source.");
        }

        $(this.source).on("input", () => this.showAutocompleteResults());
        $(this.source).focusin(() => this.showAutocompleteResults());

        $("body").click((e) => {
            if (!$.contains(this.resultSource, e.target)) {
                this.closeAllAutocompleteResults();
            }
        });
    }

    showAutocompleteResults() {
        const value = this.source.value;

        if (!value || value.length < this.minimalCharactersThreshold) {
            return;
        }

        this.closeAllAutocompleteResults();

        const requestData: JQuery.PlainObject = {
            input: value
        };

        $.ajax({
            type: "GET",
            url: this.getUrl,
            dataType: "html",
            data: requestData,
            success: (response: string) => {
                $(this.resultSource).prepend(response);
                const resultDiv = $(this.resultSource).children(`.${Autocomplete.autocompleteResultsClass}`);

                const height = resultDiv.height() as number;
                const defaultTop = parseInt(resultDiv.css("top"));

                resultDiv.css("top", `${defaultTop - height}px`);
                resultDiv.mCustomScrollbar();
            },
            error: () => {
                const errorDiv = document.createElement("div");
                $(errorDiv).addClass(`user-search-error text-danger ${Autocomplete.autocompleteResultsClass}`);

                const text = document.createElement("p");
                text.innerText = "There is an issue with the search service. Please, try again later.";

                errorDiv.appendChild(text);
                $(this.resultSource).prepend(errorDiv);

                const height = $(errorDiv).height() as number;
                const defaultTop = parseInt($(errorDiv).css("top"));

                console.log("Height: " + height + " default top: " + defaultTop); 

                $(errorDiv).css("top", `${defaultTop - height}px`);
            }
        });
    }

    closeAllAutocompleteResults() {
        $(`.${Autocomplete.autocompleteResultsClass}`).remove();
    }
}