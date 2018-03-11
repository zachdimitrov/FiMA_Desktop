using FiMA.Data;
using FiMA.Data.Common.Contracts;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FiMA.Forms.FrontOffice
{
    public partial class InvestorCreate : Form
    {
        private IRepository<INVESTORS_FUNDS> _investorsFundsRepo;
        private IRepository<COUNTRIES> _countriesRepo;
        private IRepository<TOWNS> _townsRepo;
        private IRepository<TYPES> _typesRepo;
        private IRepository<MUNICIPALITY> _municRepo;

        public InvestorCreate(
            IRepository<INVESTORS_FUNDS> investorsFundsRepo,
            IRepository<COUNTRIES> countriesRepo,
            IRepository<TOWNS> townsRepo,
            IRepository<TYPES> typesRepo,
            IRepository<MUNICIPALITY> municRepo
            )
        {
            this._investorsFundsRepo = investorsFundsRepo;
            this._countriesRepo = countriesRepo;
            this._townsRepo = townsRepo;
            this._typesRepo = typesRepo;
            this._municRepo = municRepo;

            InitializeComponent();
        }

        /// <summary>
        /// Setup all data in form fields when form is loaded
        /// </summary>
        private void InvestorCreate_Load(object sender, System.EventArgs e)
        {
            this.dataGrid.DataSource = _investorsFundsRepo.GetAll();
            this.comboTypePerson.DataSource = _typesRepo.GetAll<string>(null, x => x.TYPE);
            this.comboEmployee.DataSource = new List<string>() { "СЛУЖИТЕЛ", "ПЪЛНОМОЩНИК", "НЕПРИЛОЖИМО" };
            this.comboRegisterCd.DataSource = new List<string>() { "ДА", "НЕ" };
            this.comboDds.DataSource = new List<string>() { "ДА", "НЕ" };
            this.comboMailCountry.DataSource = _countriesRepo.GetAll<string>(null, x=>x.COUNTRY);
            this.comboPersCountry.DataSource = _countriesRepo.GetAll<string>(null, x=>x.COUNTRY);
            this.comboMailTown.DataSource = _townsRepo.GetAll<string>(null, x=>x.TOWNNAME);
            this.comboPersTown.DataSource = _townsRepo.GetAll<string>(null, x=>x.TOWNNAME);
            this.comboPersMunic.DataSource = _municRepo.GetAll<string>(null, x=>x.MUNICIPALITY1);
            this.comboMailMunic.DataSource = _municRepo.GetAll<string>(null, x=>x.MUNICIPALITY1);
        }
    }
}
