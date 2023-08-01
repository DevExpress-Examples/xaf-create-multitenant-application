namespace OutlookInspired.Win.UserControls
{
    partial class CustomerLayoutView
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
            colName1 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colName1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            colHomeOffice = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colHomeOffice = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            colBillingAddress1 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colBillingAddress1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            colImage = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_layoutViewColumn1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colName1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colHomeOffice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colBillingAddress1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = layoutView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(911, 760);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { layoutView1 });
            // 
            // layoutView1
            // 
            layoutView1.CardCaptionFormat = "{2}";
            layoutView1.CardMinSize = new Size(231, 165);
            layoutView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] { colName1, colHomeOffice, colBillingAddress1, colImage });
            layoutView1.DetailHeight = 512;
            layoutView1.FieldCaptionFormat = "{0}";
            layoutView1.GridControl = gridControl1;
            layoutView1.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_colName1 });
            layoutView1.Name = "layoutView1";
            layoutView1.OptionsBehavior.AllowExpandCollapse = false;
            layoutView1.OptionsBehavior.AllowRuntimeCustomization = false;
            layoutView1.OptionsBehavior.Editable = false;
            layoutView1.OptionsBehavior.ReadOnly = true;
            layoutView1.OptionsMultiRecordMode.MultiRowScrollBarOrientation = DevExpress.XtraGrid.Views.Layout.ScrollBarOrientation.Vertical;
            layoutView1.OptionsSelection.MultiSelect = true;
            layoutView1.OptionsView.AllowHotTrackFields = false;
            layoutView1.OptionsView.FocusRectStyle = DevExpress.XtraGrid.Views.Layout.FocusRectStyle.None;
            layoutView1.OptionsView.ShowHeaderPanel = false;
            layoutView1.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.MultiRow;
            layoutView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] { new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colName1, DevExpress.Data.ColumnSortOrder.Ascending) });
            layoutView1.TemplateCard = layoutViewCard1;
            // 
            // colName1
            // 
            colName1.FieldName = "Name";
            colName1.LayoutViewField = layoutViewField_colName1;
            colName1.MinWidth = 30;
            colName1.Name = "colName1";
            colName1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count) });
            colName1.Width = 112;
            // 
            // layoutViewField_colName1
            // 
            layoutViewField_colName1.EditorPreferredWidth = 20;
            layoutViewField_colName1.Location = new Point(0, 0);
            layoutViewField_colName1.Name = "layoutViewField_colName1";
            layoutViewField_colName1.Size = new Size(324, 104);
            layoutViewField_colName1.TextSize = new Size(72, 13);
            // 
            // colHomeOffice
            // 
            colHomeOffice.Caption = "HOME OFFICE";
            colHomeOffice.FieldName = "HomeOfficeLine";
            colHomeOffice.LayoutViewField = layoutViewField_colHomeOffice;
            colHomeOffice.MinWidth = 30;
            colHomeOffice.Name = "colHomeOffice";
            colHomeOffice.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            colHomeOffice.OptionsFilter.AllowFilter = false;
            colHomeOffice.Width = 112;
            // 
            // layoutViewField_colHomeOffice
            // 
            layoutViewField_colHomeOffice.EditorPreferredWidth = 125;
            layoutViewField_colHomeOffice.Location = new Point(129, 0);
            layoutViewField_colHomeOffice.Name = "layoutViewField_colHomeOffice";
            layoutViewField_colHomeOffice.Size = new Size(199, 43);
            layoutViewField_colHomeOffice.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            layoutViewField_colHomeOffice.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_colHomeOffice.TextSize = new Size(87, 13);
            layoutViewField_colHomeOffice.TextToControlDistance = 0;
            // 
            // colBillingAddress1
            // 
            colBillingAddress1.Caption = "BILLING ADDRESS";
            colBillingAddress1.FieldName = "BillingAddressLine";
            colBillingAddress1.LayoutViewField = layoutViewField_colBillingAddress1;
            colBillingAddress1.MinWidth = 30;
            colBillingAddress1.Name = "colBillingAddress1";
            colBillingAddress1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            colBillingAddress1.OptionsFilter.AllowFilter = false;
            colBillingAddress1.Width = 112;
            // 
            // layoutViewField_colBillingAddress1
            // 
            layoutViewField_colBillingAddress1.EditorPreferredWidth = 125;
            layoutViewField_colBillingAddress1.Location = new Point(129, 43);
            layoutViewField_colBillingAddress1.Name = "layoutViewField_colBillingAddress1";
            layoutViewField_colBillingAddress1.Size = new Size(199, 147);
            layoutViewField_colBillingAddress1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            layoutViewField_colBillingAddress1.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_colBillingAddress1.TextSize = new Size(87, 13);
            layoutViewField_colBillingAddress1.TextToControlDistance = 0;
            // 
            // colImage
            // 
            colImage.Caption = "IMAGE";
            colImage.FieldName = "Logo";
            colImage.LayoutViewField = layoutViewField_layoutViewColumn1;
            colImage.MinWidth = 30;
            colImage.Name = "colImage";
            colImage.OptionsColumn.AllowEdit = false;
            colImage.OptionsColumn.AllowFocus = false;
            colImage.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            colImage.OptionsFilter.AllowFilter = false;
            colImage.Width = 112;
            // 
            // layoutViewField_layoutViewColumn1
            // 
            layoutViewField_layoutViewColumn1.EditorPreferredWidth = 79;
            layoutViewField_layoutViewColumn1.Location = new Point(0, 0);
            layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
            layoutViewField_layoutViewColumn1.Size = new Size(129, 190);
            layoutViewField_layoutViewColumn1.TextSize = new Size(0, 0);
            layoutViewField_layoutViewColumn1.TextVisible = false;
            // 
            // layoutViewCard1
            // 
            layoutViewCard1.CustomizationFormText = "TemplateCard";
            layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_colHomeOffice, layoutViewField_colBillingAddress1, layoutViewField_layoutViewColumn1 });
            layoutViewCard1.Name = "layoutViewCard1";
            layoutViewCard1.OptionsItemText.TextToControlDistance = 5;
            layoutViewCard1.Text = "TemplateCard";
            // 
            // labelControl1
            // 
            labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            labelControl1.Dock = DockStyle.Bottom;
            labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Top;
            labelControl1.LineVisible = true;
            labelControl1.Location = new Point(0, 760);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(94, 23);
            labelControl1.TabIndex = 2;
            labelControl1.Text = "labelControl1";
            // 
            // CustomerLayoutView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Controls.Add(labelControl1);
            Name = "CustomerLayoutView";
            Size = new Size(911, 783);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colName1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colHomeOffice).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colBillingAddress1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colName1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colName1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colHomeOffice;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colHomeOffice;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colBillingAddress1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colBillingAddress1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colImage;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
