using MetroFramework;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateUserFields.Common
{
    public static class MessageService
    {
        public static void ShowSucess(this MetroForm form, string msg)
        {
            MetroMessageBox.Show(form, msg, "Invent", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowError(this MetroForm form,  string msg)
        {
            MetroMessageBox.Show(form, msg, "Invent", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowError(this MetroForm form, Exception ex)
        {
            ShowError(form, ex.Message);
        }

        public static void ShowInf(this MetroForm form,string msg)
        {
            MetroMessageBox.Show(form, msg, "Invent", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
