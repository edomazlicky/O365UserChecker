/*
 * Copyright 2013 Eric Domazlicky
   This file is part of O365 User Checker
   O365 User Checker is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
   O365 User Checker is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
   You should have received a copy of the GNU General Public License. If not, see http://www.gnu.org/licenses/. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Xml;

namespace O365UserChecker
{
    /// <remarks>
    /// Controller class to execute the verify and reset UPN processes
    /// </remarks>
    class VerifyController
    {

        private string adminusername, adminpassword;
        private SecureString password;
        private List<VerifyRule> verify_rules;
        private NameValueCollection ToStringHints;

        public VerifyController(string adminusername, string adminpassword)
        {
            this.adminusername = adminusername;
            this.adminpassword = adminpassword;
            string xmlfilename = Application.StartupPath + "\\verifyrules.xml";
            ToStringHints = LoadToStringHints(xmlfilename);
            verify_rules = VerifyRuleFactory.LoadFromFile(xmlfilename);                        
            password = new SecureString();
            foreach (char c in adminpassword)
                password.AppendChar(c);
            password.MakeReadOnly();
            
        }

        private static NameValueCollection LoadToStringHints(string xmlfilename)
        {
            NameValueCollection ToStringHints = new NameValueCollection();
            using (XmlReader reader = XmlReader.Create(xmlfilename))
            {
                while (reader.ReadToFollowing("typehint"))
                {
                    reader.MoveToAttribute("name");
                    string name = reader.Value;
                    reader.MoveToAttribute("property");
                    string propvalue = reader.Value;
                    ToStringHints[name] = propvalue;
                }
            }
            return ToStringHints;
        }



        /// <summary>
        /// Executes the user verify and returns a list of strings with results suitable for display
        /// </summary>
        public List<String> O365Verify(string userupn, string userdomain)
        {
            LocalUserFactory LocalUser = new LocalUserFactory(userdomain);
            List<String> retvalue = new List<String>();
            using (CmdletExecutor cmdlet = new CmdletExecutor(adminusername, password, ToStringHints))
            {
                NameValueCollection localuser = LocalUser.GetUserProperties(userupn);
                if (localuser == null)
                {
                    retvalue.Add("FAILED to get properties for on-prem user");
                    return retvalue;
                }
                cmdlet.LoginToOffice365();
                foreach (VerifyRule rule in verify_rules)
                {
                    retvalue.Add("Verifying rule:" + rule.CommandName);
                    NameValueCollection ruleparam = new NameValueCollection();
                    ruleparam[rule.ArgumentName] = ReplaceUPN(rule.ArgumentValue, userupn);
                    string ErrorOutput = "";                    

                    NameValueCollection o365user = cmdlet.ExecuteCommand(rule.CommandName, ruleparam, rule.PSType, ref ErrorOutput);
                    if (ErrorOutput.Length > 0)
                        retvalue.Add("FAILED to execute:" + ErrorOutput);
                    else
                    {
                        foreach (MatchProperty match in rule.Matches)
                        {
                            string matchinfo = "";
                            bool matchresult = match.IsMatch(o365user, localuser, ref matchinfo);
                            if (matchresult)
                                retvalue.Add("Matched " + match.PropertyName);
                            else
                                retvalue.Add("FAILED match:" + match.PropertyName + " values:" + matchinfo);
                        }
                    }
                }
            }
            return retvalue;
        }

        /// <summary>
        /// Sets the user's UPN back to the onmicrosoft.com UPN, then set it back to the original again, fixes a lot of 
        /// "interesting" O365 issues (assumes adminusername domain is the onmicrosoft.com domain)
        /// </summary>
        public bool ResetUPN(string userupn)
        {
            bool result = false;
            if (adminusername.ToLower().EndsWith("onmicrosoft.com"))
            {
                string[] defaultDomainParts = adminusername.Split("@".ToCharArray());
                if (defaultDomainParts.Length == 2)
                {
                    string defaultDomain = defaultDomainParts[1];
                    using (CmdletExecutor cmdlet = new CmdletExecutor(adminusername, password, ToStringHints))
                    {
                        cmdlet.LoginToOffice365();
                        NameValueCollection setparams = new NameValueCollection();
                        string error_output = "";
                        string onmicrosoft_upn = userupn.Split("@".ToCharArray())[0]+"@"+defaultDomain;
                        setparams.Add("UserPrincipalName",userupn);
                        setparams.Add("NewUserPrincipalName", onmicrosoft_upn);
                        cmdlet.ExecuteCommand("Set-MsolUserPrincipalName", setparams, PowershellType.MSOL, ref error_output);
                        if (error_output.Length <= 0)
                        {
                            // now set it back to normal
                            setparams.Clear();
                            setparams.Add("UserPrincipalName", onmicrosoft_upn);
                            setparams.Add("NewUserPrincipalName", userupn);
                            cmdlet.ExecuteCommand("Set-MsolUserPrincipalName", setparams, PowershellType.MSOL, ref error_output);
                            result = error_output.Length <= 0;
                        }
                    }
                }
            }            
            return result;
        }
        

        private static string ReplaceUPN(string Value,string userUPN)
        {
            return Value.Replace("%UPN%", userUPN);
        }

        /// <summary>
        /// static executor functions for easy multi-threading
        /// </summary>
        public static List<String> ExecuteO365Verify(string adminusername, string adminpassword, string userupn, string localdomain)
        {
            VerifyController controller = new VerifyController(adminusername, adminpassword);            
            return controller.O365Verify(userupn, localdomain);
        }

        
        public static bool ExecuteResetUPN(string adminusername, string adminpassword, string userupn, string localdomain)
        {            
            VerifyController controller = new VerifyController(adminusername, adminpassword);
            return controller.ResetUPN(userupn);
        }

    }
}
