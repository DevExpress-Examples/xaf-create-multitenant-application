
export async function ChartInit(element, model) {
    return new DevExpress.viz.dxFunnel(element, { ...model.options });
}



