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

using System.ComponentModel;
using System.Drawing;
using DevExpress.Drawing;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Resources.Reports {
	public class ProductSalesSummary : XtraReport {
		#region Designer generated code

		private TopMarginBand topMarginBand1;
		private DetailBand detailBand1;
		private CollectionDataSource bindingSource1;
		private ReportHeaderBand ReportHeader;
		private XRPictureBox xrPictureBox2;
		private XRPageInfo xrPageInfo2;
		private XRPageInfo xrPageInfo1;
		private XRTable xrTable3;
		private XRTableRow xrTableRow8;
		private XRTableCell xrTableCell15;
		private XRTableRow xrTableRow7;
		private XRTableCell xrTableCell7;
		private XRTableCell xrTableCell14;
		private XRTableRow xrTableRow3;
		private XRTableCell xrTableCell8;
		private XRTableCell xrTableCell9;
		private XRPictureBox xrPictureBox4;
		private XRTable xrTable2;
		private XRTableRow xrTableRow2;
		private XRTableCell xrTableCell4;
		private XRTableCell xrTableCell5;
		private XRTable xrTable1;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell2;
		private ReportFooterBand ReportFooter;
		private XRChart xrChart1;
		private XRTable xrTable8;
		private XRTableRow xrTableRow12;
		private XRTableCell xrTableCell18;
		private XRTableRow xrTableRow13;
		private XRTableCell xrTableCell19;
		private XRTableRow xrTableRow14;
		private XRTableCell xrTableCell20;
		private XRTableRow xrTableRow15;
		private XRTableCell xrTableCell21;
		private XRTableRow xrTableRow16;
		private XRTableCell xrTableCell22;
		private XRTableRow xrTableRow17;
		private XRTableCell xrTableCell23;
		private CalculatedField orderDate;
		private BottomMarginBand bottomMarginBand1;
		public ProductSalesSummary() {
			InitializeComponent();
			InitializeMultiValueDateParameter();
		}
		
		private void InitializeMultiValueDateParameter() {
			// ParameterHelper.InitializeMultiValueDateParameter(paramYears);
		}

		private void InitializeComponent() {
			ComponentResourceManager resources = new ComponentResourceManager(typeof(ProductSalesSummary));
			XRSummary xrSummary1 = new XRSummary();
			XRSummary xrSummary2 = new XRSummary();
			XRSummary xrSummary3 = new XRSummary();
			Series series1 = new Series();
			PieSeriesLabel pieSeriesLabel1 = new PieSeriesLabel();
			PieSeriesView pieSeriesView1 = new PieSeriesView();
			PieSeriesView pieSeriesView2 = new PieSeriesView();
			this.topMarginBand1 = new TopMarginBand();
			this.xrPictureBox2 = new XRPictureBox();
			this.detailBand1 = new DetailBand();
			this.bottomMarginBand1 = new BottomMarginBand();
			this.xrPageInfo2 = new XRPageInfo();
			this.xrPageInfo1 = new XRPageInfo();
			this.bindingSource1 = new CollectionDataSource();
			this.ReportHeader = new ReportHeaderBand();
			this.xrTable3 = new XRTable();
			this.xrTableRow8 = new XRTableRow();
			this.xrTableCell15 = new XRTableCell();
			this.xrTableRow7 = new XRTableRow();
			this.xrTableCell7 = new XRTableCell();
			this.xrTableCell14 = new XRTableCell();
			this.xrTableRow3 = new XRTableRow();
			this.xrTableCell8 = new XRTableCell();
			this.xrTableCell9 = new XRTableCell();
			this.xrPictureBox4 = new XRPictureBox();
			this.xrTable2 = new XRTable();
			this.xrTableRow2 = new XRTableRow();
			this.xrTableCell4 = new XRTableCell();
			this.xrTableCell5 = new XRTableCell();
			this.xrTable1 = new XRTable();
			this.xrTableRow1 = new XRTableRow();
			this.xrTableCell1 = new XRTableCell();
			this.xrTableCell2 = new XRTableCell();
			this.ReportFooter = new ReportFooterBand();
			this.xrTable8 = new XRTable();
			this.xrTableRow12 = new XRTableRow();
			this.xrTableCell18 = new XRTableCell();
			this.xrTableRow13 = new XRTableRow();
			this.xrTableCell19 = new XRTableCell();
			this.xrTableRow14 = new XRTableRow();
			this.xrTableCell20 = new XRTableCell();
			this.xrTableRow15 = new XRTableRow();
			this.xrTableCell21 = new XRTableCell();
			this.xrTableRow16 = new XRTableRow();
			this.xrTableCell22 = new XRTableCell();
			this.xrTableRow17 = new XRTableRow();
			this.xrTableCell23 = new XRTableCell();
			this.xrChart1 = new XRChart();
			this.orderDate = new CalculatedField();
			((ISupportInitialize)(this.bindingSource1)).BeginInit();
			((ISupportInitialize)(this.xrTable3)).BeginInit();
			((ISupportInitialize)(this.xrTable2)).BeginInit();
			((ISupportInitialize)(this.xrTable1)).BeginInit();
			((ISupportInitialize)(this.xrTable8)).BeginInit();
			((ISupportInitialize)(this.xrChart1)).BeginInit();
			((ISupportInitialize)(series1)).BeginInit();
			((ISupportInitialize)(pieSeriesLabel1)).BeginInit();
			((ISupportInitialize)(pieSeriesView1)).BeginInit();
			((ISupportInitialize)(pieSeriesView2)).BeginInit();
			((ISupportInitialize)(this)).BeginInit();
			// 
			// topMarginBand1
			// 
			this.topMarginBand1.Controls.AddRange(new XRControl[] {
				this.xrPictureBox2});
			this.topMarginBand1.HeightF = 125F;
			this.topMarginBand1.Name = "topMarginBand1";
			// 
			// xrPictureBox2
			// 
			this.xrPictureBox2.ImageSource = new ImageSource("img", resources.GetString("xrPictureBox2.ImageSource"));
			this.xrPictureBox2.LocationFloat = new PointFloat(470.8333F, 52.08333F);
			this.xrPictureBox2.Name = "xrPictureBox2";
			this.xrPictureBox2.SizeF = new SizeF(170.8333F, 56.41184F);
			this.xrPictureBox2.Sizing = ImageSizeMode.StretchImage;
			// 
			// detailBand1
			// 
			this.detailBand1.Font = new DXFont("Segoe UI", 10F);
			this.detailBand1.HeightF = 0F;
			this.detailBand1.Name = "detailBand1";
			this.detailBand1.SortFields.AddRange(new GroupField[] {
				new GroupField("Order.OrderDate", XRColumnSortOrder.Ascending)});
			this.detailBand1.StylePriority.UseFont = false;
			this.detailBand1.StylePriority.UseForeColor = false;
			// 
			// bottomMarginBand1
			// 
			this.bottomMarginBand1.Controls.AddRange(new XRControl[] {
				this.xrPageInfo2,
				this.xrPageInfo1});
			this.bottomMarginBand1.Font = new DXFont("Segoe UI", 11F);
			this.bottomMarginBand1.HeightF = 102F;
			this.bottomMarginBand1.Name = "bottomMarginBand1";
			this.bottomMarginBand1.StylePriority.UseFont = false;
			// 
			// xrPageInfo2
			// 
			this.xrPageInfo2.ForeColor = Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
			this.xrPageInfo2.LocationFloat = new PointFloat(485.4167F, 0F);
			this.xrPageInfo2.Name = "xrPageInfo2";
			this.xrPageInfo2.Padding = new PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo2.PageInfo = PageInfo.DateTime;
			this.xrPageInfo2.SizeF = new SizeF(156.25F, 23F);
			this.xrPageInfo2.StylePriority.UseForeColor = false;
			this.xrPageInfo2.StylePriority.UseTextAlignment = false;
			this.xrPageInfo2.TextAlignment = TextAlignment.TopRight;
			this.xrPageInfo2.TextFormatString = "{0:MMMM d, yyyy}";
			// 
			// xrPageInfo1
			// 
			this.xrPageInfo1.ForeColor = Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
			this.xrPageInfo1.LocationFloat = new PointFloat(0F, 0F);
			this.xrPageInfo1.Name = "xrPageInfo1";
			this.xrPageInfo1.Padding = new PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo1.SizeF = new SizeF(156.25F, 23F);
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
			this.ReportHeader.Controls.AddRange(new XRControl[] {
				this.xrTable3,
				this.xrPictureBox4,
				this.xrTable2});
			this.ReportHeader.HeightF = 225.8333F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// xrTable3
			// 
			this.xrTable3.LocationFloat = new PointFloat(203.3125F, 43.67973F);
			this.xrTable3.Name = "xrTable3";
			this.xrTable3.Padding = new PaddingInfo(5, 0, 0, 0, 100F);
			this.xrTable3.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow8,
				this.xrTableRow7,
				this.xrTableRow3});
			this.xrTable3.SizeF = new SizeF(429.6875F, 130.7124F);
			this.xrTable3.StylePriority.UsePadding = false;
			// 
			// xrTableRow8
			// 
			this.xrTableRow8.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell15});
			this.xrTableRow8.Name = "xrTableRow8";
			this.xrTableRow8.Weight = 1.3733330546061278D;
			// 
			// xrTableCell15
			// 
			this.xrTableCell15.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Product.Name]")});
			this.xrTableCell15.Font = new DXFont("Segoe UI", 26.25F);
			this.xrTableCell15.ForeColor = Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(178)))), ((int)(((byte)(144)))));
			this.xrTableCell15.Name = "xrTableCell15";
			this.xrTableCell15.StylePriority.UseFont = false;
			this.xrTableCell15.StylePriority.UseForeColor = false;
			this.xrTableCell15.Weight = 3D;
			// 
			// xrTableRow7
			// 
			this.xrTableRow7.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell7,
				this.xrTableCell14});
			this.xrTableRow7.Font = new DXFont("Segoe UI", 10F);
			this.xrTableRow7.ForeColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.xrTableRow7.Name = "xrTableRow7";
			this.xrTableRow7.StylePriority.UseFont = false;
			this.xrTableRow7.StylePriority.UseForeColor = false;
			this.xrTableRow7.StylePriority.UsePadding = false;
			this.xrTableRow7.StylePriority.UseTextAlignment = false;
			this.xrTableRow7.TextAlignment = TextAlignment.BottomLeft;
			this.xrTableRow7.Weight = 1.2978654898467186D;
			// 
			// xrTableCell7
			// 
			this.xrTableCell7.Name = "xrTableCell7";
			this.xrTableCell7.Text = "CURRENT INVENTORY";
			this.xrTableCell7.Weight = 1.4122964395059123D;
			// 
			// xrTableCell14
			// 
			this.xrTableCell14.Name = "xrTableCell14";
			this.xrTableCell14.StylePriority.UseFont = false;
			this.xrTableCell14.StylePriority.UseForeColor = false;
			this.xrTableCell14.Text = "IN MANUFACTURING";
			this.xrTableCell14.Weight = 1.5877035604940877D;
			// 
			// xrTableRow3
			// 
			this.xrTableRow3.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell8,
				this.xrTableCell9});
			this.xrTableRow3.Font = new DXFont("Segoe UI", 18F, DXFontStyle.Bold);
			this.xrTableRow3.Name = "xrTableRow3";
			this.xrTableRow3.StylePriority.UseFont = false;
			this.xrTableRow3.StylePriority.UseTextAlignment = false;
			this.xrTableRow3.TextAlignment = TextAlignment.MiddleLeft;
			this.xrTableRow3.Weight = 1.0915353464368094D;
			// 
			// xrTableCell8
			// 
			this.xrTableCell8.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Product.CurrentInventory]")});
			this.xrTableCell8.Multiline = true;
			this.xrTableCell8.Name = "xrTableCell8";
			this.xrTableCell8.Weight = 1.4122964395059121D;
			// 
			// xrTableCell9
			// 
			this.xrTableCell9.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Product.Manufacturing]")});
			this.xrTableCell9.Multiline = true;
			this.xrTableCell9.Name = "xrTableCell9";
			this.xrTableCell9.Weight = 1.5877035604940879D;
			// 
			// xrPictureBox4
			// 
			this.xrPictureBox4.BorderColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.xrPictureBox4.Borders = ((BorderSide)((((BorderSide.Left | BorderSide.Top) 
			                                             | BorderSide.Right) 
			                                            | BorderSide.Bottom)));
			this.xrPictureBox4.BorderWidth = 1F;
			this.xrPictureBox4.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "ImageSource", "[Product].[PrimaryImage].[Data]")});
			this.xrPictureBox4.LocationFloat = new PointFloat(4.00001F, 49.40628F);
			this.xrPictureBox4.Name = "xrPictureBox4";
			this.xrPictureBox4.SizeF = new SizeF(185.1414F, 127.9859F);
			this.xrPictureBox4.Sizing = ImageSizeMode.StretchImage;
			this.xrPictureBox4.StylePriority.UseBorderColor = false;
			this.xrPictureBox4.StylePriority.UseBorders = false;
			this.xrPictureBox4.StylePriority.UseBorderWidth = false;
			// 
			// xrTable2
			// 
			this.xrTable2.LocationFloat = new PointFloat(0F, 0F);
			this.xrTable2.Name = "xrTable2";
			this.xrTable2.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow2});
			this.xrTable2.SizeF = new SizeF(641.9999F, 28.71094F);
			// 
			// xrTableRow2
			// 
			this.xrTableRow2.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell4,
				this.xrTableCell5});
			this.xrTableRow2.Name = "xrTableRow2";
			this.xrTableRow2.Weight = 0.66666681489878932D;
			// 
			// xrTableCell4
			// 
			this.xrTableCell4.BackColor = Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
			this.xrTableCell4.Font = new DXFont("Segoe UI", 11.25F);
			this.xrTableCell4.ForeColor = Color.White;
			this.xrTableCell4.Name = "xrTableCell4";
			this.xrTableCell4.Padding = new PaddingInfo(15, 0, 0, 0, 100F);
			this.xrTableCell4.StylePriority.UseBackColor = false;
			this.xrTableCell4.StylePriority.UseFont = false;
			this.xrTableCell4.StylePriority.UseForeColor = false;
			this.xrTableCell4.StylePriority.UsePadding = false;
			this.xrTableCell4.StylePriority.UseTextAlignment = false;
			this.xrTableCell4.Text = "Product Profile";
			this.xrTableCell4.TextAlignment = TextAlignment.MiddleLeft;
			this.xrTableCell4.Weight = 0.94701867179278676D;
			// 
			// xrTableCell5
			// 
			this.xrTableCell5.BackColor = Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.xrTableCell5.Name = "xrTableCell5";
			this.xrTableCell5.StylePriority.UseBackColor = false;
			this.xrTableCell5.Weight = 2.0252039691253749D;
			// 
			// xrTable1
			// 
			this.xrTable1.LocationFloat = new PointFloat(0F, 0F);
			this.xrTable1.Name = "xrTable1";
			this.xrTable1.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow1});
			this.xrTable1.SizeF = new SizeF(641.9999F, 28.71094F);
			// 
			// xrTableRow1
			// 
			this.xrTableRow1.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell1,
				this.xrTableCell2});
			this.xrTableRow1.Name = "xrTableRow1";
			this.xrTableRow1.Weight = 0.66666681489878932D;
			// 
			// xrTableCell1
			// 
			this.xrTableCell1.BackColor = Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
			this.xrTableCell1.Font = new DXFont("Segoe UI", 11.25F);
			this.xrTableCell1.ForeColor = Color.White;
			this.xrTableCell1.Name = "xrTableCell1";
			this.xrTableCell1.Padding = new PaddingInfo(15, 0, 0, 0, 100F);
			this.xrTableCell1.StylePriority.UseBackColor = false;
			this.xrTableCell1.StylePriority.UseFont = false;
			this.xrTableCell1.StylePriority.UseForeColor = false;
			this.xrTableCell1.StylePriority.UsePadding = false;
			this.xrTableCell1.StylePriority.UseTextAlignment = false;
			this.xrTableCell1.Text = "Orders";
			this.xrTableCell1.TextAlignment = TextAlignment.MiddleLeft;
			this.xrTableCell1.Weight = 0.94701867179278676D;
			// 
			// xrTableCell2
			// 
			this.xrTableCell2.BackColor = Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.xrTableCell2.Font = new DXFont("Segoe UI", 11.5F);
			this.xrTableCell2.Name = "xrTableCell2";
			this.xrTableCell2.Padding = new PaddingInfo(0, 10, 0, 0, 100F);
			this.xrTableCell2.StylePriority.UseBackColor = false;
			this.xrTableCell2.StylePriority.UseFont = false;
			this.xrTableCell2.StylePriority.UsePadding = false;
			this.xrTableCell2.StylePriority.UseTextAlignment = false;
			this.xrTableCell2.Text = "2011 - 2013";
			this.xrTableCell2.TextAlignment = TextAlignment.MiddleRight;
			this.xrTableCell2.Weight = 2.0252039691253749D;
			// 
			// ReportFooter
			// 
			this.ReportFooter.Controls.AddRange(new XRControl[] {
				this.xrTable8,
				this.xrChart1,
				this.xrTable1});
			this.ReportFooter.HeightF = 333.3333F;
			this.ReportFooter.Name = "ReportFooter";
			// 
			// xrTable8
			// 
			this.xrTable8.LocationFloat = new PointFloat(403.0465F, 81.83333F);
			this.xrTable8.Name = "xrTable8";
			this.xrTable8.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow12,
				this.xrTableRow13,
				this.xrTableRow14,
				this.xrTableRow15,
				this.xrTableRow16,
				this.xrTableRow17});
			this.xrTable8.SizeF = new SizeF(238.9533F, 158.0092F);
			// 
			// xrTableRow12
			// 
			this.xrTableRow12.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell18});
			this.xrTableRow12.Name = "xrTableRow12";
			this.xrTableRow12.Weight = 0.77837459842722589D;
			// 
			// xrTableCell18
			// 
			this.xrTableCell18.Font = new DXFont("Segoe UI", 10F);
			this.xrTableCell18.Name = "xrTableCell18";
			this.xrTableCell18.StylePriority.UseFont = false;
			this.xrTableCell18.Text = "Total orders for the [Product.Name]";
			this.xrTableCell18.Weight = 3D;
			// 
			// xrTableRow13
			// 
			this.xrTableRow13.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell19});
			this.xrTableRow13.Name = "xrTableRow13";
			this.xrTableRow13.Weight = 1.042431401190909D;
			// 
			// xrTableCell19
			// 
			this.xrTableCell19.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "sumSum([Total])")});
			this.xrTableCell19.Font = new DXFont("Segoe UI", 14F, DXFontStyle.Bold);
			this.xrTableCell19.Name = "xrTableCell19";
			this.xrTableCell19.StylePriority.UseFont = false;
			xrSummary1.Running = SummaryRunning.Report;
			this.xrTableCell19.Summary = xrSummary1;
			this.xrTableCell19.TextFormatString = "{0:$#,#}";
			this.xrTableCell19.Weight = 3D;
			// 
			// xrTableRow14
			// 
			this.xrTableRow14.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell20});
			this.xrTableRow14.Name = "xrTableRow14";
			this.xrTableRow14.Weight = 0.69866359482761187D;
			// 
			// xrTableCell20
			// 
			this.xrTableCell20.Font = new DXFont("Segoe UI", 10F);
			this.xrTableCell20.Name = "xrTableCell20";
			this.xrTableCell20.StylePriority.UseFont = false;
			this.xrTableCell20.Text = "Total cost of goods sold";
			this.xrTableCell20.Weight = 3D;
			// 
			// xrTableRow15
			// 
			this.xrTableRow15.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell21});
			this.xrTableRow15.Name = "xrTableRow15";
			this.xrTableRow15.Weight = 1.1819583095512345D;
			// 
			// xrTableCell21
			// 
			this.xrTableCell21.Font = new DXFont("Segoe UI", 14F, DXFontStyle.Bold);
			this.xrTableCell21.Name = "xrTableCell21";
			this.xrTableCell21.StylePriority.UseFont = false;
			xrSummary2.FormatString = "{0:$#,#;0:$#,#; - }";
			xrSummary2.Func = SummaryFunc.Custom;
			this.xrTableCell21.Summary = xrSummary2;
			this.xrTableCell21.Weight = 3D;
			this.xrTableCell21.BeforePrint += new BeforePrintEventHandler(this.xrTableCell21_BeforePrint);
			// 
			// xrTableRow16
			// 
			this.xrTableRow16.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell22});
			this.xrTableRow16.Name = "xrTableRow16";
			this.xrTableRow16.Weight = 0.65786547518207683D;
			// 
			// xrTableCell22
			// 
			this.xrTableCell22.CanGrow = false;
			this.xrTableCell22.Font = new DXFont("Segoe UI", 10F);
			this.xrTableCell22.Name = "xrTableCell22";
			this.xrTableCell22.StylePriority.UseFont = false;
			this.xrTableCell22.Text = "Total units sold ";
			this.xrTableCell22.Weight = 3D;
			// 
			// xrTableRow17
			// 
			this.xrTableRow17.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell23});
			this.xrTableRow17.Name = "xrTableRow17";
			this.xrTableRow17.Weight = 1.0400433368797812D;
			// 
			// xrTableCell23
			// 
			this.xrTableCell23.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "sumSum([ProductUnits])")});
			this.xrTableCell23.Font = new DXFont("Segoe UI", 14F, DXFontStyle.Bold);
			this.xrTableCell23.Name = "xrTableCell23";
			this.xrTableCell23.StylePriority.UseFont = false;
			xrSummary3.Running = SummaryRunning.Report;
			this.xrTableCell23.Summary = xrSummary3;
			this.xrTableCell23.Weight = 3D;
			// 
			// xrChart1
			// 
			this.xrChart1.BorderColor = Color.Black;
			this.xrChart1.Borders = BorderSide.None;
			this.xrChart1.EmptyChartText.DXFont = new DXFont("Segoe UI", 12F);
			this.xrChart1.EmptyChartText.Text = "\r\n";
			this.xrChart1.Legend.AlignmentVertical = LegendAlignmentVertical.Center;
			this.xrChart1.Legend.Border.Visibility = DefaultBoolean.False;
			this.xrChart1.Legend.DXFont = new DXFont("Segoe UI", 11F);
			this.xrChart1.Legend.EnableAntialiasing = DefaultBoolean.True;
			this.xrChart1.Legend.EquallySpacedItems = false;
			this.xrChart1.Legend.LegendID = -1;
			this.xrChart1.Legend.MarkerSize = new Size(20, 20);
			this.xrChart1.Legend.Padding.Left = 30;
			this.xrChart1.LocationFloat = new PointFloat(0F, 28.71097F);
			this.xrChart1.Name = "xrChart1";
			series1.ArgumentDataMember = "orderDate";
			series1.ArgumentScaleType = ScaleType.Qualitative;
			pieSeriesLabel1.TextPattern = "{V:$#,#}";
			series1.Label = pieSeriesLabel1;
			series1.LabelsVisibility = DefaultBoolean.False;
			series1.LegendTextPattern = "{A}: {V:$#,#}\n";
			series1.Name = "Series 1";
			series1.QualitativeSummaryOptions.SummaryFunction = "SUM([Total])";
			series1.SeriesID = 0;
			pieSeriesView1.Border.Visibility = DefaultBoolean.True;
			series1.View = pieSeriesView1;
			this.xrChart1.SeriesSerializable = new Series[] {
				series1};
			this.xrChart1.SeriesTemplate.View = pieSeriesView2;
			this.xrChart1.SizeF = new SizeF(366.6193F, 274.8307F);
			// 
			// orderDate
			// 
			this.orderDate.Expression = "GetYear([Order.OrderDate])";
			this.orderDate.FieldType = FieldType.Int32;
			this.orderDate.Name = "orderDate";
			// 
			// ProductSalesSummary
			// 
			this.Bands.AddRange(new Band[] {
				this.topMarginBand1,
				this.detailBand1,
				this.bottomMarginBand1,
				this.ReportHeader,
				this.ReportFooter});
			this.CalculatedFields.AddRange(new CalculatedField[] {
				this.orderDate});
			this.DataSource = this.bindingSource1;
			this.Font = new DXFont("Segoe UI", 9.75F);
			this.Margins = new DXMargins(104F, 104F, 125F, 102F);
			this.Version = "23.1";
			this.BeforePrint += new BeforePrintEventHandler(this.EmployeeSummary_BeforePrint);
			((ISupportInitialize)(this.bindingSource1)).EndInit();
			((ISupportInitialize)(this.xrTable3)).EndInit();
			((ISupportInitialize)(this.xrTable2)).EndInit();
			((ISupportInitialize)(this.xrTable1)).EndInit();
			((ISupportInitialize)(this.xrTable8)).EndInit();
			((ISupportInitialize)(pieSeriesLabel1)).EndInit();
			((ISupportInitialize)(pieSeriesView1)).EndInit();
			((ISupportInitialize)(series1)).EndInit();
			((ISupportInitialize)(pieSeriesView2)).EndInit();
			((ISupportInitialize)(this.xrChart1)).EndInit();
			((ISupportInitialize)(this)).EndInit();

		}
		
		private void EmployeeSummary_BeforePrint(object sender, CancelEventArgs e) {
			// string stringYears = (string)paramYears.Value;
			// bool isEmptyYears = string.IsNullOrEmpty(stringYears);
			// this.ReportFooter.Visible = !isEmptyYears;
			// if(isEmptyYears) 
			// 	return; 
			// string[] years = stringYears.Split(',').ToArray();
			// SetOrdersText(years);
			// SetFilterString(years);	 
		}
		void SetFilterString(string[] years){
			string[] filterYears = new string[years.Length];
			for(int i = 0; i < years.Length; i++) {
				filterYears[i] = "[Order.OrderDate.Year] == " + years[i];
			}
			this.FilterString = string.Join(" || ", filterYears);
		}
		void SetOrdersText(string[] years) {
			int countYears = years.Length;
			if(countYears > 1 && Convert.ToInt32(years[countYears - 1]) - Convert.ToInt32(years[0]) == countYears - 1) {
				xrTableCell2.Text = string.Join(" - ", new string[] { years[0], years[countYears - 1] });
			} else {
				// xrTableCell2.Text = (string)paramYears.Value;
			}   
		}
		private void xrTableCell21_BeforePrint(object sender, CancelEventArgs e) {
			object product = GetCurrentColumnValue("Product");
			if(product != null) {
				decimal cost = ((Product)product).Cost;
				decimal totalUnits = (decimal)xrTableCell23.Summary.GetResult();
				xrTableCell21.Text = (cost * totalUnits).ToString("$#,#");
			} else
				xrTableCell21.Text = "";
		}

		#endregion
	}
}
