var mapTypes = [{
        key: "roadmap",
        name: "Road Map"
    }, {
        key: "satellite",
        name: "Satellite (Photographic) Map"
    }, {
        key: "hybrid",
        name: "Hybrid Map"
}];
export function printMap(dxMapInstance) {
    let mapElement = dxMapInstance.element();
    document.body.innerHTML = "";
    document.body.appendChild(mapElement);
    window.print();
    location.reload();
}

export function updateRouteMode(dxMapInstance, newMode) {
    let routes = dxMapInstance.option('routes');
    routes = routes.map(route => ({ ...route, mode: newMode }));
    dxMapInstance.option('routes', routes);
}
export async function MapInit(element,model) {
    let closestParent = element.closest(".dxbl-modal-body");
    console.log(model.routes)
    let dxMap = new DevExpress.ui.dxMap(element, {
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
        type: mapTypes[0].key
    });
    debugger;
    return  dxMap;
    
}