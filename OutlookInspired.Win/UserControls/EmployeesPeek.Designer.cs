using DevExpress.XtraGrid.Views.Layout;
using OutlookInspired.Win.Editors;

namespace OutlookInspired.Win.UserControls
{
    partial class EmployeesPeek
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
            layoutViewField1 = new LayoutViewField();
            layoutViewAddress = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_layoutViewColumn1 = new LayoutViewField();
            colFullName = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField2 = new LayoutViewField();
            layoutViewColumnEmail = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            layoutViewField_layoutViewColumn1_1 = new LayoutViewField();
            layoutViewColumnPhone = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            layoutViewField_layoutViewColumn1_2 = new LayoutViewField();
            layoutViewCard2 = new LayoutViewCard();
            repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHypertextLabel1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1_1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1_2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHyperLinkEdit1).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.EmbeddedNavigator.Margin = new Padding(2, 2, 2, 2);
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = layoutView2;
            gridControl1.Margin = new Padding(2, 2, 2, 2);
            gridControl1.Name = "gridControl1";
            gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemHypertextLabel1, repositoryItemHyperLinkEdit1 });
            gridControl1.Size = new Size(1188, 897);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { layoutView2 });
            // 
            // layoutView2
            // 
            layoutView2.ActiveFilterEnabled = false;
            layoutView2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            layoutView2.CardMinSize = new Size(279, 208);
            layoutView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] { colPhoto, layoutViewAddress, colFullName, layoutViewColumnEmail, layoutViewColumnPhone });
            layoutView2.DetailHeight = 516;
            layoutView2.GridControl = gridControl1;
            layoutView2.Name = "layoutView2";
            layoutView2.OptionsBehavior.Editable = false;
            layoutView2.OptionsFind.AlwaysVisible = true;
            layoutView2.OptionsFind.ShowFindButton = false;
            layoutView2.OptionsFind.ShowSearchNavButtons = false;
            layoutView2.OptionsHeaderPanel.EnableCarouselModeButton = false;
            layoutView2.OptionsHeaderPanel.EnableColumnModeButton = false;
            layoutView2.OptionsHeaderPanel.EnableCustomizeButton = false;
            layoutView2.OptionsHeaderPanel.EnableMultiColumnModeButton = false;
            layoutView2.OptionsHeaderPanel.EnableMultiRowModeButton = false;
            layoutView2.OptionsHeaderPanel.EnablePanButton = false;
            layoutView2.OptionsHeaderPanel.EnableRowModeButton = false;
            layoutView2.OptionsHeaderPanel.EnableSingleModeButton = false;
            layoutView2.OptionsHeaderPanel.ShowCarouselModeButton = false;
            layoutView2.OptionsHeaderPanel.ShowColumnModeButton = false;
            layoutView2.OptionsHeaderPanel.ShowCustomizeButton = false;
            layoutView2.OptionsView.ShowCardCaption = false;
            layoutView2.OptionsView.ShowHeaderPanel = false;
            layoutView2.OptionsView.ViewMode = LayoutViewMode.MultiRow;
            layoutView2.TemplateCard = layoutViewCard2;
            // 
            // colPhoto
            // 
            colPhoto.AppearanceCell.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point);
            colPhoto.AppearanceCell.Options.UseFont = true;
            colPhoto.AppearanceCell.Options.UseTextOptions = true;
            colPhoto.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            colPhoto.FieldName = "Picture.Data";
            colPhoto.LayoutViewField = layoutViewField1;
            colPhoto.MinWidth = 17;
            colPhoto.Name = "colPhoto";
            colPhoto.OptionsColumn.AllowEdit = false;
            colPhoto.OptionsColumn.AllowFocus = false;
            colPhoto.Width = 50;
            // 
            // layoutViewField1
            // 
            layoutViewField1.EditorPreferredWidth = 105;
            layoutViewField1.Location = new Point(0, 30);
            layoutViewField1.Name = "layoutViewField1";
            layoutViewField1.Size = new Size(109, 156);
            layoutViewField1.TextSize = new Size(0, 0);
            layoutViewField1.TextVisible = false;
            // 
            // layoutViewAddress
            // 
            layoutViewAddress.AppearanceHeader.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.DisabledText;
            layoutViewAddress.AppearanceHeader.Options.UseForeColor = true;
            layoutViewAddress.Caption = "ADDRESS";
            layoutViewAddress.FieldName = "Address";
            layoutViewAddress.LayoutViewField = layoutViewField_layoutViewColumn1;
            layoutViewAddress.MinWidth = 29;
            layoutViewAddress.Name = "layoutViewAddress";
            layoutViewAddress.Width = 109;
            // 
            // layoutViewField_layoutViewColumn1
            // 
            layoutViewField_layoutViewColumn1.EditorPreferredWidth = 151;
            layoutViewField_layoutViewColumn1.Location = new Point(109, 30);
            layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
            layoutViewField_layoutViewColumn1.Size = new Size(155, 53);
            layoutViewField_layoutViewColumn1.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_layoutViewColumn1.TextSize = new Size(76, 19);
            // 
            // colFullName
            // 
            colFullName.AppearanceCell.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            colFullName.AppearanceCell.Options.UseFont = true;
            colFullName.FieldName = "FullName";
            colFullName.LayoutViewField = layoutViewField2;
            colFullName.MinWidth = 17;
            colFullName.Name = "colFullName";
            colFullName.OptionsColumn.AllowEdit = false;
            colFullName.OptionsColumn.AllowFocus = false;
            colFullName.Width = 182;
            // 
            // layoutViewField2
            // 
            layoutViewField2.EditorPreferredWidth = 259;
            layoutViewField2.Location = new Point(0, 0);
            layoutViewField2.Name = "layoutViewField2";
            layoutViewField2.OptionsTableLayoutItem.ColumnIndex = 1;
            layoutViewField2.Size = new Size(264, 30);
            layoutViewField2.TextSize = new Size(0, 0);
            layoutViewField2.TextVisible = false;
            // 
            // layoutViewColumnEmail
            // 
            layoutViewColumnEmail.AppearanceHeader.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.DisabledText;
            layoutViewColumnEmail.AppearanceHeader.Options.UseForeColor = true;
            layoutViewColumnEmail.Caption = "EMAIL";
            layoutViewColumnEmail.ColumnEdit = repositoryItemHyperLinkEdit1;
            layoutViewColumnEmail.FieldName = "Email";
            layoutViewColumnEmail.LayoutViewField = layoutViewField_layoutViewColumn1_1;
            layoutViewColumnEmail.MinWidth = 29;
            layoutViewColumnEmail.Name = "layoutViewColumnEmail";
            layoutViewColumnEmail.Width = 109;
            // 
            // repositoryItemHypertextLabel1
            // 
            repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // layoutViewField_layoutViewColumn1_1
            // 
            layoutViewField_layoutViewColumn1_1.EditorPreferredWidth = 151;
            layoutViewField_layoutViewColumn1_1.Location = new Point(109, 83);
            layoutViewField_layoutViewColumn1_1.Name = "layoutViewField_layoutViewColumn1_1";
            layoutViewField_layoutViewColumn1_1.Size = new Size(155, 50);
            layoutViewField_layoutViewColumn1_1.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_layoutViewColumn1_1.TextSize = new Size(76, 19);
            // 
            // layoutViewColumnPhone
            // 
            layoutViewColumnPhone.AppearanceHeader.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.DisabledText;
            layoutViewColumnPhone.AppearanceHeader.Options.UseForeColor = true;
            layoutViewColumnPhone.Caption = "PHONE";
            layoutViewColumnPhone.FieldName = "HomePhone";
            layoutViewColumnPhone.LayoutViewField = layoutViewField_layoutViewColumn1_2;
            layoutViewColumnPhone.MinWidth = 29;
            layoutViewColumnPhone.Name = "layoutViewColumnPhone";
            layoutViewColumnPhone.Width = 109;
            // 
            // layoutViewField_layoutViewColumn1_2
            // 
            layoutViewField_layoutViewColumn1_2.EditorPreferredWidth = 151;
            layoutViewField_layoutViewColumn1_2.Location = new Point(109, 133);
            layoutViewField_layoutViewColumn1_2.Name = "layoutViewField_layoutViewColumn1_2";
            layoutViewField_layoutViewColumn1_2.Size = new Size(155, 53);
            layoutViewField_layoutViewColumn1_2.TextLocation = DevExpress.Utils.Locations.Top;
            layoutViewField_layoutViewColumn1_2.TextSize = new Size(76, 19);
            // 
            // layoutViewCard2
            // 
            layoutViewCard2.CustomizationFormText = "TemplateCard";
            layoutViewCard2.GroupBordersVisible = false;
            layoutViewCard2.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            layoutViewCard2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutViewField1, layoutViewField2, layoutViewField_layoutViewColumn1, layoutViewField_layoutViewColumn1_1, layoutViewField_layoutViewColumn1_2 });
            layoutViewCard2.Name = "layoutViewCard2";
            layoutViewCard2.OptionsItemText.TextToControlDistance = 5;
            layoutViewCard2.Text = "TemplateCard";
            // 
            // repositoryItemHyperLinkEdit1
            // 
            repositoryItemHyperLinkEdit1.AutoHeight = false;
            repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            // 
            // EmployeesPeek
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Margin = new Padding(2, 2, 2, 2);
            Name = "EmployeesPeek";
            Size = new Size(1188, 897);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField2).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHypertextLabel1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1_1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewField_layoutViewColumn1_2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutViewCard2).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHyperLinkEdit1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private LayoutView layoutView1;
        private LayoutViewField layoutViewField_colPhoto;
        private LayoutViewField layoutViewField_colFullName;
        private LayoutViewField layoutViewField_colTitle;
        private LayoutViewCard layoutViewCard1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colPhoto;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colFullName;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn layoutViewAddress;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn layoutViewColumnEmail;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn layoutViewColumnPhone;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private LayoutView layoutView2;
        private LayoutViewField layoutViewField1;
        private LayoutViewField layoutViewField_layoutViewColumn1;
        private LayoutViewField layoutViewField2;
        private LayoutViewField layoutViewField_layoutViewColumn1_1;
        private LayoutViewField layoutViewField_layoutViewColumn1_2;
        private LayoutViewCard layoutViewCard2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
    }
}
