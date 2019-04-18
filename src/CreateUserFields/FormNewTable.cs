using CreateUserFields.Domain;
using MetroFramework;
using MetroFramework.Forms;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateUserFields
{
    public partial class FormNewTable : MetroForm
    {
        private Company _company;
        private ISAPTableRepository _tableRepository;

        public FormNewTable(Company company, ISAPTableRepository tableRepository)
        {
            InitializeComponent();

            if (company == null)
                throw new Exception("Conexão SAP inválida");
            if(!company.Connected)
                throw new Exception("Empresa SAP não conectada");

            _company = company;
            _tableRepository = tableRepository;
        }

        private void metroButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void metroButtonClose_Click(object sender, EventArgs e)
        {
            try
            {
                ValidFields();

                ValidIfExistsTable();

                var table = new SAPTables() { Name = metroTextBoxTableName.Text, Description = metroTextBoxTableDescription.Text, System = "N" };

                _tableRepository.Add(table);
                MetroMessageBox.Show(this, "Tabela registrada com sucesso!", "INVENT", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "INVENT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidIfExistsTable()
        {
            try
            {
                var recordSet = _company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;

                recordSet.DoQuery($"select count(*) from \"{metroTextBoxTableName.Text}\"");                
            }
            catch (Exception)
            {

                throw new Exception($"Tabela {metroTextBoxTableName.Text} não encontrada");
            }

        }

        private void ValidFields()
        {
            if (string.IsNullOrEmpty(metroTextBoxTableName.Text))
                throw new Exception("Informe o nome da tabela SAP");

            if (string.IsNullOrEmpty(metroTextBoxTableDescription.Text))
                throw new Exception("Informe uma descrição para a tabela SAP");
        }

        private void metroTextBoxTableName_Leave(object sender, EventArgs e)
        {
            metroTextBoxTableName.Text = metroTextBoxTableName.Text.ToUpper();
        }
    }
}
