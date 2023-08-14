using System.Reflection;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraSplashScreen;

namespace OutlookInspired.Win {
    public partial class XafDemoSplashScreen : DemoSplashScreen {
        private string GetSplashScreenImageResourcesName() {
            string splashScreenImageResourceName = "SplashScreenImage.svg";
            foreach(string resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
                if(resourceName.EndsWith(splashScreenImageResourceName)) {
                    return resourceName;
                }
            }
            return splashScreenImageResourceName;
        }
        private void LoadSplashImageFromResource() {
            var assembly = Assembly.GetExecutingAssembly();
            var svgStream = assembly.GetManifestResourceStream(GetSplashScreenImageResourcesName());
            if (svgStream is null) {
                throw new System.IO.FileNotFoundException(GetSplashScreenImageResourcesName());
            }
            svgStream.Position = 0;
            pictureEdit2.SvgImage = SvgImage.FromStream(svgStream);
        }
        public XafDemoSplashScreen() {
            InitializeComponent();
            LoadSplashImageFromResource();
            Load += (_, _) => BackColor = Color.FromArgb(46,46,46);
        }
        public override void ProcessCommand(Enum cmd, object arg) {
            base.ProcessCommand(cmd, arg);
            if((UpdateSplashCommand)cmd == UpdateSplashCommand.Description) {
                labelControl2.Text = (string)arg;
            }
        }
        protected override void DrawBackground(PaintEventArgs e){ }
    }
}
