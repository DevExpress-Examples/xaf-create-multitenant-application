

let devExtremeInitPromise = null;
export async function ensureDevExtremeAsync() {
    const scriptLoader = await import(`/js/ComponentBase.js`);
    await loadDevExtreme(scriptLoader);
}

function loadDevExtreme(scriptLoader) {
    return devExtremeInitPromise || (devExtremeInitPromise = new Promise(async (resolve, _) => {
        await scriptLoader.loadScriptAsync("https://cdnjs.cloudflare.com/ajax/libs/devextreme-quill/1.6.2/dx-quill.min.js");
        await scriptLoader.loadScriptAsync("https://cdn3.devexpress.com/jslib/23.1.3/js/dx.all.js");
        await scriptLoader.loadScriptAsync("https://cdn3.devexpress.com/jslib/23.1.5/js/vectormap-data/usa.js");
        await scriptLoader.loadScriptAsync("https://cdn3.devexpress.com/jslib/23.1.5/js/vectormap-data/world.js");
        await scriptLoader.loadStylesheetAsync("https://cdn3.devexpress.com/jslib/23.1.3/css/dx.common.css");
        await scriptLoader.loadStylesheetAsync("https://cdn3.devexpress.com/jslib/23.1.3/css/dx.material.orange.dark.compact.css");
        resolve();
    }));
}

