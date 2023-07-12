using System.Reflection;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Svg;
using DevExpress.XtraSplashScreen;

namespace OutlookInspired.Win {
    public partial class XafSplashScreen : SplashScreen {
		private void LoadBlankLogo() {
            var assembly = Assembly.GetExecutingAssembly();
            var blankLogoResourceName = assembly.GetName().Name + ".Images.Logo.svg";
            var svgStream = assembly.GetManifestResourceStream(blankLogoResourceName);
            if(svgStream != null) {
                svgStream.Position = 0;
                peLogo.SvgImage = SvgImage.FromStream(svgStream);
            }
        }
        protected override void DrawContent(GraphicsCache graphicsCache, Skin skin) {
            var bounds = ClientRectangle;
            bounds.Width--; bounds.Height--;
            graphicsCache.Graphics.DrawRectangle(graphicsCache.GetPen(Color.FromArgb(255, 87, 87, 87), 1), bounds);
        }
        protected void UpdateLabelsPosition() {
            labelApplicationName.CalcBestSize();
            int newLeft = (Width - labelApplicationName.Width) / 2;
            labelApplicationName.Location = new Point(newLeft, labelApplicationName.Top);
            labelSubtitle.CalcBestSize();
            newLeft = (Width - labelSubtitle.Width) / 2;
            labelSubtitle.Location = new Point(newLeft, labelSubtitle.Top);
        }
        public XafSplashScreen() {
            InitializeComponent();
			LoadBlankLogo();
            labelCopyright.Text = $"Copyright © {DateTime.Now.Year} Company Name{Environment.NewLine}All rights reserved.";
            UpdateLabelsPosition();
        }
        
        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg) {
            base.ProcessCommand(cmd, arg);
            if((UpdateSplashCommand)cmd == UpdateSplashCommand.Description) {
                labelStatus.Text = (string)arg;
            }
        }
        
        #endregion

    }
}