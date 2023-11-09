export async function SetChartSource(chart, model) {
    chart.option("dataSource",model.options.dataSource)
}
export async function ChartInit(element, model) {
    return new DevExpress.viz.dxFunnel(element, { ...model.options });
}



