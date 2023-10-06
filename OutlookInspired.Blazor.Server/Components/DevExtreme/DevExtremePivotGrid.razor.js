


export async function PivotGridInit(element, model) {
    return new DevExpress.ui.dxPivotGrid(element, {
        allowSortingBySummary: true,
        allowSorting: true,
        allowFiltering: true,
        allowExpandAll: true,
        height: "100%",
        width: "100%",
        showBorders: true,
        fieldChooser: {
            enabled: false
        },
        dataSource: model.dataSource
    });
}



