export async function SetLayerDatasource(vectorMap, layer) {
    let layers = vectorMap.option('layers');
    let targetLayer = layers.find(l => l.name === layer.name);
    targetLayer.dataSource = layer.dataSource;
    vectorMap.option('layers', layers);
}

function EvalStringDatasource(model) {
    model.options.layers.map(layer => {
        if (typeof layer.dataSource === 'string') {
            layer.dataSource = eval(layer.dataSource)
        }
        return layer;
    })
}

function AssignTooltip(model) {
    model.options.tooltip = {
        ...model.options.tooltip,
        customizeTooltip: function (arg) {
            return arg.layer.type === 'marker' ? {text: arg.attribute('tooltip')} : null;
        }
    };
}
function AssignOnClick(model, dotnetCallback) {
    return {
        ...model.options,
        onClick: arg => {
            const clickedElement = arg.target;
            if (clickedElement != null) {
                clickedElement.selected(!clickedElement.selected());

                const attributes = model.options.attributes.reduce((acc, f) => {
                    acc[f] = clickedElement.attribute(f);
                    return acc;
                }, {});

                dotnetCallback.invokeMethodAsync('Invoke', attributes);
            }
        }
    };
}

export async function InitVectorMap(element,model,dotnetCallback) {
    debugger
    EvalStringDatasource(model);
    AssignTooltip(model);
    return new DevExpress.viz.dxVectorMap(element, AssignOnClick(model, dotnetCallback));
}


