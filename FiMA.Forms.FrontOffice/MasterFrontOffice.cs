using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiMA.Forms.FrontOffice
{
    public partial class MasterFrontOffice : Form
    {
        public MasterFrontOffice()
        {
            InitializeComponent();
        }

        [Inject]
        public InvestorCreate Form { private get; set; }

        private void buttonInvestorCreate_Click(object sender, EventArgs e)
        {
            Form.MdiParent = this;
            Form.Show();
        }
    }
}
