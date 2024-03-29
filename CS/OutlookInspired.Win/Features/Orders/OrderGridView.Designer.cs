﻿namespace OutlookInspired.Win.Features.Orders
{
    partial class OrderGridView
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
            gridViewOrderItems = new DevExpress.XtraGrid.Views.Grid.GridView();
            colProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            colProductUnits = new DevExpress.XtraGrid.Columns.GridColumn();
            colProductPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            colDiscount = new DevExpress.XtraGrid.Columns.GridColumn();
            colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            colId = new DevExpress.XtraGrid.Columns.GridColumn();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colPaymentStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            colShipmentStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            colInvoiceNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            colOrderDate = new DevExpress.XtraGrid.Columns.GridColumn();
            colTotalAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            colStore = new DevExpress.XtraGrid.Columns.GridColumn();
            colCustomer = new DevExpress.XtraGrid.Columns.GridColumn();
            colShipDate1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colShippingAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            colPaymentTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            colRefundTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            colShipmentCourier = new DevExpress.XtraGrid.Columns.GridColumn();
            colId1 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)gridViewOrderItems).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            SuspendLayout();
            // 
            // gridViewOrderItems
            // 
            gridViewOrderItems.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colProduct, colProductUnits, colProductPrice, colDiscount, colTotal, colId });
            gridViewOrderItems.DetailHeight = 614;
            gridViewOrderItems.FixedLineWidth = 1;
            gridViewOrderItems.GridControl = gridControl1;
            gridViewOrderItems.Name = "gridViewOrderItems";
            gridViewOrderItems.OptionsEditForm.PopupEditFormWidth = 1440;
            gridViewOrderItems.OptionsView.AllowHtmlDrawHeaders = true;
            gridViewOrderItems.OptionsView.ShowFooter = true;
            gridViewOrderItems.OptionsView.ShowGroupPanel = false;
            gridViewOrderItems.OptionsView.ShowIndicator = false;
            gridViewOrderItems.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            // 
            // colProduct
            // 
            colProduct.Caption = "<b>PRODUCT</b>";
            colProduct.FieldName = "Product.Name";
            colProduct.MinWidth = 24;
            colProduct.Name = "colProduct";
            colProduct.OptionsColumn.AllowFocus = false;
            colProduct.Visible = true;
            colProduct.VisibleIndex = 0;
            colProduct.Width = 90;
            // 
            // colProductUnits
            // 
            colProductUnits.Caption = "UNITS";
            colProductUnits.FieldName = "ProductUnits";
            colProductUnits.MinWidth = 24;
            colProductUnits.Name = "colProductUnits";
            colProductUnits.OptionsColumn.AllowFocus = false;
            colProductUnits.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ProductUnits", "SUM={0}") });
            colProductUnits.Visible = true;
            colProductUnits.VisibleIndex = 1;
            colProductUnits.Width = 90;
            // 
            // colProductPrice
            // 
            colProductPrice.Caption = "UNIT PRICE";
            colProductPrice.DisplayFormat.FormatString = "c";
            colProductPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colProductPrice.FieldName = "ProductPrice";
            colProductPrice.MinWidth = 24;
            colProductPrice.Name = "colProductPrice";
            colProductPrice.OptionsColumn.AllowFocus = false;
            colProductPrice.Visible = true;
            colProductPrice.VisibleIndex = 2;
            colProductPrice.Width = 90;
            // 
            // colDiscount
            // 
            colDiscount.Caption = "DISCOUNT";
            colDiscount.DisplayFormat.FormatString = "c";
            colDiscount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colDiscount.FieldName = "Discount";
            colDiscount.MinWidth = 24;
            colDiscount.Name = "colDiscount";
            colDiscount.OptionsColumn.AllowFocus = false;
            colDiscount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Average, "Discount", "AVG={0:c}") });
            colDiscount.Visible = true;
            colDiscount.VisibleIndex = 3;
            colDiscount.Width = 90;
            // 
            // colTotal
            // 
            colTotal.Caption = "TOTAL";
            colTotal.DisplayFormat.FormatString = "c";
            colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colTotal.FieldName = "Total";
            colTotal.MinWidth = 24;
            colTotal.Name = "colTotal";
            colTotal.OptionsColumn.AllowFocus = false;
            colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "SUM={0:c}") });
            colTotal.Visible = true;
            colTotal.VisibleIndex = 4;
            colTotal.Width = 90;
            // 
            // colId
            // 
            colId.FieldName = "Id";
            colId.MinWidth = 24;
            colId.Name = "colId";
            colId.OptionsColumn.AllowFocus = false;
            colId.Width = 90;
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.EmbeddedNavigator.Margin = new Padding(4, 4, 4, 4);
            gridLevelNode1.LevelTemplate = gridViewOrderItems;
            gridLevelNode1.RelationName = "OrderItems";
            gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] { gridLevelNode1 });
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = gridView1;
            gridControl1.Margin = new Padding(4, 4, 4, 4);
            gridControl1.Name = "gridControl1";
            gridControl1.ShowOnlyPredefinedDetails = true;
            gridControl1.Size = new Size(1577, 1223);
            gridControl1.TabIndex = 1;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1, gridViewOrderItems });
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colPaymentStatus, colShipmentStatus, colInvoiceNumber, colOrderDate, colTotalAmount, colStore, colCustomer, colShipDate1, colShippingAmount, colPaymentTotal, colRefundTotal, colShipmentCourier, colId1 });
            gridView1.DetailHeight = 614;
            gridView1.FixedLineWidth = 1;
            gridView1.GridControl = gridControl1;
            gridView1.GroupCount = 1;
            gridView1.Name = "gridView1";
            gridView1.OptionsBehavior.AllowPixelScrolling = DevExpress.Utils.DefaultBoolean.False;
            gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsDetail.AllowZoomDetail = false;
            gridView1.OptionsDetail.DetailMode = DevExpress.XtraGrid.Views.Grid.DetailMode.Embedded;
            gridView1.OptionsDetail.ShowDetailTabs = false;
            gridView1.OptionsDetail.SmartDetailExpandButtonMode = DevExpress.XtraGrid.Views.Grid.DetailExpandButtonMode.AlwaysEnabled;
            gridView1.OptionsDetail.SmartDetailHeight = true;
            gridView1.OptionsEditForm.PopupEditFormWidth = 1440;
            gridView1.OptionsFind.AlwaysVisible = true;
            gridView1.OptionsFind.FindNullPrompt = "Search Orders (Ctrl + F)";
            gridView1.OptionsFind.ShowClearButton = false;
            gridView1.OptionsFind.ShowFindButton = false;
            gridView1.OptionsMenu.ShowConditionalFormattingItem = true;
            gridView1.OptionsView.ShowFooter = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] { new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colCustomer, DevExpress.Data.ColumnSortOrder.Ascending), new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colOrderDate, DevExpress.Data.ColumnSortOrder.Ascending) });
            // 
            // colPaymentStatus
            // 
            colPaymentStatus.FieldName = "PaymentStatusImage";
            colPaymentStatus.MinWidth = 24;
            colPaymentStatus.Name = "colPaymentStatus";
            colPaymentStatus.OptionsColumn.AllowFocus = false;
            colPaymentStatus.OptionsColumn.AllowMove = false;
            colPaymentStatus.OptionsColumn.FixedWidth = true;
            colPaymentStatus.OptionsColumn.ShowCaption = false;
            colPaymentStatus.Visible = true;
            colPaymentStatus.VisibleIndex = 0;
            colPaymentStatus.Width = 48;
            // 
            // colShipmentStatus
            // 
            colShipmentStatus.FieldName = "ShipmentStatusImage";
            colShipmentStatus.MinWidth = 24;
            colShipmentStatus.Name = "colShipmentStatus";
            colShipmentStatus.OptionsColumn.AllowFocus = false;
            colShipmentStatus.OptionsColumn.AllowMove = false;
            colShipmentStatus.OptionsColumn.FixedWidth = true;
            colShipmentStatus.OptionsColumn.ShowCaption = false;
            colShipmentStatus.Visible = true;
            colShipmentStatus.VisibleIndex = 1;
            colShipmentStatus.Width = 48;
            // 
            // colInvoiceNumber
            // 
            colInvoiceNumber.Caption = "INVOICE #";
            colInvoiceNumber.FieldName = "InvoiceNumber";
            colInvoiceNumber.MinWidth = 24;
            colInvoiceNumber.Name = "colInvoiceNumber";
            colInvoiceNumber.OptionsColumn.AllowFocus = false;
            colInvoiceNumber.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "InvoiceNumber", "{0}") });
            colInvoiceNumber.Visible = true;
            colInvoiceNumber.VisibleIndex = 2;
            colInvoiceNumber.Width = 84;
            // 
            // colOrderDate
            // 
            colOrderDate.Caption = "ORDER DATE";
            colOrderDate.FieldName = "OrderDate";
            colOrderDate.MinWidth = 24;
            colOrderDate.Name = "colOrderDate";
            colOrderDate.OptionsColumn.AllowFocus = false;
            colOrderDate.Visible = true;
            colOrderDate.VisibleIndex = 3;
            colOrderDate.Width = 94;
            // 
            // colTotalAmount
            // 
            colTotalAmount.Caption = "ORDER TOTAL";
            colTotalAmount.DisplayFormat.FormatString = "c";
            colTotalAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colTotalAmount.FieldName = "TotalAmount";
            colTotalAmount.MinWidth = 24;
            colTotalAmount.Name = "colTotalAmount";
            colTotalAmount.OptionsColumn.AllowFocus = false;
            colTotalAmount.Visible = true;
            colTotalAmount.VisibleIndex = 7;
            colTotalAmount.Width = 108;
            // 
            // colStore
            // 
            colStore.Caption = "STORE";
            colStore.FieldName = "Store.Crest.CityName";
            colStore.MinWidth = 24;
            colStore.Name = "colStore";
            colStore.OptionsColumn.AllowFocus = false;
            colStore.Visible = true;
            colStore.VisibleIndex = 5;
            colStore.Width = 96;
            // 
            // colCustomer
            // 
            colCustomer.Caption = "COMPANY";
            colCustomer.FieldName = "Customer.Name";
            colCustomer.MinWidth = 24;
            colCustomer.Name = "colCustomer";
            colCustomer.OptionsColumn.AllowFocus = false;
            colCustomer.Visible = true;
            colCustomer.VisibleIndex = 5;
            colCustomer.Width = 106;
            // 
            // colShipDate1
            // 
            colShipDate1.Caption = "SHIP DATE";
            colShipDate1.FieldName = "ShipDate";
            colShipDate1.MinWidth = 24;
            colShipDate1.Name = "colShipDate1";
            colShipDate1.OptionsColumn.AllowFocus = false;
            colShipDate1.Visible = true;
            colShipDate1.VisibleIndex = 4;
            colShipDate1.Width = 94;
            // 
            // colShippingAmount
            // 
            colShippingAmount.Caption = "SHIPPING AMOUNT";
            colShippingAmount.DisplayFormat.FormatString = "c";
            colShippingAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colShippingAmount.FieldName = "ShippingAmount";
            colShippingAmount.MinWidth = 24;
            colShippingAmount.Name = "colShippingAmount";
            colShippingAmount.OptionsColumn.AllowFocus = false;
            colShippingAmount.Visible = true;
            colShippingAmount.VisibleIndex = 6;
            colShippingAmount.Width = 132;
            // 
            // colPaymentTotal
            // 
            colPaymentTotal.Caption = "PAYMENT TOTAL";
            colPaymentTotal.DisplayFormat.FormatString = "c";
            colPaymentTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colPaymentTotal.FieldName = "PaymentTotal";
            colPaymentTotal.MinWidth = 24;
            colPaymentTotal.Name = "colPaymentTotal";
            colPaymentTotal.OptionsColumn.AllowFocus = false;
            colPaymentTotal.Visible = true;
            colPaymentTotal.VisibleIndex = 8;
            colPaymentTotal.Width = 108;
            // 
            // colRefundTotal
            // 
            colRefundTotal.Caption = "REFUND TOTAL";
            colRefundTotal.DisplayFormat.FormatString = "c";
            colRefundTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colRefundTotal.FieldName = "RefundTotal";
            colRefundTotal.MinWidth = 24;
            colRefundTotal.Name = "colRefundTotal";
            colRefundTotal.OptionsColumn.AllowFocus = false;
            colRefundTotal.Width = 90;
            // 
            // colShipmentCourier
            // 
            colShipmentCourier.Caption = "SHIPMENT COURIER";
            colShipmentCourier.FieldName = "ShipmentCourier";
            colShipmentCourier.MinWidth = 24;
            colShipmentCourier.Name = "colShipmentCourier";
            colShipmentCourier.OptionsColumn.AllowFocus = false;
            colShipmentCourier.Width = 90;
            // 
            // colId1
            // 
            colId1.FieldName = "Id";
            colId1.MinWidth = 24;
            colId1.Name = "colId1";
            colId1.Width = 90;
            // 
            // OrderGridView
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "OrderGridView";
            Size = new Size(1577, 1223);
            ((System.ComponentModel.ISupportInitialize)gridViewOrderItems).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colPaymentStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colShipmentStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderDate;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colStore;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomer;
        private DevExpress.XtraGrid.Columns.GridColumn colShipDate1;
        private DevExpress.XtraGrid.Columns.GridColumn colShippingAmount;
        private DevExpress.XtraGrid.Columns.GridColumn colPaymentTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colRefundTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colShipmentCourier;
        private DevExpress.XtraGrid.Columns.GridColumn colId1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOrderItems;
        private DevExpress.XtraGrid.Columns.GridColumn colProduct;
        private DevExpress.XtraGrid.Columns.GridColumn colProductUnits;
        private DevExpress.XtraGrid.Columns.GridColumn colProductPrice;
        private DevExpress.XtraGrid.Columns.GridColumn colDiscount;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
    }
}
