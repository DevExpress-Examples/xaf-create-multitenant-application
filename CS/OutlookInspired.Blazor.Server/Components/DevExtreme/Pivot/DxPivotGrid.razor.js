function PivotCellProgressBar(model) {
    model.options.onCellPrepared = function (e) {
        if (e.area === 'data' && model.options.dataSource.dataFields[e.columnIndex].isProgressBar) {
            const fullProgressBar = document.createElement('div');
            fullProgressBar.style.width = '100%';
            fullProgressBar.style.backgroundColor = 'transparent'; 
            fullProgressBar.style.height = '20px';
            fullProgressBar.style.position = 'relative';
            const progressBar = document.createElement('div');
            progressBar.className = 'progress-bar';
            const progress = Math.round(e.cell.value * 100);
            progressBar.style.width = progress + '%';
            progressBar.style.backgroundColor = '#8F4700';
            progressBar.style.height = '100%'; 
            const progressLabel = document.createElement('span');
            progressLabel.className = 'progress-label';
            progressLabel.innerHTML = progress + '%';
            progressLabel.style.position = 'absolute';
            progressLabel.style.left = '50%';
            progressLabel.style.top = '50%';
            progressLabel.style.transform = 'translate(-50%, -50%)';
            progressLabel.style.color = '#fff';
            fullProgressBar.appendChild(progressBar);
            fullProgressBar.appendChild(progressLabel);
            e.cellElement.innerHTML = '';
            e.cellElement.appendChild(fullProgressBar);
        }
    };
}


export async function SetPivotGridSource(dxPivot, model) {
    dxPivot.option("dataSource",model.options.dataSource)
}
export async function PivotGridInit(element, model) {
    PivotCellProgressBar(model);
    return new DevExpress.ui.dxPivotGrid(element, model.options);
}
