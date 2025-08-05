namespace OutlookInspired.Win.Features.Products
{
    partial class ProductCardView
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
            colImage = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colImage = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            colName = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colName = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            colDescription = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            layoutViewField_colDescription = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            layoutViewColumnCost = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_layoutViewColumn1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            layoutViewColumnSalesPrice = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_layoutViewColumn1_1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemMemoEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colDescription).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1_1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = layoutView1;
            gridControl1.Name = "gridControl1";
            gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemMemoEdit1 });
            gridControl1.Size = new Size(781, 602);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { layoutView1 });
            // 
            // layoutView1
            // 
            layoutView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            layoutView1.CardMinSize = new Size(241, 227);
            layoutView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] { colImage, colName, colDescription, layoutViewColumnCost, layoutViewColumnSalesPrice });
            layoutView1.DetailHeight = 409;
            layoutView1.GridControl = gridControl1;
            layoutView1.Name = "layoutView1";
            layoutView1.OptionsBehavior.AllowExpandCollapse = false;
            layoutView1.OptionsBehavior.AllowRuntimeCustomization = false;
            layoutView1.OptionsBehavior.Editable = false;
            layoutView1.OptionsBehavior.ReadOnly = true;
            layoutView1.OptionsFilter.AllowFilterEditor = false;
            layoutView1.OptionsFind.AlwaysVisible = true;
            layoutView1.OptionsFind.ShowFindButton = false;
            layoutView1.OptionsFind.ShowSearchNavButtons = false;
            layoutView1.OptionsHeaderPanel.EnableCustomizeButton = false;
            layoutView1.OptionsView.AllowHotTrackFields = false;
            layoutView1.OptionsView.FocusRectStyle = DevExpress.XtraGrid.Views.Layout.FocusRectStyle.None;
            layoutView1.OptionsView.ShowCardCaption = false;
            layoutView1.OptionsView.ShowHeaderPanel = false;
            layoutView1.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.MultiRow;
            layoutView1.TemplateCard = layoutViewCard1;
            // 
            // colImage
            // 
            colImage.FieldName = "PrimaryImage.Data";
            colImage.LayoutViewField = layoutViewField_colImage;
            colImage.MinWidth = 16;
            colImage.Name = "colImage";
            colImage.OptionsColumn.AllowEdit = false;
            colImage.OptionsColumn.AllowFocus = false;
            colImage.OptionsFilter.AllowFilter = false;
            colImage.Width = 43;
            // 
            // layoutViewField_colImage
            // 
            layoutViewField_colImage.EditorPreferredWidth = 139;
            layoutViewField_colImage.Location = new Point(0, 24);
            layoutViewField_colImage.Name = "layoutViewField_colImage";
            layoutViewField_colImage.Size = new Size(142, 86);
            layoutViewField_colImage.TextVisible = false;
            // 
            // colName
            // 
            colName.AppearanceCell.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            colName.AppearanceCell.Options.UseFont = true;
            colName.FieldName = "Name";
            colName.LayoutViewField = layoutViewField_colName;
            colName.MinWidth = 16;
            colName.Name = "colName";
            colName.OptionsColumn.AllowEdit = false;
            colName.OptionsColumn.AllowFocus = false;
            colName.OptionsFilter.AllowFilter = false;
            colName.Width = 175;
            // 
            // layoutViewField_colName
            // 
            layoutViewField_colName.EditorPreferredWidth = 223;
            layoutViewField_colName.Location = new Point(0, 0);
            layoutViewField_colName.Name = "layoutViewField_colName";
            layoutViewField_colName.Size = new Size(225, 24);
            layoutViewField_colName.TextVisible = false;
            // 
            // colDescription
            // 
            colDescription.AppearanceCell.Options.UseTextOptions = true;
            colDescription.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            colDescription.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            colDescription.ColumnEdit = repositoryItemMemoEdit1;
            colDescription.FieldName = "DescriptionString";
            colDescription.LayoutViewField = layoutViewField_colDescription;
            colDescription.MinWidth = 16;
            colDescription.Name = "colDescription";
            colDescription.OptionsColumn.AllowEdit = false;
            colDescription.OptionsColumn.AllowFocus = false;
            colDescription.OptionsFilter.AllowFilter = false;
            colDescription.Width = 175;
            // 
            // repositoryItemMemoEdit1
            // 
            repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // layoutViewField_colDescription
            // 
            layoutViewField_colDescription.EditorPreferredWidth = 223;
            layoutViewField_colDescription.Location = new Point(0, 110);
            layoutViewField_colDescription.MaxSize = new Size(0, 71);
            layoutViewField_colDescription.MinSize = new Size(19, 71);
            layoutViewField_colDescription.Name = "layoutViewField_colDescription";
            layoutViewField_colDescription.Size = new Size(225, 71);
            layoutViewField_colDescription.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutViewField_colDescription.TextVisible = false;
            // 
            // layoutViewColumnCost
            // 
            layoutViewColumnCost.AppearanceCell.Options.UseTextOptions = true;
            layoutViewColumnCost.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            layoutViewColumnCost.Caption = "COST";
            layoutViewColumnCost.FieldName = "Cost";
            layoutViewColumnCost.LayoutViewField = layoutViewField_layoutViewColumn1;
            layoutViewColumnCost.MinWidth = 24;
            layoutViewColumnCost.Name = "layoutViewColumnCost";
            layoutViewColumnCost.OptionsFilter.AllowFilter = false;
            layoutViewColumnCost.Width = 89;
            // 
            // layoutViewField_layoutViewColumn1
            // 
            layoutViewField_layoutViewColumn1.AppearanceItemCaption.ForeColor = Color.Gray;
            layoutViewField_layoutViewColumn1.AppearanceItemCaption.Options.UseForeColor = true;
            layoutViewField_layoutViewColumn1.EditorPreferredWidth = 80;
            layoutViewField_layoutViewColumn1.Location = new Point(142, 24);
            layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
            layoutViewField_layoutViewColumn1.Size = new Size(83, 43);
            layoutViewField_layoutViewColumn1.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_layoutViewColumn1.TextSize = new Size(72, 16);
            // 
            // layoutViewColumnSalesPrice
            // 
            layoutViewColumnSalesPrice.AppearanceCell.Options.UseTextOptions = true;
            layoutViewColumnSalesPrice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            layoutViewColumnSalesPrice.Caption = "SALE PRICE";
            layoutViewColumnSalesPrice.FieldName = "SalePrice";
            layoutViewColumnSalesPrice.LayoutViewField = layoutViewField_layoutViewColumn1_1;
            layoutViewColumnSalesPrice.MinWidth = 24;
            layoutViewColumnSalesPrice.Name = "layoutViewColumnSalesPrice";
            layoutViewColumnSalesPrice.OptionsFilter.AllowFilter = false;
            layoutViewColumnSalesPrice.Width = 89;
            // 
            // layoutViewField_layoutViewColumn1_1
            // 
            layoutViewField_layoutViewColumn1_1.AppearanceItemCaption.ForeColor = Color.Gray;
            layoutViewField_layoutViewColumn1_1.AppearanceItemCaption.Options.UseForeColor = true;
            layoutViewField_layoutViewColumn1_1.EditorPreferredWidth = 80;
            layoutViewField_layoutViewColumn1_1.Location = new Point(142, 67);
            layoutViewField_layoutViewColumn1_1.Name = "layoutViewField_layoutViewColumn1_1";
            layoutViewField_layoutViewColumn1_1.Size = new Size(83, 43);
            layoutViewField_layoutViewColumn1_1.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_layoutViewColumn1_1.TextSize = new Size(72, 16);
            // 
            // layoutViewCard1
            // 
            layoutViewCard1.CustomizationFormText = "TemplateCard";
            layoutViewCard1.GroupBordersVisible = false;
            layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_colImage, layoutViewField_colName, layoutViewField_layoutViewColumn1, layoutViewField_layoutViewColumn1_1, layoutViewField_colDescription });
            layoutViewCard1.Name = "layoutViewCard1";
            layoutViewCard1.OptionsItemText.TextToControlDistance = 5;
            layoutViewCard1.Text = "TemplateCard";
            layoutViewCard1.TextVisible = false;
            // 
            // labelControl1
            // 
            labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            labelControl1.Dock = DockStyle.Bottom;
            labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Top;
            labelControl1.LineVisible = true;
            labelControl1.Location = new Point(0, 602);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(75, 19);
            labelControl1.TabIndex = 2;
            labelControl1.Text = "labelControl1";
            // 
            // ProductCardView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Controls.Add(labelControl1);
            Name = "ProductCardView";
            Size = new Size(781, 621);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colName).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemMemoEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colDescription).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1_1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colImage;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colName;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colDescription;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn layoutViewColumnCost;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn layoutViewColumnSalesPrice;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colImage;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colName;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colDescription;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn1_1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
    }
}
