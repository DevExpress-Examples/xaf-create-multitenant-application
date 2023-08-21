using DevExpress.XtraGrid.Views.Layout;

namespace OutlookInspired.Win.UserControls
{
    partial class CustomerGridView
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
            var gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            var gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            layoutViewEmployees = new LayoutView();
            colPhoto = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colPhoto = new LayoutViewField();
            colFullName1 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colFullName1 = new LayoutViewField();
            colAddress1 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colAddress1 = new LayoutViewField();
            colEmail1 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colEmail1 = new LayoutViewField();
            colMobilePhone = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_colMobilePhone = new LayoutViewField();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridViewOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            colEmployee = new DevExpress.XtraGrid.Columns.GridColumn();
            colInvoiceNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            colOrderDate = new DevExpress.XtraGrid.Columns.GridColumn();
            colShipDate = new DevExpress.XtraGrid.Columns.GridColumn();
            colSaleAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            colShippingAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            colTotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            gridViewCustomers = new DevExpress.XtraGrid.Views.Grid.GridView();
            colName = new DevExpress.XtraGrid.Columns.GridColumn();
            colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            colCity = new DevExpress.XtraGrid.Columns.GridColumn();
            colState = new DevExpress.XtraGrid.Columns.GridColumn();
            colZipCode = new DevExpress.XtraGrid.Columns.GridColumn();
            colPhone = new DevExpress.XtraGrid.Columns.GridColumn();
            colLogo = new DevExpress.XtraGrid.Columns.GridColumn();
            layoutViewCard1 = new LayoutViewCard();
            Item1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)layoutViewEmployees).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colPhoto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colFullName1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colAddress1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colEmail1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colMobilePhone).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewCustomers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Item1).BeginInit();
            SuspendLayout();
            // 
            // layoutViewEmployees
            // 
            layoutViewEmployees.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            layoutViewEmployees.CardCaptionFormat = "{3}";
            layoutViewEmployees.CardHorzInterval = 20;
            layoutViewEmployees.CardMinSize = new Size(300, 196);
            layoutViewEmployees.CardVertInterval = 20;
            layoutViewEmployees.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] { colPhoto, colFullName1, colAddress1, colEmail1, colMobilePhone });
            layoutViewEmployees.DetailHeight = 512;
            layoutViewEmployees.FieldCaptionFormat = "{0}";
            layoutViewEmployees.GridControl = gridControl1;
            layoutViewEmployees.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_colFullName1 });
            layoutViewEmployees.Name = "layoutViewEmployees";
            layoutViewEmployees.OptionsBehavior.AllowRuntimeCustomization = false;
            layoutViewEmployees.OptionsBehavior.Editable = false;
            layoutViewEmployees.OptionsBehavior.ReadOnly = true;
            layoutViewEmployees.OptionsFind.AlwaysVisible = true;
            layoutViewEmployees.OptionsFind.FindNullPrompt = "Search Employees (Ctrl + F)";
            layoutViewEmployees.OptionsFind.ShowClearButton = false;
            layoutViewEmployees.OptionsFind.ShowCloseButton = false;
            layoutViewEmployees.OptionsFind.ShowFindButton = false;
            layoutViewEmployees.OptionsItemText.TextToControlDistance = 2;
            layoutViewEmployees.OptionsView.AllowHotTrackFields = false;
            layoutViewEmployees.OptionsView.FocusRectStyle = FocusRectStyle.None;
            layoutViewEmployees.OptionsView.ShowHeaderPanel = false;
            layoutViewEmployees.OptionsView.ViewMode = LayoutViewMode.Row;
            layoutViewEmployees.TemplateCard = layoutViewCard1;
            // 
            // colPhoto
            // 
            colPhoto.CustomizationCaption = "Photo";
            colPhoto.FieldName = "Picture.Data";
            colPhoto.LayoutViewField = layoutViewField_colPhoto;
            colPhoto.MinWidth = 30;
            colPhoto.Name = "colPhoto";
            colPhoto.OptionsColumn.AllowEdit = false;
            colPhoto.OptionsColumn.AllowFocus = false;
            colPhoto.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            colPhoto.OptionsFilter.AllowFilter = false;
            colPhoto.Width = 112;
            // 
            // layoutViewField_colPhoto
            // 
            layoutViewField_colPhoto.EditorPreferredWidth = 106;
            layoutViewField_colPhoto.Location = new Point(0, 0);
            layoutViewField_colPhoto.MaxSize = new Size(120, 136);
            layoutViewField_colPhoto.MinSize = new Size(120, 136);
            layoutViewField_colPhoto.Name = "layoutViewField_colPhoto";
            layoutViewField_colPhoto.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 12, 2, 2);
            layoutViewField_colPhoto.Size = new Size(120, 186);
            layoutViewField_colPhoto.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutViewField_colPhoto.TextSize = new Size(0, 0);
            layoutViewField_colPhoto.TextVisible = false;
            // 
            // colFullName1
            // 
            colFullName1.CustomizationCaption = "Full Name";
            colFullName1.FieldName = "FullName";
            colFullName1.LayoutViewField = layoutViewField_colFullName1;
            colFullName1.MinWidth = 30;
            colFullName1.Name = "colFullName1";
            colFullName1.OptionsColumn.AllowFocus = false;
            colFullName1.Width = 112;
            // 
            // layoutViewField_colFullName1
            // 
            layoutViewField_colFullName1.EditorPreferredWidth = 20;
            layoutViewField_colFullName1.Location = new Point(0, 0);
            layoutViewField_colFullName1.Name = "layoutViewField_colFullName1";
            layoutViewField_colFullName1.Size = new Size(416, 233);
            layoutViewField_colFullName1.TextSize = new Size(67, 13);
            // 
            // colAddress1
            // 
            colAddress1.Caption = "ADDRESS";
            colAddress1.CustomizationCaption = "ADDRESS";
            colAddress1.FieldName = "Address";
            colAddress1.LayoutViewField = layoutViewField_colAddress1;
            colAddress1.MinWidth = 30;
            colAddress1.Name = "colAddress1";
            colAddress1.OptionsColumn.AllowFocus = false;
            colAddress1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            colAddress1.OptionsFilter.AllowFilter = false;
            colAddress1.Width = 112;
            // 
            // layoutViewField_colAddress1
            // 
            layoutViewField_colAddress1.EditorPreferredWidth = 157;
            layoutViewField_colAddress1.Location = new Point(120, 0);
            layoutViewField_colAddress1.MaxSize = new Size(152, 62);
            layoutViewField_colAddress1.MinSize = new Size(152, 62);
            layoutViewField_colAddress1.Name = "layoutViewField_colAddress1";
            layoutViewField_colAddress1.Size = new Size(161, 62);
            layoutViewField_colAddress1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutViewField_colAddress1.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_colAddress1.TextSize = new Size(70, 19);
            // 
            // colEmail1
            // 
            colEmail1.Caption = "EMAIL";
            colEmail1.CustomizationCaption = "EMAIL";
            colEmail1.FieldName = "Email";
            colEmail1.LayoutViewField = layoutViewField_colEmail1;
            colEmail1.MinWidth = 30;
            colEmail1.Name = "colEmail1";
            colEmail1.OptionsColumn.AllowFocus = false;
            colEmail1.OptionsFilter.AllowFilter = false;
            colEmail1.Width = 112;
            // 
            // layoutViewField_colEmail1
            // 
            layoutViewField_colEmail1.EditorPreferredWidth = 157;
            layoutViewField_colEmail1.Location = new Point(120, 62);
            layoutViewField_colEmail1.Name = "layoutViewField_colEmail1";
            layoutViewField_colEmail1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 8, 2);
            layoutViewField_colEmail1.Size = new Size(161, 57);
            layoutViewField_colEmail1.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_colEmail1.TextSize = new Size(70, 19);
            // 
            // colMobilePhone
            // 
            colMobilePhone.Caption = "PHONE";
            colMobilePhone.CustomizationCaption = "PHONE";
            colMobilePhone.FieldName = "MobilePhone";
            colMobilePhone.LayoutViewField = layoutViewField_colMobilePhone;
            colMobilePhone.MinWidth = 30;
            colMobilePhone.Name = "colMobilePhone";
            colMobilePhone.OptionsColumn.AllowFocus = false;
            colMobilePhone.OptionsFilter.AllowFilter = false;
            colMobilePhone.Width = 112;
            // 
            // layoutViewField_colMobilePhone
            // 
            layoutViewField_colMobilePhone.EditorPreferredWidth = 157;
            layoutViewField_colMobilePhone.Location = new Point(120, 119);
            layoutViewField_colMobilePhone.Name = "layoutViewField_colMobilePhone";
            layoutViewField_colMobilePhone.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 8, 2);
            layoutViewField_colMobilePhone.Size = new Size(161, 57);
            layoutViewField_colMobilePhone.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_colMobilePhone.TextSize = new Size(70, 19);
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridLevelNode1.LevelTemplate = layoutViewEmployees;
            gridLevelNode1.RelationName = "Employees";
            gridLevelNode2.LevelTemplate = gridViewOrders;
            gridLevelNode2.RelationName = "Orders";
            gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] { gridLevelNode1, gridLevelNode2 });
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = gridViewCustomers;
            gridControl1.Name = "gridControl1";
            gridControl1.ShowOnlyPredefinedDetails = true;
            gridControl1.Size = new Size(1423, 1018);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridViewOrders, gridViewCustomers, layoutViewEmployees });
            // 
            // gridViewOrders
            // 
            gridViewOrders.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            gridViewOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colEmployee, colInvoiceNumber, colOrderDate, colShipDate, colSaleAmount, colShippingAmount, colTotalAmount });
            gridViewOrders.DetailHeight = 512;
            gridViewOrders.FixedLineWidth = 1;
            gridViewOrders.GridControl = gridControl1;
            gridViewOrders.Name = "gridViewOrders";
            gridViewOrders.OptionsEditForm.PopupEditFormWidth = 1200;
            gridViewOrders.OptionsView.ShowFooter = true;
            gridViewOrders.OptionsView.ShowGroupPanel = false;
            gridViewOrders.OptionsView.ShowIndicator = false;
            gridViewOrders.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            // 
            // colEmployee
            // 
            colEmployee.Caption = "EMPLOYEE";
            colEmployee.FieldName = "Employee.FullName";
            colEmployee.Name = "colEmployee";
            colEmployee.OptionsColumn.AllowFocus = false;
            colEmployee.Visible = true;
            colEmployee.VisibleIndex = 1;
            colEmployee.Width = 317;
            // 
            // colInvoiceNumber
            // 
            colInvoiceNumber.Caption = "INVOICE NUMBER";
            colInvoiceNumber.FieldName = "InvoiceNumber";
            colInvoiceNumber.Name = "colInvoiceNumber";
            colInvoiceNumber.OptionsColumn.AllowFocus = false;
            colInvoiceNumber.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "InvoiceNumber", "{0}") });
            colInvoiceNumber.Visible = true;
            colInvoiceNumber.VisibleIndex = 0;
            colInvoiceNumber.Width = 161;
            // 
            // colOrderDate
            // 
            colOrderDate.Caption = "ORDER DATE";
            colOrderDate.FieldName = "OrderDate";
            colOrderDate.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.DateRange;
            colOrderDate.Name = "colOrderDate";
            colOrderDate.OptionsColumn.AllowFocus = false;
            colOrderDate.Visible = true;
            colOrderDate.VisibleIndex = 2;
            colOrderDate.Width = 161;
            // 
            // colShipDate
            // 
            colShipDate.Caption = "SHIP DATE";
            colShipDate.FieldName = "ShipDate";
            colShipDate.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.DateRange;
            colShipDate.Name = "colShipDate";
            colShipDate.OptionsColumn.AllowFocus = false;
            colShipDate.Visible = true;
            colShipDate.VisibleIndex = 3;
            colShipDate.Width = 161;
            // 
            // colSaleAmount
            // 
            colSaleAmount.Caption = "SALE AMOUNT";
            colSaleAmount.DisplayFormat.FormatString = "c";
            colSaleAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colSaleAmount.FieldName = "SaleAmount";
            colSaleAmount.Name = "colSaleAmount";
            colSaleAmount.OptionsColumn.AllowFocus = false;
            colSaleAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "SaleAmount", "SUM={0:c}") });
            colSaleAmount.Visible = true;
            colSaleAmount.VisibleIndex = 4;
            colSaleAmount.Width = 161;
            // 
            // colShippingAmount
            // 
            colShippingAmount.Caption = "SHIPPING AMOUNT";
            colShippingAmount.DisplayFormat.FormatString = "c";
            colShippingAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colShippingAmount.FieldName = "ShippingAmount";
            colShippingAmount.Name = "colShippingAmount";
            colShippingAmount.OptionsColumn.AllowFocus = false;
            colShippingAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ShippingAmount", "SUM={0:c}") });
            colShippingAmount.Visible = true;
            colShippingAmount.VisibleIndex = 5;
            colShippingAmount.Width = 161;
            // 
            // colTotalAmount
            // 
            colTotalAmount.Caption = "TOTAL AMOUNT";
            colTotalAmount.DisplayFormat.FormatString = "c";
            colTotalAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colTotalAmount.FieldName = "TotalAmount";
            colTotalAmount.Name = "colTotalAmount";
            colTotalAmount.OptionsColumn.AllowFocus = false;
            colTotalAmount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TotalAmount", "SUM={0:c}") });
            colTotalAmount.Visible = true;
            colTotalAmount.VisibleIndex = 6;
            colTotalAmount.Width = 167;
            // 
            // gridViewCustomers
            // 
            gridViewCustomers.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colName, colAddress, colCity, colState, colZipCode, colPhone, colLogo });
            gridViewCustomers.DetailHeight = 512;
            gridViewCustomers.FixedLineWidth = 1;
            gridViewCustomers.GridControl = gridControl1;
            gridViewCustomers.Name = "gridViewCustomers";
            gridViewCustomers.OptionsBehavior.AllowPixelScrolling = DevExpress.Utils.DefaultBoolean.False;
            gridViewCustomers.OptionsBehavior.AutoExpandAllGroups = true;
            gridViewCustomers.OptionsBehavior.Editable = false;
            gridViewCustomers.OptionsBehavior.ReadOnly = true;
            gridViewCustomers.OptionsDetail.AllowZoomDetail = false;
            gridViewCustomers.OptionsDetail.SmartDetailExpandButtonMode = DevExpress.XtraGrid.Views.Grid.DetailExpandButtonMode.CheckAllDetails;
            gridViewCustomers.OptionsDetail.SmartDetailHeight = true;
            gridViewCustomers.OptionsEditForm.PopupEditFormWidth = 1200;
            gridViewCustomers.OptionsFilter.ColumnFilterPopupMode = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode.Excel;
            gridViewCustomers.OptionsFind.AlwaysVisible = true;
            gridViewCustomers.OptionsFind.FindNullPrompt = "Search Customers (Ctrl + F)";
            gridViewCustomers.OptionsFind.ShowClearButton = false;
            gridViewCustomers.OptionsFind.ShowFindButton = false;
            gridViewCustomers.OptionsSelection.MultiSelect = true;
            gridViewCustomers.OptionsView.ShowFooter = true;
            gridViewCustomers.OptionsView.ShowGroupPanel = false;
            gridViewCustomers.OptionsView.ShowIndicator = false;
            gridViewCustomers.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridViewCustomers.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] { new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colName, DevExpress.Data.ColumnSortOrder.Ascending) });
            // 
            // colName
            // 
            colName.Caption = "CUSTOMER";
            colName.FieldName = "Name";
            colName.Name = "colName";
            colName.OptionsColumn.AllowFocus = false;
            colName.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Name", "{0}") });
            colName.Visible = true;
            colName.VisibleIndex = 1;
            colName.Width = 222;
            // 
            // colAddress
            // 
            colAddress.Caption = "ADDRESS";
            colAddress.FieldName = "HomeOfficeLine";
            colAddress.Name = "colAddress";
            colAddress.OptionsColumn.AllowFocus = false;
            colAddress.Visible = true;
            colAddress.VisibleIndex = 2;
            colAddress.Width = 113;
            // 
            // colCity
            // 
            colCity.Caption = "CITY";
            colCity.FieldName = "HomeOfficeCity";
            colCity.Name = "colCity";
            colCity.OptionsColumn.AllowFocus = false;
            colCity.Visible = true;
            colCity.VisibleIndex = 4;
            colCity.Width = 71;
            // 
            // colState
            // 
            colState.Caption = "STATE";
            colState.FieldName = "HomeOfficeState";
            colState.Name = "colState";
            colState.OptionsColumn.AllowFocus = false;
            colState.Visible = true;
            colState.VisibleIndex = 3;
            colState.Width = 68;
            // 
            // colZipCode
            // 
            colZipCode.Caption = "ZIP CODE";
            colZipCode.FieldName = "HomeOfficeZipCode";
            colZipCode.Name = "colZipCode";
            colZipCode.OptionsColumn.AllowFocus = false;
            colZipCode.Visible = true;
            colZipCode.VisibleIndex = 5;
            colZipCode.Width = 90;
            // 
            // colPhone
            // 
            colPhone.Caption = "PHONE";
            colPhone.FieldName = "Phone";
            colPhone.Name = "colPhone";
            colPhone.OptionsColumn.AllowFocus = false;
            colPhone.Visible = true;
            colPhone.VisibleIndex = 6;
            colPhone.Width = 156;
            // 
            // colLogo
            // 
            colLogo.Caption = "LOGO";
            colLogo.FieldName = "Logo";
            colLogo.ImageOptions.Alignment = StringAlignment.Center;
            colLogo.Name = "colLogo";
            colLogo.OptionsColumn.AllowFocus = false;
            colLogo.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            colLogo.OptionsColumn.AllowSize = false;
            colLogo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            colLogo.OptionsColumn.FixedWidth = true;
            colLogo.OptionsColumn.ShowCaption = false;
            colLogo.OptionsFilter.AllowFilter = false;
            colLogo.Visible = true;
            colLogo.VisibleIndex = 0;
            colLogo.Width = 57;
            // 
            // layoutViewCard1
            // 
            layoutViewCard1.CustomizationFormText = "TemplateCard";
            layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_colAddress1, layoutViewField_colEmail1, layoutViewField_colPhoto, layoutViewField_colMobilePhone, Item1 });
            layoutViewCard1.Name = "layoutViewCard1";
            layoutViewCard1.OptionsItemText.TextToControlDistance = 2;
            layoutViewCard1.Text = "TemplateCard";
            // 
            // Item1
            // 
            Item1.AllowHotTrack = false;
            Item1.CustomizationFormText = "Item1";
            Item1.Location = new Point(120, 176);
            Item1.Name = "Item1";
            Item1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            Item1.Size = new Size(161, 10);
            Item1.TextSize = new Size(0, 0);
            // 
            // CustomerGridView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Name = "CustomerGridView";
            Size = new Size(1423, 1018);
            ((System.ComponentModel.ISupportInitialize)layoutViewEmployees).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colPhoto).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colFullName1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colAddress1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colEmail1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colMobilePhone).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewCustomers).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Item1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOrders;
        private DevExpress.XtraGrid.Columns.GridColumn colEmployee;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderDate;
        private DevExpress.XtraGrid.Columns.GridColumn colShipDate;
        private DevExpress.XtraGrid.Columns.GridColumn colSaleAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colShippingAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmount;
        private LayoutView layoutViewEmployees;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewCustomers;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colAddress;
        private DevExpress.XtraGrid.Columns.GridColumn colCity;
        private DevExpress.XtraGrid.Columns.GridColumn colState;
        private DevExpress.XtraGrid.Columns.GridColumn colZipCode;
        private DevExpress.XtraGrid.Columns.GridColumn colPhone;
        private DevExpress.XtraGrid.Columns.GridColumn colLogo;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colPhoto;
        private LayoutViewField layoutViewField_colPhoto;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colFullName1;
        private LayoutViewField layoutViewField_colFullName1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colAddress1;
        private LayoutViewField layoutViewField_colAddress1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colEmail1;
        private LayoutViewField layoutViewField_colEmail1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colMobilePhone;
        private LayoutViewField layoutViewField_colMobilePhone;
        private LayoutViewCard layoutViewCard1;
        private DevExpress.XtraLayout.EmptySpaceItem Item1;
    }
}
