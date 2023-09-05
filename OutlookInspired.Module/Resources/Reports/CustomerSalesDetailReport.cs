#region Copyright (c) 2000-2023 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{                                                                   }
{                                                                   }
{       Copyright (c) 2000-2023 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2023 Developer Express Inc.

using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.XtraReports.UI;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Resources.Reports {
	public class CustomerSalesDetailReport : DevExpress.XtraReports.UI.XtraReport {
		private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
		private DevExpress.XtraReports.UI.DetailBand detailBand1;
		private CollectionDataSource bindingSource1;
		private System.ComponentModel.IContainer components;
		private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
		private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
		private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
		private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
		private DevExpress.XtraReports.Parameters.Parameter paramOrderDate;
		private DevExpress.XtraReports.Parameters.Parameter paramFromDate;
		private DevExpress.XtraReports.Parameters.Parameter paramToDate;
		private XRTable xrTable3;
		private XRTableRow xrTableRow8;
		private XRTableCell xrTableCell15;
		private XRTableRow xrTableRow7;
		private XRTableCell xrTableCell7;
		private XRTableCell xrTableCell14;
		private XRTableRow xrTableRow3;
		private XRTableCell xrTableCell8;
		private XRTableCell xrTableCell9;
		private XRTableRow xrTableRow4;
		private XRTableCell xrTableCell10;
		private XRTableCell xrTableCell11;
		private XRTableRow xrTableRow5;
		private XRTableCell xrTableCell12;
		private XRTableCell xrTableCell13;
		private XRPictureBox xrPictureBox4;
		private XRPageInfo xrPageInfo2;
		private XRTable xrTable2;
		private XRTableRow xrTableRow2;
		private XRTableCell xrTableCell4;
		private XRTableCell xrTableCell5;
		private XRTable xrTable1;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell2;
		private XRTable xrTable4;
		private XRTableRow xrTableRow9;
		private XRTableCell xrTableCell19;
		private XRTableCell xrTableCell20;
		private XRTableCell xrTableCell21;
		private XRTableCell xrTableCell22;
		private XRTableCell xrTableCell23;
		private XRTable xrTable5;
		private XRTableRow xrTableRow10;
		private XRTableCell xrTableCell24;
		private XRTableCell xrTableCell25;
		private XRTableCell xrTableCell26;
		private XRTableCell xrTableCell27;
		private XRTableCell xrTableCell28;
		private XRTable xrTable10;
		private XRTableRow xrTableRow16;
		private XRTableCell xrTableCell40;
		private XRTableCell xrTableCell41;
		private XRTable xrTable11;
		private XRTableRow xrTableRow17;
		private XRTableCell xrTableCell45;
		private XRTableRow xrTableRow18;
		private XRTableCell xrTableCell46;
		private XRTableRow xrTableRow19;
		private XRTableCell xrTableCell47;
		private XRTableRow xrTableRow20;
		private XRTableCell xrTableCell48;
		private XRTableRow xrTableRow21;
		private XRTableCell xrTableCell49;
		private XRTableRow xrTableRow22;
		private XRTable xrTable6;
		private XRTableRow xrTableRow6;
		private XRTableCell xrTableCell3;
		private XRLabel xrLabel1;
		private XRLabel xrLabel6;
		private XRTableCell xrTableCell6;
		private XRLabel xrLabel2;
		private XRLabel xrLabel7;
		private XRTableCell xrTableCell17;
		private XRLabel xrLabel8;
		private XRLabel xrLabel3;
		private XRTableCell xrTableCell18;
		private XRLabel xrLabel9;
		private XRLabel xrLabel4;
		private XRTableCell xrTableCell16;
		private XRLabel xrLabel10;
		private XRLabel xrLabel5;
		private XRTable xrTable7;
		private XRTableRow xrTableRow11;
		private XRTableCell xrTableCell29;
		private XRLabel xrLabel11;
		private XRTableCell xrTableCell30;
		private XRLabel xrLabel13;
		private XRTableCell xrTableCell31;
		private XRLabel xrLabel16;
		private XRTableCell xrTableCell32;
		private XRLabel xrLabel18;
		private XRTableCell xrTableCell33;
		private XRLabel xrLabel20;
		private XRTable xrTable9;
		private XRTableRow xrTableRow15;
		private XRTableCell xrTableCell42;
		private XRTableCell xrTableCell43;
		private XRLabel xrLabel12;
		private XRTableCell xrTableCell44;
		private XRLabel xrLabel14;
		private DetailReportBand DetailReport;
		private DetailBand Detail;
		private ReportHeaderBand ReportHeader1;
		private ReportFooterBand ReportFooter;
		public XRChart xrChart1;
		private XRTableCell xrTableCell50;
		private XRLabel xrLabel22;
		private XRLabel xrLabel23;
		private XRLabel xrLabel19;
		private XRLabel xrLabel21;
		private XRLabel xrLabel15;
		private XRLabel xrLabel17;
		private XRTable xrTable8;
		private XRTableRow xrTableRow12;
		private XRTableCell xrTableCell34;
		private XRTableCell xrTableCell35;
		private XRTableRow xrTableRow14;
		private XRTableCell xrTableCell38;
		private XRTableCell xrTableCell39;
		private XRTableRow xrTableRow13;
		private XRTableCell xrTableCell36;
		private XRTableCell xrTableCell37;
		private XRTableRow xrTableRow23;
		private GroupHeaderBand GroupHeader1;
		private XRTableCell xrTableCell51;
		[Obsolete]
		public CustomerSalesDetailReport() {
			InitializeComponent();
			// ParameterHelper.InitializeDateTimeParameters(paramFromDate, paramToDate);
		}
		public void SetChartData(object data) {
			xrChart1.DataSource = data;
			xrChart1.Series[0].ArgumentDataMember = "ProductCategory";
			UpdateChartFilter();
		}
		void UpdateChartFilter() {
			xrChart1.Series[0].FilterCriteria = new GroupOperator(
				new BinaryOperator("OrderDate", paramFromDate.Value, BinaryOperatorType.GreaterOrEqual),
				new BinaryOperator("OrderDate", paramToDate.Value, BinaryOperatorType.LessOrEqual)
			);
		}
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerSalesDetailReport));
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PieSeriesLabel pieSeriesLabel1 = new DevExpress.XtraCharts.PieSeriesLabel();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView1 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView2 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
            this.paramFromDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.paramToDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox4 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.bindingSource1 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
            this.xrTable10 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable11 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow21 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.paramOrderDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable9 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.ReportHeader1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // paramFromDate
            // 
            this.paramFromDate.Description = "ParamFromDate";
            this.paramFromDate.Name = "paramFromDate";
            this.paramFromDate.Type = typeof(global::System.DateTime);
            this.paramFromDate.ValueInfo = "2013-01-01";
            this.paramFromDate.Visible = false;
            // 
            // paramToDate
            // 
            this.paramToDate.Description = "ParamToDate";
            this.paramToDate.Name = "paramToDate";
            this.paramToDate.Type = typeof(global::System.DateTime);
            this.paramToDate.ValueInfo = "2015-01-01";
            this.paramToDate.Visible = false;
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.topMarginBand1.HeightF = 119F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(473.9583F, 51F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(170.8333F, 56.41184F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.detailBand1.HeightF = 29.16667F;
            this.detailBand1.KeepTogether = true;
            this.detailBand1.KeepTogetherWithDetailReports = true;
            this.detailBand1.Name = "detailBand1";
            this.detailBand1.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("DueDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // xrTable4
            // 
            this.xrTable4.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(5.928103F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9});
            this.xrTable4.SizeF = new System.Drawing.SizeF(635.7385F, 29.16667F);
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell23});
            this.xrTableRow9.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableRow9.StylePriority.UseFont = false;
            this.xrTableRow9.StylePriority.UsePadding = false;
            this.xrTableRow9.Weight = 0.87499997138977259D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderDate", "{0:MM/dd/yyyy}")});
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Weight = 0.59173941979041467D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "InvoiceNumber")});
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Weight = 0.47683549147385823D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PONumber")});
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Weight = 0.38046329204852769D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "StoreCity")});
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell22.StylePriority.UsePadding = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Weight = 0.908654526930589D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeFullName")});
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 6, 0, 0, 100F);
            this.xrTableCell23.StylePriority.UsePadding = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell23.Weight = 0.57648507925180281D;
            // 
            // xrTable3
            // 
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(176.4658F, 51.12502F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8,
            this.xrTableRow7,
            this.xrTableRow3,
            this.xrTableRow4,
            this.xrTableRow5});
            this.xrTable3.SizeF = new System.Drawing.SizeF(462.5F, 184.1186F);
            this.xrTable3.StylePriority.UsePadding = false;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1.3733330546061278D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomerName")});
            this.xrTableCell15.Font = new DevExpress.Drawing.DXFont("Segoe UI", 26.25F);
            this.xrTableCell15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(178)))), ((int)(((byte)(144)))));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseForeColor = false;
            this.xrTableCell15.Weight = 3D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell14});
            this.xrTableRow7.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F);
            this.xrTableRow7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.StylePriority.UseFont = false;
            this.xrTableRow7.StylePriority.UseForeColor = false;
            this.xrTableRow7.StylePriority.UsePadding = false;
            this.xrTableRow7.StylePriority.UseTextAlignment = false;
            this.xrTableRow7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            this.xrTableRow7.Weight = 1.1629306803809705D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "HOME OFFICE";
            this.xrTableCell7.Weight = 1.4122964395059121D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UseForeColor = false;
            this.xrTableCell14.Text = "BILLING ADDRESS";
            this.xrTableCell14.Weight = 1.5877035604940879D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8,
            this.xrTableCell9});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1.2264701559025575D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "[Order.Customer.HomeOfficeLine]\r\n[Order.Customer.HomeOfficeCity]";
            this.xrTableCell8.Weight = 1.4122964395059121D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Text = "[Order.Customer.BillingAddressLine]\r\n[Order.Customer.BillingAddressCity]";
            this.xrTableCell9.Weight = 1.5877035604940879D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell11});
            this.xrTableRow4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.StylePriority.UseForeColor = false;
            this.xrTableRow4.StylePriority.UsePadding = false;
            this.xrTableRow4.StylePriority.UseTextAlignment = false;
            this.xrTableRow4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            this.xrTableRow4.Weight = 0.84523535774366332D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "PHONE";
            this.xrTableCell10.Weight = 1.4122964395059121D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "FAX";
            this.xrTableCell11.Weight = 1.5877035604940879D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell13});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 0.61225922764545693D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Order.Customer.Phone")});
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Weight = 1.4122964395059121D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Order.Customer.Fax")});
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Weight = 1.5877035604940879D;
            // 
            // xrPictureBox4
            // 
            this.xrPictureBox4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrPictureBox4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPictureBox4.BorderWidth = 1F;
            this.xrPictureBox4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("ImageSource", null, "CustomerImage")});
            this.xrPictureBox4.LocationFloat = new DevExpress.Utils.PointFloat(7.965815F, 53.125F);
            this.xrPictureBox4.Name = "xrPictureBox4";
            this.xrPictureBox4.SizeF = new System.Drawing.SizeF(158.7394F, 190.1483F);
            this.xrPictureBox4.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            this.xrPictureBox4.StylePriority.UseBorderColor = false;
            this.xrPictureBox4.StylePriority.UseBorders = false;
            this.xrPictureBox4.StylePriority.UseBorderWidth = false;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.bottomMarginBand1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            this.bottomMarginBand1.StylePriority.UseFont = false;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(485.4167F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(156.25F, 23F);
            this.xrPageInfo2.StylePriority.UseForeColor = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrPageInfo2.TextFormatString = "{0:MMMM d, yyyy}";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(156.25F, 23F);
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            this.xrPageInfo1.TextFormatString = "Page {0} of {1}";
            // 
            // bindingSource1
            // 
            this.bindingSource1.Name = "bindingSource1";
            this.bindingSource1.ObjectTypeName = "OutlookInspired.Module.BusinessObjects.OrderItem";
            this.bindingSource1.TopReturnedRecords = 0;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrChart1,
            this.xrTable10,
            this.xrTable11,
            this.xrTable2,
            this.xrTable3,
            this.xrPictureBox4});
            this.ReportHeader.HeightF = 563.3893F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrChart1
            // 
            this.xrChart1.BorderColor = System.Drawing.Color.Black;
            this.xrChart1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrChart1.EmptyChartText.DXFont = new DevExpress.Drawing.DXFont("Segoe UI", 12F);
            this.xrChart1.EmptyChartText.Text = "\r\n";
            this.xrChart1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.Center;
            this.xrChart1.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.xrChart1.Legend.DXFont = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrChart1.Legend.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
            this.xrChart1.Legend.EquallySpacedItems = false;
            this.xrChart1.Legend.LegendID = -1;
            this.xrChart1.Legend.MarkerSize = new System.Drawing.Size(20, 20);
            this.xrChart1.Legend.Padding.Left = 30;
            this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(5.928087F, 309.5011F);
            this.xrChart1.Name = "xrChart1";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Qualitative;
            pieSeriesLabel1.TextPattern = "{V:$#,#}";
            series1.Label = pieSeriesLabel1;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series1.LegendTextPattern = "{A}\n{V:$#,#}\n";
            series1.Name = "Series 1";
            series1.QualitativeSummaryOptions.SummaryFunction = "SUM([Total])";
            series1.SeriesID = 0;
            pieSeriesView1.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
            series1.View = pieSeriesView1;
            this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.xrChart1.SeriesTemplate.View = pieSeriesView2;
            this.xrChart1.SizeF = new System.Drawing.SizeF(356.6193F, 248.0208F);
            // 
            // xrTable10
            // 
            this.xrTable10.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTable10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 272.0011F);
            this.xrTable10.Name = "xrTable10";
            this.xrTable10.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow16});
            this.xrTable10.SizeF = new System.Drawing.SizeF(647.9999F, 37.5F);
            this.xrTable10.StylePriority.UseFont = false;
            // 
            // xrTableRow16
            // 
            this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell40,
            this.xrTableCell41});
            this.xrTableRow16.Name = "xrTableRow16";
            this.xrTableRow16.Weight = 1.3333334941638817D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
            this.xrTableCell40.ForeColor = System.Drawing.Color.White;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0, 100F);
            this.xrTableCell40.StylePriority.UseBackColor = false;
            this.xrTableCell40.StylePriority.UseFont = false;
            this.xrTableCell40.StylePriority.UseForeColor = false;
            this.xrTableCell40.StylePriority.UsePadding = false;
            this.xrTableCell40.StylePriority.UseTextAlignment = false;
            this.xrTableCell40.Text = "Analysis";
            this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell40.Weight = 0.8195229174103581D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0, 100F);
            this.xrTableCell41.StylePriority.UseBackColor = false;
            this.xrTableCell41.StylePriority.UsePadding = false;
            this.xrTableCell41.StylePriority.UseTextAlignment = false;
            this.xrTableCell41.Text = "July 1, 2013 to July 31, 2013";
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell41.Weight = 2.1804770825896416D;
            // 
            // xrTable11
            // 
            this.xrTable11.LocationFloat = new DevExpress.Utils.PointFloat(429.7018F, 358.3021F);
            this.xrTable11.Name = "xrTable11";
            this.xrTable11.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow17,
            this.xrTableRow18,
            this.xrTableRow19,
            this.xrTableRow20,
            this.xrTableRow21,
            this.xrTableRow22});
            this.xrTable11.SizeF = new System.Drawing.SizeF(220.0368F, 148.504F);
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell45});
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 0.77837459842722589D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F);
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.Text = "Total sales in date range was";
            this.xrTableCell45.Weight = 3D;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell46});
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1.3828073576824857D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.Total")});
            this.xrTableCell46.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StylePriority.UseFont = false;
            xrSummary1.FormatString = "{0:$#,#}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell46.Summary = xrSummary1;
            this.xrTableCell46.Weight = 3D;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell47});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 0.83881804389028847D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F);
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.Text = "Total discounts on orders was ";
            this.xrTableCell47.Weight = 3D;
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell48});
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Weight = 1.28206876997332D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.Discount")});
            this.xrTableCell48.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StylePriority.UseFont = false;
            xrSummary2.FormatString = "{0:$#,#}";
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell48.Summary = xrSummary2;
            this.xrTableCell48.Weight = 3D;
            // 
            // xrTableRow21
            // 
            this.xrTableRow21.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell49});
            this.xrTableRow21.Name = "xrTableRow21";
            this.xrTableRow21.Weight = 0.71793123002668D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F);
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.StylePriority.UseFont = false;
            this.xrTableCell49.Text = "Top-selling store was";
            this.xrTableCell49.Weight = 3D;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell50});
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 1D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.Total")});
            this.xrTableCell50.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.StylePriority.UseFont = false;
            xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell50.Summary = xrSummary3;
            this.xrTableCell50.Weight = 3D;
            this.xrTableCell50.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrTableCell50_SummaryGetResult);
            this.xrTableCell50.SummaryReset += new System.EventHandler(this.xrTableCell50_SummaryReset);
            this.xrTableCell50.SummaryRowChanged += new System.EventHandler(this.xrTableCell50_SummaryRowChanged);
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(647.9999F, 37.5F);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1.3333334941638817D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
            this.xrTableCell4.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11.25F);
            this.xrTableCell4.ForeColor = System.Drawing.Color.White;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0, 100F);
            this.xrTableCell4.StylePriority.UseBackColor = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseForeColor = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Customer Profile";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 0.8195229174103581D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBackColor = false;
            this.xrTableCell5.Weight = 2.1804770825896416D;
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(650.0001F, 32.29167F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2});
            this.xrTableRow1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.StylePriority.UseFont = false;
            this.xrTableRow1.Weight = 1.3333334941638817D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
            this.xrTableCell1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11.25F);
            this.xrTableCell1.ForeColor = System.Drawing.Color.White;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0, 100F);
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Orders";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 0.80308245213023211D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0, 100F);
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Sorted by : Order Date";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell2.Weight = 2.206177656583951D;
            // 
            // xrTable5
            // 
            this.xrTable5.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(5.927976F, 62.49994F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10});
            this.xrTable5.SizeF = new System.Drawing.SizeF(635.7386F, 28.125F);
            this.xrTable5.StylePriority.UseFont = false;
            this.xrTable5.StylePriority.UseTextAlignment = false;
            this.xrTable5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell28});
            this.xrTableRow10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableRow10.StylePriority.UseForeColor = false;
            this.xrTableRow10.StylePriority.UsePadding = false;
            this.xrTableRow10.Weight = 0.84374993562698863D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UsePadding = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "ORDER DATE";
            this.xrTableCell24.Weight = 0.59173941979041478D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "INVOICE #";
            this.xrTableCell25.Weight = 0.47683563232421888D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell26.StylePriority.UsePadding = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "PO #";
            this.xrTableCell26.Weight = 0.38046315119816709D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell27.StylePriority.UsePadding = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "STORE";
            this.xrTableCell27.Weight = 0.908654526930589D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 6, 0, 0, 100F);
            this.xrTableCell28.StylePriority.UsePadding = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "ORDERED BY";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell28.Weight = 0.576485360952524D;
            // 
            // paramOrderDate
            // 
            this.paramOrderDate.Description = "ParamOrderDate";
            this.paramOrderDate.Name = "paramOrderDate";
            this.paramOrderDate.Type = typeof(bool);
            this.paramOrderDate.ValueInfo = "True";
            this.paramOrderDate.Visible = false;
            // 
            // xrTable6
            // 
            this.xrTable6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(132F, 0F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable6.SizeF = new System.Drawing.SizeF(517F, 29.18F);
            this.xrTable6.StylePriority.UseBorderColor = false;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrTableRow6.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableRow6.BorderWidth = 1F;
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell6,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell16});
            this.xrTableRow6.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.StylePriority.UseBorderDashStyle = false;
            this.xrTableRow6.StylePriority.UseBorders = false;
            this.xrTableRow6.StylePriority.UseBorderWidth = false;
            this.xrTableRow6.StylePriority.UseFont = false;
            this.xrTableRow6.Weight = 0.99312458613261245D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
            this.xrTableCell3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLabel6});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.Weight = 0.80976249989033178D;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(2.107574F, 2.180233F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(135.97F, 25F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.Text = "Product";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(2.11F, 27.18023F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(135.97F, 2.000021F);
            this.xrLabel6.StylePriority.UseBorders = false;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel7});
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Weight = 0.31075230478016674D;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.18F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(52.4F, 25F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Unit";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 27.18F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(52.39993F, 2.000021F);
            this.xrLabel7.StylePriority.UseBorders = false;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel3});
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Weight = 0.74349946878361461D;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0.15F, 27.18F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(127.62F, 2F);
            this.xrLabel8.StylePriority.UseBorders = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0.1453552F, 2.180233F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(127.6163F, 25F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Price";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrLabel4});
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Weight = 0.52946757061084682D;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(1.41F, 27.18F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(88.73712F, 2.000021F);
            this.xrLabel9.StylePriority.UseBorders = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1.410004F, 2.179966F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(88.73721F, 25F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Discount";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell16.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.xrLabel5});
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseBorders = false;
            this.xrTableCell16.Weight = 0.6065181559350401D;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0.75F, 27.18F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(101.2372F, 2.071886F);
            this.xrLabel10.StylePriority.UseBorders = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0.7538452F, 2.180012F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(101.2372F, 25F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UsePadding = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Item Total";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTable7
            // 
            this.xrTable7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(132F, 0F);
            this.xrTable7.Name = "xrTable7";
            this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow11});
            this.xrTable7.SizeF = new System.Drawing.SizeF(517F, 24F);
            this.xrTable7.StylePriority.UseBorderColor = false;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrTableRow11.BorderWidth = 1F;
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29,
            this.xrTableCell30,
            this.xrTableCell31,
            this.xrTableCell32,
            this.xrTableCell33});
            this.xrTableRow11.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.StylePriority.UseBorderDashStyle = false;
            this.xrTableRow11.StylePriority.UseBorders = false;
            this.xrTableRow11.StylePriority.UseBorderWidth = false;
            this.xrTableRow11.StylePriority.UseFont = false;
            this.xrTableRow11.Weight = 0.81989936523876228D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell29.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11});
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseBorders = false;
            this.xrTableCell29.Weight = 0.80976249989033178D;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.ProductCategory")});
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(1.848602F, 0F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(7, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(136.229F, 23.91371F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.BeforePrint += new DevExpress.XtraReports.UI.BeforePrintEventHandler(this.xrLabel11_BeforePrint);
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel13});
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Weight = 0.31075230478016674D;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.ProductUnits")});
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(52.40005F, 23.9137F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel16});
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Weight = 0.74349946878361461D;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.ProductPrice", "{0:$#,#}")});
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(0.1453552F, 0F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(127.6163F, 24F);
            this.xrLabel16.StylePriority.UseBorders = false;
            this.xrLabel16.StylePriority.UsePadding = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel18});
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Weight = 0.52946757061084682D;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.Discount", "{0:$#,#;$#,#; - }")});
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(1.410034F, 0F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(88.73712F, 24F);
            this.xrLabel18.StylePriority.UseBorders = false;
            this.xrLabel18.StylePriority.UsePadding = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell33.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel20});
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseBorders = false;
            this.xrTableCell33.Weight = 0.6065181559350401D;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrLabel20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.Total", "{0:$#,#}")});
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(0.7538757F, 0F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(101.2372F, 24.00001F);
            this.xrLabel20.StylePriority.UseBorders = false;
            this.xrLabel20.StylePriority.UsePadding = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTable9
            // 
            this.xrTable9.LocationFloat = new DevExpress.Utils.PointFloat(132F, 0F);
            this.xrTable9.Name = "xrTable9";
            this.xrTable9.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow15});
            this.xrTable9.SizeF = new System.Drawing.SizeF(517F, 2F);
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrTableRow15.BorderWidth = 1F;
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell44});
            this.xrTableRow15.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.StylePriority.UseBorderDashStyle = false;
            this.xrTableRow15.StylePriority.UseBorders = false;
            this.xrTableRow15.StylePriority.UseBorderWidth = false;
            this.xrTableRow15.StylePriority.UseFont = false;
            this.xrTableRow15.Weight = 0.0010782962159689652D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell42.BorderWidth = 2F;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseBorders = false;
            this.xrTableCell42.StylePriority.UseBorderWidth = false;
            this.xrTableCell42.Weight = 1.8568665710533374D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTableCell43.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell43.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12});
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseBorderColor = false;
            this.xrTableCell43.StylePriority.UseBorders = false;
            this.xrTableCell43.Weight = 0.5298717482676697D;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(2.641846F, 0F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(88.73718F, 2.117312F);
            this.xrLabel12.StylePriority.UseBorders = false;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTableCell44.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell44.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel14});
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseBorderColor = false;
            this.xrTableCell44.StylePriority.UseBorders = false;
            this.xrTableCell44.Weight = 0.61326168067899334D;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(1.916016F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(101.2372F, 2.122624F);
            this.xrLabel14.StylePriority.UseBorders = false;
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.ReportHeader1,
            this.ReportFooter});
            this.DetailReport.DataMember = "OrderItems";
            this.DetailReport.DataSource = this.bindingSource1;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7});
            this.Detail.HeightF = 24F;
            this.Detail.Name = "Detail";
            // 
            // ReportHeader1
            // 
            this.ReportHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.ReportHeader1.HeightF = 29.18F;
            this.ReportHeader1.Name = "ReportHeader1";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable8,
            this.xrTable9});
            this.ReportFooter.HeightF = 138.2184F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrTable8
            // 
            this.xrTable8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(452F, 2F);
            this.xrTable8.Name = "xrTable8";
            this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12,
            this.xrTableRow14,
            this.xrTableRow13,
            this.xrTableRow23});
            this.xrTable8.SizeF = new System.Drawing.SizeF(197.0001F, 86.46875F);
            this.xrTable8.StylePriority.UseBorderColor = false;
            this.xrTable8.StylePriority.UseBorders = false;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell34,
            this.xrTableCell35});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1.046874917837296D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell34.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel15});
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseBorders = false;
            this.xrTableCell34.Weight = 1.1747578993203811D;
            // 
            // xrLabel15
            // 
            this.xrLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.xrLabel15.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrLabel15.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(2.641846F, 0F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(88.73718F, 24.99997F);
            this.xrLabel15.StylePriority.UseBackColor = false;
            this.xrLabel15.StylePriority.UseBorderColor = false;
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UsePadding = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "SUB TOTAL";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell35.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel17});
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.StylePriority.UseBorders = false;
            this.xrTableCell35.Weight = 1.3137093943675247D;
            // 
            // xrLabel17
            // 
            this.xrLabel17.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrLabel17.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrderItems.Total")});
            this.xrLabel17.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(101.4678F, 24.99997F);
            this.xrLabel17.StylePriority.UseBorderColor = false;
            this.xrLabel17.StylePriority.UseBorders = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UsePadding = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            xrSummary4.FormatString = "{0:$#,#}";
            xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel17.Summary = xrSummary4;
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell38,
            this.xrTableCell39});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1.0468756054609312D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell38.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel21});
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StylePriority.UseBorders = false;
            this.xrTableCell38.Weight = 1.1747578993203811D;
            // 
            // xrLabel21
            // 
            this.xrLabel21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.xrLabel21.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrLabel21.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(2.641846F, 0F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(88.73718F, 25.00004F);
            this.xrLabel21.StylePriority.UseBackColor = false;
            this.xrLabel21.StylePriority.UseBorderColor = false;
            this.xrLabel21.StylePriority.UseBorders = false;
            this.xrLabel21.StylePriority.UsePadding = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "SHIPPING";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell39.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel19});
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseBorders = false;
            this.xrTableCell39.Weight = 1.3137093943675247D;
            // 
            // xrLabel19
            // 
            this.xrLabel19.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrLabel19.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ShippingAmount", "{0:$#,#}")});
            this.xrLabel19.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(101.4679F, 25.00004F);
            this.xrLabel19.StylePriority.UseBorderColor = false;
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UsePadding = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            xrSummary5.FormatString = "{0:$#,#}";
            this.xrLabel19.Summary = xrSummary5;
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell36,
            this.xrTableCell37});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1.2812499462205338D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell36.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel22});
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.StylePriority.UseBorders = false;
            this.xrTableCell36.Weight = 1.1747578993203811D;
            // 
            // xrLabel22
            // 
            this.xrLabel22.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrLabel22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel22.CanGrow = false;
            this.xrLabel22.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(2.641846F, 0F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(88.73718F, 30.75446F);
            this.xrLabel22.StylePriority.UseBorderColor = false;
            this.xrLabel22.StylePriority.UseBorders = false;
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UsePadding = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "TOTAL";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell37.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel23});
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.StylePriority.UseBorders = false;
            this.xrTableCell37.Weight = 1.3137093943675247D;
            // 
            // xrLabel23
            // 
            this.xrLabel23.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrLabel23.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel23.CanGrow = false;
            this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TotalAmount", "{0:$#,#}")});
            this.xrLabel23.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 6, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(101.468F, 30.75446F);
            this.xrLabel23.StylePriority.UseBorderColor = false;
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UsePadding = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell51});
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Weight = 0.083749385919608343D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell51.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell51.BorderWidth = 2F;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.StylePriority.UseBorderColor = false;
            this.xrTableCell51.StylePriority.UseBorders = false;
            this.xrTableCell51.StylePriority.UseBorderWidth = false;
            this.xrTableCell51.Weight = 2.4884672936879064D;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.xrTable5});
            this.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.GroupHeader1.HeightF = 90.62494F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // CustomerSalesDetailReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1,
            this.ReportHeader,
            this.DetailReport,
            this.GroupHeader1});
            this.DataSource = this.bindingSource1;
            this.DesignerOptions.ShowExportWarnings = false;
            this.FilterString = "[OrderDate] >= ?paramFromDate And [OrderDate] <= ?paramToDate";
            this.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(100F, 100F, 119F, 100F);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.paramOrderDate,
            this.paramFromDate,
            this.paramToDate});
            this.Version = "23.1";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.CustomerSalesDetail_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		private void CustomerSalesDetail_DataSourceDemanded(object sender, EventArgs e) {
			if(Equals(true, paramOrderDate.Value)) {
				xrTableCell2.Text = "Sorted by Order Date";
				this.detailBand1.SortFields[0].FieldName = "OrderDate";
			} else {
				xrTableCell2.Text = "Sorted by Invoice #";
				this.detailBand1.SortFields[0].FieldName = "InvoiceNumber";
			}
			xrTableCell41.Text = ((DateTime)paramFromDate.Value).ToString("MMMM d, yyyy") + " to " + ((DateTime)paramToDate.Value).ToString("MMMM d, yyyy");
			UpdateChartFilter();
		}
		private void xrLabel11_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e) {
			OrderItem currentOrderInfo = (OrderItem)DetailReport.GetCurrentRow();
			if(currentOrderInfo != null) {
				(sender as XRLabel).Text = currentOrderInfo.Product.Category.ToString();
			}
		}
		class StoreInfo {
			public StoreInfo(string city) {
				this.City = city;
			}
			public string City { get; private set; }
			public decimal TotalSales { get; set; }
		}
		Dictionary<Guid, StoreInfo> storeSales = new Dictionary<Guid, StoreInfo>();
		private void xrTableCell50_SummaryRowChanged(object sender, EventArgs e) {
			Order currentInfo = (Order)GetCurrentRow();
			decimal total = currentInfo.OrderItems.Sum(x => x.Total);
			if(storeSales.ContainsKey(currentInfo.Store.ID)) {
				storeSales[currentInfo.Store.ID].TotalSales += total;
			} else {
				storeSales.Add(currentInfo.Store.ID, new StoreInfo(currentInfo.Store.City) { TotalSales = total });
			}
		}
		private void xrTableCell50_SummaryGetResult(object sender, SummaryGetResultEventArgs e) {
			e.Result = storeSales.Count == 0 ? " - " : storeSales.Values.Aggregate((x, y) => x.TotalSales > y.TotalSales ? x : y).City;
			e.Handled = true;
		}
		private void xrTableCell50_SummaryReset(object sender, EventArgs e) {
			storeSales.Clear();
		}
	}
}
