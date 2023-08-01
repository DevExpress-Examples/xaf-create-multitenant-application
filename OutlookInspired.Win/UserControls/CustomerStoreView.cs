using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class CustomerStoreView : BaseUserControl
    {
        public CustomerStoreView()
        {
            InitializeComponent();
        }

        protected override Type GetObjectType()
        {
            return typeof(CustomerStore);
        }
    }
}
