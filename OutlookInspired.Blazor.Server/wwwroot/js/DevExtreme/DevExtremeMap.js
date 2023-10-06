export function printMap(dxMapInstance) {
    let mapElement = dxMapInstance.element();
    document.body.innerHTML = "";
    document.body.appendChild(mapElement);
    setTimeout(() => {
        window.print();
        location.reload();
    }, 2000);
    
}

export function updateMapRouteMode(dxMapInstance, newMode) {
    let routes = dxMapInstance.option('routes');
    routes = routes.map(route => ({ ...route, mode: newMode }));
    dxMapInstance.option('routes', routes);
}
export async function InitDxMap(element,model) {
    let closestParent = element.closest(".dxbl-modal-body");
    return new DevExpress.ui.dxMap(element, {
        center: JSON.stringify(model.center),
        markers: model.markers.map(marker => ({
            location: `${marker.location.lat}, ${marker.location.lng}`
        })),
        routes: model.routes,
        zoom: model.zoom,
        height: closestParent.clientHeight*0.97,
        width: "100%",
        provider: model.provider,
        apiKey: {
            bing: model.apiKey,
        },
        type: "roadmap"
    });
}
export function updateDatasource(dxMapInstance, model) {
    dxMapInstance.option('layers[1].dataSource', model.features);
}

export async function InitVectorMap(element,model,dotnetCallback) {
    let closestParent = element.closest(".dxbl-modal-body");
    debugger;
    return new DevExpress.viz.dxVectorMap(element, {
        height: closestParent.clientHeight*0.97,
        width: "100%",
        onClick: arg => {
            const clickedElement = arg.target;
            if (clickedElement != null)
                clickedElement.selected(!clickedElement.selected());
            dotnetCallback.invokeMethodAsync('Invoke', arg.target.attribute("city"))
        },
        onDisposing: () => dotnetHelper.dispose(),
        provider: model.provider,
        apiKey: {
            bing: model.apiKey,
        },
        layers: [{
            dataSource: DevExpress.viz.map.sources.usa,
            hoverEnabled: false,
            selectionMode: 'single'
        }, {
            name: 'pies',
            dataSource: model.features,
            elementType: 'pie',
            dataField: 'values',
            palette: model.palette
        }],
        tooltip: {
            enabled: true,
            customizeTooltip(arg) {
                if (arg.layer.type === 'marker') {
                    return { text: arg.attribute('tooltip') };
                }
                return null;
            },
            zIndex:100000
        },
        bounds: model.bounds,
    });
        
}


