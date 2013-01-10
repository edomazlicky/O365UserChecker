using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace O365UserChecker
{
    public partial class FMain : Form
    {
        
        public FMain()
        {
            InitializeComponent();
        }

        private void BVerifyStudent_Click(object sender, EventArgs e)
        {
            if ((EAdminUsername.Text.Length > 0) && (EStudentUPN.Text.Length > 0) && (EAdminPassword.Text.Length > 0) && (EDomain.Text.Length > 0))
            {
                List<String> Output = VerifyController.ExecuteO365Verify(EAdminUsername.Text, EAdminPassword.Text, EStudentUPN.Text, EDomain.Text);
                EVerifyStatus.Clear();
                EVerifyStatus.Lines = Output.ToArray();
            }
            else
                MessageBox.Show("Please fill in all required fields");
        }
    }
}
