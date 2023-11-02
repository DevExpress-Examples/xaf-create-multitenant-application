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
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;

namespace OutlookInspired.Module.Resources.Reports {
	public class ProductProfile : XtraReport {
		#region Designer generated code

		private TopMarginBand topMarginBand1;
		private DetailBand detailBand1;
		private CollectionDataSource bindingSource1;
		private ReportHeaderBand ReportHeader;
		private XRPageInfo xrPageInfo1;
		private XRPageInfo xrPageInfo2;
		private XRPictureBox xrPictureBox2;
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
		private XRTable xrTable1;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell2;
		private XRTable xrTable2;
		private XRTableRow xrTableRow2;
		private XRTableCell xrTableCell4;
		private XRTableCell xrTableCell5;
		private Parameter paramImages;
		private XRPictureBox xrPictureBox1;
		private XRLabel xrLabel1;
		private DetailReportBand DetailReport;
		private DetailBand Detail;
		private XRBarCode xrBarCode1;
		private BottomMarginBand bottomMarginBand1;
		public ProductProfile() {
			InitializeComponent();
			BeforePrint += ProductProfile_BeforePrint;
		}
		private void ProductProfile_BeforePrint(object sender, CancelEventArgs e) {
			SetShowImages((bool)Parameters["paramImages"].Value);
		}
		public void SetShowImages(bool show) {
			if(DetailReport.Visible != show) DetailReport.Visible = show;
		}
		private void InitializeComponent() {
			ComponentResourceManager resources = new ComponentResourceManager(typeof(ProductProfile));
			QRCodeGenerator qrCodeGenerator1 = new QRCodeGenerator();
			this.topMarginBand1 = new TopMarginBand();
			this.xrPictureBox2 = new XRPictureBox();
			this.detailBand1 = new DetailBand();
			this.xrPictureBox1 = new XRPictureBox();
			this.bottomMarginBand1 = new BottomMarginBand();
			this.xrPageInfo2 = new XRPageInfo();
			this.xrPageInfo1 = new XRPageInfo();
			this.bindingSource1 = new CollectionDataSource();
			this.ReportHeader = new ReportHeaderBand();
			this.xrLabel1 = new XRLabel();
			this.xrTable3 = new XRTable();
			this.xrTableRow8 = new XRTableRow();
			this.xrTableCell15 = new XRTableCell();
			this.xrTableRow7 = new XRTableRow();
			this.xrTableCell7 = new XRTableCell();
			this.xrTableCell14 = new XRTableCell();
			this.xrTableRow3 = new XRTableRow();
			this.xrTableCell8 = new XRTableCell();
			this.xrTableCell9 = new XRTableCell();
			this.xrTableRow4 = new XRTableRow();
			this.xrTableCell10 = new XRTableCell();
			this.xrTableCell11 = new XRTableCell();
			this.xrTableRow5 = new XRTableRow();
			this.xrTableCell12 = new XRTableCell();
			this.xrTableCell13 = new XRTableCell();
			this.xrTable1 = new XRTable();
			this.xrTableRow1 = new XRTableRow();
			this.xrTableCell1 = new XRTableCell();
			this.xrTableCell2 = new XRTableCell();
			this.xrTable2 = new XRTable();
			this.xrTableRow2 = new XRTableRow();
			this.xrTableCell4 = new XRTableCell();
			this.xrTableCell5 = new XRTableCell();
			this.xrBarCode1 = new XRBarCode();
			this.paramImages = new Parameter();
			this.DetailReport = new DetailReportBand();
			this.Detail = new DetailBand();
			((ISupportInitialize)(this.bindingSource1)).BeginInit();
			((ISupportInitialize)(this.xrTable3)).BeginInit();
			((ISupportInitialize)(this.xrTable1)).BeginInit();
			((ISupportInitialize)(this.xrTable2)).BeginInit();
			((ISupportInitialize)(this)).BeginInit();
			// 
			// topMarginBand1
			// 
			this.topMarginBand1.Controls.AddRange(new XRControl[] {
				this.xrPictureBox2});
			this.topMarginBand1.HeightF = 136.7939F;
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
			this.detailBand1.MultiColumn.ColumnCount = 4;
			this.detailBand1.Name = "detailBand1";
			this.detailBand1.StylePriority.UseFont = false;
			this.detailBand1.StylePriority.UseForeColor = false;
			// 
			// xrPictureBox1
			// 
			this.xrPictureBox1.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "ImageSource", "[Picture.Data]")});
			this.xrPictureBox1.LocationFloat = new PointFloat(0F, 0F);
			this.xrPictureBox1.Name = "xrPictureBox1";
			this.xrPictureBox1.SizeF = new SizeF(156.25F, 145.9167F);
			this.xrPictureBox1.Sizing = ImageSizeMode.ZoomImage;
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
			this.bindingSource1.ObjectTypeName = "OutlookInspired.Module.BusinessObjects.Product";
			this.bindingSource1.TopReturnedRecords = 0;
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new XRControl[] {
				this.xrLabel1,
				this.xrTable3,
				this.xrTable1,
				this.xrTable2,
				this.xrBarCode1});
			this.ReportHeader.HeightF = 408.5997F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// xrLabel1
			// 
			this.xrLabel1.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[DescriptionString]")});
			this.xrLabel1.Font = new DXFont("Segoe UI", 11F);
			this.xrLabel1.LocationFloat = new PointFloat(0F, 312.4583F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new SizeF(641.6667F, 96.14142F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.Text = "xrLabel1";
			// 
			// xrTable3
			// 
			this.xrTable3.LocationFloat = new PointFloat(172.5001F, 43F);
			this.xrTable3.Name = "xrTable3";
			this.xrTable3.Padding = new PaddingInfo(5, 0, 0, 0, 100F);
			this.xrTable3.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow8,
				this.xrTableRow7,
				this.xrTableRow3,
				this.xrTableRow4,
				this.xrTableRow5});
			this.xrTable3.SizeF = new SizeF(462.5F, 174.6491F);
			this.xrTable3.StylePriority.UsePadding = false;
			// 
			// xrTableRow8
			// 
			this.xrTableRow8.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell15});
			this.xrTableRow8.Name = "xrTableRow8";
			this.xrTableRow8.Weight = 1.6412696279040691D;
			// 
			// xrTableCell15
			// 
			this.xrTableCell15.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Name]")});
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
			this.xrTableRow7.Weight = 0.97720188876250247D;
			// 
			// xrTableCell7
			// 
			this.xrTableCell7.Name = "xrTableCell7";
			this.xrTableCell7.Text = "LEAD ENGINEER";
			this.xrTableCell7.Weight = 1.4122964395059121D;
			// 
			// xrTableCell14
			// 
			this.xrTableCell14.Name = "xrTableCell14";
			this.xrTableCell14.StylePriority.UseFont = false;
			this.xrTableCell14.StylePriority.UseForeColor = false;
			this.xrTableCell14.Text = "LEAD SUPPORT TECH";
			this.xrTableCell14.Weight = 1.5877035604940879D;
			// 
			// xrTableRow3
			// 
			this.xrTableRow3.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell8,
				this.xrTableCell9});
			this.xrTableRow3.Name = "xrTableRow3";
			this.xrTableRow3.Weight = 0.897639921474321D;
			// 
			// xrTableCell8
			// 
			this.xrTableCell8.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Engineer.FullName]")});
			this.xrTableCell8.Multiline = true;
			this.xrTableCell8.Name = "xrTableCell8";
			this.xrTableCell8.Weight = 1.4122964395059121D;
			// 
			// xrTableCell9
			// 
			this.xrTableCell9.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Support.FullName]")});
			this.xrTableCell9.Multiline = true;
			this.xrTableCell9.Name = "xrTableCell9";
			this.xrTableCell9.Weight = 1.5877035604940879D;
			// 
			// xrTableRow4
			// 
			this.xrTableRow4.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell10,
				this.xrTableCell11});
			this.xrTableRow4.ForeColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.xrTableRow4.Name = "xrTableRow4";
			this.xrTableRow4.StylePriority.UseForeColor = false;
			this.xrTableRow4.StylePriority.UsePadding = false;
			this.xrTableRow4.StylePriority.UseTextAlignment = false;
			this.xrTableRow4.TextAlignment = TextAlignment.BottomLeft;
			this.xrTableRow4.Weight = 0.59861193158800252D;
			// 
			// xrTableCell10
			// 
			this.xrTableCell10.Name = "xrTableCell10";
			this.xrTableCell10.Text = "CURRENT INVENTORY";
			this.xrTableCell10.Weight = 1.4122964395059121D;
			// 
			// xrTableCell11
			// 
			this.xrTableCell11.Name = "xrTableCell11";
			this.xrTableCell11.Text = "IN MANUFCATURING";
			this.xrTableCell11.Weight = 1.5877035604940879D;
			// 
			// xrTableRow5
			// 
			this.xrTableRow5.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell12,
				this.xrTableCell13});
			this.xrTableRow5.Name = "xrTableRow5";
			this.xrTableRow5.Weight = 0.8370213138692D;
			// 
			// xrTableCell12
			// 
			this.xrTableCell12.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[CurrentInventory]")});
			this.xrTableCell12.Name = "xrTableCell12";
			this.xrTableCell12.Weight = 1.4122964395059121D;
			// 
			// xrTableCell13
			// 
			this.xrTableCell13.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Manufacturing]")});
			this.xrTableCell13.Name = "xrTableCell13";
			this.xrTableCell13.Weight = 1.5877035604940879D;
			// 
			// xrTable1
			// 
			this.xrTable1.Font = new DXFont("Segoe UI", 11F);
			this.xrTable1.LocationFloat = new PointFloat(0F, 249.2435F);
			this.xrTable1.Name = "xrTable1";
			this.xrTable1.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow1});
			this.xrTable1.SizeF = new SizeF(641.6666F, 31.70107F);
			this.xrTable1.StylePriority.UseFont = false;
			// 
			// xrTableRow1
			// 
			this.xrTableRow1.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell1,
				this.xrTableCell2});
			this.xrTableRow1.Name = "xrTableRow1";
			this.xrTableRow1.Weight = 1.3333334941638817D;
			// 
			// xrTableCell1
			// 
			this.xrTableCell1.BackColor = Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
			this.xrTableCell1.ForeColor = Color.White;
			this.xrTableCell1.Name = "xrTableCell1";
			this.xrTableCell1.Padding = new PaddingInfo(15, 0, 0, 0, 100F);
			this.xrTableCell1.StylePriority.UseBackColor = false;
			this.xrTableCell1.StylePriority.UseFont = false;
			this.xrTableCell1.StylePriority.UseForeColor = false;
			this.xrTableCell1.StylePriority.UsePadding = false;
			this.xrTableCell1.StylePriority.UseTextAlignment = false;
			this.xrTableCell1.Text = "Specifications";
			this.xrTableCell1.TextAlignment = TextAlignment.MiddleLeft;
			this.xrTableCell1.Weight = 0.63123575090946848D;
			// 
			// xrTableCell2
			// 
			this.xrTableCell2.BackColor = Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.xrTableCell2.Name = "xrTableCell2";
			this.xrTableCell2.Padding = new PaddingInfo(0, 10, 0, 0, 100F);
			this.xrTableCell2.StylePriority.UseBackColor = false;
			this.xrTableCell2.StylePriority.UsePadding = false;
			this.xrTableCell2.StylePriority.UseTextAlignment = false;
			this.xrTableCell2.TextAlignment = TextAlignment.MiddleRight;
			this.xrTableCell2.Weight = 1.727367429135799D;
			// 
			// xrTable2
			// 
			this.xrTable2.LocationFloat = new PointFloat(0F, 0F);
			this.xrTable2.Name = "xrTable2";
			this.xrTable2.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow2});
			this.xrTable2.SizeF = new SizeF(642F, 29.76803F);
			// 
			// xrTableRow2
			// 
			this.xrTableRow2.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell4,
				this.xrTableCell5});
			this.xrTableRow2.Name = "xrTableRow2";
			this.xrTableRow2.Weight = 1.0584190458553351D;
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
			this.xrTableCell4.Weight = 0.8195229174103581D;
			// 
			// xrTableCell5
			// 
			this.xrTableCell5.BackColor = Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.xrTableCell5.Name = "xrTableCell5";
			this.xrTableCell5.StylePriority.UseBackColor = false;
			this.xrTableCell5.Weight = 2.1526998647195241D;
			// 
			// xrBarCode1
			// 
			this.xrBarCode1.Alignment = TextAlignment.MiddleCenter;
			this.xrBarCode1.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Name]")});
			this.xrBarCode1.LocationFloat = new PointFloat(7.916673F, 42.72013F);
			this.xrBarCode1.Module = 5F;
			this.xrBarCode1.Name = "xrBarCode1";
			this.xrBarCode1.Padding = new PaddingInfo(10, 10, 0, 0, 100F);
			this.xrBarCode1.ShowText = false;
			this.xrBarCode1.SizeF = new SizeF(155.48F, 136.7939F);
			this.xrBarCode1.StylePriority.UseTextAlignment = false;
			qrCodeGenerator1.CompactionMode = QRCodeCompactionMode.Byte;
			qrCodeGenerator1.Version = QRCodeVersion.Version1;
			this.xrBarCode1.Symbology = qrCodeGenerator1;
			this.xrBarCode1.Text = "WWW";
			this.xrBarCode1.TextAlignment = TextAlignment.MiddleCenter;
			// 
			// paramImages
			// 
			this.paramImages.Description = "Images";
			this.paramImages.Name = "paramImages";
			this.paramImages.Type = typeof(bool);
			this.paramImages.ValueInfo = "True";
			this.paramImages.Visible = false;
			// 
			// DetailReport
			// 
			this.DetailReport.Bands.AddRange(new Band[] {
				this.Detail});
			this.DetailReport.DataMember = "Images";
			this.DetailReport.DataSource = this.bindingSource1;
			this.DetailReport.Level = 0;
			this.DetailReport.Name = "DetailReport";
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new XRControl[] {
				this.xrPictureBox1});
			this.Detail.HeightF = 145.9167F;
			this.Detail.MultiColumn.ColumnCount = 4;
			this.Detail.MultiColumn.Layout = ColumnLayout.AcrossThenDown;
			this.Detail.MultiColumn.Mode = MultiColumnMode.UseColumnCount;
			this.Detail.Name = "Detail";
			// 
			// ProductProfile
			// 
			this.Bands.AddRange(new Band[] {
				this.topMarginBand1,
				this.detailBand1,
				this.bottomMarginBand1,
				this.ReportHeader,
				this.DetailReport});
			this.DataSource = this.bindingSource1;
			this.Font = new DXFont("Segoe UI", 9.75F);
			this.Margins = new DXMargins(104F, 104F, 136.7939F, 102F);
			this.Parameters.AddRange(new Parameter[] {
				this.paramImages});
			this.Version = "23.1";
			((ISupportInitialize)(this.bindingSource1)).EndInit();
			((ISupportInitialize)(this.xrTable3)).EndInit();
			((ISupportInitialize)(this.xrTable1)).EndInit();
			((ISupportInitialize)(this.xrTable2)).EndInit();
			((ISupportInitialize)(this)).EndInit();

		}

		#endregion
	} 
}
