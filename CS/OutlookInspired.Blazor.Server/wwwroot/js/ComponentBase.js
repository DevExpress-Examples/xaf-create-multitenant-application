
    export function printElement(element){
        document.body.innerHTML = '';
        document.body.appendChild(element);
        setTimeout(() => {
            window.print();
            location.reload();
        }, 2000);
    };

    export function loadScriptAsync(src) {
        return new Promise((resolve, _) => {
            const scriptEl = document.createElement("SCRIPT");
            scriptEl.src = src;
            scriptEl.onload = resolve;
            document.head.appendChild(scriptEl);
        });
    };

    export function loadStylesheetAsync(href){
        return new Promise((resolve, _) => {
            const stylesheetEl = document.createElement("LINK");
            stylesheetEl.href = href;
            stylesheetEl.rel = "stylesheet";
            stylesheetEl.onload = resolve;
            document.head.appendChild(stylesheetEl);
        });
    };
