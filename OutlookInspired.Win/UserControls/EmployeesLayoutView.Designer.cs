using DevExpress.XtraGrid.Views.Layout;
using OutlookInspired.Win.Editors;

namespace OutlookInspired.Win.UserControls
{
    partial class EmployeesLayoutView
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
            layoutView2 = new LayoutView();
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
            layoutViewCard1 = new LayoutViewCard();
            Item1 = new DevExpress.XtraLayout.EmptySpaceItem();
            repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colPhoto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colFullName1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colAddress1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colEmail1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colMobilePhone).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Item1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHypertextLabel1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHyperLinkEdit1).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.EmbeddedNavigator.Margin = new Padding(2);
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = layoutView2;
            gridControl1.Margin = new Padding(2);
            gridControl1.Name = "gridControl1";
            gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemHypertextLabel1, repositoryItemHyperLinkEdit1 });
            gridControl1.Size = new Size(1188, 874);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { layoutView2 });
            // 
            // layoutView2
            // 
            layoutView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            layoutView2.CardCaptionFormat = "{3}";
            layoutView2.CardHorzInterval = 20;
            layoutView2.CardMinSize = new Size(300, 196);
            layoutView2.CardVertInterval = 20;
            layoutView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] { colPhoto, colFullName1, colAddress1, colEmail1, colMobilePhone });
            layoutView2.DetailHeight = 512;
            layoutView2.FieldCaptionFormat = "{0}";
            layoutView2.GridControl = gridControl1;
            layoutView2.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_colFullName1 });
            layoutView2.Name = "layoutView2";
            layoutView2.OptionsBehavior.AllowRuntimeCustomization = false;
            layoutView2.OptionsBehavior.Editable = false;
            layoutView2.OptionsBehavior.ReadOnly = true;
            layoutView2.OptionsFind.AlwaysVisible = true;
            layoutView2.OptionsFind.FindNullPrompt = "Search Employees (Ctrl + F)";
            layoutView2.OptionsFind.ShowClearButton = false;
            layoutView2.OptionsFind.ShowCloseButton = false;
            layoutView2.OptionsFind.ShowFindButton = false;
            layoutView2.OptionsItemText.TextToControlDistance = 2;
            layoutView2.OptionsView.AllowHotTrackFields = false;
            layoutView2.OptionsView.FocusRectStyle = FocusRectStyle.None;
            layoutView2.OptionsView.ShowHeaderPanel = false;
            layoutView2.OptionsView.ViewMode = LayoutViewMode.MultiRow;
            layoutView2.TemplateCard = layoutViewCard1;
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
            colFullName1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count) });
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
            // layoutViewCard1
            // 
            layoutViewCard1.CustomizationFormText = "TemplateCard";
            layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField_colAddress1, layoutViewField_colEmail1, layoutViewField_colPhoto, layoutViewField_colMobilePhone, Item1 });
            layoutViewCard1.Name = "layoutViewTemplateCard";
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
            // repositoryItemHypertextLabel1
            // 
            repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // repositoryItemHyperLinkEdit1
            // 
            repositoryItemHyperLinkEdit1.AutoHeight = false;
            repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            // 
            // labelControl1
            // 
            labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            labelControl1.Dock = DockStyle.Bottom;
            labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Top;
            labelControl1.LineVisible = true;
            labelControl1.Location = new Point(0, 874);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(94, 23);
            labelControl1.TabIndex = 1;
            labelControl1.Text = "labelControl1";
            // 
            // EmployeesLayoutView
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Controls.Add(labelControl1);
            Margin = new Padding(2);
            Name = "EmployeesLayoutView";
            Size = new Size(1188, 897);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colPhoto).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colFullName1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colAddress1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colEmail1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_colMobilePhone).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Item1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHypertextLabel1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHyperLinkEdit1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;

        private DevExpress.XtraGrid.Columns.LayoutViewColumn colPhoto;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private LayoutView layoutView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
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
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
