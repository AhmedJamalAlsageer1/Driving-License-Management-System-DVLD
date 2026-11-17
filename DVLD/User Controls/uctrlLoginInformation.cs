using DVLD.DTOs;
using DVLD.Inputs;
using DVLD.ViewModel;
using DVLDBisnessLayer;
using System.Windows.Forms;

namespace DVLD.User_Controls
{
    public partial class uctrlLoginInformation : UserControl
    {
        public uctrlLoginInformation()
        {
            InitializeComponent();
        }
        // تحميل بيانات مستخدم تم إدخاله

        private void DisplayUserData(string UserID, string UserName, string StatusActive)
        {
            llUserID.Text = UserID;
            llUserName.Text = UserName;
            llActivity.Text = StatusActive;
        }

        public void UpdateControl(string UserID, string UserName, string StatusActive)
        {
            DisplayUserData(UserID, UserName, StatusActive);
        }
    }
}
