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

function AssignAnnotation(model, annotationData) {
    if (!model.options.annotations) return;
    
    model.options.commonAnnotationSettings = {
        type: 'custom',
        template(annotation, container) {
            const svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
            svg.setAttribute("class", "annotation");

            const foreignObject = document.createElementNS("http://www.w3.org/2000/svg", "foreignObject");
            
            foreignObject.setAttribute("width", annotation.width);
            foreignObject.setAttribute("height", annotation.height);
            const div = document.createElement("div");
            div.innerHTML = annotation.data; 

            foreignObject.appendChild(div);
            svg.appendChild(foreignObject);
            container.appendChild(svg);
        }
    };
}
function AssignOnClick(model, dotnetCallback) {
    model.options = {
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
function AssignOnDisposing(model, dotnetCallback) {
    model.options = {
        ...model.options,
        onDisposing: () => dotnetCallback.dispose()
    };
}
function AssignTooltip(model) {
    model.options.tooltip = {
        ...model.options.tooltip,
        customizeTooltip: function (arg) {
            return arg.layer.type === 'marker' ? {text: arg.attribute('tooltip')} : null;
        }
    };
}
export async function InitVectorMap(element, model, dotnetCallback) {
    AssignTooltip(model);
    if (dotnetCallback) {
        AssignOnDisposing(model, dotnetCallback);
        AssignOnClick(model, dotnetCallback);
    }
    AssignAnnotation(model);
    EvalStringDatasource(model);
    return new DevExpress.viz.dxVectorMap(element, {
        ...model.options,
        onDrawn: e => model.readyReference.invokeMethodAsync('Invoke', null),
        onDisposing: () => model.readyReference.dispose()    
    });
}


