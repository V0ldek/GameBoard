class PopupGenerator {
    getHtmlTemplate(headerClass) {
        return `<div class="popover rounded" role="tooltip">
                    <div class="arrow"></div>
                    <h3 class="popover-header ${headerClass}"></h3>
                    <div class="popover-body"></div>
                </div>`;
    }
    generateSuccessPopup(source, jqXhr) {
        this.generatePopup(source, this.getHtmlTemplate("popover-header-success"), jqXhr);
    }
    generateErrorPopup(source, jqXhr) {
        this.generatePopup(source, this.getHtmlTemplate("popover-header-error"), jqXhr);
    }
    generatePopup(source, template, jqXhr) {
        const response = jqXhr.responseJSON;
        source.setAttribute("title", response.title);
        source.setAttribute("data-content", response.message);
        $(source).popover({
            trigger: "focus",
            template
        });
        $(source).popover("show");
    }
}
//# sourceMappingURL=popupGenerator.js.map