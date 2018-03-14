using FiMA.Data;
using FiMA.Data.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private INVESTORS_FUNDS model;

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
            this.model = new INVESTORS_FUNDS();

            InitializeComponent();
        }

        /// <summary>
        /// Setup all data in form fields when form is loaded
        /// </summary>
        private void InvestorCreate_Load(object sender, System.EventArgs e)
        {
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

            this.buttonClientDelete.Visible = false;
            this.buttonClientSave.Visible = false;
        }

        private void btnClientSearch_Click(object sender, System.EventArgs e)
        {
            // check if Id is correct
            var idText = this.textId.Text;
            if (idText.Length > 10 || 1 > idText.Length)
            {
                MessageBox.Show("Не ста въвели правилно ЕГН/БУЛСТАТ!");
            }

            // check if Type Person selected
            if (this.comboTypePerson.SelectedIndex <= -1)
            {
                MessageBox.Show("Не е избран тип лице!");
            }

            // check if Employee or Attorney selected
            if (this.comboEmployee.SelectedIndex <= -1)
            {
                MessageBox.Show("Не сте избрали СЛУЖИТЕЛ/ПЪЛНОМОЩНИК!");
            }

            // find all occurancies and load to table, show a message that record exist
            var dataSource = _investorsFundsRepo.GetAll<INVESTORS_FUNDS>(x => (x.PERSONALID_BULSTAT.IndexOf(idText) >= 0), null)
                .Select(x => 
                new {
                    columnId = x.CLIENTID,
                    clientString = x.CLIENTID_STRING,
                    id = x.PERSONALID_BULSTAT,
                    firstName = x.FIRSTNAME,
                    lastName = x.LASTNAME,
                    fullName = x.FULL_NAME,
                })
                .ToList();

            if (dataSource.Count() > 0)
            {
                MessageBox.Show("Клиентът вече съществува и няма да бъде създаден!");
            }

            this.dataGrid.DataSource = dataSource;
        }

        // when row clicked load data to fields
        private void dataGrid_DoubleClick(object sender, System.EventArgs e)
        {
            if (dataGrid.CurrentRow.Index != -1)
            {
                int id = (int)dataGrid.CurrentRow.Cells["columnId"].Value;
                model = this._investorsFundsRepo.GetById(id);

                this.PopulateForm(model);
            }
        }

        private void PopulateForm(INVESTORS_FUNDS model)
        {
            // initial info
            this.comboTypePerson.SelectedIndex = comboTypePerson.FindStringExact(model.TYPE_PERSON);
            this.comboEmployee.SelectedIndex = comboEmployee.FindStringExact(model.EMPLOYEE_AUTHORISED);
            this.comboRegisterCd.SelectedIndex = comboRegisterCd.FindStringExact(model.CD_REG);
            this.comboDds.SelectedIndex = comboDds.FindStringExact(model.DDS_REGISTERED);

            // address
            this.comboMailCountry.SelectedIndex = comboMailCountry.FindStringExact(model.COUNTRY_ADDRESS_ID);
            this.comboPersCountry.SelectedIndex = comboPersCountry.FindStringExact(model.COUNTRY1);
            this.comboMailTown.SelectedIndex = comboMailTown.FindStringExact(model.TOWN_ADDRESS_ID);
            this.comboPersTown.SelectedIndex = comboPersTown.FindStringExact(model.TOWN1);
            this.comboMailMunic.SelectedIndex = comboMailMunic.FindStringExact(model.MUNICIPALITY);
            this.comboPersMunic.SelectedIndex = comboPersMunic.FindStringExact(model.MUNICIPALITY);

            this.textPersStreet.Text = model.STREET;
            this.textMailAddress.Text = model.ADDRESS_ID;
            this.textPersDistr.Text = model.RESIDENCE;
            this.textPersStrNum.Text = model.STRNUM;
            this.textPersApt.Text = model.FLAT;
            this.textPersFloor.Text = model.FLOOR1;

            this.textId.Text = model.PERSONALID_BULSTAT;
            this.textClientId.Text = model.CLIENTID_STRING;
            this.textFirstName.Text = model.FIRSTNAME;
            this.textFamilyName.Text = model.LASTNAME;

            // bank accounts
            this.textIban1.Text = model.IBAN1;
            this.textBicCode1.Text = model.BIC1;
            this.textBankName1.Text = model.BANK1;

            this.textIban2.Text = model.IBAN2;
            this.textBicCode2.Text = model.BIC2;
            this.textBankName2.Text = model.BANK2;

            this.textIban3.Text = model.IBAN3;
            this.textBicCode3.Text = model.BIC3;
            this.textBankName3.Text = model.BANK3;

            this.buttonClientDelete.Visible = true;
            this.buttonClientSave.Visible = true;
            this.buttonClientSave.Text = "Обнови";
        }

        // hide tabs based on person type
        // show client number and status if existing
    }
}
