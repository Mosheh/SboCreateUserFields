using CreateUserFields.Controls;
using CreateUserFields.Domain;
using CreateUserFields.Infra;
using CreateUserFields.Languages;
using MetroFramework;
using MetroFramework.Forms;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateUserFields
{
    public partial class FormNewTable : MetroForm, IForm
    {
        private Company _company;
        private ISAPTableRepository _tableRepository;
        private Setting _setting;
        public FormNewTable(Company company, ISAPTableRepository tableRepository)
        {
            InitializeComponent();
            _setting = new Infra.SettingRepository(DataConnection.Instance).GetSetting();

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

        public void SetText()
        {
            var culture = GetCulture();
            this.Text = LanguageService.Label(LabelEnum.NewTable, culture);
            metroLabelTableName.Text = LanguageService.Label(LabelEnum.TableName, culture);
            metroLabelDescription.Text = LanguageService.Label(LabelEnum.Description, culture);
            metroButtonCancel.Text = LanguageService.Label(LabelEnum.Cancel, culture);
        }

        private CultureInfo GetCulture()
        {
            if (string.IsNullOrEmpty(_setting.Language) ||
                _setting.Language.Equals("English"))
                return new CultureInfo("en-US");
            else
                return new CultureInfo("pt-BR");
        }
    }
}
