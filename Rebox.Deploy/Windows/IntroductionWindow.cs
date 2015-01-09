using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rebox.Deploy.Windows
{
    public partial class IntroductionWindow
        : BaseInstallerWindow
    {
        public IntroductionWindow()
        {
            InitializeComponent();

            prevButton.Enabled = false;

            var introControl = new IntroductionUserControl {Dock = DockStyle.Fill};
            splitContainer1.Panel1.Controls.Add(introControl);
        }
    }
}
