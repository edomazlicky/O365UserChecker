using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace O365UserChecker
{
    
    class VerifyController
    {
        /// <summary>
        /// Executes the user verify and returns a list of strings with results suitable for display
        /// </summary>
        public static List<String> ExecuteO365Verify(string adminusername, string adminpassword, string userupn, string userdomain)
        {
            string xmlfilename = Application.StartupPath + "\\verifyrules.xml";
            List<VerifyRule> verify_rules = VerifyRuleFactory.LoadFromFile(xmlfilename);
            LocalUserFactory LocalUser = new LocalUserFactory(userdomain);
            List<String> retvalue = new List<String>();
            SecureString password = new SecureString();
            foreach (char c in adminpassword)            
                password.AppendChar(c);
            password.MakeReadOnly();
            CmdletExecutor cmdlet = new CmdletExecutor(adminusername, password, xmlfilename);
            cmdlet.LoginToOffice365();
            foreach (VerifyRule rule in verify_rules)
            {
                retvalue.Add("Verifying rule:" + rule.CommandName);
                NameValueCollection ruleparam = new NameValueCollection();
                ruleparam[rule.ArgumentName] = ReplaceUPN(rule.ArgumentValue, userupn);
                string ErrorOutput = "";
                NameValueCollection localuser = LocalUser.GetUserProperties(userupn);
                NameValueCollection o365user = cmdlet.ExecuteCommand(rule.CommandName, ruleparam, ref ErrorOutput);
                if (ErrorOutput.Length > 0)
                    retvalue.Add("FAILed to execute:" + ErrorOutput);
                else
                {
                    foreach (MatchProperty match in rule.Matches)
                    {
                        string matchinfo = "";
                        bool matchresult = match.IsMatch(o365user, localuser, ref matchinfo);
                        if (matchresult)
                            retvalue.Add("Matched " + match.PropertyName);
                        else
                            retvalue.Add("FAILED match:" + match.PropertyName+" values:"+matchinfo);
                    }
                }
            }
            cmdlet.Dispose();
            return retvalue;
        }

        private static string ReplaceUPN(string Value,string userUPN)
        {
            return Value.Replace("%UPN%", userUPN);
        }
    }
}
