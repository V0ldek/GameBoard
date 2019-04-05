interface IResponse {
    title: string;
    message: string;
}

class PopupGenerator {
    private getHtmlTemplate(headerClass: string) {
        return `<div class="popover rounded" role="tooltip">
                    <div class="arrow"></div>
                    <h3 class="popover-header ${headerClass}"></h3>
                    <div class="popover-body"></div>
                </div>`;
    }

    generateSuccessPopup(source: HTMLElement, jqXhr: JQueryXHR) {
        this.generatePopup(source, this.getHtmlTemplate("popover-header-success"), jqXhr);
    }

    generateErrorPopup(source: HTMLElement, jqXhr: JQueryXHR) {
        this.generatePopup(source, this.getHtmlTemplate("popover-header-error"), jqXhr);
    }

    private generatePopup(source: HTMLElement, template: string, jqXhr: JQueryXHR) {
        const response: IResponse = jqXhr.responseJSON;

        source.setAttribute("title", response.title);
        source.setAttribute("data-content", response.message);
        $(source).popover({
            trigger: "focus",
            template
        });
        $(source).popover("show");
    }
}