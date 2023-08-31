namespace OutlookInspired.Win.UserControls
{
    partial class MapsUserControl
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
            mapControl = new DevExpress.XtraMap.MapControl();
            imageTilesLayer1 = new DevExpress.XtraMap.ImageLayer();
            bingMapDataProvider1 = new DevExpress.XtraMap.BingMapDataProvider();
            informationLayer1 = new DevExpress.XtraMap.InformationLayer();
            bingGeocodeDataProvider1 = new DevExpress.XtraMap.BingGeocodeDataProvider();
            informationLayer2 = new DevExpress.XtraMap.InformationLayer();
            bingSearchDataProvider1 = new DevExpress.XtraMap.BingSearchDataProvider();
            informationLayer3 = new DevExpress.XtraMap.InformationLayer();
            bingRouteDataProvider1 = new DevExpress.XtraMap.BingRouteDataProvider();
            ((System.ComponentModel.ISupportInitialize)mapControl).BeginInit();
            SuspendLayout();
            // 
            // mapControl
            // 
            mapControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            mapControl.Dock = DockStyle.Fill;
            mapControl.Layers.Add(imageTilesLayer1);
            mapControl.Layers.Add(informationLayer1);
            mapControl.Layers.Add(informationLayer2);
            mapControl.Layers.Add(informationLayer3);
            mapControl.Location = new Point(0, 0);
            mapControl.Margin = new Padding(5);
            mapControl.Name = "mapControl";
            mapControl.SearchPanelOptions.Visible = false;
            mapControl.Size = new Size(936, 814);
            mapControl.TabIndex = 19;
            mapControl.ZoomLevel = 8D;
            imageTilesLayer1.DataProvider = bingMapDataProvider1;
            informationLayer1.DataProvider = bingGeocodeDataProvider1;
            bingGeocodeDataProvider1.GenerateLayerItems = false;
            informationLayer2.DataProvider = bingSearchDataProvider1;
            bingSearchDataProvider1.GenerateLayerItems = false;
            informationLayer3.DataProvider = bingRouteDataProvider1;
            informationLayer3.HighlightedItemStyle.Stroke = Color.Cyan;
            informationLayer3.HighlightedItemStyle.StrokeWidth = 3;
            informationLayer3.ItemStyle.Stroke = Color.Cyan;
            informationLayer3.ItemStyle.StrokeWidth = 3;
            bingRouteDataProvider1.RouteOptions.DistanceUnit = DevExpress.XtraMap.DistanceMeasureUnit.Mile;
            // 
            // UserControl1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mapControl);
            Name = "MapsUserControl";
            Size = new Size(936, 814);
            ((System.ComponentModel.ISupportInitialize)mapControl).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraMap.MapControl mapControl;
        private DevExpress.XtraMap.ImageLayer imageTilesLayer1;
        private DevExpress.XtraMap.BingMapDataProvider bingMapDataProvider1;
        private DevExpress.XtraMap.InformationLayer informationLayer1;
        private DevExpress.XtraMap.BingGeocodeDataProvider bingGeocodeDataProvider1;
        private DevExpress.XtraMap.InformationLayer informationLayer2;
        private DevExpress.XtraMap.BingSearchDataProvider bingSearchDataProvider1;
        private DevExpress.XtraMap.InformationLayer informationLayer3;
        private DevExpress.XtraMap.BingRouteDataProvider bingRouteDataProvider1;
    }
}
