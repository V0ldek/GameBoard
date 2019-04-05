class Autocomplete {
    constructor(source, resultSource, getUrl, minimalCharactersThreshold = 3) {
        this.source = source;
        this.minimalCharactersThreshold = minimalCharactersThreshold;
        this.resultSource = resultSource;
        this.getUrl = getUrl;
        console.log(this.source);
        if (!this.source) {
            throw new Error("Cannot setup autocomplete on a null source.");
        }
        if (!this.resultSource) {
            throw new Error("Cannot setup autocomplete on with a null result source.");
        }
        this.source.addEventListener("input", () => this.showAutocompleteResults());
        this.source.addEventListener("focusin", () => this.showAutocompleteResults());
        document.body.addEventListener("click", (e) => {
            const targetElement = e.target;
            if (targetElement && !this.resultSource.contains(targetElement)) {
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
        fetch(`${this.getUrl}?input=${value}`, {
            method: "GET",
        })
            .then(response => this.createAutocompleteResults(response))
            .catch((reason) => this.createErrorResult(reason));
    }
    createAutocompleteResults(response) {
        response.text()
            .then(responseText => {
            if (!response.ok) {
                this.createErrorResult(responseText);
                return;
            }
            const template = document.createElement("template");
            template.innerHTML = responseText.trim();
            const resultDiv = template.content.firstChild;
            if (!resultDiv) {
                console.error("Autocomplete - could not create a results div from response.");
            }
            this.resultSource.prepend(resultDiv);
            this.setAutocompleteResultsPosition(resultDiv);
            $(resultDiv).mCustomScrollbar();
        })
            .catch(reason => this.createErrorResult(reason));
    }
    createErrorResult(reason) {
        console.error(reason);
        const errorDiv = document.createElement("div");
        errorDiv.classList.add("user-search-error", "text-danger", `${Autocomplete.autocompleteResultsClass}`);
        const text = document.createElement("p");
        text.innerText = "There is an issue with the search service. Please, try again later.";
        errorDiv.appendChild(text);
        this.resultSource.prepend(errorDiv);
        this.setAutocompleteResultsPosition(errorDiv);
    }
    setAutocompleteResultsPosition(result) {
        const height = result.offsetHeight;
        const defaultTop = window.getComputedStyle(result).top;
        const defaultTopAsInt = defaultTop ? parseInt(defaultTop) : 0;
        result.style.top = `${defaultTopAsInt - height}px`;
    }
    closeAllAutocompleteResults() {
        document.querySelectorAll(`.${Autocomplete.autocompleteResultsClass}`).forEach((el) => el.remove());
    }
}
Autocomplete.autocompleteResultsClass = "autocomplete-items";
//# sourceMappingURL=autocomplete.js.map