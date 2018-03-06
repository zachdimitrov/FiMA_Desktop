using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace FiMA.FrontOffice
{
    public partial class EfCrudTest : Form
    {
        public Employees model = new Employees();

        public EfCrudTest()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Clear();
        }

        private void Clear()
        {
            this.txtAddress.Text = "";
            this.txtName.Text = "";
            this.txtFamily.Text = "";

            this.btnSave.Text = "Save";
            this.btnDelete.Enabled = false;

            model.Id = 0;
        }

        private void EfCrudTest_Load(object sender, EventArgs e)
        {
            this.Clear();
            using (EF_CRUD_TestEntities db = new EF_CRUD_TestEntities())
            {
                this.comboCities.DataSource = db.Cities.Select(x => x.Name).ToList();

                // make it readonly
                this.comboCities.DropDownStyle = ComboBoxStyle.DropDownList;

                this.dataGrid.DataSource = db.Employees.Select(x => new { x.Id, x.Name, x.Family }).ToList();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.model.Name = txtName.Text;
            this.model.Family = txtFamily.Text;
            this.model.Address = txtAddress.Text;

            using (EF_CRUD_TestEntities db = new EF_CRUD_TestEntities())
            {
                var city = db.Cities
                    .Where(x => x.Name == (string)comboCities.SelectedValue)
                    .FirstOrDefault();

                int cityId = db.Cities
                    .Where(x => x.Name == (string)comboCities.SelectedValue)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                model.CityId = cityId;

                if (model.Id == 0)
                {
                    db.Employees.Add(model);
                }
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                }

                db.SaveChanges();
            }

            populateDataGridView();
            MessageBox.Show($"Employeee {this.txtName.Text} inserted to DB!");
            this.Clear();
        }

        private void comboCities_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void populateDataGridView()
        {
            using (EF_CRUD_TestEntities db = new EF_CRUD_TestEntities())
            {
                this.dataGrid.DataSource = db.Employees.Select(x => new { x.Id, x.Name, x.Family }).ToList();
            }
        }

        private void dataGrid_DoubleClick(object sender, EventArgs e)
        {
            if (dataGrid.CurrentRow.Index != -1)
            {
                model.Id = (int)dataGrid.CurrentRow.Cells["EmployeeId"].Value;
                using (EF_CRUD_TestEntities db = new EF_CRUD_TestEntities())
                {
                    var empl = db.Employees.Where(x => x.Id == model.Id).FirstOrDefault();
                    model = empl;
                    txtAddress.Text = model.Address;
                    txtFamily.Text = model.Family;
                    comboCities.SelectedIndex = comboCities.FindString(db.Cities
                        .Where(x => x.Id == model.CityId)
                        .Select(y => y.Name)
                        .FirstOrDefault());

                    txtName.Text = model.Name;
                }

                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "EF CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (EF_CRUD_TestEntities db = new EF_CRUD_TestEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                    {
                        db.Employees.Attach(model);
                    }

                    db.Employees.Remove(model);
                    db.SaveChanges();
                }

                populateDataGridView();
                Clear();
                MessageBox.Show("Employeee removed from DB!");
            }
        }
    }
}
