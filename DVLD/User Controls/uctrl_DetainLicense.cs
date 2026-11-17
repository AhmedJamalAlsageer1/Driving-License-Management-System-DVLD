using DVLD.DTOs.Presention_DTO.Licnese.LocalLicense.Detain_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.User_Controls
{
    public partial class uctrl_DetainLicense : UserControl
    {
        public uctrl_DetainLicense()
        {
            InitializeComponent();
        }

        public decimal? DetainFines
        {
            get
            {
                if(decimal.TryParse(mtxtFines.Text, out decimal Fines))
                    return Fines;

                return null;
            }
        }

        private void mtxtFines_Click(object sender, EventArgs e)
        {
            if(mtxtFines.SelectionStart > mtxtFines.Text.Length)
                mtxtFines.SelectionStart = mtxtFines.Text.Length;
        }

        private void mtxtFines_DoubleClick(object sender, EventArgs e)
        {
            mtxtFines.SelectionStart = 0;
            mtxtFines.SelectionLength = mtxtFines.Text.Length;
        }

        private void mtxtFines_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsControl(e.KeyChar))
            {
                return;
            }

            if(e.KeyChar == '.' && mtxtFines.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            if(mtxtFines.Text.Contains("."))
            {
                int Index = mtxtFines.Text.IndexOf(".");
                string decimals = mtxtFines.Text.Substring(Index + 1);
                if(decimals.Length >= 3)
                {
                    e.Handled = true;
                }
            }
        }

        public void LoadDefaultData()
        {
            lblDetainID.Text = "-1";
            lblDetainDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            mtxtFines.Text = "";
            lblCreatedby.Text = "[???]";
            lblLicenseID.Text = "-1";
        }

        public void LoadData(int LicenseID)
        {
            lblDetainID.Text = "-1";
            lblDetainDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            mtxtFines.Text = "";
            lblCreatedby.Text = SharedLayer.Context.AppContext.CurrentUser.UserName;
            lblLicenseID.Text = LicenseID.ToString();
        }

        public void LoadData(DTO_DetainLicense_Info dto)
        {
            lblDetainID.Text = dto.DetainID.ToString();
            lblDetainDate.Text = dto.DetainDate.ToString("dd/MM/yyyy");
            mtxtFines.Text = dto.FineFees.ToString("G29");
            lblCreatedby.Text = dto.UserName;
            lblLicenseID.Text = dto.LicenseID.ToString();
        }

    }
}
