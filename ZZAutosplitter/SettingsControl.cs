using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZZAutosplitter
{
    partial class SettingsControl : UserControl
    {
        private readonly Settings settings;

        public SettingsControl(Settings settings)
        {
            this.settings = settings;
            InitializeComponent();
        }
    }
}
