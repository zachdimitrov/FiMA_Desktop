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
    }
}
