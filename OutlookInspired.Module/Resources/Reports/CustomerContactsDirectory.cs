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

using DevExpress.Persistent.Base.ReportsV2;

namespace OutlookInspired.Module.Resources.Reports {
	public class CustomerContactsDirectory : DevExpress.XtraReports.UI.XtraReport {
		private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
		private DevExpress.XtraReports.UI.DetailBand detailBand1;
		private CollectionDataSource bindingSource1;
		private System.ComponentModel.IContainer components;
		private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
		private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
		private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
		private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
		private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
		private DevExpress.XtraReports.UI.XRTable xrTable1;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
		private DevExpress.XtraReports.UI.XRTable xrTable2;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
		private DevExpress.XtraReports.UI.XRLine xrLine1;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow8;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow9;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
		private DevExpress.XtraReports.UI.XRTableRow xrTableRow10;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
		private DevExpress.XtraReports.UI.XRLabel xrLabel1;
		private DevExpress.XtraReports.UI.CalculatedField FirstLetter;
		private DevExpress.XtraReports.Parameters.Parameter paramAscending;
		private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
		private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
		public CustomerContactsDirectory() {
			InitializeComponent();
		}
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerContactsDirectory));
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.bindingSource1 = new DevExpress.Persistent.Base.ReportsV2.CollectionDataSource();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.FirstLetter = new DevExpress.XtraReports.UI.CalculatedField();
            this.paramAscending = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.topMarginBand1.HeightF = 125F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(466.6667F, 52.20191F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(173.9583F, 56.41183F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrTable2});
            this.detailBand1.HeightF = 224F;
            this.detailBand1.KeepTogether = true;
            this.detailBand1.Name = "detailBand1";
            this.detailBand1.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("FirstName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FirstLetter]")});
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 48F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 17.27706F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.ProcessDuplicatesMode = DevExpress.XtraReports.UI.ProcessDuplicatesMode.Merge;
            this.xrLabel1.SizeF = new System.Drawing.SizeF(69.79166F, 78.125F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel1.WordWrap = false;
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(180.0856F, 11.44531F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow4,
            this.xrTableRow5,
            this.xrTableRow6,
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow9,
            this.xrTableRow10});
            this.xrTable2.SizeF = new System.Drawing.SizeF(460.0477F, 187.119F);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 0.6338577290674352D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.CanGrow = false;
            this.xrTableCell4.Font = new DevExpress.Drawing.DXFont("Segoe UI", 20F);
            this.xrTableCell4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(178)))), ((int)(((byte)(144)))));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseForeColor = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "[Prefix]. [FullName]";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            this.xrTableCell4.Weight = 3D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 0.34068025346384434D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseForeColor = false;
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "[CustomerStore.Customer.Name] ([CustomerStore.City] Store)";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell5.Weight = 3D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 0.37828530166861157D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1});
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Weight = 3D;
            // 
            // xrLine1
            // 
            this.xrLine1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(460.0477F, 12.71196F);
            this.xrLine1.StylePriority.UseForeColor = false;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8});
            this.xrTableRow5.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F);
            this.xrTableRow5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.StylePriority.UseFont = false;
            this.xrTableRow5.StylePriority.UseForeColor = false;
            this.xrTableRow5.Weight = 0.21567219504415658D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.CanGrow = false;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBorderColor = false;
            this.xrTableCell7.StylePriority.UseForeColor = false;
            this.xrTableCell7.StylePriority.UsePadding = false;
            this.xrTableCell7.Text = "STORE ADDRESS";
            this.xrTableCell7.Weight = 1.4868341453229292D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.CanGrow = false;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "PHONE";
            this.xrTableCell8.Weight = 1.5131658546770708D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell10});
            this.xrTableRow6.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F);
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.StylePriority.UseFont = false;
            this.xrTableRow6.Weight = 0.23600393509690332D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.CanGrow = false;
            this.xrTableCell9.Multiline = true;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.RowSpan = 2;
            this.xrTableCell9.Text = "[CustomerStore.Line]\r\n[CustomerStore.City]";
            this.xrTableCell9.Weight = 1.4868341548048936D;
            this.xrTableCell9.WordWrap = false;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.CanGrow = false;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UsePadding = false;
            this.xrTableCell10.Text = "[CustomerStore.Phone] (Store)";
            this.xrTableCell10.Weight = 1.5131658451951064D;
            this.xrTableCell10.WordWrap = false;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell12});
            this.xrTableRow7.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F);
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.StylePriority.UseFont = false;
            this.xrTableRow7.Weight = 0.23926277709568763D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.CanGrow = false;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "xrTableCell8";
            this.xrTableCell11.Weight = 1.4868341548048936D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.CanGrow = false;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "[MobilePhone] (Mobile)";
            this.xrTableCell12.Weight = 1.5131658451951064D;
            this.xrTableCell12.WordWrap = false;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 0.12622171748791217D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Weight = 3D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell14,
            this.xrTableCell15});
            this.xrTableRow9.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F);
            this.xrTableRow9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.StylePriority.UseFont = false;
            this.xrTableRow9.StylePriority.UseForeColor = false;
            this.xrTableRow9.Weight = 0.22588296444312883D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "POSITION";
            this.xrTableCell14.Weight = 1.486834109845256D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "EMAIL";
            this.xrTableCell15.Weight = 1.513165890154744D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16,
            this.xrTableCell17});
            this.xrTableRow10.Font = new DevExpress.Drawing.DXFont("Segoe UI", 8.25F);
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.StylePriority.UseFont = false;
            this.xrTableRow10.Weight = 0.34098411262588169D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Position]")});
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Weight = 1.4868337384779746D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Email]")});
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Weight = 1.5131662615220254D;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.bottomMarginBand1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.bottomMarginBand1.HeightF = 104F;
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
            this.bindingSource1.ObjectTypeName = "OutlookInspired.Module.BusinessObjects.CustomerEmployee";
            this.bindingSource1.TopReturnedRecords = 0;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 31F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StylePriority.UseFont = false;
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(641.6667F, 29.69642F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(128)))), ((int)(((byte)(71)))));
            this.xrTableCell1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 13F);
            this.xrTableCell1.ForeColor = System.Drawing.Color.White;
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Directory";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 0.7808441558441559D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Weight = 0.043932629870129913D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBackColor = false;
            this.xrTableCell3.Weight = 2.1752232142857144D;
            // 
            // FirstLetter
            // 
            this.FirstLetter.Expression = "Substring([LastName], 0, 1)";
            this.FirstLetter.Name = "FirstLetter";
            // 
            // paramAscending
            // 
            this.paramAscending.Description = "Ascending";
            this.paramAscending.Name = "paramAscending";
            this.paramAscending.Type = typeof(bool);
            this.paramAscending.ValueInfo = "True";
            this.paramAscending.Visible = false;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("LastName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 0F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // CustomerContactsDirectory
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1,
            this.PageHeader,
            this.GroupHeader1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.FirstLetter});
            this.DataSource = this.bindingSource1;
            this.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(104F, 104F, 125F, 104F);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.paramAscending});
            this.Version = "23.1";
            this.BeforePrint += new DevExpress.XtraReports.UI.BeforePrintEventHandler(this.CustomerContactDirectory_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}
		private void CustomerContactDirectory_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e) {
			if(Equals(true, paramAscending.Value)) {
				this.GroupHeader1.GroupFields[0].SortOrder = DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending;
				this.detailBand1.SortFields[0].SortOrder = DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending;
			} else {
				this.GroupHeader1.GroupFields[0].SortOrder = DevExpress.XtraReports.UI.XRColumnSortOrder.Descending;
				this.detailBand1.SortFields[0].SortOrder = DevExpress.XtraReports.UI.XRColumnSortOrder.Descending;
			}
		}
	}
}
