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
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;

namespace OutlookInspired.Module.Resources.Reports {
	public class CustomerProfile : XtraReport {
		#region Designer generated code

		private TopMarginBand topMarginBand1;
		private DetailBand detailBand1;
		private CollectionDataSource bindingSource1;
		private ReportHeaderBand ReportHeader;
		private XRTable xrTable1;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell3;
		private XRTable xrTable2;
		private XRTableRow xrTableRow8;
		private XRTableCell xrTableCell15;
		private XRTableRow xrTableRow7;
		private XRTableCell xrTableCell14;
		private DetailReportBand DetailReport;
		private DetailBand Detail;
		private ReportHeaderBand ReportHeader1;
		private XRTable xrTable4;
		private XRTableRow xrTableRow2;
		private XRTableCell xrTableCell4;
		private XRTableCell xrTableCell6;
		private XRPageInfo xrPageInfo2;
		private XRPageInfo xrPageInfo1;
		private BottomMarginBand bottomMarginBand1;
		private XRPictureBox xrPictureBox1;
		private XRPictureBox xrPictureBox4;
		private XRTableCell xrTableCell7;
		private XRTableRow xrTableRow3;
		private XRTableCell xrTableCell8;
		private XRTableCell xrTableCell9;
		private XRTableRow xrTableRow4;
		private XRTableCell xrTableCell10;
		private XRTableCell xrTableCell11;
		private XRTableRow xrTableRow5;
		private XRTableCell xrTableCell12;
		private XRTableCell xrTableCell13;
		private XRTable xrTable3;
		private XRTableRow xrTableRow6;
		private XRTableCell xrTableCell2;
		private XRTableCell xrTableCell5;
		private XRTableRow xrTableRow10;
		private XRTableCell xrTableCell18;
		private XRTableCell xrTableCell19;
		private XRTableRow xrTableRow9;
		private XRTableCell xrTableCell16;
		private XRTableCell xrTableCell17;
		private XRTableCell xrTableCell20;
		private CalculatedField employeePosition;
		private GroupHeaderBand GroupHeader1;
		private Parameter paramContacts;
		public CustomerProfile() {
			InitializeComponent();
		}
		private void InitializeComponent() {
			ComponentResourceManager resources = new ComponentResourceManager(typeof(CustomerProfile));
			XRSummary xrSummary1 = new XRSummary();
			this.topMarginBand1 = new TopMarginBand();
			this.xrPictureBox1 = new XRPictureBox();
			this.detailBand1 = new DetailBand();
			this.xrPictureBox4 = new XRPictureBox();
			this.xrTable2 = new XRTable();
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
			this.bottomMarginBand1 = new BottomMarginBand();
			this.xrPageInfo2 = new XRPageInfo();
			this.xrPageInfo1 = new XRPageInfo();
			this.bindingSource1 = new CollectionDataSource();
			this.ReportHeader = new ReportHeaderBand();
			this.xrTable1 = new XRTable();
			this.xrTableRow1 = new XRTableRow();
			this.xrTableCell1 = new XRTableCell();
			this.xrTableCell3 = new XRTableCell();
			this.DetailReport = new DetailReportBand();
			this.Detail = new DetailBand();
			this.xrTable3 = new XRTable();
			this.xrTableRow6 = new XRTableRow();
			this.xrTableCell2 = new XRTableCell();
			this.xrTableCell5 = new XRTableCell();
			this.xrTableRow10 = new XRTableRow();
			this.xrTableCell18 = new XRTableCell();
			this.xrTableCell19 = new XRTableCell();
			this.xrTableRow9 = new XRTableRow();
			this.xrTableCell16 = new XRTableCell();
			this.xrTableCell20 = new XRTableCell();
			this.xrTableCell17 = new XRTableCell();
			this.ReportHeader1 = new ReportHeaderBand();
			this.xrTable4 = new XRTable();
			this.xrTableRow2 = new XRTableRow();
			this.xrTableCell4 = new XRTableCell();
			this.xrTableCell6 = new XRTableCell();
			this.GroupHeader1 = new GroupHeaderBand();
			this.paramContacts = new Parameter();
			this.employeePosition = new CalculatedField();
			((ISupportInitialize)(this.xrTable2)).BeginInit();
			((ISupportInitialize)(this.bindingSource1)).BeginInit();
			((ISupportInitialize)(this.xrTable1)).BeginInit();
			((ISupportInitialize)(this.xrTable3)).BeginInit();
			((ISupportInitialize)(this.xrTable4)).BeginInit();
			((ISupportInitialize)(this)).BeginInit();
			// 
			// topMarginBand1
			// 
			this.topMarginBand1.Controls.AddRange(new XRControl[] {
				this.xrPictureBox1});
			this.topMarginBand1.Font = new DXFont("Segoe UI", 9.75F);
			this.topMarginBand1.HeightF = 124.875F;
			this.topMarginBand1.Name = "topMarginBand1";
			this.topMarginBand1.StylePriority.UseFont = false;
			// 
			// xrPictureBox1
			// 
			this.xrPictureBox1.ImageSource = new ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
			this.xrPictureBox1.LocationFloat = new PointFloat(473.1667F, 52.58816F);
			this.xrPictureBox1.Name = "xrPictureBox1";
			this.xrPictureBox1.SizeF = new SizeF(170.8333F, 56.41184F);
			this.xrPictureBox1.Sizing = ImageSizeMode.StretchImage;
			// 
			// detailBand1
			// 
			this.detailBand1.Controls.AddRange(new XRControl[] {
				this.xrPictureBox4,
				this.xrTable2});
			this.detailBand1.HeightF = 237.2124F;
			this.detailBand1.KeepTogether = true;
			this.detailBand1.Name = "detailBand1";
			// 
			// xrPictureBox4
			// 
			this.xrPictureBox4.BorderColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
			this.xrPictureBox4.Borders = ((BorderSide)((((BorderSide.Left | BorderSide.Top) 
			                                             | BorderSide.Right) 
			                                            | BorderSide.Bottom)));
			this.xrPictureBox4.BorderWidth = 1F;
			this.xrPictureBox4.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "ImageSource", "[Logo]")});
			this.xrPictureBox4.LocationFloat = new PointFloat(10F, 16.05509F);
			this.xrPictureBox4.Name = "xrPictureBox4";
			this.xrPictureBox4.SizeF = new SizeF(158.7394F, 190.1483F);
			this.xrPictureBox4.Sizing = ImageSizeMode.ZoomImage;
			this.xrPictureBox4.StylePriority.UseBorderColor = false;
			this.xrPictureBox4.StylePriority.UseBorders = false;
			this.xrPictureBox4.StylePriority.UseBorderWidth = false;
			// 
			// xrTable2
			// 
			this.xrTable2.LocationFloat = new PointFloat(179.1667F, 14.58333F);
			this.xrTable2.Name = "xrTable2";
			this.xrTable2.Padding = new PaddingInfo(5, 0, 0, 0, 100F);
			this.xrTable2.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow8,
				this.xrTableRow7,
				this.xrTableRow3,
				this.xrTableRow4,
				this.xrTableRow5});
			this.xrTable2.SizeF = new SizeF(462.5F, 184.1186F);
			this.xrTable2.StylePriority.UsePadding = false;
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
				new ExpressionBinding("BeforePrint", "Text", "[Name]")});
			this.xrTableCell15.Font = new DXFont("Segoe UI", 26.25F);
			this.xrTableCell15.ForeColor = Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(178)))), ((int)(((byte)(144)))));
			this.xrTableCell15.Name = "xrTableCell15";
			this.xrTableCell15.StylePriority.UseFont = false;
			this.xrTableCell15.StylePriority.UseForeColor = false;
			this.xrTableCell15.Text = "[Prefix].[FullName]";
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
			this.xrTableRow3.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell8,
				this.xrTableCell9});
			this.xrTableRow3.Name = "xrTableRow3";
			this.xrTableRow3.Weight = 1.2264701559025575D;
			// 
			// xrTableCell8
			// 
			this.xrTableCell8.Multiline = true;
			this.xrTableCell8.Name = "xrTableCell8";
			this.xrTableCell8.Text = "[HomeOfficeLine]\r\n[HomeOfficeCity]";
			this.xrTableCell8.Weight = 1.4122964395059121D;
			// 
			// xrTableCell9
			// 
			this.xrTableCell9.Multiline = true;
			this.xrTableCell9.Name = "xrTableCell9";
			this.xrTableCell9.Text = "[BillingAddressLine]\r\n[BillingAddressCity]";
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
			this.xrTableRow5.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell12,
				this.xrTableCell13});
			this.xrTableRow5.Name = "xrTableRow5";
			this.xrTableRow5.Weight = 0.61225922764545693D;
			// 
			// xrTableCell12
			// 
			this.xrTableCell12.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Phone]")});
			this.xrTableCell12.Name = "xrTableCell12";
			this.xrTableCell12.Weight = 1.4122964395059121D;
			// 
			// xrTableCell13
			// 
			this.xrTableCell13.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[Fax]")});
			this.xrTableCell13.Name = "xrTableCell13";
			this.xrTableCell13.Weight = 1.5877035604940879D;
			// 
			// bottomMarginBand1
			// 
			this.bottomMarginBand1.Controls.AddRange(new XRControl[] {
				this.xrPageInfo2,
				this.xrPageInfo1});
			this.bottomMarginBand1.HeightF = 127.0833F;
			this.bottomMarginBand1.Name = "bottomMarginBand1";
			// 
			// xrPageInfo2
			// 
			this.xrPageInfo2.ForeColor = Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
			this.xrPageInfo2.LocationFloat = new PointFloat(544.5415F, 18.00003F);
			this.xrPageInfo2.Name = "xrPageInfo2";
			this.xrPageInfo2.Padding = new PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo2.PageInfo = PageInfo.DateTime;
			this.xrPageInfo2.SizeF = new SizeF(99.95856F, 23F);
			this.xrPageInfo2.StylePriority.UseForeColor = false;
			this.xrPageInfo2.TextFormatString = "{0:MMMM dd, yyyy}";
			// 
			// xrPageInfo1
			// 
			this.xrPageInfo1.ForeColor = Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
			this.xrPageInfo1.LocationFloat = new PointFloat(2.000014F, 18.00003F);
			this.xrPageInfo1.Name = "xrPageInfo1";
			this.xrPageInfo1.Padding = new PaddingInfo(2, 2, 0, 0, 100F);
			this.xrPageInfo1.SizeF = new SizeF(102.0834F, 23.00008F);
			this.xrPageInfo1.StylePriority.UseForeColor = false;
			this.xrPageInfo1.TextFormatString = "Page {0} of {1}";
			// 
			// bindingSource1
			// 
			this.bindingSource1.Name = "bindingSource1";
			this.bindingSource1.ObjectTypeName = "OutlookInspired.Module.BusinessObjects.Customer";
			this.bindingSource1.TopReturnedRecords = 0;
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new XRControl[] {
				this.xrTable1});
			this.ReportHeader.HeightF = 31.62498F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// xrTable1
			// 
			this.xrTable1.LocationFloat = new PointFloat(0F, 0F);
			this.xrTable1.Name = "xrTable1";
			this.xrTable1.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow1});
			this.xrTable1.SizeF = new SizeF(647.9999F, 31.62498F);
			// 
			// xrTableRow1
			// 
			this.xrTableRow1.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell1,
				this.xrTableCell3});
			this.xrTableRow1.Name = "xrTableRow1";
			this.xrTableRow1.Weight = 1.1244439019097223D;
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
			this.xrTableCell1.Text = "Customer Profile";
			this.xrTableCell1.TextAlignment = TextAlignment.MiddleLeft;
			this.xrTableCell1.Weight = 0.8195229174103581D;
			// 
			// xrTableCell3
			// 
			this.xrTableCell3.BackColor = Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.xrTableCell3.Name = "xrTableCell3";
			this.xrTableCell3.StylePriority.UseBackColor = false;
			this.xrTableCell3.Weight = 2.1804770825896416D;
			// 
			// DetailReport
			// 
			this.DetailReport.Bands.AddRange(new Band[] {
				this.Detail,
				this.ReportHeader1,
				this.GroupHeader1});
			this.DetailReport.DataMember = "Employees";
			this.DetailReport.DataSource = this.bindingSource1;
			this.DetailReport.Level = 0;
			this.DetailReport.Name = "DetailReport";
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new XRControl[] {
				this.xrTable3});
			this.Detail.HeightF = 90.21397F;
			this.Detail.KeepTogether = true;
			this.Detail.Name = "Detail";
			this.Detail.SortFields.AddRange(new GroupField[] {
				new GroupField("FirstName", XRColumnSortOrder.Ascending)});
			// 
			// xrTable3
			// 
			this.xrTable3.LocationFloat = new PointFloat(0F, 14F);
			this.xrTable3.Name = "xrTable3";
			this.xrTable3.Padding = new PaddingInfo(5, 0, 0, 0, 100F);
			this.xrTable3.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow6,
				this.xrTableRow10,
				this.xrTableRow9});
			this.xrTable3.SizeF = new SizeF(605.5085F, 68.64409F);
			this.xrTable3.StylePriority.UsePadding = false;
			this.xrTable3.StylePriority.UseTextAlignment = false;
			this.xrTable3.TextAlignment = TextAlignment.MiddleLeft;
			// 
			// xrTableRow6
			// 
			this.xrTableRow6.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell2,
				this.xrTableCell5});
			this.xrTableRow6.Name = "xrTableRow6";
			this.xrTableRow6.Weight = 1D;
			// 
			// xrTableCell2
			// 
			this.xrTableCell2.Font = new DXFont("Segoe UI", 9.75F, DXFontStyle.Bold);
			this.xrTableCell2.Name = "xrTableCell2";
			this.xrTableCell2.StylePriority.UseFont = false;
			this.xrTableCell2.Text = "[Prefix]. [FullName]";
			this.xrTableCell2.Weight = 0.83865710918719949D;
			// 
			// xrTableCell5
			// 
			this.xrTableCell5.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[CustomerStore].[Line]")});
			this.xrTableCell5.Name = "xrTableCell5";
			this.xrTableCell5.Weight = 1.9559974318284257D;
			// 
			// xrTableRow10
			// 
			this.xrTableRow10.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell18,
				this.xrTableCell19});
			this.xrTableRow10.Name = "xrTableRow10";
			this.xrTableRow10.Weight = 0.84110173306221525D;
			// 
			// xrTableCell18
			// 
			this.xrTableCell18.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[employeePosition]")});
			this.xrTableCell18.Name = "xrTableCell18";
			this.xrTableCell18.Weight = 0.8386571091871996D;
			// 
			// xrTableCell19
			// 
			this.xrTableCell19.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "[CustomerStore].[City]")});
			this.xrTableCell19.Name = "xrTableCell19";
			this.xrTableCell19.Weight = 1.9559974318284259D;
			// 
			// xrTableRow9
			// 
			this.xrTableRow9.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell16,
				this.xrTableCell20,
				this.xrTableCell17});
			this.xrTableRow9.Name = "xrTableRow9";
			this.xrTableRow9.Weight = 0.90466097880219154D;
			// 
			// xrTableCell16
			// 
			this.xrTableCell16.Name = "xrTableCell16";
			this.xrTableCell16.Text = "[CustomerStore.City] Store";
			this.xrTableCell16.Weight = 0.83865710918719949D;
			// 
			// xrTableCell20
			// 
			this.xrTableCell20.Name = "xrTableCell20";
			this.xrTableCell20.Text = "[CustomerStore.Phone] (Store)";
			this.xrTableCell20.Weight = 0.69990013709435117D;
			// 
			// xrTableCell17
			// 
			this.xrTableCell17.Name = "xrTableCell17";
			this.xrTableCell17.Text = "[MobilePhone] (Mobile)";
			this.xrTableCell17.Weight = 1.2560972947340747D;
			// 
			// ReportHeader1
			// 
			this.ReportHeader1.Controls.AddRange(new XRControl[] {
				this.xrTable4});
			this.ReportHeader1.HeightF = 31.30298F;
			this.ReportHeader1.Name = "ReportHeader1";
			// 
			// xrTable4
			// 
			this.xrTable4.LocationFloat = new PointFloat(0F, 0F);
			this.xrTable4.Name = "xrTable4";
			this.xrTable4.Rows.AddRange(new XRTableRow[] {
				this.xrTableRow2});
			this.xrTable4.SizeF = new SizeF(647.9999F, 31.30298F);
			// 
			// xrTableRow2
			// 
			this.xrTableRow2.Cells.AddRange(new XRTableCell[] {
				this.xrTableCell4,
				this.xrTableCell6});
			this.xrTableRow2.Name = "xrTableRow2";
			this.xrTableRow2.Weight = 1.1129949951171876D;
			// 
			// xrTableCell4
			// 
			this.xrTableCell4.BackColor = Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
			this.xrTableCell4.Font = new DXFont("Segoe UI", 11F);
			this.xrTableCell4.ForeColor = Color.White;
			this.xrTableCell4.Name = "xrTableCell4";
			this.xrTableCell4.Padding = new PaddingInfo(15, 0, 0, 0, 100F);
			this.xrTableCell4.StylePriority.UseBackColor = false;
			this.xrTableCell4.StylePriority.UseFont = false;
			this.xrTableCell4.StylePriority.UseForeColor = false;
			this.xrTableCell4.StylePriority.UsePadding = false;
			this.xrTableCell4.StylePriority.UseTextAlignment = false;
			this.xrTableCell4.Text = "Contacts";
			this.xrTableCell4.TextAlignment = TextAlignment.MiddleLeft;
			this.xrTableCell4.Weight = 0.81952277612524749D;
			// 
			// xrTableCell6
			// 
			this.xrTableCell6.BackColor = Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(217)))), ((int)(((byte)(217)))));
			this.xrTableCell6.ExpressionBindings.AddRange(new ExpressionBinding[] {
				new ExpressionBinding("BeforePrint", "Text", "sumCount([Id])")});
			this.xrTableCell6.Font = new DXFont("Segoe UI", 11F);
			this.xrTableCell6.Multiline = true;
			this.xrTableCell6.Name = "xrTableCell6";
			this.xrTableCell6.Padding = new PaddingInfo(0, 10, 0, 0, 100F);
			this.xrTableCell6.StylePriority.UseBackColor = false;
			this.xrTableCell6.StylePriority.UseFont = false;
			this.xrTableCell6.StylePriority.UsePadding = false;
			this.xrTableCell6.StylePriority.UseTextAlignment = false;
			xrSummary1.Running = SummaryRunning.Report;
			this.xrTableCell6.Summary = xrSummary1;
			this.xrTableCell6.TextAlignment = TextAlignment.MiddleRight;
			this.xrTableCell6.TextFormatString = "# of Employees: {0}";
			this.xrTableCell6.Weight = 2.1804772238747523D;
			// 
			// GroupHeader1
			// 
			this.GroupHeader1.GroupFields.AddRange(new GroupField[] {
				new GroupField("LastName", XRColumnSortOrder.Ascending)});
			this.GroupHeader1.HeightF = 0F;
			this.GroupHeader1.Name = "GroupHeader1";
			// 
			// paramContacts
			// 
			this.paramContacts.Description = "Contacts";
			this.paramContacts.Name = "paramContacts";
			this.paramContacts.Type = typeof(bool);
			this.paramContacts.ValueInfo = "True";
			this.paramContacts.Visible = false;
			// 
			// employeePosition
			// 
			this.employeePosition.Expression = "Upper([Employees.Position])";
			this.employeePosition.Name = "employeePosition";
			// 
			// CustomerProfile
			// 
			this.Bands.AddRange(new Band[] {
				this.topMarginBand1,
				this.detailBand1,
				this.bottomMarginBand1,
				this.ReportHeader,
				this.DetailReport});
			this.CalculatedFields.AddRange(new CalculatedField[] {
				this.employeePosition});
			this.DataSource = this.bindingSource1;
			this.Font = new DXFont("Segoe UI", 9.75F);
			this.Margins = new DXMargins(100F, 100F, 124.875F, 127.0833F);
			this.Parameters.AddRange(new Parameter[] {
				this.paramContacts});
			this.Version = "23.1";
			((ISupportInitialize)(this.xrTable2)).EndInit();
			((ISupportInitialize)(this.bindingSource1)).EndInit();
			((ISupportInitialize)(this.xrTable1)).EndInit();
			((ISupportInitialize)(this.xrTable3)).EndInit();
			((ISupportInitialize)(this.xrTable4)).EndInit();
			((ISupportInitialize)(this)).EndInit();

		}

		#endregion
	}
}
