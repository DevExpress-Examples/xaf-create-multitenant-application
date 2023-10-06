function PivotCellProgressBar(fields) {
    return  {
        onCellPrepared: function (e) {
            if (e.area === 'data' && fields[e.columnIndex].isProgressBar) {
                const progressBar = document.createElement('div');
                progressBar.className = 'progress-bar';

                const progress = Math.round(e.cell.value * 100);
                const progressLabel = document.createElement('span');
                progressLabel.className = 'progress-label';
                progressLabel.innerHTML = progress + '%';

                progressBar.style.width = progress + '%';
                progressBar.style.backgroundColor = '#4caf50';
                progressBar.style.height = '20px';
                progressBar.style.position = 'relative';

                progressLabel.style.position = 'absolute';
                progressLabel.style.left = '50%';
                progressLabel.style.top = '50%';
                progressLabel.style.transform = 'translate(-50%, -50%)';
                progressLabel.style.color = '#fff';

                progressBar.appendChild(progressLabel);

                e.cellElement.innerHTML = '';
                e.cellElement.appendChild(progressBar);
            }
        }
    };
    
}

export async function PivotGridInit(element, model) {
    debugger
    ;
    const finalOptions = { ...model.options, ...PivotCellProgressBar(model.options.dataSource.dataFields) };
    return new DevExpress.ui.dxPivotGrid(element, finalOptions);
}



