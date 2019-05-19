class AutocompleteFriends {
    private readonly source: HTMLInputElement;
    private readonly resultSource: HTMLElement;
    private readonly minimalCharactersThreshold: number;
    private readonly timeoutDuration: number;
    private readonly getUrl: string;
    private readonly userName: string;
    private readonly groupId : string;
    private readonly autocompleteResultsClass : string;
    private currentTimeout: number | null = null;

    public constructor(source: HTMLInputElement,
        resultSource: HTMLElement,
        getUrl: string,
        friendUserName: string,
        groupId : string,
        minimalCharactersThreshold = 3,
        timeoutDuration = 500) {
        this.source = source;
        this.minimalCharactersThreshold = minimalCharactersThreshold;
        this.timeoutDuration = timeoutDuration;
        this.resultSource = resultSource;
        this.getUrl = getUrl;
        this.userName = friendUserName;
        this.groupId = groupId;
        this.autocompleteResultsClass = "autocomplete-" + groupId;

        if (!this.source) {
            throw new Error("Cannot setup autocomplete on a null source.");
        }
        if (!this.resultSource) {
            throw new Error("Cannot setup autocomplete on with a null result source.");
        }

        this.source.addEventListener("input", () => this.newResultsTimeout());
        this.source.addEventListener("focusin", () => this.newResultsTimeout());

        document.body.addEventListener("click",
            (e) => {
                const targetElement = e.target as HTMLElement;

                if (targetElement && !this.resultSource.contains(targetElement)) {
                    this.closeAllAutocompleteResults();
                }
            });
    }

    private newResultsTimeout() {
        if (this.currentTimeout) {
            clearTimeout(this.currentTimeout);
        }
        this.currentTimeout = setTimeout(() => this.showAutocompleteResults(), this.timeoutDuration);
    }

    showAutocompleteResults() {
        const value = this.source.value;

        this.closeAllAutocompleteResults();

        if (!value || value.length < this.minimalCharactersThreshold) {
            return;
        }

        fetch(`${this.getUrl}?input=${value}&userName=${this.userName}&groupId=${this.groupId}`,
            {
                method: "GET",
            })
            .then(response => this.createAutocompleteResults(response))
            .catch((reason) => this.createErrorResult(reason));
    }

    private createAutocompleteResults(response: Response) {
        response.text()
            .then(responseText => {
                if (!response.ok) {
                    this.createErrorResult(responseText);
                    return;
                }

                const template = document.createElement("template");
                template.innerHTML = responseText.trim();
                const resultDiv = template.content.firstChild as HTMLElement;

                if (!resultDiv) {
                    console.error("Autocomplete - could not create a results div from response.");
                }

                this.resultSource.prepend(resultDiv);

                $(resultDiv)
                    .mCustomScrollbar({
                        theme: "minimal"
                    });

                this.setAutocompleteResultsPosition(resultDiv);
            })
            .catch(reason => this.createErrorResult(reason));
    }

    private createErrorResult(reason: any) {
        console.error(reason);

        const errorDiv = document.createElement("div");
        errorDiv.classList.add("user-search-error", "text-danger", `${this.autocompleteResultsClass}`);

        const text = document.createElement("p");
        text.innerText = "There is an issue with the search service. Please, try again later.";

        errorDiv.appendChild(text);
        this.resultSource.prepend(errorDiv);

        this.setAutocompleteResultsPosition(errorDiv);
    }

    private setAutocompleteResultsPosition(result: HTMLElement) {
        const height = result.offsetHeight;
        const defaultTop = window.getComputedStyle(result).top;
        const defaultTopAsInt = defaultTop ? parseInt(defaultTop) : 0;

        result.style.top = `${defaultTopAsInt - height}px`;
    }

    closeAllAutocompleteResults() {
        document.querySelectorAll(`.${this.autocompleteResultsClass}`).forEach((el) => el.remove())
    };
}