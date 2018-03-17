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
        private IUnitOfWork _uow;
        private INVESTORS_FUNDS model;
        private bool newClient;

        public InvestorCreate(
            IRepository<INVESTORS_FUNDS> investorsFundsRepo,
            IRepository<COUNTRIES> countriesRepo,
            IRepository<TOWNS> townsRepo,
            IRepository<TYPES> typesRepo,
            IRepository<MUNICIPALITY> municRepo,
            IUnitOfWork uow
            )
        {
            this._investorsFundsRepo = investorsFundsRepo;
            this._countriesRepo = countriesRepo;
            this._townsRepo = townsRepo;
            this._typesRepo = typesRepo;
            this._municRepo = municRepo;
            this._uow = uow;

            InitializeComponent();
        }

        #region Controls event methods
        /// <summary>
        /// Setup all data in form fields when form is loaded
        /// </summary>
        private void InvestorCreate_Load(object sender, System.EventArgs e)
        {
            this.LoadInitialForm();
        }

        private void btnClientSearch_Click(object sender, System.EventArgs e)
        {
            var idText = this.textId.Text;
            if(!this.CheckPrimaryFields(idText))
            {
                return;
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
                this.newClient = false;
            }
            else
            {
                MessageBox.Show("Не е открит клиент с такова ЕГН или ЕИК. Може да запишете като нов клиент!");
                this.LoadInitialForm();
                this.buttonClientSave.Visible = true;
                this.newClient = true;
            }

            this.dataGrid.DataSource = dataSource;
        }

        /// <summary>
        /// when row clicked load data to fields
        /// </summary>
        private void dataGrid_DoubleClick(object sender, System.EventArgs e)
        {
            if (dataGrid.CurrentRow.Index != -1)
            {
                int id = (int)dataGrid.CurrentRow.Cells["columnId"].Value;
                model = this._investorsFundsRepo.GetById(id);

                this.PopulateFormFromModel(model);
            }
        }

        private void buttonClientSave_Click(object sender, EventArgs e)
        {
            var idText = this.textId.Text;
            if (!this.CheckPrimaryFields(idText))
            {
                return;
            }

            model = this.CreateModelFromForm();

            if (this.newClient == true)
            {
                this._investorsFundsRepo.Add(model);
            }
            else
            {
                this._investorsFundsRepo.Update(model);
            }

            try
            {
                this._uow.Commit();
                MessageBox.Show("Записът е успешен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неъспешен запис! Опитайте отново!" + "\n" + ex.Message);
            }
        }

        private void comboTypePerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeFormForPersonType();
        }
        #endregion

        #region Custom helper methods
        private bool CheckPrimaryFields(string idText)
        {
            var result = true;
            // check if Id is correct
            if (idText.Length > 10 || 1 > idText.Length)
            {
                MessageBox.Show("Не ста въвели правилно ЕГН/БУЛСТАТ!");
                result = false;
            }

            // check if Type Person selected
            if (this.comboTypePerson.SelectedIndex <= -1)
            {
                MessageBox.Show("Не е избран тип лице!");
                result = false;
            }

            // check if Employee or Attorney selected
            if (this.comboEmployee.SelectedIndex <= -1)
            {
                MessageBox.Show("Не сте избрали СЛУЖИТЕЛ/ПЪЛНОМОЩНИК!");
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Takes all information about client from form and creates data model
        /// </summary>
        private INVESTORS_FUNDS CreateModelFromForm()
        {
            var md = new INVESTORS_FUNDS();

            md.CLIENTID = this._investorsFundsRepo.GetAll<int>(null, x => x.CLIENTID).Max() + 1;
            md.PERSONALID_BULSTAT = this.textId.Text;
            md.TYPE_PERSON = this.comboTypePerson.Text;
            md.EMPLOYEE_AUTHORISED = this.comboEmployee.Text;
            md.CD_REG = this.comboRegisterCd.Text;

            // create this here - do not use field!
            if (this.newClient)
            {
                md.CLIENTID_STRING = "noClString"; // TODO:
            }
            else
            {
                md.CLIENTID_STRING = this.textClientId.Text;
            }

            // address
            md.COUNTRY_ADDRESS_ID = this.comboMailCountry.Text;
            md.COUNTRY1 = this.comboPersCountry.Text;
            md.TOWN_ADDRESS_ID = this.comboMailTown.Text;
            md.TOWN1 = this.comboPersTown.Text;
            md.MUNICIPALITY = this.comboMailMunic.Text;

            md.STREET = this.textPersStreet.Text;
            md.ADDRESS_ID = this.textMailAddress.Text;
            md.RESIDENCE = this.textPersDistr.Text;
            md.STRNUM = this.textPersStrNum.Text;
            md.FLAT = this.textPersApt.Text;
            md.FLOOR1 = this.textPersFloor.Text;

            // person
            md.FIRSTNAME = this.textFirstName.Text;
            md.SECONDNAME = this.textMiddleName.Text;
            md.LASTNAME = this.textFamilyName.Text;

            md.CLIENT_BIRTHDATE = this.dateTimeBirthDate.Text;
            md.PERSONAL_ID = this.textPassportNumber.Text;
            md.PERSONAL_ID_DATE = this.dateTimePassportIssued.Text;
            md.PERSONAL_ID_ISSUED_BY = this.textPassportIssued.Text;

            // company
            md.FULL_NAME = this.textCompanyName.Text;
            md.ID_NUMBER_TAX_ID = this.textCompanyTaxId.Text;
            md.DDS_REGISTERED = this.comboDds.Text;
            md.TYPE_ORGANIZATION = this.comboOrgType.Text;

            // bank accounts
            md.IBAN1 = this.textIban1.Text;
            md.BIC1 = this.textBicCode1.Text;
            md.BANK1 = this.textBankName1.Text;

            md.IBAN2 = this.textIban2.Text;
            md.BIC2 = this.textBicCode2.Text;
            md.BANK2 = this.textBankName2.Text;

            md.IBAN3 = this.textIban3.Text;
            md.BIC3 = this.textBicCode3.Text;
            md.BANK3 = this.textBankName3.Text;

            // other data
            md.E_MAIL = this.textEmail.Text;
            md.TEL_FIXED = this.textPhone.Text;
            md.TEL_MOBILE = this.textMobile.Text;
            md.CD_GLOBID = int.Parse(this.textGlobalIdCd.Text);
            md.CD_BIC = this.textBicCodeCd.Text;

            // attorney data
            md.AUTHORISED_TYPE = this.textAttorneyType.Text;
            md.AUTHORISED_DOC = this.textAttorneyDoc.Text;
            md.AUTHORISED_DATE = this.dateTimeAttorney.Text;
            md.AUTH_NOTARY = this.textAttorneyNotary.Text;

            // select authorised

            md.CL_STATUS = "OK";

            return md;
        }

        /// <summary>
        /// Takes all information about selected client and populates fields of form
        /// </summary>
        private void PopulateFormFromModel(INVESTORS_FUNDS model)
        {
            // initial info
            this.textId.Text = model.PERSONALID_BULSTAT;
            this.comboTypePerson.SelectedIndex = comboTypePerson.FindStringExact(model.TYPE_PERSON);
            this.comboEmployee.SelectedIndex = comboEmployee.FindStringExact(model.EMPLOYEE_AUTHORISED);
            this.comboRegisterCd.SelectedIndex = comboRegisterCd.FindStringExact(model.CD_REG);
            this.tеxtStatus.Text = model.CL_STATUS;
            this.textClientId.Text = model.CLIENTID_STRING;

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

            // person
            this.textFirstName.Text = model.FIRSTNAME;
            this.textMiddleName.Text = model.SECONDNAME;
            this.textFamilyName.Text = model.LASTNAME;

            this.dateTimeBirthDate.Value = this.normalizeDate(model.CLIENT_BIRTHDATE);
            this.textPassportNumber.Text = model.PERSONAL_ID;
            this.dateTimePassportIssued.Value = this.normalizeDate(model.PERSONAL_ID_DATE);
            this.textPassportIssued.Text = model.PERSONAL_ID_ISSUED_BY;

            // company
            this.textCompanyName.Text = model.FULL_NAME;
            this.textCompanyTaxId.Text = model.ID_NUMBER_TAX_ID;
            this.comboDds.SelectedIndex = comboDds.FindStringExact(model.DDS_REGISTERED);
            this.comboOrgType.SelectedIndex = comboOrgType.FindStringExact(model.TYPE_ORGANIZATION);

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

            // other data
            this.textEmail.Text = model.E_MAIL;
            this.textPhone.Text = model.TEL_FIXED;
            this.textMobile.Text = model.TEL_MOBILE;
            this.textGlobalIdCd.Text = model.CD_GLOBID.ToString();
            this.textBicCodeCd.Text = model.CD_BIC;

            // attorney data
            this.textAttorneyType.Text = model.AUTHORISED_TYPE;
            this.textAttorneyDoc.Text = model.AUTHORISED_DOC;
            this.dateTimeAttorney.Value = this.normalizeDate(model.AUTHORISED_DATE);
            this.textAttorneyNotary.Text = model.AUTH_NOTARY;

            // show hidden buttons
            this.buttonClientDelete.Visible = true;
            this.buttonClientSave.Visible = true;

            // show client number and status if existing
            this.labelClientId.Visible = true;
            this.textClientId.Visible = true;

            this.labelStatus.Visible = true;
            this.tеxtStatus.Visible = true;

            this.buttonClientSave.Text = "ОБНОВИ";

            this.ChangeFormForPersonType();
        }

        /// <summary>
        /// Load form with initial settings and hidden buttons
        /// </summary>
        private void LoadInitialForm()
        {
            this.model = new INVESTORS_FUNDS();

            this.comboTypePerson.DataSource = _typesRepo.GetAll<string>(null, x => x.TYPE);
            this.comboEmployee.DataSource = new List<string>() { "СЛУЖИТЕЛ", "ПЪЛНОМОЩНИК", "НЕПРИЛОЖИМО" };
            this.comboRegisterCd.DataSource = new List<string>() { "ДА", "НЕ" };
            this.comboDds.DataSource = new List<string>() { "ДА", "НЕ" };
            this.comboMailCountry.DataSource = _countriesRepo.GetAll<string>(null, x => x.COUNTRY);
            this.comboPersCountry.DataSource = _countriesRepo.GetAll<string>(null, x => x.COUNTRY);
            this.comboMailTown.DataSource = _townsRepo.GetAll<string>(null, x => x.TOWNNAME);
            this.comboPersTown.DataSource = _townsRepo.GetAll<string>(null, x => x.TOWNNAME);
            this.comboPersMunic.DataSource = _municRepo.GetAll<string>(null, x => x.MUNICIPALITY1);
            this.comboMailMunic.DataSource = _municRepo.GetAll<string>(null, x => x.MUNICIPALITY1);

            this.buttonClientDelete.Visible = false;
            this.buttonClientSave.Visible = false;

            this.labelClientId.Visible = false;
            this.textClientId.Visible = false;

            this.labelStatus.Visible = false;
            this.tеxtStatus.Visible = false;
        }

        /// <summary>
        /// Hide and disable unused tabs when type of client is changed
        /// </summary>
        private void ChangeFormForPersonType()
        {
            ((Control)this.tabFirm).Enabled = true;
            ((Control)this.tabPerson).Enabled = true;

            var typePerson = this.comboTypePerson.Text.ToLower();
            if (typePerson.IndexOf("физическо") >= 0)
            {
                ((Control)this.tabFirm).Enabled = false;
                this.tabControlClientData.SelectedTab = this.tabPerson;
            }
            else
            {
                ((Control)this.tabPerson).Enabled = false;
                this.tabControlClientData.SelectedTab = this.tabFirm;
            }

            if (typePerson.IndexOf("българско") >= 0)
            {
                this.comboMailCountry.SelectedIndex = comboMailCountry.FindStringExact("БЪЛГАРИЯ");
                ((Control)this.comboMailCountry).Enabled = false;
                this.comboPersCountry.SelectedIndex = comboPersCountry.FindStringExact("БЪЛГАРИЯ");
                ((Control)this.comboPersCountry).Enabled = false;
            }
            else
            {
                ((Control)this.comboMailCountry).Enabled = true;
                ((Control)this.comboPersCountry).Enabled = true;
            }
        }

        /// <summary>
        /// Convert string date from DB to DateTime object or return today's date
        /// </summary>
        private DateTime normalizeDate(string date)
        {
            DateTime dateToConvert;
            return (date != "" && DateTime.TryParse(date, out dateToConvert))? dateToConvert:DateTime.Now;
        }

        private string CreateClientIdString(string id)
        {
            return "";
        }

        #endregion
    }
}
