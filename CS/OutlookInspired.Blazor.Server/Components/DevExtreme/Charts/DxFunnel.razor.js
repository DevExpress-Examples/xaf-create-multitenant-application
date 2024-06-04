let isScheduled = false;

export async function SetChartSource(chart, model) {
    if (!isScheduled) {
        isScheduled = true;
        setTimeout(async () => {
            await updateChartSource(chart, model);
            isScheduled = false;
        }, 0);
    }
}

async function updateChartSource(chart, model) {
    chart.option("dataSource", model.options.dataSource);
}
export async function ChartInit(element, model) {
    return new DevExpress.viz.dxFunnel(element, { ...model.options });
}



