/*
 * Copyright 2013 Eric Domazlicky
   This file is part of O365 User Checker
   O365 User Checker is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
   O365 User Checker is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
   You should have received a copy of the GNU General Public License. If not, see http://www.gnu.org/licenses/. 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using O365UserChecker.Properties;

namespace O365UserChecker
{
    
    public partial class FMain : Form
    {
        
        public FMain()
        {
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }

        private void BVerifyStudent_Click(object sender, EventArgs e)
        {
            if (ActionValidate())
            {
                BWVerifyUser.RunWorkerAsync(GetWorkArgsFromGUI());
                EVerifyStatus.Clear();
            }
        }

        private void BWVerifyUser_DoWork(object sender, DoWorkEventArgs e)
        {
            NameValueCollection WorkArgs = (NameValueCollection)e.Argument;
            e.Result = VerifyController.ExecuteO365Verify(WorkArgs["AdminUsername"], WorkArgs["AdminPassword"], 
                                                          WorkArgs["StudentUPN"], WorkArgs["Domain"]);
        }

        private void BWVerifyUser_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {           
            EVerifyStatus.Lines = ((List<String>)e.Result).ToArray();
        }      


        private void BResetUPN_Click(object sender, EventArgs e)
        {
            if (ActionValidate())
            {
                BWResetUpn.RunWorkerAsync(GetWorkArgsFromGUI());
                EVerifyStatus.Clear();
            }            
        }

        private void BWResetUpn_DoWork(object sender, DoWorkEventArgs e)
        {
            NameValueCollection WorkArgs = (NameValueCollection)e.Argument;
            e.Result = VerifyController.ExecuteResetUPN(WorkArgs["AdminUsername"], WorkArgs["AdminPassword"],
                                                          WorkArgs["StudentUPN"], WorkArgs["Domain"]);
        }

        private void BWResetUpn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EVerifyStatus.Text = "ResetUPN:" + e.Result.ToString();
        }

        private NameValueCollection GetWorkArgsFromGUI()
        {
            NameValueCollection WorkArgs = new NameValueCollection();
            WorkArgs["AdminUsername"] = EAdminUsername.Text;
            WorkArgs["AdminPassword"] = EAdminPassword.Text;
            WorkArgs["StudentUPN"] = EStudentUPN.Text;
            WorkArgs["Domain"] = EDomain.Text;
            return WorkArgs;
        }

        private bool ActionValidate()
        {
            if (BWVerifyUser.IsBusy || BWResetUpn.IsBusy)
            {
                MessageBox.Show("Wait for the current operation to complete");
                return false;
            }

            if ((EAdminUsername.Text.Length <=0) || (EStudentUPN.Text.Length <= 0) || (EAdminPassword.Text.Length <= 0)
                    || (EDomain.Text.Length <= 0))
            {
                MessageBox.Show("Please fill in all required fields");
                return false;
            }

            return true;
        }
                


    }
}
