namespace OutlookInspired.Win.UserControls
{
    partial class CustomerStoreView
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
            layoutView1 = new DevExpress.XtraGrid.Views.Layout.LayoutView();
            gridColumnCrestLargeImage = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_gridColumnCrestLargeImage = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            gridColumnCrestCityName = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_gridColumnCrestCityName = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            gridColumnAddressLine = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_gridColumnAddressLine = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            item1 = new DevExpress.XtraLayout.EmptySpaceItem();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_gridColumnCrestLargeImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_gridColumnCrestCityName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_gridColumnAddressLine).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)item1).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = layoutView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(1290, 1077);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { layoutView1 });
            // 
            // layoutView1
            // 
            layoutView1.CardMinSize = new Size(245, 216);
            layoutView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] { gridColumnCrestLargeImage, gridColumnCrestCityName, gridColumnAddressLine });
            layoutView1.GridControl = gridControl1;
            layoutView1.Name = "layoutView1";
            layoutView1.OptionsView.ShowCardCaption = false;
            layoutView1.OptionsView.ShowHeaderPanel = false;
            layoutView1.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.MultiRow;
            layoutView1.TemplateCard = layoutViewCard1;
            // 
            // gridColumnCrestLargeImage
            // 
            gridColumnCrestLargeImage.Caption = "image";
            gridColumnCrestLargeImage.FieldName = "Crest.LargeImage";
            gridColumnCrestLargeImage.LayoutViewField = layoutViewField_gridColumnCrestLargeImage;
            gridColumnCrestLargeImage.MinWidth = 30;
            gridColumnCrestLargeImage.Name = "gridColumnCrestLargeImage";
            gridColumnCrestLargeImage.Width = 112;
            // 
            // layoutViewField_gridColumnCrestLargeImage
            // 
            layoutViewField_gridColumnCrestLargeImage.EditorPreferredWidth = 211;
            layoutViewField_gridColumnCrestLargeImage.Location = new Point(0, 0);
            layoutViewField_gridColumnCrestLargeImage.Name = "layoutViewField_gridColumnCrestLargeImage";
            layoutViewField_gridColumnCrestLargeImage.Size = new Size(215, 30);
            layoutViewField_gridColumnCrestLargeImage.TextSize = new Size(0, 0);
            layoutViewField_gridColumnCrestLargeImage.TextVisible = false;
            // 
            // gridColumnCrestCityName
            // 
            gridColumnCrestCityName.AppearanceCell.Options.UseTextOptions = true;
            gridColumnCrestCityName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridColumnCrestCityName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            gridColumnCrestCityName.Caption = "City";
            gridColumnCrestCityName.FieldName = "Crest.CityName";
            gridColumnCrestCityName.LayoutViewField = layoutViewField_gridColumnCrestCityName;
            gridColumnCrestCityName.MinWidth = 30;
            gridColumnCrestCityName.Name = "gridColumnCrestCityName";
            gridColumnCrestCityName.Width = 112;
            // 
            // layoutViewField_gridColumnCrestCityName
            // 
            layoutViewField_gridColumnCrestCityName.AppearanceItemCaption.Options.UseTextOptions = true;
            layoutViewField_gridColumnCrestCityName.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            layoutViewField_gridColumnCrestCityName.EditorPreferredWidth = 221;
            layoutViewField_gridColumnCrestCityName.Location = new Point(0, 30);
            layoutViewField_gridColumnCrestCityName.Name = "layoutViewField_gridColumnCrestCityName";
            layoutViewField_gridColumnCrestCityName.Size = new Size(225, 30);
            layoutViewField_gridColumnCrestCityName.TextSize = new Size(0, 0);
            layoutViewField_gridColumnCrestCityName.TextVisible = false;
            // 
            // gridColumnAddressLine
            // 
            gridColumnAddressLine.AppearanceCell.Options.UseTextOptions = true;
            gridColumnAddressLine.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridColumnAddressLine.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            gridColumnAddressLine.Caption = "Address";
            gridColumnAddressLine.FieldName = "AddressLine";
            gridColumnAddressLine.LayoutViewField = layoutViewField_gridColumnAddressLine;
            gridColumnAddressLine.MinWidth = 30;
            gridColumnAddressLine.Name = "gridColumnAddressLine";
            gridColumnAddressLine.Width = 112;
            // 
            // layoutViewField_gridColumnAddressLine
            // 
            layoutViewField_gridColumnAddressLine.AppearanceItemCaption.Options.UseTextOptions = true;
            layoutViewField_gridColumnAddressLine.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            layoutViewField_gridColumnAddressLine.EditorPreferredWidth = 221;
            layoutViewField_gridColumnAddressLine.Location = new Point(0, 60);
            layoutViewField_gridColumnAddressLine.Name = "layoutViewField_gridColumnAddressLine";
            layoutViewField_gridColumnAddressLine.Size = new Size(225, 30);
            layoutViewField_gridColumnAddressLine.TextSize = new Size(0, 0);
            layoutViewField_gridColumnAddressLine.TextVisible = false;
            // 
            // layoutViewCard1
            // 
            layoutViewCard1.CustomizationFormText = "TemplateCard";
            layoutViewCard1.GroupBordersVisible = false;
            layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_gridColumnCrestLargeImage, layoutViewField_gridColumnCrestCityName, layoutViewField_gridColumnAddressLine, item1 });
            layoutViewCard1.Name = "layoutViewTemplateCard";
            layoutViewCard1.OptionsItemText.TextToControlDistance = 5;
            layoutViewCard1.Text = "TemplateCard";
            // 
            // item1
            // 
            item1.AllowHotTrack = false;
            item1.CustomizationFormText = "item1";
            item1.Location = new Point(215, 0);
            item1.Name = "item1";
            item1.Size = new Size(10, 30);
            item1.TextSize = new Size(0, 0);
            // 
            // labelControl1
            // 
            labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            labelControl1.Dock = DockStyle.Bottom;
            labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Top;
            labelControl1.LineVisible = true;
            labelControl1.Location = new Point(0, 1077);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(94, 23);
            labelControl1.TabIndex = 3;
            labelControl1.Text = "labelControl1";
            // 
            // CustomerStoreView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Controls.Add(labelControl1);
            Name = "CustomerStoreView";
            Size = new Size(1290, 1100);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_gridColumnCrestLargeImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_gridColumnCrestCityName).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_gridColumnAddressLine).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).EndInit();
            ((System.ComponentModel.ISupportInitialize)item1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumnCrestLargeImage;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumnCrestCityName;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumnAddressLine;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumnCrestLargeImage;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumnCrestCityName;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumnAddressLine;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private DevExpress.XtraLayout.EmptySpaceItem item1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
