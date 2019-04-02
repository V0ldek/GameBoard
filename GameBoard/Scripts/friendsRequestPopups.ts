interface IResponse {
    title: string;
    message: string;
}

function friendRequestSent(source: JQuery<HTMLElement>, jqXhr: JQueryXHR) {
    console.log(jqXhr);
    const response: IResponse = jqXhr.responseJSON;

    source.attr("title", response.title);
    source.attr("data-content", response.message);
    source.popover({
        trigger: "focus",
        template: '<div class="popover rounded" role="tooltip"><div class="arrow"></div>' +
            '<h3 class="popover-header popover-header-success"></h3>' +
            '<div class="popover-body"></div></div>'
    });
    source.popover("show");
}

function friendRequestError(source: JQuery<HTMLElement>, jqXhr: JQueryXHR) {
    const response: IResponse = jqXhr.responseJSON;

    source.attr("title", response.title);
    source.attr("data-content", response.message);
    source.popover({
        trigger: "focus",
        template: '<div class="popover rounded" role="tooltip"><div class="arrow"></div>' +
            '<h3 class="popover-header popover-header-error"></h3>' +
            '<div class="popover-body"></div></div>'
    });
    source.popover("show");
}