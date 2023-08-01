namespace OutlookInspired.Win.UserControls
{
    partial class CustomerStoreWinExplorerView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            winExplorerView1 = new DevExpress.XtraGrid.Views.WinExplorer.WinExplorerView();
            colCrestCity = new DevExpress.XtraGrid.Columns.GridColumn();
            colCrestLargeImage = new DevExpress.XtraGrid.Columns.GridColumn();
            colCrestSmallImage = new DevExpress.XtraGrid.Columns.GridColumn();
            colAddressLines = new DevExpress.XtraGrid.Columns.GridColumn();
            colCustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)winExplorerView1).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = winExplorerView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(1224, 738);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { winExplorerView1 });
            // 
            // winExplorerView1
            // 
            winExplorerView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colCrestCity, colCrestLargeImage, colCrestSmallImage, colAddressLines, colCustomerName });
            winExplorerView1.GridControl = gridControl1;
            winExplorerView1.Name = "winExplorerView1";
            // 
            // colCrestCity
            // 
            colCrestCity.Caption = "gridColumn1";
            colCrestCity.FieldName = "Crest.CityName";
            colCrestCity.MinWidth = 30;
            colCrestCity.Name = "colCrestCity";
            colCrestCity.Visible = true;
            colCrestCity.VisibleIndex = 0;
            colCrestCity.Width = 112;
            // 
            // colCrestLargeImage
            // 
            colCrestLargeImage.Caption = "gridColumn1";
            colCrestLargeImage.FieldName = "Crest.LargeImage";
            colCrestLargeImage.MinWidth = 30;
            colCrestLargeImage.Name = "colCrestLargeImage";
            colCrestLargeImage.Visible = true;
            colCrestLargeImage.VisibleIndex = 0;
            colCrestLargeImage.Width = 112;
            // 
            // colCrestSmallImage
            // 
            colCrestSmallImage.Caption = "gridColumn1";
            colCrestSmallImage.FieldName = "Crest.SmallImage";
            colCrestSmallImage.MinWidth = 30;
            colCrestSmallImage.Name = "colCrestSmallImage";
            colCrestSmallImage.Visible = true;
            colCrestSmallImage.VisibleIndex = 0;
            colCrestSmallImage.Width = 112;
            // 
            // colAddressLines
            // 
            colAddressLines.Caption = "gridColumn1";
            colAddressLines.FieldName = "Address";
            colAddressLines.MinWidth = 30;
            colAddressLines.Name = "colAddressLines";
            colAddressLines.Visible = true;
            colAddressLines.VisibleIndex = 0;
            colAddressLines.Width = 112;
            // 
            // colCustomerName
            // 
            colCustomerName.Caption = "gridColumn1";
            colCustomerName.FieldName = "CustomerName";
            colCustomerName.MinWidth = 30;
            colCustomerName.Name = "colCustomerName";
            colCustomerName.Visible = true;
            colCustomerName.VisibleIndex = 0;
            colCustomerName.Width = 112;
            // 
            // CustomerStoreWinExplorerView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Name = "CustomerStoreWinExplorerView";
            Size = new Size(1224, 738);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)winExplorerView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.WinExplorer.WinExplorerView winExplorerView1;
        private DevExpress.XtraGrid.Columns.GridColumn colCrestCity;
        private DevExpress.XtraGrid.Columns.GridColumn colCrestLargeImage;
        private DevExpress.XtraGrid.Columns.GridColumn colCrestSmallImage;
        private DevExpress.XtraGrid.Columns.GridColumn colAddressLines;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerName;
    }
}
