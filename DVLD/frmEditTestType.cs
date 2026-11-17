using DVLD.DTOs;
using DVLDBisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmEditTestType : Form
    {
        private DTOTestTypes _dtoTestType = null;
        public frmEditTestType(int ID)
        {
            InitializeComponent();

            if (ID <= 0)
                return;

            if(clsTestTypes.GetTestTypeByID(ID, out DTOs.DTOTestTypes dtoTestType))
            {
                if(dtoTestType != null)
                {
                    _dtoTestType = dtoTestType;
                    FillControllsByData(dtoTestType);
                }
                else
                {
                    MessageBox.Show("حصل خطأ أثناء تحميل البيانات!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FillControllsByData(DTOs.DTOTestTypes dtoTestType)
        {
            llID.Text = dtoTestType.ID.ToString();
            txtTitle.Text = dtoTestType.Title.ToString();
            txtDescription.Text = dtoTestType.Description.ToString();
            mtxtFees.Text = dtoTestType.Fees.ToString("N0", CultureInfo.InvariantCulture);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool isActualDataModified()
        {
            if (txtTitle.Text.Trim() != _dtoTestType.Title)
                return true;
            else if (txtDescription.Text.Trim() != _dtoTestType.Description)
                return true;
            else if(decimal.TryParse(mtxtFees.Text.ToString(), out decimal value))
            {
                if(value != _dtoTestType.Fees)
                    return true;
            }
            
            return false;
        }

        private DTOTestTypes PreparingModifiedData()
        {
            DTOTestTypes dto = new DTOTestTypes();

            if (!decimal.TryParse(mtxtFees.Text, out decimal value))
                return null;

            return new DTOTestTypes
            {
                ID = _dtoTestType.ID,
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Fees = value,
            };
        }

        private bool istxtTitleValid()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                errorProvider1.SetError(txtTitle, "القيمة فارغة يرجى تعبئتها");
                return false;
            }
            errorProvider1.SetError(txtTitle, "");
            return true;
        }

        private bool istxtDescriptionValid()
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                errorProvider1.SetError(txtDescription, "القيمة فارغة يرجى تعبئتها");
                return false;
            }
            errorProvider1.SetError(txtDescription, "");
            return true;
        }

        private bool ismtxtFees()
        {
            bool isvalidvalue = false;
            if (decimal.TryParse(mtxtFees.Text.ToString(), out decimal value))
            {
                isvalidvalue = (value >= 1);
            }

            if(isvalidvalue)
            {
                errorProvider1.SetError(mtxtFees, "");
            }
            else
            {
                errorProvider1.SetError(mtxtFees, "يرجى إدخال قيمة صالحة!");
            }
            return isvalidvalue;
        }

        private bool isValid()
        {
            if(istxtTitleValid() && istxtDescriptionValid() && ismtxtFees())
                return true;
            return false;
        }

        public Action OnDataUpdated;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_dtoTestType.ID <= 0)
                return;

            if (!isValid())
            {
                MessageBox.Show("من فضلك تحقق من إدخال قيم صالحة للحقول.!", "فشل التعديل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(!isActualDataModified())
            {
                MessageBox.Show("لا يوجد بيانات لتعديلها حتى يعدل على البيانات الأصلية!", "فشل التعديل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DTOTestTypes dto = PreparingModifiedData();

            if (dto == null)
            {
                MessageBox.Show("لا يوجد بيانات لحظفها الكائن فارغ.", "تعذر التعديل", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTestTypes Type = clsTestTypes.GetTestTypeByID(_dtoTestType.ID);
            if(Type == null)
            {
                MessageBox.Show("حدث خطأ أثناء البحث عن المعرف لتعديل البيانات، من المتوقع أنه تم حذفه من قبل!", "فشل التعديل", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Type.UpdateData(dto))
            {
                _dtoTestType = dto;
                OnDataUpdated?.Invoke();
                MessageBox.Show("تم تعديل البيانات بنجاح.", "نجحت عملية التحديث.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("تعذر تحديث البيانات بسبب خطئ ما في البيانات الأساسية", "تعذر التعديل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            istxtTitleValid();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            istxtDescriptionValid();
        }

        private void mtxtFees_TextChanged(object sender, EventArgs e)
        {
            ismtxtFees();
        }

        private void mtxtFees_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (mtxtFees.SelectionStart >= mtxtFees.Text.Length)
                    mtxtFees.Select(mtxtFees.Text.Length, 0);
            }));
        }

        private void mtxtFees_DoubleClick(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                mtxtFees.Select(0, mtxtFees.Text.Length); // تحديد النص بالكامل
            }));
        }

        private void frmEditTestType_Shown(object sender, EventArgs e)
        {
            txtDescription.SelectionStart = 0;
            txtDescription.SelectionLength = 0;
        }
    }
}
