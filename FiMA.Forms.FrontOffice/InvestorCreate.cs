using FiMA.Data;
using FiMA.Data.Common.Contracts;
using System.Windows.Forms;

namespace FiMA.Forms.FrontOffice
{
    public partial class InvestorCreate : Form
    {
        private IRepository<INVESTORS_FUNDS> _investorsFundsRepo;

        public InvestorCreate(IRepository<INVESTORS_FUNDS> investorsFundsRepo)
        {
            this._investorsFundsRepo = investorsFundsRepo;
            InitializeComponent();
        }

        private void InvestorCreate_Load(object sender, System.EventArgs e)
        {
            this.dataGrid.DataSource = _investorsFundsRepo.GetAll();
        }

        private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void tabPerson_Click(object sender, System.EventArgs e)
        {

        }

        private void label17_Click(object sender, System.EventArgs e)
        {

        }

        private void label11_Click(object sender, System.EventArgs e)
        {

        }
    }
}
