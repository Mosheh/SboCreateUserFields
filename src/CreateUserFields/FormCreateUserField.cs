using CreateUserFields.Common;
using CreateUserFields.Domain;
using CreateUserFields.Infra;
using MetroFramework;
using MetroFramework.Controls;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateUserFields
{
    public partial class FormCreateUserFields : MetroFramework.Forms.MetroForm
    {
        Company _company = new Company();
        ISAPTableRepository _tableRepository;
        IConnectionParamRepository _connectionParamRepository;
        private ConnectionParam _connectinoParam;
        private BoDataServerTypes _selectedServerType;

        public FormCreateUserFields()
        {
            InitializeComponent();
            metroGridTables.AutoGenerateColumns = false;
            metroTabControl1.SelectedIndex = 0;

            FillServerTypes();
            FormatFiedType();
            FormatFieldSubType();
            metroTabPageCreateUserField.Hide();

            _tableRepository = new Infra.SAPTableRepository(DataConnection.Instance);
            _connectionParamRepository = new Infra.ConnectionParamRepository(DataConnection.Instance);

            _connectinoParam = _connectionParamRepository.GetConnectionParam();
            if (_connectinoParam == null) _connectinoParam = new ConnectionParam();
            FillControls();
        }

        private void FormatFieldSubType()
        {
            foreach (BoFldSubTypes item in Enum.GetValues(typeof(BoFldSubTypes)))
            {
                metroComboBoxFieldSubType.Items.Add(item);
            }
        }

        private void FormatFiedType()
        {
            foreach (BoFieldTypes item in Enum.GetValues(typeof(BoFieldTypes)))
            {
                metroComboBoxFieldType.Items.Add(item);
            }
            metroComboBoxFieldType.SelectedIndexChanged += MetroComboBoxFieldType_SelectedIndexChanged;
            metroComboBoxFieldType.SelectedIndex = 0;
        }

        private void MetroComboBoxFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBoxFieldType.SelectedItem == null)
                return;

            var selectedType = (BoFieldTypes)Convert.ToInt32(metroComboBoxFieldType.SelectedItem);
            SetVisible(metroTextBoxSize, selectedType == BoFieldTypes.db_Alpha);
            SetVisible(metroLabelSize, selectedType == BoFieldTypes.db_Alpha);
            SetVisible(metroComboBoxFieldSubType, selectedType == BoFieldTypes.db_Float);
            SetVisible(metroLabelFieldSubType, selectedType == BoFieldTypes.db_Float);

            if (selectedType == BoFieldTypes.db_Alpha && metroComboBoxFieldSubType.Items.Count > 1)
                metroComboBoxFieldSubType.SelectedIndex = 0;
        }

        private void SetVisible(Control control, bool v)
        {
            control.Visible = v;
        }

        void FillSAPTables()
        {
            var tables = _tableRepository.GetAll();

            metroGridTables.DataSource = tables;
        }

        private void FillControls()
        {
            if (_connectinoParam == null)
                return;

            if (_connectinoParam.ServerType > 0)
                metroComboBoxServerType.SelectedItem = (BoDataServerTypes)_connectinoParam.ServerType;
            metroTextBoxServer.Text = _connectinoParam.Server;
            metroTextBoxUser.Text = _connectinoParam.DbUser;
            metroTextBoxPassword.Text = _connectinoParam.DbPassword;
            metroTextBoxCompanyDb.Text = _connectinoParam.CompanyDb;
            metroTextBoxSAPUser.Text = _connectinoParam.SAPUser;
            metroTextBoxSAPPassword.Text = _connectinoParam.SAPPassword;
        }

        private void FillEntity()
        {
            if (_connectinoParam == null)
                return;

            if (metroComboBoxServerType.SelectedItem != null)
                _connectinoParam.ServerType = (int)metroComboBoxServerType.SelectedItem;

            _connectinoParam.Server = metroTextBoxServer.Text;
            _connectinoParam.DbUser = metroTextBoxUser.Text;
            _connectinoParam.DbPassword = metroTextBoxPassword.Text;
            _connectinoParam.CompanyDb = metroTextBoxCompanyDb.Text;
            _connectinoParam.SAPUser = metroTextBoxSAPUser.Text;
            _connectinoParam.SAPPassword = metroTextBoxSAPPassword.Text;
        }

        private void FillServerTypes()
        {
            foreach (BoDataServerTypes boDataServerType in Enum.GetValues(typeof(BoDataServerTypes)))
            {
                metroComboBoxServerType.Items.Add(boDataServerType);
            }
        }

        void ConnectOnSAP()
        {

            _company.DbServerType = _selectedServerType;
            _company.language = BoSuppLangs.ln_Portuguese_Br;
            _company.Server = metroTextBoxServer.Text;
            _company.DbUserName = metroTextBoxUser.Text;
            _company.DbPassword = metroTextBoxPassword.Text;
            _company.UserName = metroTextBoxSAPUser.Text;
            _company.Password = metroTextBoxSAPPassword.Text;
            _company.CompanyDB = metroTextBoxCompanyDb.Text;
            if (_company.Connect() != 0)
                throw new Exception(_company.GetLastErrorDescription());
        }

        void SaveConnectionParam()
        {
            try
            {
                if (_connectinoParam == null)
                    return;
                FillEntity();
                _connectionParamRepository.Save(_connectinoParam);
            }
            catch
            {

            }
        }

        private void ValidFields()
        {
            if (metroComboBoxServerType.SelectedItem == null)
            {
                SetFocus(metroComboBoxServerType);
                throw new Exception("Selecione o tipo de servidor");
            }
            if (string.IsNullOrEmpty(metroTextBoxServer.Text))
            {
                SetFocus(metroTextBoxServer);
                throw new Exception("Informe o servidor (IP ou Nome)");
            }
            if (string.IsNullOrEmpty(metroTextBoxUser.Text))
            {
                SetFocus(metroTextBoxUser);
                throw new Exception("Informe o nome do usuário de banco de dados");
            }
            if (string.IsNullOrEmpty(metroTextBoxPassword.Text))
            {
                SetFocus(metroTextBoxPassword);
                throw new Exception("Informe a senha do usuário de banco de dados");
            }
            if (string.IsNullOrEmpty(metroTextBoxCompanyDb.Text))
            {
                SetFocus(metroTextBoxCompanyDb);
                throw new Exception("Informe o nome da base de dados SAP");
            }
            if (string.IsNullOrEmpty(metroTextBoxSAPUser.Text))
            {
                SetFocus(metroTextBoxSAPUser);
                throw new Exception("Informe o usuário SAP");
            }
            if (string.IsNullOrEmpty(metroTextBoxSAPPassword.Text))
            {
                SetFocus(metroTextBoxSAPPassword);
                throw new Exception("Informe a senha do usuário SAP");
            }
        }

        void SetFocus(Control control)
        {
            control.Select();
        }

        private void metroButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                metroLinkWaiting.Visible = true;
                metroLinkWaiting.Refresh();
                ValidFields();
                _selectedServerType = (BoDataServerTypes)metroComboBoxServerType.SelectedItem;
                //Thread thread = new Thread(ConnectOnSAP);
                //thread.Start();

                ConnectOnSAP();

                SaveConnectionParam();
                EnableSelecionTables();
                FillSAPTables();

                this.ShowSucess("Conexão realizada com sucesso!");

                metroTabControl1.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
            finally
            {
                metroLinkWaiting.Visible = false;

                this.Select();
            }
        }

        private void EnableSelecionTables()
        {
            if (_company.Connected)
                metroTabPageCreateUserField.Show();
            else
                metroTabPageCreateUserField.Hide();
        }

        private void metroButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroTextBoxSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void metroButtonCreateUserField_Click(object sender, EventArgs e)
        {
            try
            {
                ValidUserFieldInf();

                if (!_company.Connected)
                    throw new Exception("Empresa não conectado");
                _company.Disconnect();
                ConnectOnSAP();

                var sapTables = metroGridTables.CurrentRow.DataBoundItem as SAPTables;
                if (sapTables == null)
                    throw new Exception("Selecione uma tabela SAP");
                var type = (BoFieldTypes)Convert.ToInt32(metroComboBoxFieldType.SelectedItem);
                var subType = (BoFldSubTypes)Convert.ToInt32(metroComboBoxFieldSubType.SelectedItem);

                var md = _company.GetBusinessObject(BoObjectTypes.oUserFields) as UserFieldsMD;
                md.TableName = sapTables.Name;
                md.Name = metroTextBoxUserFieldName.Text;
                md.Description = metroTextBoxUserFieldDescription.Text;
                md.Type = type;
                md.SubType = subType;
                if (type == BoFieldTypes.db_Alpha)
                    md.Size = Convert.ToInt32(metroTextBoxSize.Text.Replace(".", ""));                

                if (md.Add() != 0)
                    throw new Exception(_company.GetLastErrorDescription());

                MetroMessageBox.Show(this, "Campo criado com sucesso!");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void ValidUserFieldInf()
        {
            if (string.IsNullOrEmpty(metroTextBoxUserFieldName.Text))
                throw new Exception("Informe o nome do campo de usuário");

            if (string.IsNullOrEmpty(metroTextBoxUserFieldDescription.Text))
                throw new Exception("Informe descrição para o campo de usuário");

            if (string.IsNullOrEmpty(metroTextBoxSize.Text) && IsAlphType())
                throw new Exception("Informe o tamanho do campo de usuário");

            if (metroGridTables.SelectedRows.Count == 0)
                throw new Exception("Selecione uma tabela SAP");
        }

        private bool IsAlphType()
        {
            if (metroComboBoxFieldType.SelectedItem == null)
                return false;
            else
                return ((BoFieldTypes)Convert.ToInt32(metroComboBoxFieldType.SelectedItem) == BoFieldTypes.db_Alpha);

        }

        private void metroButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new FormNewTable(_company, _tableRepository);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    FillSAPTables();
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void metroButtonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroGridTables.SelectedRows.Count == 0)
                    throw new Exception("Selecione uma tabela SAP");

                var sapTables = metroGridTables.CurrentRow.DataBoundItem as SAPTables;
                if (sapTables == null)
                    throw new Exception("Selecione uma tabela SAP");
                if (sapTables.IsSystem)
                    throw new Exception("Não é possível remover uma tabela padrão do sistema");

                if (MetroMessageBox.Show(this, "Deseja remover esse registro?", "INVENT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                _tableRepository.Remove(sapTables.Id);

                FillSAPTables();

                this.ShowSucess("Registro remotivo com sucesso!");
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://inventsoftware.com.br");
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }
    }
}
