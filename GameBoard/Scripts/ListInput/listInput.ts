class ListInput {
    private textareaInput: HTMLTextAreaElement;
    private previewList: HTMLUListElement;
    private mCustomScrollbarOnPreview: boolean;

    constructor(textareaInput: HTMLTextAreaElement, previewList: HTMLUListElement, mCustomScrollbarOnPreview: boolean) {
        this.textareaInput = textareaInput;
        this.previewList = previewList;
        this.mCustomScrollbarOnPreview = mCustomScrollbarOnPreview;

        this.textareaInput.addEventListener("input", () => {
            this.update();
        });
        this.update();
    }

    private update() {
        const list = this.parseListFromTextInput(this.textareaInput.value);
        this.renderPreview(list);
    }

    private parseListFromTextInput(input: string): string[] {
        return input.split(new RegExp("\n")).filter(s => s);
    }

    private renderPreview(list: string[]) {
        let liSource: HTMLElement;

        if (!this.mCustomScrollbarOnPreview) {
            liSource = this.previewList;
        } else {
            liSource = this.previewList.querySelector(".mCSB_container") as HTMLElement;
        }

        this.removeAllLis(liSource);
        this.generateLisFromList(liSource, list);
    }

    private removeAllLis(liSource: HTMLElement) {
        liSource.querySelectorAll(`li`).forEach(li => li.remove());
    }

    private generateLisFromList(liSource: HTMLElement, list: string[]) {
        for (const item of list) {
            const li = document.createElement("li");
            li.innerHTML = item;
            liSource.appendChild(li);
        }
    }
}