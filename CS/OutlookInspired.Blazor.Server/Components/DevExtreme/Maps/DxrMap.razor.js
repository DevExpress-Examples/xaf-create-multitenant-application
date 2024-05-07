
export function SetRouteMode(dxMapInstance, newMode) {
    let routes = dxMapInstance.option('routes');
    routes = routes.map(route => ({ ...route, mode: newMode }));
    dxMapInstance.option('routes', routes);
}

export async function InitDxMap(element, model) {
    return new DevExpress.ui.dxMap(element, {
        ...model.options,
        onReady: e=>model.readyReference.invokeMethodAsync('Invoke',null),
        onDisposing: () => model.readyReference.dispose()
    });
}


