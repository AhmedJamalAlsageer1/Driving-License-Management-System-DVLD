using DVLD.DTOs;
using DVLDBisnessLayer;
using SharedLayer;
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
using System.Windows.Forms.VisualStyles;

namespace DVLD
{
    public partial class frmUpdateApplicationTypes : Form
    {
        private int _TypeID = -1;
        public frmUpdateApplicationTypes(int TypeID)
        {
            InitializeComponent();
            if (TypeID <= 0)
            {
                MessageBox.Show("هذا الرقم غير موجود.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Result<DTOApplicationTypes> ResultApplicationType =  clsApplicationTypesBL.GetApplicationTypeByID(TypeID);
            if (ResultApplicationType.isSuccess)
            {
                if (ResultApplicationType.Value != null)
                {
                    _TypeID = TypeID;
                    FillDataOnControlls(ResultApplicationType.Value);
                }
            }

            
        }

        private void FillDataOnControlls(DTOApplicationTypes dtoType)
        {
            llID.Text = dtoType.ID.ToString();
            txtTitle.Text = dtoType.Title.ToString();
            mtxtFees.Text = dtoType.fees.ToString("N0", CultureInfo.InvariantCulture);
        }

        private void mtxtFees_Enter(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (mtxtFees.SelectionStart >= mtxtFees.Text.Length)
                    mtxtFees.Select(mtxtFees.Text.Length, 0);
            }));
        }

        private void mtxtFees_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (mtxtFees.SelectionStart >= mtxtFees.Text.Length)
                    mtxtFees.Select(mtxtFees.Text.Length, 0);
            }));
        }

        private void mtxtFees_MouseClick(object sender, MouseEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (mtxtFees.SelectionStart >= mtxtFees.Text.Length)
                    mtxtFees.Select(mtxtFees.Text.Length, 0);
            }));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateApplicationTypes_Shown(object sender, EventArgs e)
        {
            txtTitle.SelectionLength = 0;
            txtTitle.SelectionStart = 0;
        }

        private bool isActualDataModified(DTOApplicationTypes dtoType)
        {
            if (dtoType.Title != txtTitle.Text.Trim())
                return true;

            if(decimal.TryParse(mtxtFees.Text.ToString(), out decimal inputfees))
            {
                if (dtoType.fees != inputfees)
                    return true;
            }

            return false;
        }

        private void UpdateData(DTOApplicationTypes dtoType)
        {
            dtoType.Title = txtTitle.Text.Trim();
            if(decimal.TryParse(mtxtFees.Text, out decimal inputfees))
                dtoType.fees = inputfees;
        }

        public Action OnDataUpdated;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_TypeID != -1)
            {
                Result<DTOApplicationTypes> resultDTOApplicationType = clsApplicationTypesBL.GetApplicationTypeByID(_TypeID);

                if (!resultDTOApplicationType.isSuccess)
                {
                    MessageBox.Show(resultDTOApplicationType.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (resultDTOApplicationType.Value != null)
                {
                    var dto = resultDTOApplicationType.Value;
                    if (isActualDataModified(dto))
                    {
                        UpdateData(resultDTOApplicationType.Value);

                        Result<clsApplicationTypesBL> resultApplicationType = clsApplicationTypesBL.GetApplicationTypebyID(dto.ID);
                        clsApplicationTypesBL ApplicationType = resultApplicationType.Value;

                        OperationResult ResultUpdate = ApplicationType.UpdateApplicationType(dto);
                        if (ResultUpdate.IsSuccess)
                        {
                            MessageBox.Show("اكتملت عملية التحديث بنجاح!", "نجحت عملية التحديث", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            OnDataUpdated?.Invoke();
                        }
                        else
                            MessageBox.Show("حدث خطأ أثناء عملية التحديث!", "فشلت علمية التحديث", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("من فضلك عدل على أي بيانات لنحدث!", "تنويه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
        }
    }
}
